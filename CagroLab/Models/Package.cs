namespace CagroLab.Models
{
    public class Package
    {
        public int Id { get; set; }
        public DateTime Package_Date { get; set; }
        public string Title { get; set; }
        public string Package_Description { get; set; }
        public int Account_Id { get; set; } // Ensure this matches the migration
        public Account Account { get; set; } // Navigation property

        public bool IsDeleted { get; set; } = false;
        public int Lab_Id { get; set; }
        public Lab Lab { get; set; }
        public ICollection<Sample> Samples { get; set; } // Navigation property

    }
}
