namespace CagroLab.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Account_Password { get; set; }
        public string? Full_Name { get; set; }
        public DateTime Last_Activity { get; set; }

        public DateTime Last_Login { get; set; }
        public bool Is_Active { get; set; }

        public bool Main_Account { get; set; }

        public int Lab_Id { get; set; }

        public Lab? Lab { get; set; }

        public ICollection<Package> Packages { get; set; }
        public ICollection<Sample> Samples { get; set; }

    }

}
