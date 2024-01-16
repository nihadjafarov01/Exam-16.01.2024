using Exam2.Contexts;
using Exam2.ViewModels.PortfolioVMs;
using Microsoft.AspNetCore.Mvc;

namespace Exam2.Controllers
{
    public class HomeController : Controller
    {
        Exam2DbContext _context {  get; }

        public HomeController(Exam2DbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
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
    }
}