using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TiemTraSua.Data;
using TiemTraSua.Models;
using TiemTraSua.Models.Authentication;
using TiemTraSua.ViewModels;
using X.PagedList;

namespace TiemTraSua.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("HomeAdmin")]
    public class HomeAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        IWebHostEnvironment hostEnvironment;

        public HomeAdminController(ApplicationDbContext context, IWebHostEnvironment hc)
        {
            _context = context;
            hostEnvironment = hc;
        }

        [Route("")]
        [Authentication]
        public IActionResult Index(int? page)
        {
            int pageSize = 30;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            var listItem = (from product in _context.TbSanPhams
                            join type in _context.TbNhomSanPhams on product.MaNhomSp equals type.MaNhomSp
                            orderby product.MaSanPham
                            select new ProductViewModel
                            {
                                MaSanPham = product.MaSanPham,
                                TenSanPham = product.TenSanPham,
                                GiaBan = product.GiaBan,
                                MoTa = product.MoTa,
                                HinhAnh = product.HinhAnh,
                                GhiChu = product.GhiChu,
                                LoaiSanPham = type.TenNhomSp
                            }).ToList();

            PagedList<ProductViewModel> pagedListItem = new PagedList<ProductViewModel>(listItem, pageNumber, pageSize);

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

            var listItem = _context.TbSanPhams.AsNoTracking().Where(x => x.TenSanPham.ToLower().Contains(search)).OrderBy(x => x.MaSanPham).ToList();
            PagedList<TbSanPham> pagedListItem = new PagedList<TbSanPham>(listItem, pageNumber, pageSize);

            return View(pagedListItem);
        }

        [Route("Create")]
        [Authentication]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.MaNhomSp = new SelectList(_context.TbNhomSanPhams.ToList(), "MaNhomSp", "TenNhomSp");
            return View();
        }

        [Route("Create")]
        [Authentication]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TbSanPham sanPham, IFormFile imageFile)
        {
            if (sanPham == null)
            {
                TempData["Message"] = "D? li?u s?n ph?m không h?p l?";
                return RedirectToAction("Index", "HomeAdmin");
            }
            string fileName = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                // T?o thu m?c n?u chua t?n t?i
                string uploadFolder = Path.Combine(hostEnvironment.WebRootPath, "img", "products");
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
                sanPham.HinhAnh = fileName;
            }
            // Thêm s?n ph?m vào database
            _context.TbSanPhams.Add(sanPham);
            _context.SaveChanges();
            TempData["Message"] = "Thêm s?n ph?m thành công";
            return RedirectToAction("Index", "HomeAdmin");
        }


        [Route("Details")]
        [Authentication]
        [HttpGet]
        public IActionResult Details(int id, string name)
        {
            var productItem = (from product in _context.TbSanPhams
                            join type in _context.TbNhomSanPhams on product.MaNhomSp equals type.MaNhomSp
                            where product.MaSanPham == id
                            select new ProductViewModel
                            {
                                MaSanPham = product.MaSanPham,
                                TenSanPham = product.TenSanPham,
                                GiaBan = product.GiaBan,
                                MoTa = product.MoTa,
                                HinhAnh = product.HinhAnh,
                                GhiChu = product.GhiChu,
                                LoaiSanPham = type.TenNhomSp
                            }).SingleOrDefault();

            ViewBag.name = name;

            return View(productItem);
        }

        [Route("Edit")]
        [Authentication]
        [HttpGet]
        public IActionResult Edit(int id, string name)
        {
            var sanPham = _context.TbSanPhams.Find(id);

            ViewBag.MaNhomSp = new SelectList(_context.TbNhomSanPhams.ToList(), "MaNhomSp", "TenNhomSp");
            ViewBag.name = name;

            return View(sanPham);
        }

        [Route("Edit")]
        [Authentication]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TbSanPham sanPham, IFormFile imageFile)
        {
            var product = _context.TbSanPhams.Find(sanPham.MaSanPham);
            if (product == null)
                return NotFound();

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadFolder = Path.Combine(hostEnvironment.WebRootPath, "img", "products");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                string filePath = Path.Combine(uploadFolder, fileName);

                imageFile.CopyTo(new FileStream(filePath, FileMode.Create));

                product.HinhAnh = fileName; // c?p nh?t hình m?i
            }

            // C?p nh?t các tru?ng khác
            product.TenSanPham = sanPham.TenSanPham;
            product.GiaBan = sanPham.GiaBan;
            product.MoTa = sanPham.MoTa;
            product.GhiChu = sanPham.GhiChu;
            product.MaNhomSp = sanPham.MaNhomSp;

            _context.SaveChanges();

            TempData["Message"] = "S?a s?n ph?m thành công";
            return RedirectToAction("Index");
        }

        [Route("Delete")]
        [Authentication]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            TempData["Message"] = "";
            var chiTietHoaDon = _context.TbChiTietHoaDonBans.Where(x => x.MaSanPham == id).ToList();

            if (chiTietHoaDon.Count() > 0)
            {
                TempData["Message"] = "Không xoá du?c s?n ph?m";

                return RedirectToAction("Index", "HomeAdmin");
            }

            _context.Remove(_context.TbSanPhams.Find(id));
            _context.SaveChanges();

            TempData["Message"] = "S?n ph?m dã du?c xoá";

            return RedirectToAction("Index", "HomeAdmin");
        }
    }
}
