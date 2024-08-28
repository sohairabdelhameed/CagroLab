using System.ComponentModel.DataAnnotations;

namespace CagroLab.ViewModel
{
    public class AccountRegistrationViewModel
    {


        
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Account_Password { get; set; }

            [Required]
            [Display(Name = "Full Name")]
            public string Full_Name { get; set; }

            [Required]
            [Display(Name = "Is Active")]
            public bool Is_Active { get; set; }

            [Required]
            [Display(Name = "Main Account")]
            public bool Main_Account { get; set; }



            // Hidden field for Lab Id
            public int Lab_Id { get; set; }
        }


    
}
