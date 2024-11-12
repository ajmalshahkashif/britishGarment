using CommonModule.DB;
using System.ComponentModel.DataAnnotations;

namespace Admin_Module.Models
{
    public class VendorEditViewModel
    {
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        public string? Address { get; set; }
        [Display(Name = "Mobile No")]
        public string? MobileNo { get; set; }
    }
}
