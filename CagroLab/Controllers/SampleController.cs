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
        {
            var samples = await _dbContext.Sample
                .Where(p => p.Package_Id == id)
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
                return RedirectToAction("Login", "Accounts");
            }

            if (packageId == null)
            {
                //TODO: redirect to somewhere else
            }

            var sampleTypes = new List<String>() { "Blood", "Tissue", "Bone marrow"};

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
            _logger.LogInformation("Create sample request: {@ViewModel}", viewModel);

            var accountId = HttpContext.Session.GetInt32("Account_Id");
            if (ModelState.IsValid)
            {

                // Create a new Sample object
                var sample = new Sample();

                sample.Account_Id = (int)viewModel.Account_Id!;
                sample.Package_Id = (int)viewModel.Package_Id;
                sample.Sample_Type = $"[{viewModel.Sample_Type_Id}] - [{viewModel.Concatenated_SubSample_Types}]";
                sample.Patient_Name = viewModel.Patient_Name;
                sample.Patient_Phone = viewModel.Patient_Phone;
                sample.Status = viewModel.Status;
                

                try
                {
                    _dbContext.Sample.Add(sample);
                    _dbContext.SaveChanges();
                    _logger.LogInformation("Sample created successfully with ID {SampleId}.", sample.Id);
                    return RedirectToAction(nameof(ListByPackage), new { id = sample.Package_Id });
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

            return RedirectToAction(nameof(Create), new { packageId = viewModel.Account_Id});
      
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(SampleViewModel viewModel)
        {
            var sample = await _dbContext.Sample.FirstOrDefaultAsync(p => p.Id == viewModel.Id);
            try
            {
                sample.Status = viewModel.Status;
                _dbContext.Update(sample);
                _dbContext.SaveChanges();
                _logger.LogInformation("Sample updated successfully with ID {SampleId}.", sample.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sample.");
                ModelState.AddModelError("", "An error occurred while updating the sample.");
            }
            if (sample != null)
            {
                return RedirectToAction("ListByPackage", new { id = sample.Package_Id });
            }
            else
            {
                return RedirectToAction("Index", "Package");
            }
        }

    }
}
