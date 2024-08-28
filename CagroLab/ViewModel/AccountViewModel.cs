using CagroLab.Models;
using System.ComponentModel.DataAnnotations;

namespace CagroLab.ViewModel
{
    public class AccountViewModel
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        [DataType(DataType.Password)]
        public string? Account_Password { get; set; } // Field for new password

        public string? Full_Name { get; set; }
        public DateTime? Last_Activity { get; set; }
        public DateTime? Last_Login { get; set; }
        public bool Is_Active { get; set; }
        public bool Main_Account { get; set; }

        public int Lab_Id { get; set; }
    }

}
