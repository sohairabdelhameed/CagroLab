using CagroLab.Models;

namespace CagroLab.ViewModel
{
    public class AccountListViewModel
    {
        public int? Account_Id { get; set; }
        public int? Lab_Id { get; set; }

        public List<Account>? Accounts { get; set; }


        public CreateAccountDto NewAccount { get; set; }
    }

    public class CreateAccountDto
    {
        public string? Username { get; set; }
        public string? Account_Password { get; set; }
        public string? Full_Name { get; set; }
    }
}
