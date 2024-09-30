using System.ComponentModel.DataAnnotations;

namespace CommonModule.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your username.")]
        [StringLength(40, ErrorMessage = "Username cannot exceed 40 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
