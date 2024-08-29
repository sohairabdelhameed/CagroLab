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

            var account = _dbContext.Account.Include(a => a.Lab)
                .FirstOrDefault(a => (a.Username == model.Username) && a.Account_Password == model.Password);

            if (account == null)
            {
                model.Message = "Username and password do not match";
                return View(model);
            }

            HttpContext.Session.SetString("Username", account.Username);
            HttpContext.Session.SetInt32("Account_Id", account.Id);
            HttpContext.Session.SetInt32("Lab_Id", account.Lab_Id);

            // Set a boolean value as a string in session
            HttpContext.Session.SetString("IsLab", account.Main_Account.ToString());

            if (account.Main_Account == true)
            {
                return RedirectToAction("Index", "AccountsDetails", new { id = account.Lab_Id });
            }

            return RedirectToAction("Index", "Package", new { id = account.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Clear the session data
            HttpContext.Session.Clear();

            // Redirect to the login page or home page
            return RedirectToAction("Login");
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
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateAccountDto model)
        {
            if (ModelState.IsValid)
            {
                var lab = _dbContext.Lab.Find(HttpContext.Session.GetInt32("Lab_Id"));

                if (lab != null)
                {
                    var account = new Account
                    {
                        Username = model.Username,
                        Account_Password = model.Account_Password,
                        Full_Name = model.Full_Name,
                        //Is_Active = model.Is_Active,
                        //Main_Account = model.Main_Account,
                        Lab = lab,
                        Last_Activity = DateTime.Now,
                        Last_Login = DateTime.Now
                    };


                    _dbContext.Account.Add(account);
                    _dbContext.SaveChanges();

                    return RedirectToAction("Index", "AccountsDetails", new { id = lab.Id });
                }

                // If the lab is not found, return an error
                ModelState.AddModelError("", "Lab not found.");
            }

            return View(model);
        }
    }
}
