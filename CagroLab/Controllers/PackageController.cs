using CagroLab.Context;
using CagroLab.Models;
using CagroLab.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CagroLab.Controllers
{
    public class PackageController : Controller
    {
        private readonly CagroLabDbContext _dbContext;
        private readonly ILogger<PackageController> _logger;

        public PackageController(CagroLabDbContext dbContext, ILogger<PackageController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IActionResult Details(int id)
        {
            var package = _dbContext.Package
                .Include(p => p.Account)
                .Include(p => p.Lab)
                .FirstOrDefault(p => p.Id == id);

            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        [HttpGet]
        public async Task<IActionResult> PackagesByAccount(int id)
        {
            var packages = await _dbContext.Package
                .Where(p => p.Account_Id == id) 
                .Include(p => p.Account)
                .Include(p => p.Lab)
                .ToListAsync();

            if (packages == null || !packages.Any())
            {
                return NotFound();
            }

            return View(packages);
        }



        public IActionResult Index(int? accountId)
        {
            if (accountId == null)
            {
                return BadRequest("Account ID is required.");
            }

            var packages = _dbContext.Package
                .Where(p => p.Account_Id == accountId)
                .ToList();

            var viewModel = new PackageListViewModel()
            {
                Packages = packages.ToList(),
                Account_Id = accountId
            };
            //if (packages == null || !packages.Any())
            //{
            //    return NotFound("No packages found for the given account.");
            //}

            return View(viewModel);
        }

        public IActionResult Create(int? accountId)
        {
            var labId = HttpContext.Session.GetInt32("Lab_Id");
            if (labId == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if no Lab ID
            }

            var accounts = _dbContext.Account
                .Where(a => a.Lab_Id == labId)
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Username
                }).ToList();

            var viewModel = new PackageViewModel
            {
                Accounts = accounts,
                Lab_Id = labId.Value,
                Account_Id = accountId ?? 0 // Default to 0 if accountId is null
            };

            return View(viewModel);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PackageViewModel viewModel)
        {
            _logger.LogInformation("Create package request: {@ViewModel}", viewModel);

            if (ModelState.IsValid)
            {
                var labId = HttpContext.Session.GetInt32("Lab_Id");
                if (labId == null)
                {
                    _logger.LogWarning("Lab ID not found in session. Redirecting to login.");
                    return RedirectToAction("Login", "Account");
                }

                var package = new Package
                {
                    Package_Date = viewModel.Package_Date,
                    Title = viewModel.Title,
                    Package_Description = viewModel.Package_Description,
                    Account_Id = viewModel.Account_Id,
                    Lab_Id = (int)labId
                };

                try
                {
                    _dbContext.Add(package);
                    _dbContext.SaveChanges();
                    _logger.LogInformation("Package created successfully with ID {PackageId}.", package.Id);
                    return RedirectToAction(nameof(Index), new { accountId = viewModel.Account_Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating package.");
                    ModelState.AddModelError("", "An error occurred while creating the package.");
                }
            }
            else
            {
                // Log ModelState errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Model state error: {ErrorMessage}", error.ErrorMessage);
                }
            }

            // Repopulate the account list and return the view
            var accounts = _dbContext.Account
                .Where(a => a.Lab_Id == (int)HttpContext.Session.GetInt32("Lab_Id"))
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Username
                }).ToList();

            viewModel.Accounts = accounts;
            return View(viewModel);
        }





        public IActionResult Edit(int id)
        {
            var package = _dbContext.Package.Find(id);
            if (package == null)
            {
                return NotFound();
            }
            ViewData["Account_Id"] = new SelectList(_dbContext.Account, "Id", "Username", package.Account_Id);
            ViewData["Lab_Id"] = new SelectList(_dbContext.Lab, "Id", "Lab_Name", package.Lab_Id);
            return View(package);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Package_Date,Title,Package_Description,Account_Id,Lab_Id")] Package package)
        {
            if (id != package.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _dbContext.Update(package);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Account_Id"] = new SelectList(_dbContext.Account, "Id", "Username", package.Account_Id);
            ViewData["Lab_Id"] = new SelectList(_dbContext.Lab, "Id", "Lab_Name", package.Lab_Id);
            return View(package);
        }

        public IActionResult Delete(int id)
        {
            var package = _dbContext.Package
                .Include(p => p.Account)
                .Include(p => p.Lab)
                .FirstOrDefault(p => p.Id == id);

            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var package = _dbContext.Package.Find(id);
            if (package != null)
            {
                _dbContext.Package.Remove(package);
                _dbContext.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
