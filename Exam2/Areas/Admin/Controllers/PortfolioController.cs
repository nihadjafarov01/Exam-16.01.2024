using Exam2.Contexts;
using Exam2.Models;
using Exam2.ViewModels.PortfolioVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam2.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class PortfolioController : Controller
    {
        Exam2DbContext _context {  get; }
        IWebHostEnvironment _env {  get; }
        public PortfolioController(Exam2DbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            List<PortfolioListItemVM> data = _context.Portfolios.Select(x => new PortfolioListItemVM
            {
                Description = x.Description,
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                Title = x.Title,
            }).ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PortfolioCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var filePath = Path.Combine(_env.WebRootPath, "portfolio_imgs", vm.ImageFile.FileName);
            var stream = new FileStream(filePath, FileMode.Create);

            await _context.Portfolios.AddAsync(new Portfolio
            {
                Description = vm.Description,
                ImageUrl = vm.ImageFile.FileName,
                Title = vm.Title,
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if(id == null ||  id <= 0)
            {
                return BadRequest();
            }
            Portfolio model = await _context.Portfolios.FindAsync(id);
            PortfolioUpdateVM vm = new PortfolioUpdateVM
            {
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Title = model.Title,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, PortfolioUpdateVM vm)
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Portfolio model = await _context.Portfolios.FindAsync(id);
            model.Description = vm.Description;
            model.ImageUrl = vm.ImageUrl;
            model.Title = vm.Title;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }
            Portfolio model = await _context.Portfolios.FindAsync(id);
            _context.Portfolios.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
