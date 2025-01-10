using System.ComponentModel.DataAnnotations;

namespace TopiCos.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Both passwords must be same.")]
        public string ConfirmPassword { get; set; }
    }
}
