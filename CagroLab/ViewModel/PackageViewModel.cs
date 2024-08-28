using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CagroLab.ViewModel
{
    public class PackageViewModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime Package_Date { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Package_Description { get; set; }

        [Required]
        public int Account_Id { get; set; }

        public IEnumerable<SelectListItem> Accounts { get; set; }
    }

}
