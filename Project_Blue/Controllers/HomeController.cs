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
            var baiPost = db.BaiPosts.ToList();
            return View(baiPost);              
        }
        [HttpPost]
        public async Task<IActionResult> BaiPost(BaiPost baiPost, IFormFile anhBaiPost )
        {
            var UserId = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == CheckIDUser.check);
   
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
