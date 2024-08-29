namespace CagroLab.Models
{
    public class Sample
    {
        public int Id { get; set; }

        public int Package_Id { get; set; }

        public Package Package { get; set; }
        public string Sample_Type { get; set; }

        public string? Patient_Name  { get; set; }

       
        public int Patient_Id { get; set; }

        public string? Patient_Phone { get; set; }

        public bool IsDeleted { get; set; } = false;
        public int Account_Id { get; set; }
       
        public Account Account { get; set; }
       





    }
}
