using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Blue.Models;
using System.Security.Cryptography;
using System.Text;

namespace Project_Blue.Controllers
{
    public class AdminController : Controller
    {
        ProjectBlueContext db = new ProjectBlueContext();
        public static string GetMd5Hash(string password)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
        public IActionResult Login(int? checkLogin)
        {
            var idAdmin = TempData.Peek("adminId") as int?;
            if (checkLogin == 0)
            {
                ViewBag.checkLogin = "You must Login!";
            }
            if (HttpContext.Session.Keys.Contains("adminId_" + idAdmin))
            {
                return RedirectToAction("Home", new { id = idAdmin, checkLogin = 1 });
            }
            Admin ttcn = new Admin();
            return View(ttcn);
        }
        [HttpPost]
        public IActionResult Login(Admin admin)
        {
            if(admin.Username == null || admin.Password == null)
            {
                return View();
            }
            var password = GetMd5Hash(admin.Password);
            var admins = db.Admins.FirstOrDefault(p => (p.Username == admin.Username
            || p.Gmail == admin.Username) && p.Password == password);
            if (admins == null)
            {
                ModelState.AddModelError(string.Empty, "Incorrect Account or Password!");
                return View();
            }
            if (HttpContext.Session.GetInt32("adminId_" + admins.Idadmin) != null)
            {
                HttpContext.Session.Remove("adminId_" + admins.Idadmin);
            }
            HttpContext.Session.SetInt32("adminId_" + admins.Idadmin, admins.Idadmin);
            ViewBag.idUser = admins.Idadmin;
            TempData["adminId"] = admins.Idadmin;
            return RedirectToAction("Home", new { id = admins.Idadmin, checkLogin = 1 });
        }


        [HttpPost]
        public IActionResult Register(Admin admin)
        {
            if (ModelState.IsValid)
            {
                var userGmail = db.Admins.FirstOrDefault(p => p.Gmail == admin.Gmail);
                if (userGmail != null)
                {
                    ModelState.AddModelError("Gmail", "Email đã tồn tại!");
                    return View();
                }
                var userName = db.Admins.FirstOrDefault(p => p.Username == admin.Username);
                if (userName != null)
                {
                    ModelState.AddModelError("UserName", "Tên tài khoản đã tồn tại!");
                    return View();
                }
                admin.Image = "/imagesUser/User.png";
                var password = GetMd5Hash(admin.Password);
                admin.Password = password;
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Home", "Admin", new { id = admin.Idadmin, checkLogin = 1 }) ;
            }
            return View();
        }
        public IActionResult Home(int? id,int? checkLogin)
        {
            if (HttpContext.Session.Keys.Contains("adminId_" + id))
            {
                var adminId = HttpContext.Session.GetInt32("adminId_" + id);
                if (checkLogin != null)
                {
                    if (adminId != 0)
                    {
                        var Admin = db.Admins.FirstOrDefault(p => p.Idadmin == adminId);
                        if (Admin != null)
                        {
                            ViewBag.Admin = Admin.Name;
                            ViewBag.AnhDaiDien = Admin.Image;
                        }
                        var admins = db.Admins
                            .Include(p => p.GenderNavigation)
                            .ToList();
                        Admin admin = new Admin();
                        ViewBag.IdinforAdmin = adminId;
                        var PostACmt = new PostACmt
                        {
                            Admin = admin,
                            Admins = admins,
                        };
                        return View(PostACmt);
                    }
                }
                return RedirectToAction("Login", new { checkLogin = 0 });
            }
            else
            {
                return RedirectToAction("Login", new { checkLogin = 0 });
            }
        }

        [HttpPost]
        public IActionResult DeleteAdmin(int id)
        {
            var admin = db.Admins.FirstOrDefault(p => p.Idadmin == id);
            if (admin != null)
            {
                db.Admins.Remove(admin);
                db.SaveChanges();
                return RedirectToAction("home");
            }
            return NotFound();
        }

        public IActionResult navAdmin()
        {
            var admins = db.Admins
                .Include(p => p.GenderNavigation)
                .ToList();
            Admin admin = new Admin(); 
            var PostACmt = new PostACmt
            {
                Admin = admin,
                Admins = admins,
            };
            return View(PostACmt);
        }

        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            var baiPost = db.BaiPosts.FirstOrDefault(p => p.MaBaiPost == id);
            if (baiPost != null)
            {
                db.BaiPosts.Remove(baiPost);
                db.SaveChanges();
                return RedirectToAction("home");
            }
            return NotFound();
        }
        
        public IActionResult navPost()
        {
            var baiPosts = db.BaiPosts
                .Include(p => p.MaNguoiPostNavigation)
                .ToList();
            return View(baiPosts);
        }
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == id);
            if (user != null)
            {
                db.ThongTinCaNhans.Remove(user);
                db.SaveChanges();
                return RedirectToAction("home");
            }
            return NotFound();
        }

        public IActionResult navUser()
        {
            var ttcn = db.ThongTinCaNhans
                .Include(p => p.TrangThaiNavigation)
                .Include(p => p.GioiTinhNavigation)
                .ToList();
            return View(ttcn);
        }
        public IActionResult Logout()
        {
            var adminId = TempData.Peek("adminId") as int?;
            HttpContext.Session.Remove("adminId_" + adminId);
            return RedirectToAction("Login");
        }

    }
}
