using CagroLab.Context;
using CagroLab.Models;
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

        public IActionResult Edit(int id)
        {
            var account = _dbContext.Account.FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Account account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _dbContext.Update(account);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
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
                _dbContext.Account.Remove(account);
                _dbContext.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
