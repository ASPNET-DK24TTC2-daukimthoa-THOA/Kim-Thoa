using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TiemTraSua.Data;
using TiemTraSua.Models;
using TiemTraSua.Models.Authentication;
using X.PagedList;

namespace TiemTraSua.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Accounts")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        

        [Route("")]
        [Route("Index")]
        [Authentication]
        public IActionResult Index(int? page)
        {
            int pageSize = 30;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var listItem = _context.TbQuanTriViens.AsNoTracking().OrderBy(x => x.TenNguoiDung).ToList();
            PagedList<TbQuanTriVien> pagedListItem = new PagedList<TbQuanTriVien>(listItem, pageNumber, pageSize);

            return View(pagedListItem);
        }

        [Route("Create")]
        [Authentication]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [Authentication]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TbQuanTriVien quanTriVien)
        {
            _context.TbQuanTriViens.Add(quanTriVien);
            _context.SaveChanges();
            TempData["Message"] = "Thêm thành công";

            return RedirectToAction("Index", "Accounts");
        }

        [Route("Edit")]
        [Authentication]
        [HttpGet]
        public IActionResult Edit(int id, string name)
        {
            var quanTriVien = _context.TbQuanTriViens.Find(id);
            ViewBag.id = id;

            return View(quanTriVien);
        }

        [Route("Edit")]
        [Authentication]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TbQuanTriVien quanTriVien)
        {
            _context.Entry(quanTriVien).State = EntityState.Modified;
            _context.SaveChanges();

            TempData["Message"] = "S?a thành công";

            return RedirectToAction("Index", "Accounts");
        }

        [Route("Delete")]
        [Authentication]
        [HttpGet]
        public IActionResult Delete(string id)
        {
            TempData["Message"] = "";

            _context.Remove(_context.TbQuanTriViens.Find(id));
            _context.SaveChanges();

            TempData["Message"] = "Xoá thành công";

            return RedirectToAction("Index", "Accounts");
        }
    }
}
