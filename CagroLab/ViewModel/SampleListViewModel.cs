using CagroLab.Models;

namespace CagroLab.ViewModel
{
    public class SampleListViewModel
    {
        public int? Account_Id { get; set; }
        public int? Package_Id { get; set; }
        public List<Sample>? Samples { get; set; }

        public CreateSampleDto NewSample { get; set; }
    }

    public class CreateSampleDto
    {
        public int? Account_Id { get; set; }
        public int? Package_Id { get; set; }
        public int Sample_Type_Id { get; set; }
        public string? Patient_Name { get; set; }
        public string? Patient_Phone { get; set; }
    }
}
