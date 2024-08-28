using System.ComponentModel.DataAnnotations;

namespace CagroLab.ViewModel
{
    public class LabRegistrationViewModel
    {
        [Required]
        public string? Lab_Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Lab_Phone { get; set; }

        public string? Address { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }

        [Required]
        public string? Lab_Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Lab_Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Lab_Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }

}
