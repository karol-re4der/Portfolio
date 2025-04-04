using Microsoft.AspNetCore.Mvc;
using Portfolio.Data;
using Portfolio.Models;
using System.Diagnostics;

namespace Portfolio.Controllers
{
    public class SectionController : Controller
    {
        private readonly ILogger<SectionController> _logger;
        private readonly ApplicationDbContext _db;

        public SectionController(ILogger<SectionController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Albums(string section)
        {
            Section model = _db.Section.FirstOrDefault(x => x.SectionName.Equals(section));

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
