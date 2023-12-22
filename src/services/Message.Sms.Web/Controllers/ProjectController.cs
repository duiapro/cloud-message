using Message.Sms.Web.Infrastructure.Tools;
using Message.Sms.Web.Models.ViewModel;
using Message.Sms.Web.Repositories;
using Message.Sms.Web.Repositories.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Message.Sms.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ProjectController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(string searchString, bool? status = true)
        {
            System.Linq.Expressions.Expression<Func<Project, bool>> filter = channel => true;
            if (!string.IsNullOrEmpty(searchString))
            {
                filter = project => project.ProjectName.Contains(searchString);
            }
            if (status.HasValue)
            {
                filter = project => project.Status == status;
            }
            ViewBag.SearchString = searchString ?? string.Empty;
            ViewBag.Status = status;
            ViewData["Data"] = await _dbContext.Projects.Where(filter).OrderByDescending(project => project.CreateTime).ToListAsync();

            return View();
        }

        public async Task<IActionResult> Create()
        {
            ViewData["ApiServiceProviderId"] = await _dbContext.ApiServiceProviders.ToListAsync();
            return View(new CreateProjectViewModel());
        }

        public async Task<IActionResult> Details(Guid keyId)
        {
            var projects = await _dbContext.Projects.FindAsync(keyId);

            return View();
        }

        public async Task<IActionResult> UpdatePartialView(Guid keyId)
        {
            ViewData["ApiServiceProviderId"] = await _dbContext.ApiServiceProviders.ToListAsync();
            return PartialView("./Component/_Update", await _dbContext.Channels.FindAsync(keyId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var apiServiceProviders = await _dbContext.ApiServiceProviders.FirstOrDefaultAsync(provider => provider.KeyId == model.ApiServiceProviderId);
                if (apiServiceProviders is null)
                {
                    ModelState.AddModelError(nameof(model.ApiServiceProviderId), "ApiServiceProviderId not exist.");
                }
                if (_dbContext.Projects.Any(provider => provider.ProjectName == model.ProjectName))
                {
                    ModelState.AddModelError(nameof(model.ProjectName), "projectName exist.");
                }
                if (model.Icon is null)
                {
                    ModelState.AddModelError(nameof(model.Icon), "Icon is not.");
                }
                if (ModelState.ErrorCount == 0)
                {
                    var icon = await UploadHelper.ImageAsync(model.Icon);
                    await _dbContext.Projects.AddAsync(new(model.ApiServiceProviderId, apiServiceProviders.Type, model.ProjectName, model.SubTitle, model.Sort,
                        model.Grade, icon, model.Description));
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["ApiServiceProviderId"] = await _dbContext.ApiServiceProviders.ToListAsync();
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task DeleteAsync(Guid keyId)
        {
            var data = await _dbContext.Projects.FindAsync(keyId);
            if (data != null)
            {
                _dbContext.Remove(data);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("KeyId does not exist.");
            }
        }
    }
}
