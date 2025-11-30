using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TiemTraSua.Data;
using TiemTraSua.Models;
using TiemTraSua.Models.Authentication;
using X.PagedList;

namespace TiemTraSua.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/News")]
    public class NewsManageController : Controller
    {
        private readonly ApplicationDbContext _context;
        IWebHostEnvironment hostEnvironment;

        public NewsManageController(ApplicationDbContext context, IWebHostEnvironment hc)
        {
            _context = context;
            hostEnvironment = hc;

        }

        [Route("")]
        [Route("Index")]
        [Authentication]
        public IActionResult Index(int? page)
        {
            int pageSize = 30;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var listItem = _context.TbTinTucs.AsNoTracking().OrderBy(x => x.MaTinTuc).ToList();
            PagedList<TbTinTuc> pagedListItem = new PagedList<TbTinTuc>(listItem, pageNumber, pageSize);

            return View(pagedListItem);
        }

        [Route("Search")]
        [Authentication]
        [HttpGet]
        public IActionResult Search(int? page, string search)
        {
            int pageSize = 30;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            search = search.ToLower();
            ViewBag.search = search;

            var listItem = _context.TbTinTucs.AsNoTracking().Where(x => x.TieuDe.ToLower().Contains(search)).OrderBy(x => x.MaTinTuc).ToList();
            PagedList<TbTinTuc> pagedListItem = new PagedList<TbTinTuc>(listItem, pageNumber, pageSize);

            return View(pagedListItem);
        }

        [Route("Create")]
        [Authentication]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.NguoiDang = new SelectList(_context.TbQuanTriViens.ToList(), "TenNguoiDung", "TenNguoiDung");

            return View();
        }

        [Route("Create")]
        [Authentication]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TbTinTuc tinTuc, IFormFile imageFile)
        {
            string fileName = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                // T?o thu m?c n?u chua t?n t?i
                string uploadFolder = Path.Combine(hostEnvironment.WebRootPath, "img", "news");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                // T?o tên file duy nh?t
                fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
            }
            // Gán tên file ?nh vào s?n ph?m
            if (fileName != null)
            {
                tinTuc.HinhAnh = fileName;
            }
            _context.TbTinTucs.Add(tinTuc);
            _context.SaveChanges();
            TempData["Message"] = "Thêm thành công";
            return RedirectToAction("Index", "NewsManage");
        }

        [Route("Details")]
        [Authentication]
        [HttpGet]
        public IActionResult Details(int id, string name)
        {
            var tinTuc = _context.TbTinTucs.SingleOrDefault(x => x.MaTinTuc == id);
            ViewBag.name = name;
            return View(tinTuc);
        }

        [Route("Edit")]
        [Authentication]
        [HttpGet]
        public IActionResult Edit(int id, string name)
        {
            var tinTuc = _context.TbTinTucs.Find(id);
            ViewBag.NguoiDang = new SelectList(_context.TbQuanTriViens.ToList(), "TenNguoiDung", "TenNguoiDung");
            ViewBag.name = name;
            return View(tinTuc);
        }

        [Route("Edit")]
        [Authentication]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TbTinTuc tinTuc, IFormFile imageFile)
        {
            var product = _context.TbTinTucs.Find(tinTuc.MaTinTuc);
            if (product == null)
                return NotFound();

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadFolder = Path.Combine(hostEnvironment.WebRootPath, "img", "news");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                string filePath = Path.Combine(uploadFolder, fileName);

                imageFile.CopyTo(new FileStream(filePath, FileMode.Create));

                product.HinhAnh = fileName; // c?p nh?t hình m?i
            }
            _context.SaveChanges();
            TempData["Message"] = "S?a thành công";
            return RedirectToAction("Index", "NewsManage");
        }

        [Route("Delete")]
        [Authentication]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            TempData["Message"] = "";

            _context.Remove(_context.TbTinTucs.Find(id));
            _context.SaveChanges();

            TempData["Message"] = "Xoá thành công";

            return RedirectToAction("Index", "NewsManage");
        }
    }
}
