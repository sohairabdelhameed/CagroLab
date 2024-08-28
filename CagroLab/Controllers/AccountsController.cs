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
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(LabRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
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

                _dbContext.Lab.Add(lab);
                _dbContext.SaveChanges();

                // Redirect to login or another page after successful registration
                return RedirectToAction("Login");
            }

            // If the model is invalid, return the view with the model to display validation errors
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lab = _dbContext.Lab
                    .FirstOrDefault(l => l.Lab_Username == model.Username && l.Lab_Password == model.Password);

                if (lab != null)
                {
                    // Store the LabId in session for later use
                    HttpContext.Session.SetInt32("Lab_Id", lab.Id);


                    return RedirectToAction("Details", "AccountsDetails", new { id = lab.Id });
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View(model);
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
