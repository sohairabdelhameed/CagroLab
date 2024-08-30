﻿using CagroLab.Models;

namespace CagroLab.ViewModel
{
    public class SampleListViewModel
    {
        public int? Account_Id { get; set; }
        public int? Package_Id { get; set; }
        public List<Sample>? Samples { get; set; }

        public CreateSampleDto NewSample { get; set; }

        public int SelectedId { get; set; }
    }

    public class CreateSampleDto
    {
        public int? Account_Id { get; set; }
        public int? Package_Id { get; set; }
        public string Sample_Type_Id { get; set; }  // Changed to string to match dropdown values
        public string SubSample_Type_Id { get; set; } // New property for sub-sample type
        public string Concatenated_SubSample_Types { get; set; } 
        public string? Patient_Name { get; set; }
        public string? Patient_Phone { get; set; }
        public string? Status { get; set; }
    }
}
