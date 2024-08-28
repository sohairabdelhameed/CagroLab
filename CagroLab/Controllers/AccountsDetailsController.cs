using CagroLab.Context;
using CagroLab.Models;
using CagroLab.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CagroLab.Controllers
{
    public class AccountsDetailsController : Controller
    {
        private readonly CagroLabDbContext _dbContext;

        private readonly ILogger<HomeController> _logger;

        public AccountsDetailsController(CagroLabDbContext dbContext, ILogger<HomeController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public IActionResult Details(int id)
        {
            var lab = _dbContext.Lab
                .FirstOrDefault(l => l.Id == id);

            if (lab == null)
            {
                return NotFound();
            }

            return View(lab);
        }
        public IActionResult ListsAccounts(int id)
        {
            var lab = _dbContext.Lab
                .Include(l => l.Accounts)
                .FirstOrDefault(l => l.Id == id);

            if (lab == null)
            {
                return NotFound();
            }

            return View(lab);
        }

        public IActionResult ViewAccounts()
        {
            // Retrieve the Lab_Id from the session
            var labId = HttpContext.Session.GetInt32("Lab_Id");

            if (labId == null)
            {
                // If Lab_Id is not in the session, redirect to login
                return RedirectToAction("Login", "CreatingAccounts");
            }

            // Get the lab and its accounts based on the Lab_Id from the session
            var lab = _dbContext.Lab
                .Include(l => l.Accounts)
                .FirstOrDefault(l => l.Id == labId);

            if (lab == null)
            {
                return NotFound();
            }

            return View(lab);
        }

        //public IActionResult Edit(int id)
        //{
        //    var account = _dbContext.Account.FirstOrDefault(a => a.Id == id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(account);
        //}


        public IActionResult Edit(int id)
        {
            var account = _dbContext.Account.FirstOrDefault(a => a.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            var accountViewModel = new AccountViewModel
            {
                Id = account.Id,
                Username = account.Username,
                Account_Password = account.Account_Password,
                Full_Name = account.Full_Name,
                Last_Activity = account.Last_Activity,
                Last_Login = account.Last_Login,
                Is_Active = account.Is_Active,
                Main_Account = account.Main_Account,
                Lab_Id = account.Lab_Id
            };

            return View(accountViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AccountViewModel accountViewModel)
        {
            var labId = HttpContext.Session.GetInt32("Lab_Id");

            if (labId == null)
            {
                return RedirectToAction("Login", "CreatingAccounts");
            }

            if (id != accountViewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var account = _dbContext.Account.FirstOrDefault(a => a.Id == id && a.Lab_Id == labId);

                if (account == null)
                {
                    return NotFound();
                }

                account.Username = accountViewModel.Username;
                account.Full_Name = accountViewModel.Full_Name;
                account.Last_Activity = accountViewModel.Last_Activity;
                account.Last_Login = accountViewModel.Last_Login;
                account.Is_Active = accountViewModel.Is_Active;
                account.Main_Account = accountViewModel.Main_Account;
                account.Last_Activity = accountViewModel.Last_Activity ?? DateTime.Now;
                account.Lab_Id = labId.Value;

                // If a new password was entered, update it
                if (!string.IsNullOrWhiteSpace(accountViewModel.Account_Password))
                {
                    account.Account_Password = accountViewModel.Account_Password;
                }

                try
                {
                    _dbContext.Update(account);
                    _dbContext.SaveChanges();
                    return RedirectToAction(nameof(ViewAccounts));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes. Please try again.");
                    _logger.LogError(ex, "Error occurred while saving changes to account with ID {id}", id);
                }
            }

            return View(accountViewModel);
        }


 


        public IActionResult Delete(int id)
        {
            var account = _dbContext.Account.FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var account = _dbContext.Account.FirstOrDefault(a => a.Id == id);
            if (account != null)
            {
                try
                {
                    // Set IsDeleted to true instead of removing the entity
                    account.IsDeleted = true;
                    _dbContext.Account.Update(account);
                    _dbContext.SaveChanges();
                    _logger.LogInformation("Account with ID {AccountId} was soft-deleted.", account.Id);
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Error updating account with ID {AccountId}.", account.Id);
                    ModelState.AddModelError("", "Unable to delete the account. Please try again.");
                    return View(account); // Return the view with the account if there's an error
                }
            }
            return RedirectToAction(nameof(ViewAccounts)); // Redirect to the list of accounts after deletion
        }


    }
}
