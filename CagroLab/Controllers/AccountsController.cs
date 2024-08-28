using CagroLab.Context;
using CagroLab.Models;
using CagroLab.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CagroLab.Controllers
{
    //Lab & Account Creation
    public class AccountsController : Controller
    {
        private readonly CagroLabDbContext _dbContext;

        private readonly ILogger<HomeController> _logger;

        public AccountsController(CagroLabDbContext dbContext, ILogger<HomeController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(LabRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault();
                ModelState.AddModelError(string.Empty, errorMessages);
                return View(model);
            }

            var lab = new Lab
            {
                Lab_Name = model.Lab_Name,
                Email = model.Email,
                Lab_Phone = model.Lab_Phone,
                Address = model.Address,
                Region = model.Region,
                City = model.City,
                Lab_Username = model.Lab_Username,
                Lab_Password = model.Lab_Password
            };

            var account = new Account
            {
                Username = model.Email,
                Account_Password = model.Lab_Password,
                Full_Name = model.Lab_Name,
                Is_Active = true,
                Main_Account = true,
                Lab = lab,
            };


            _dbContext.Lab.Add(lab);
            _dbContext.Account.Add(account);
            _dbContext.SaveChanges();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault();
                ModelState.AddModelError(string.Empty, errorMessages);
                return View(model);
            }

            // Check if the login is for a Lab
            var lab = _dbContext.Lab
                .FirstOrDefault(l => (l.Lab_Username == model.Username || l.Email == model.Username) && l.Lab_Password == model.Password);

            if (lab is null)
            {
                HttpContext.Session.SetInt32("Account_Id", account.Id);
                return RedirectToAction("ADetails", "AccountsDetails", new { id = account.Id });
            }

            // If neither Lab nor Account is found, return an error
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

            HttpContext.Session.SetInt32("Lab_Id", lab.Id);
            return RedirectToAction("Details", "AccountsDetails", new { id = lab.Id });
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Initialize the view model with the current LabId from the session
            var labId = HttpContext.Session.GetInt32("Lab_Id");
            if (labId.HasValue)
            {
                var viewModel = new AccountRegistrationViewModel
                {
                    Lab_Id = labId.Value
                };
                return View(viewModel);
            }

            // If no LabId is found in session, redirect to login
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Create(AccountRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lab = _dbContext.Lab.Find(model.Lab_Id);

                if (lab != null)
                {
                    var account = new Account
                    {
                        Username = model.Username,
                        Account_Password = model.Account_Password,
                        Full_Name = model.Full_Name,
                        Is_Active = model.Is_Active,
                        Main_Account = model.Main_Account,
                        Lab = lab,
                        Last_Activity = DateTime.Now,
                        Last_Login = DateTime.Now
                    };

                   
                    _dbContext.Account.Add(account);
                    _dbContext.SaveChanges();

                    return RedirectToAction("ListsAccounts", "AccountsDetails", new { id = lab.Id });
                }

                // If the lab is not found, return an error
                ModelState.AddModelError("", "Lab not found.");
            }

            return View(model);
        }
    }
}
