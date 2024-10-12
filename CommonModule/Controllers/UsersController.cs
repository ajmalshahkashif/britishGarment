using CommonModule.DB;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using CommonModule.Models;

namespace CommonModule.Controllers
{
    public class UsersController : Controller
    {
        private readonly GarmentContext _context;

        public UsersController(GarmentContext context)
        {
            _context = context;
        }

        // GET: Users
        public IActionResult Index()
        {
            var garmentContext = _context.Users.Include(u => u.RoleId);
            return View(garmentContext.ToListAsync());
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
                Role userRole = _context.Roles.FirstOrDefault(r => r.RoleId == user.RoleId);
                if (user == null || !VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }

                // Setting up the session
                HttpContext.Session.SetString("Username", user.Username);


                // Determine the user role(admin or customer) and redirect accordingly
                if (userRole.Name == "Admin")
                {
                    // Redirect to the Admin project
                    return Redirect("https://localhost:7127/"); // Replace with actual customer URL
                }
                else if (userRole.Name == "Customer")
                
                {
                    // Redirect to the Customer project
                    return Redirect("https://localhost:7154/"); // Replace with actual customer URL
                }
                else
                {
                    ModelState.AddModelError("", "Invalid user role.");
                    return View(model);
                }


                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }




        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }


        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the username or email already exists
                var existingUser = _context.Users.FirstOrDefault(u => u.Username == model.Username || u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username or Email already exists.");
                    return View(model);
                }

                var roleId = _context.Roles
                        .Where(r => r.Name == "Customer")
                        .Select(r => r.RoleId)
                        .FirstOrDefault();

                // Generate password reset token (in a real application, use a more secure method)
                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                // Create new user
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    RoleId = roleId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    PasswordResetToken = token
                };

                // Generate Password Hash and Salt
                (user.PasswordHash, user.PasswordSalt) = CreatePasswordHash(model.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        private (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return (passwordHash, passwordSalt);
            }
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Username == id);
        }



        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Email not found.");
                    return View(model);
                }

                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                var resetLink = Url.Action("ResetPassword", "Users", new { email = model.Email, token = token }, Request.Scheme);

                await SendResetEmail(model.Email, resetLink);

                user.PasswordResetToken = token;
                user.ResetTokenExpires = DateTime.Now.AddHours(1);
                await _context.SaveChangesAsync();

                ViewBag.Message = "An email has been sent. Please check your inbox";
                return View(model);
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            // Verify token and email (in a real application, ensure the token hasn't expired)
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordResetToken == token && u.ResetTokenExpires > DateTime.Now);
            if (user == null)
            {
                // Handle invalid token or email (you could redirect to an error page)
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordViewModel { Email = email, Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.PasswordResetToken == model.Token && u.ResetTokenExpires > DateTime.Now);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid token or token expired.");
                    return View(model);
                }

                // Hash the new password
                (user.PasswordHash, user.PasswordSalt) = CreatePasswordHash(model.Password);

                // Clear the reset token
                user.PasswordResetToken = null;
                user.ResetTokenExpires = null;

                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }


        private async Task SendResetEmail(string email, string resetLink)
        {
            var fromAddress = new MailAddress("kashifajmal.ksk@gmail.com", "Shah Traders");
            var toAddress = new MailAddress(email);
            const string fromPassword = "thvfhxvvvoymxska"; // Replace with your actual app password
            const string subject = "Password Reset";
            string body = $"Please reset your password by clicking on the link below:\n\n{resetLink}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587, // Port for TLS
                EnableSsl = true, // Enable SSL
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
            catch (SmtpException ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"SMTP Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Log or handle other exceptions as needed
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Users");
        }

    }
}
