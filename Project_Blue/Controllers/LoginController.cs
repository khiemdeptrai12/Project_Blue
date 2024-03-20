﻿using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Project_Blue.Models;

namespace Project_Blue.Controllers
{
    public class LoginController : Controller
    {
        ProjectBlueContext db = new ProjectBlueContext();
        

        public IActionResult Login()
        {
            IDInterface.checkUserId = 0;
            if(IDInterface.checkLogin == 0)
            {
                ViewBag.checkLogin = "You must Login!";
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(ThongTinCaNhan thongTinCaNhan)
        {
            var ttcn = db.ThongTinCaNhans.FirstOrDefault(p => p.UserName == thongTinCaNhan.UserName && p.Password == thongTinCaNhan.Password);
            if (ttcn == null)
            {
                ModelState.AddModelError(string.Empty, "Incorrect Account or Password!");
                return View("Login");
            }
            IDInterface.checkUserId = ttcn.MaKhachHang;
            return RedirectToAction("Home","Home");
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
                if (thongTinCaNhan.AnhDaiDien == null)
                {
                    thongTinCaNhan.AnhDaiDien = "/Images/User.png";
                }
                db.ThongTinCaNhans.Add(thongTinCaNhan);
                db.SaveChanges();
                return RedirectToAction("Login","Login");
            }
            return View();
        }
    }
}
