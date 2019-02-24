using System.ComponentModel.DataAnnotations;

namespace SeaStore.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
