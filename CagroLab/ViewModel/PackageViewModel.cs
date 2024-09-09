using CagroLab.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CagroLab.ViewModel
{
    public class PackageViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Package_Date { get; set; }

        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string? Package_Description { get; set; }

        [Required]
        public int Account_Id { get; set; }
        public Account? Account { get; set; } // Navigation property

        public int Lab_Id { get; set; }
        public Lab? Lab { get; set; }
     
        public IEnumerable<SelectListItem>? Accounts { get; set; }
        public string? Status { get; set; }
    }

}
