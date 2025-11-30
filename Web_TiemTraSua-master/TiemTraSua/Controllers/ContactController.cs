using TiemTraSua.Data;
using TiemTraSua.Models;
using Microsoft.AspNetCore.Mvc;

namespace TiemTraSua.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(TbPhanHoi phanHoi)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.TbPhanHois.Add(phanHoi);
                    _context.SaveChanges();
                    TempData["Status"] = "success";
                    TempData["Message"] = "G?i thành công";
                    return RedirectToAction("index");
                }
                catch
                {
                    TempData["Status"] = "error";
                    TempData["Message"] = "Không g?i du?c l?i nh?n";
                }
            }  

            return View(phanHoi);
        }
    }
}
