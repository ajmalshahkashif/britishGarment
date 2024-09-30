using System.ComponentModel.DataAnnotations;

namespace CommonModule.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
