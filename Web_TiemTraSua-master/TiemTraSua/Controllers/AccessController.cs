using TiemTraSua.Data;
using TiemTraSua.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace TiemTraSua.Controllers
{
    public class AccessController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccessController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("TenNguoiDung") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "HomeAdmin");
            }
        }

        [HttpPost]
        public IActionResult Login(TbQuanTriVien user)
        {
            if (HttpContext.Session.GetString("TenNguoiDung") == null)
            {
                var u = _context.TbQuanTriViens
                    .Where(x => x.TenNguoiDung == user.TenNguoiDung
                             && x.MatKhau == user.MatKhau)   // so sánh tr?c ti?p
                    .FirstOrDefault();

                if (u != null)
                {
                    HttpContext.Session.SetString("TenNguoiDung", u.TenNguoiDung);
                    return RedirectToAction("Index", "HomeAdmin");
                }
            }
            ViewBag.Error = "Sai tên dang nh?p ho?c m?t kh?u!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("TenNguoiDung");

            return RedirectToAction("Login", "Access");
        }
    }
}
