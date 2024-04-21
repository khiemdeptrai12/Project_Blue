using Azure.Messaging;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Project_Blue.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Project_Blue.Controllers
{
    public class LoginController : Controller
    {
        ProjectBlueContext db = new ProjectBlueContext();

        public static string GetMd5Hash(string password)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var s = data;

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
            var idUser = TempData.Peek("UserId") as int?;
            if (checkLogin == 0)
            {
                ViewBag.checkLogin = "You must Login!";
            }
            if (HttpContext.Session.Keys.Contains("userId_" + idUser))
            {
                return RedirectToAction("home","home", new {id = idUser , checkLogin=1});
            }
            ThongTinCaNhan ttcn = new ThongTinCaNhan();
            return View(ttcn);
        }
        [HttpPost]
        public IActionResult Login(ThongTinCaNhan thongTinCaNhan)
        {
            if(thongTinCaNhan.UserName == null || thongTinCaNhan.Password == null)
            {
                return View();
            }
            var password = GetMd5Hash(thongTinCaNhan.Password);
            var ttcn = db.ThongTinCaNhans.FirstOrDefault(p => (p.UserName == thongTinCaNhan.UserName 
            || p.Gmail == thongTinCaNhan.UserName) && p.Password == password);
            if (ttcn == null)
            {
                ModelState.AddModelError(string.Empty, "Incorrect Account or Password!");
                return View("Login");
            }
            if (HttpContext.Session.GetInt32("userId_" + ttcn.MaKhachHang) != null)
            {
                HttpContext.Session.Remove("userId_" + ttcn.MaKhachHang);
            }
            HttpContext.Session.SetInt32("userId_" + ttcn.MaKhachHang, ttcn.MaKhachHang);
            ViewBag.idUser = ttcn.MaKhachHang;
            TempData["UserId"] = ttcn.MaKhachHang;
            return RedirectToAction("Home","Home", new {id = ttcn.MaKhachHang, checkLogin = 1});
        }

        public IActionResult Logout()
        {
            var idUser = TempData.Peek("UserId") as int?;
            HttpContext.Session.Remove("userId_"+ idUser);
            return RedirectToAction("Login", "Login") ;
        }
        public IActionResult Register()
        {
            return View();
        }
        

        [HttpPost]
        public IActionResult Register(ThongTinCaNhan thongTinCaNhan)
        {
            if (ModelState.IsValid)
            {
                var userGmail = db.ThongTinCaNhans.FirstOrDefault(p => p.Gmail == thongTinCaNhan.Gmail);
                if (userGmail != null)
                {
                    ModelState.AddModelError("Gmail", "Email đã tồn tại!");
                    return View();
                }
                var userName = db.ThongTinCaNhans.FirstOrDefault(p => p.UserName == thongTinCaNhan.UserName);
                if (userName != null)
                {
                    ModelState.AddModelError("UserName", "Tên tài khoản đã tồn tại!");
                    return View();
                }
                thongTinCaNhan.AnhDaiDien = "/imagesUser/User.png";
                var password = GetMd5Hash(thongTinCaNhan.Password);
                thongTinCaNhan.Password = password;
                db.ThongTinCaNhans.Add(thongTinCaNhan);
                db.SaveChanges();
                return RedirectToAction("Login","Login");
            }
            return View();
        }
    }
}
