using CagroLab.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CagroLab.ViewModel
{
    public class SampleViewModel
    {
        public int Id { get; set; }

        public int Package_Id { get; set; }

        public Package Package { get; set; }
        public int Sample_Type_Id { get; set; }

        public string? Patient_Name { get; set; }


        public int Patient_Id { get; set; }

        public string? Patient_Phone { get; set; }

        public bool IsDeleted { get; set; } = false;
        public int Account_Id { get; set; }

        public Account Account { get; set; }


        public List<String>? SampleTypes { get; set; }
    }

}
