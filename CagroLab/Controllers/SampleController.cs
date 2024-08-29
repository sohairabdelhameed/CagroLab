using CagroLab.Context;
using CagroLab.Models;
using CagroLab.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CagroLab.Controllers
{
    public class SampleController : Controller
    {
        private readonly CagroLabDbContext _dbContext;
        private readonly ILogger<SampleController> _logger;
        public SampleController(CagroLabDbContext dbContext, ILogger<SampleController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public IActionResult Details(int id)
        {
            var sample = _dbContext.Sample
                .Include(p => p.Account)
                .Include(p => p.Package)
                .FirstOrDefault(p => p.Id == id);

            if (sample == null)
            {
                return NotFound();
            }

            return View(sample);
        }

        [HttpGet]
        public async Task<IActionResult> SamplesByAccount(int id)
        {
            var samples = await _dbContext.Sample
                .Where(p => p.Account_Id == id)
                .Include(p => p.Account)
                .Include(p => p.Package)
                .ToListAsync();

            if (samples == null || !samples.Any())
            {
                return NotFound();
            }

            return View(samples);
        }

        public async Task<IActionResult> ListByPackage(int id)
        {//TODO: Ahmad
            var samples = await _dbContext.Sample
                .Where(p => (int)p.Package_Id == id)
                .Include(p => p.Account)
                .Include(p => p.Package)
                .ToListAsync();

            

            var viewModel = new SampleListViewModel()
            {
                Account_Id = HttpContext.Session.GetInt32("Account_Id"),
                Package_Id = id,
                Samples = samples

            };
            return View(viewModel);
        }



        public IActionResult Index()
        {
            //var labId = HttpContext.Session.GetInt32("Lab_Id");
            var accountId = HttpContext.Session.GetInt32("Account_Id");
            if (accountId == null)
            {
                return BadRequest("Account ID is required.");
            }

            var samples = _dbContext.Sample
                .Where(p => p.Account_Id == accountId)
                .ToList();

            var viewModel = new SampleListViewModel()
            {
                Samples = samples.ToList(),
                Account_Id = accountId,
            };

            return View(viewModel);
        }

        public IActionResult Create(int? packageId)
        {
            var accountId = HttpContext.Session.GetInt32("Account_Id");

            if (accountId == null)
            {
                _logger.LogWarning("Login session invalid.");
                return RedirectToAction("Login", "Account");
            }

            if (packageId == null)
            {
                //TODO: redirect to somewhere else
            }

            var sampleTypes = new List<String>() { "Blood", "Tissue", "Bone marrow"}; //TODO: Add a proper DB table for sample types
            //TODO: Add a list of patients instead of patient name being entered as text

            var viewModel = new SampleViewModel(){
                Account_Id = accountId.Value,
                Package_Id = packageId.Value,
                SampleTypes = sampleTypes,
            };


            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateSampleDto viewModel)
        {
            //TODO: IMPORTANT: make sample type into a select, not a text input

            _logger.LogInformation("Create sample request: {@ViewModel}", viewModel);
            var accountId = HttpContext.Session.GetInt32("Account_Id");
            if (ModelState.IsValid)
            {
                var labId = HttpContext.Session.GetInt32("Lab_Id");
                if (labId == null)
                {
                    _logger.LogWarning("Lab ID not found in session. Redirecting to login.");
                    return RedirectToAction("Login", "Account");
                }


                //TODO: Use automapper
                var sample = new Sample
                {
                    Account_Id = (int)viewModel.Account_Id!,
                    Package_Id = (int)viewModel.Package_Id,
                    Sample_Type_Id = (int)viewModel.Sample_Type_Id,
                    Patient_Name = viewModel.Patient_Name,
                    Patient_Phone = viewModel.Patient_Phone,
                };

                try
                {
                    _dbContext.Sample.Add(sample);
                    _dbContext.SaveChanges();
                    _logger.LogInformation("Sample created successfully with ID {SampleId}.", sample.Id);
                    return RedirectToAction(nameof(Index), new { accountId = viewModel.Account_Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating sample.");
                    ModelState.AddModelError("", "An error occurred while creating the sample.");
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


            //viewModel.Accounts = accounts;
            return View(viewModel);
        }

    }
}
