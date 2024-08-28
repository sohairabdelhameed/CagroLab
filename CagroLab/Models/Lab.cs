namespace CagroLab.Models
{
    public class Lab
    {
        public int Id { get; set; }
        public string? Lab_Name { get; set; }

        public string?  Email { get; set; }
        public string? Lab_Phone { get; set; }

        public string? Address { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }

        public string? Lab_Username { get; set; }
        public string? Lab_Password { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Account> Accounts { get; set; } = new List<Account>();

        public ICollection<Package> Packages { get; set; } = new List<Package>();

    }
}
