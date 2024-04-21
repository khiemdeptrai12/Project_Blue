using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics;
using Project_Blue.Models;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
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
        public IActionResult Home(int? id,int? checkLogin)
        {
            if (HttpContext.Session.Keys.Contains("userId_"+ id))
            {
                var UserId = HttpContext.Session.GetInt32("userId_" + id);
                if (checkLogin != null)
                {
                    if(UserId != 0)
                    {
                        var User = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == UserId);
                        if(User != null)
                        {
                            ViewBag.TenUser = User.TenKhachHang;
                            ViewBag.AnhDaiDien = User.AnhDaiDien;
                        }
                        var listBaiPost = db.BaiPosts
                            .Include(p => p.MaNguoiPostNavigation)
                            .OrderByDescending(x => x.MaBaiPost)
                            .ToList();
                        var listCmt = db.BinhLuans
                            .Include(p => p.IdUserCmtNavigation)
                            .ToList();
                        var listTTCN = db.ThongTinCaNhans.ToList(); 
                        var listBanbe = db.BanBes
                            .Include(p => p.IdUser1Navigation)
                            .Include(p => p.IdUser2Navigation)
                            .ToList();
                        var reactionPosts = db.ReactionPosts.ToList();
                        BanBe banBe = new BanBe();
                        RoomChat roomChat = new RoomChat();
                        BinhLuan binhLuan = new BinhLuan();
                        ReactionPost reactionPost = new ReactionPost();
                        ViewBag.IdinforUser = UserId;
                        ViewBag.IdUser = UserId;

                        var PostACmt = new PostACmt
                        {
                            BaiPostList = listBaiPost,
                            BinhLuanList = listCmt,
                            thongTinCaNhans = listTTCN,
                            reactionPosts = reactionPosts,
                            BanBes = listBanbe,
                            BanBe = banBe,
                            RoomChat = roomChat,
                            binhLuan = binhLuan,
                            ReactionPost = reactionPost,
                        };
                        return View(PostACmt);     
                    }
                }
            }
            return RedirectToAction("Login", "Login", new { checkLogin  = 0 });
        }

        [HttpPost]
        public IActionResult SeachUser(string? NameSearch, int idUser)
        {

            if (NameSearch == null)
            {
                NameSearch = "empty";
            }
            var _NameSearch = Convert.ToString(NameSearch);
            HttpContext.Session.SetString("NameSearch_" + idUser, _NameSearch);
            return RedirectToAction("GetSearchUser");
        }

        [HttpGet]
        public IActionResult GetSearchUser(int idUser)
        {
            ViewBag.idUser = idUser;
            ViewBag.NameSearch = HttpContext.Session.GetString("NameSearch_" + idUser);
            var User = db.ThongTinCaNhans.ToList();
            return View(User);
        }
        public IActionResult PostStatus(int idUser)
        {
            var UserId = HttpContext.Session.GetInt32("userId_" + idUser);
            var User = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == UserId);
            if (User != null)
            {
                ViewBag.IdinforUser = User.MaKhachHang;
                ViewBag.TenUser = User.TenKhachHang;
                ViewBag.AnhDaiDien = User.AnhDaiDien;
            }
            ViewBag.idUser = UserId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BaiPost(BaiPost baiPost, IFormFile anhBaiPost)
        {
   
                if (anhBaiPost != null)
                {
                    baiPost.AnhBaiPost = await SaveImage(anhBaiPost);
                }
            
                db.BaiPosts.Add(baiPost);
                db.SaveChanges();
                return RedirectToAction("Home","Home",new { id = baiPost.MaNguoiPost, checkLogin=1 });
     
        }
        public async Task<string> SaveImage(IFormFile anhBaiPost)
        {
            var savePath = Path.Combine("wwwroot/imagesPost", anhBaiPost.FileName); 
                using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await anhBaiPost.CopyToAsync(fileStream);
            }
            return "/imagesPost/" + anhBaiPost.FileName; 
        }

        public IActionResult DeletePost(int maBaiPost, int idUser)
        {
            var baiPost = db.BaiPosts.FirstOrDefault(p => p.MaBaiPost == maBaiPost);
            if (baiPost != null)
            {
                db.BaiPosts.Remove(baiPost);
                db.SaveChanges();
                return RedirectToAction("home",new { id = idUser, checkLogin = 1 });
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult ReactionPost(ReactionPost reactionPost)
        {
            var reaction = db.ReactionPosts.FirstOrDefault(p => p.IdPost == reactionPost.IdPost && p.IdUser == reactionPost.IdUser);
            if(reaction != null)
            {
                db.ReactionPosts.Remove(reaction);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                db.ReactionPosts.Add(reactionPost);
                db.SaveChanges();
                return Ok();
            }
        }

        [HttpGet]
        public IActionResult LoadLike(int postId)
        {
            ViewBag.postId = postId;
            var baiPosts = db.BaiPosts.ToList();
            var react = db.ReactionPosts.ToList();
            var idUser = TempData.Peek("UserId") as int?;
            ViewBag.IdUser = idUser;
            ReactionPost reactionPost = new ReactionPost();
            PostACmt postACmt = new PostACmt
            {
                BaiPostList = baiPosts,
                reactionPosts = react,
                ReactionPost = reactionPost
            };
            return View(postACmt);
        }

        [HttpPost]
        public IActionResult PostCmt(BinhLuan binhLuan)
        {
            TempData["UserId"] = binhLuan.IdUserCmt;
            HttpContext.Session.SetInt32("PostId_"+ binhLuan.IdUserCmt, binhLuan.MaBaiPost);
            db.BinhLuans.Add(binhLuan);
            db.SaveChanges();
            return RedirectToAction("LoadCmt");
            
        }
        [HttpGet]
        public IActionResult LoadCmt()
        {
            var idUser = TempData.Peek("UserId") as int?;
            var maBaiPost = HttpContext.Session.GetInt32("PostId_"+ idUser);
            if (maBaiPost != null){
                var UserCmt = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == idUser);
                if (UserCmt != null)
                {
                    ViewBag.AnhUser = UserCmt.AnhDaiDien;
                }
                ViewBag.maBaiPost = maBaiPost;
                ViewBag.idUser = idUser;
                var baiPostList = db.BaiPosts.ToList();
                var loadCmt = db.BinhLuans
                    .Include(p => p.IdUserCmtNavigation)
                    .ToList();
                BinhLuan binhLuan = new BinhLuan();
                PostACmt postACmt = new PostACmt
                {
                    binhLuan = binhLuan,
                    BinhLuanList = loadCmt,
                    BaiPostList = baiPostList
                };
                return View(postACmt);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult DeleteCmt(int id, int idCmt)
        {
            var Cmt = db.BinhLuans.FirstOrDefault(p => p.MaCmt == idCmt);
            if (Cmt != null)
            {
                HttpContext.Session.SetInt32("PostId_" + id, Cmt.MaBaiPost);
                db.BinhLuans.Remove(Cmt);
                db.SaveChanges();
                return RedirectToAction("Home", new {id = id, checkLogin = 1});
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult RoomChat(RoomChat roomChat)
        {
            var room = db.RoomChats.FirstOrDefault(p => (p.MaUser1 == roomChat.MaUser1 && p.MaUser2 == roomChat.MaUser2)
            || (p.MaUser1 == roomChat.MaUser2 && p.MaUser2 == roomChat.MaUser1));
            if (room == null)
            {
                TempData["UserId"] = roomChat.MaUser1;
                TempData["UserId2"] = roomChat.MaUser2;

                db.RoomChats.Add(roomChat);
                db.SaveChanges();
                int roomID = roomChat.MaPhong;
                Message message = new Message
                {
                    IdRoomchat = roomID,
                };
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("DisplayRoomChat");
            }
            else
            {
                TempData["UserId"] = roomChat.MaUser1;
                TempData["UserId2"] = roomChat.MaUser2;
                return RedirectToAction("DisplayRoomChat");
            }
        }

        [HttpGet]
        public IActionResult DisplayRoomChat()
        {
            var idUser = TempData.Peek("UserId") as int?;
            var idUser2 = TempData.Peek("UserId2") as int?;
            var roomchat = db.RoomChats
                .Include(p => p.MaUser1Navigation)
                .Include(p => p.MaUser2Navigation)
                .ToList();
            var messages = db.Messages.ToList();
            Message message = new Message();
            PostACmt postACmt = new PostACmt
            {
                roomChats = roomchat,
                Message = message,
                messages = messages,
            };
            ViewBag.Iduser = idUser2;
            ViewBag.MyId = idUser;
            return View(postACmt);
        }

        [HttpPost]
        public IActionResult Message(Message message)
        {
            if (message.NoiDung != null)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("home");
            }
            return RedirectToAction("home");
        }

        [HttpGet]
        public IActionResult LoadMessage()
        {
            var idUser = TempData.Peek("UserId") as int?;
            var idUser2 = TempData.Peek("UserId2") as int?;
            var roomchat = db.RoomChats
                .Include(p => p.MaUser1Navigation)
                .Include(p => p.MaUser2Navigation)
                .ToList();
            var messages = db.Messages.ToList();
            Message message = new Message();
            PostACmt postACmt = new PostACmt
            {
                roomChats = roomchat,
                Message = message,
                messages = messages,
            };
            ViewBag.Iduser = idUser2;
            ViewBag.MyId = idUser;
            return View(postACmt);
        }
        [HttpPost]
        public IActionResult AddFriend(BanBe banBe)
        {
            var idUser = TempData.Peek("UserId") as int?;
            banBe.TrangThai = 2;
            db.BanBes.Add(banBe);
            db.SaveChanges();
            return RedirectToAction("home", new { id = idUser, checkLogin = 1 });
        }

        [HttpPost]
        public IActionResult Accept(BanBe banBe)
        {
            var madeFriend = db.BanBes.FirstOrDefault(p => p.IdUser1 == banBe.IdUser1 && p.IdUser2 == banBe.IdUser2);
            if (madeFriend != null)
            {
                madeFriend.TrangThai = 1;
                madeFriend.IdUser1 = banBe.IdUser1;
                madeFriend.IdUser2 = banBe.IdUser2;
                db.BanBes.Update(madeFriend);
                db.SaveChanges();
            }
            var idUser = TempData.Peek("UserId") as int?;
            return RedirectToAction("home", new { id = idUser, checkLogin = 1 });
        }

        [HttpPost]
        public IActionResult Refuse(BanBe banBe)
        {
            var madeFriend = db.BanBes.FirstOrDefault(p => p.IdUser1 == banBe.IdUser1 && p.IdUser2 == banBe.IdUser2);
            if (madeFriend != null)
            {
                db.BanBes.Remove(madeFriend);
                db.SaveChanges();
            }
            var idUser = TempData.Peek("UserId") as int?;
            return RedirectToAction("home", new { id = idUser, checkLogin = 1 });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
