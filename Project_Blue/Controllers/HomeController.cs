using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics;
using Project_Blue.Models;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
namespace Project_Blue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ProjectBlueContext db = new ProjectBlueContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Home()
        {   
            if(IDInterface.checkUserId != 0)
            {
                var User = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == IDInterface.checkUserId);
                ViewBag.TenUser = User.TenKhachHang;
                ViewBag.AnhDaiDien = User.AnhDaiDien;
                var listBaiPost = db.BaiPosts.ToList();
                IDInterface.checkLogin = 1;
                var listCmt = db.BinhLuans.ToList();
                var PostACmt = new PostACmt
                {
                    BaiPostList = listBaiPost,
                    BinhLuanList = listCmt
                };
                return View(PostACmt);     
            }
            IDInterface.checkLogin = 0;
            return RedirectToAction("Login","Login");
        }


        [HttpPost]
        public async Task<IActionResult> BaiPost(BaiPost baiPost, IFormFile anhBaiPost )
        {
            var UserId = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == IDInterface.checkUserId);
   
                if (anhBaiPost != null)
                {
                    baiPost.AnhBaiPost = await SaveImage(anhBaiPost);
                }
            
                baiPost.MaNguoiPost = UserId.MaKhachHang;
                baiPost.AnhNguoiPost = UserId.AnhDaiDien;
                baiPost.TenNguoiPost = UserId.TenKhachHang;
                db.BaiPosts.Add(baiPost);
                db.SaveChanges();
                return RedirectToAction("Home","Home");
     
        }
        [HttpPost]
        public IActionResult PostCmt(int maBaiPost, string noiDungCmt)
        {
            BinhLuan binhLuan = new BinhLuan();
            var UserCmt = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == IDInterface.checkUserId);
            if (UserCmt != null)
            {
                binhLuan.AnhNguoiCmt = UserCmt.AnhDaiDien;
                binhLuan.TenNguoiCmt = UserCmt.TenKhachHang;
            }
            binhLuan.MaBaiPost = maBaiPost;
            binhLuan.NoiDungCmt = noiDungCmt;
            db.BinhLuans.Add(binhLuan);
            db.SaveChanges();
            IDInterface.maBaiPost = maBaiPost;
            return RedirectToAction("LoadCmt");
        }
        [HttpGet]
        public IActionResult LoadCmt()
        {
            ViewBag.maBaiPost = IDInterface.maBaiPost;
            var loadCmt = db.BinhLuans.ToList();
            return View(loadCmt);
        }
        public async Task<string> SaveImage(IFormFile anhBaiPost)
        {
            var savePath = Path.Combine("wwwroot/Images", anhBaiPost.FileName); 
                using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await anhBaiPost.CopyToAsync(fileStream);
            }
            return "/Images/" + anhBaiPost.FileName; 
        }
        public IActionResult PostStatus()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
