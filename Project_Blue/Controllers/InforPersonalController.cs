﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_Blue.Models;
using System.Text.RegularExpressions;

namespace Project_Blue.Controllers
{
    public class InforPersonalController : Controller
    {
        ProjectBlueContext db = new ProjectBlueContext();

        public IActionResult Index(int id, int? checkLogin, int idUser)
        {
            if (checkLogin != null)
            {
                var User = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == idUser);
                var UserSearch = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == id);
                TempData["UserId"] = idUser;
                TempData["id"] = id;
                ViewBag.TenUser = User.TenKhachHang;
                ViewBag.AnhDaiDien = User.AnhDaiDien;
                ViewBag.TenUserSeach = UserSearch.TenKhachHang;
                var listBaiPost = db.BaiPosts
                    .Include(p => p.MaNguoiPostNavigation)
                    .OrderByDescending(x => x.MaBaiPost)
                    .ToList();
                var listCmt = db.BinhLuans.ToList();
                var listTTCN = db.ThongTinCaNhans
                    .Include(p => p.TrangThaiNavigation)
                    .Include(p => p.GioiTinhNavigation)
                    .ToList();
                var listBanbe = db.BanBes
                    .Include(p => p.IdUser1Navigation)
                    .Include (p => p.IdUser2Navigation)
                    .ToList();
                var reactionPosts = db.ReactionPosts.ToList();
                ReactionPost reactionPost = new ReactionPost();
                ViewBag.IdinforUser = idUser;
                ViewBag.IdUser = id;
                BaiPost baiPost = new BaiPost();
                BanBe banBe = new BanBe();
                RoomChat roomChat = new RoomChat();
                BinhLuan binhLuan = new BinhLuan();
                var PostACmt = new PostACmt
                {
                    BaiPostList = listBaiPost,
                    BinhLuanList = listCmt,
                    thongTinCaNhans = listTTCN,
                    BanBes = listBanbe,
                    reactionPosts = reactionPosts,
                    BaiPost = baiPost,
                    BanBe = banBe,
                    RoomChat = roomChat,
                    binhLuan = binhLuan,
                    ReactionPost = reactionPost,
                };
                return View(PostACmt);
            }
            return RedirectToAction("Login", "Login", new { checkLogin = 0 });
        }
        public IActionResult updateInfor(int? checkLogin, int idUser)
        {
            if (checkLogin != null)
            {
                var User = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == idUser);
                ViewBag.TenUser = User.TenKhachHang;
                ViewBag.AnhDaiDien = User.AnhDaiDien;
                ViewBag.IdinforUser = idUser;
                var trangthai = db.TrangThais.ToList();
                var gioitinh = db.GioiTinhs.ToList();
                ViewBag.TrangThai = new SelectList(trangthai, "IdTrangThai", "TrangThai1", User.TrangThai);
                ViewBag.GioiTinh = new SelectList(gioitinh, "IdGioiTinh", "GioiTinh1", User.GioiTinh);
                BanBe banBe = new BanBe();
                var thongTinCaNhans = db.ThongTinCaNhans.ToList();
                var banBes = db.BanBes
                    .Include(p => p.IdUser1Navigation)
                    .Include(p => p.IdUser2Navigation)
                    .ToList();
                var Users = new PostACmt
                {
                    ThongTinCaNhan = User,
                    BanBe = banBe,
                    thongTinCaNhans = thongTinCaNhans,
                    BanBes = banBes
                };
                return View(Users);
            }
            return RedirectToAction("Login", "Login", new { checkLogin = 0 } );
        }
        public bool IsNumeric(string str)
        {
            return Regex.IsMatch(str, @"^[0-9]+$");
        }

        [HttpPost]
        public async Task<IActionResult> updateInfor(ThongTinCaNhan thongTinCaNhan, IFormFile anhDaiDien)
        {
            if (thongTinCaNhan.Sdt != null)
            {
                if(thongTinCaNhan.Sdt.Length != 10 || !IsNumeric(thongTinCaNhan.Sdt))
                {
                    ModelState.AddModelError("Sdt", "Số điện thoại phải chứa đúng 10 chữ số !");
                    var User = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == thongTinCaNhan.MaKhachHang);
                    ViewBag.TenUser = User.TenKhachHang;
                    ViewBag.AnhDaiDien = User.AnhDaiDien;
                    ViewBag.IdinforUser = thongTinCaNhan.MaKhachHang;
                    var trangthai = db.TrangThais.ToList();
                    var gioitinh = db.GioiTinhs.ToList();
                    ViewBag.TrangThai = new SelectList(trangthai, "IdTrangThai", "TrangThai1", User.TrangThai);
                    ViewBag.GioiTinh = new SelectList(gioitinh, "IdGioiTinh", "GioiTinh1", User.GioiTinh);
                    BanBe banBe = new BanBe();
                    var thongTinCaNhans = db.ThongTinCaNhans.ToList();
                    var banBes = db.BanBes
                        .Include(p => p.IdUser1Navigation)
                        .Include(p => p.IdUser2Navigation)
                        .ToList();
                    var Users = new PostACmt
                    {
                        ThongTinCaNhan = User,
                        BanBe = banBe,
                        thongTinCaNhans = thongTinCaNhans,
                        BanBes = banBes,
                    };
                    return View(Users);
                }
            }
            var user = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == thongTinCaNhan.MaKhachHang);
            var idUser = HttpContext.Session.GetInt32("userId_" + thongTinCaNhan.MaKhachHang);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                if (anhDaiDien != null)
                {
                    thongTinCaNhan.AnhDaiDien = await SaveImageUser(anhDaiDien);
                }
                else
                {
                    thongTinCaNhan.AnhDaiDien = user.AnhDaiDien;
                }
                var id = idUser;
                db.Entry(user).CurrentValues.SetValues(thongTinCaNhan);
                db.SaveChanges();
                return RedirectToAction("Index",new {id = id, checkLogin = 1, idUser = idUser });
            }
        }

        private async Task<string> SaveImageUser(IFormFile anhDaiDien)
        {
            var savePath = Path.Combine("wwwroot/imagesUser", anhDaiDien.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await anhDaiDien.CopyToAsync(fileStream);
            }
            return "/imagesUser/" + anhDaiDien.FileName;
        }

        [HttpPost]
        public async Task<IActionResult> BaiPost(BaiPost baiPost, IFormFile anhBaiPost)
        {

            if (anhBaiPost != null)
            {
                baiPost.AnhBaiPost = await SaveImagePost(anhBaiPost);
            }

            db.BaiPosts.Add(baiPost);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = baiPost.MaNguoiPost, checkLogin = 1 , idUser = baiPost.MaNguoiPost });
        }
        public async Task<string> SaveImagePost(IFormFile anhBaiPost)
        {
            var savePath = Path.Combine("wwwroot/imagesPost", anhBaiPost.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await anhBaiPost.CopyToAsync(fileStream);
            }
            return "/imagesPost/" + anhBaiPost.FileName;
        }

        [HttpPost]
        public IActionResult ReactionPost(ReactionPost reactionPost)
        {
            var reaction = db.ReactionPosts.FirstOrDefault(p => p.IdPost == reactionPost.IdPost && p.IdUser == reactionPost.IdUser);
            if (reaction != null)
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

        public IActionResult DeletePost(int maBaiPost, int idUser)
        {
            var baiPost = db.BaiPosts.FirstOrDefault(p => p.MaBaiPost == maBaiPost);
            if (baiPost != null)
            {
                db.BaiPosts.Remove(baiPost);
                db.SaveChanges();
                return RedirectToAction("index", new { id = idUser, checkLogin = 1 , idUser = idUser });
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult PostCmt(BinhLuan binhLuan)
        {
            
            db.BinhLuans.Add(binhLuan);
            db.SaveChanges();
            TempData["PostId"] = binhLuan.MaBaiPost;
            return RedirectToAction("LoadCmt");
        }
        [HttpGet]
        public IActionResult LoadCmt()
        {
            var idUser = TempData.Peek("UserId") as int?;
            var id = TempData.Peek("id") as int?;
            var idPost = TempData.Peek("PostId") as int?;
            var UserCmt = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == idUser);
            if (UserCmt != null)
            {
                ViewBag.AnhUser = UserCmt.AnhDaiDien;
            }
            ViewBag.maBaiPost = idPost;
            ViewBag.IdinforUser = idUser;
            ViewBag.IdUser = id;
            var loadCmt = db.BinhLuans
                .Include(p => p.IdUserCmtNavigation)
                .ToList();
            var baiPostList = db.BaiPosts.ToList();
            BinhLuan binhLuan = new BinhLuan();
            PostACmt postACmt = new PostACmt
            {
                binhLuan = binhLuan,
                BinhLuanList = loadCmt,
                BaiPostList = baiPostList
            };
            return View(postACmt);
        }
        [HttpPost]
        public IActionResult DeleteCmt(int id, int idCmt, int idUser)
        {
            var Cmt = db.BinhLuans.FirstOrDefault(p => p.MaCmt == idCmt);
            if (Cmt != null)
            {
                TempData["PostId"] = Cmt.MaBaiPost;
                db.BinhLuans.Remove(Cmt);
                db.SaveChanges();
                return RedirectToAction("Index", new {id = id , checkLogin = 1, idUser = idUser });
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult AddFriend(BanBe banBe)
        {
            banBe.TrangThai = 2;
            db.BanBes.Add(banBe);
            db.SaveChanges();
            return RedirectToAction("index",new { id = banBe.IdUser2, checkLogin = 1 , idUser = banBe.IdUser1});
        }

        [HttpPost]
        public IActionResult Accept(BanBe banBe)
        {
            var madeFriend = db.BanBes.FirstOrDefault(p => p.IdUser1 == banBe.IdUser1 && p.IdUser2 == banBe.IdUser2);
            if(madeFriend != null)
            {
                madeFriend.TrangThai = 1;
                madeFriend.IdUser1 = banBe.IdUser1;
                madeFriend.IdUser2 = banBe.IdUser2;
                db.BanBes.Update(madeFriend);
                db.SaveChanges();
            }
            var idUser = TempData.Peek("UserId") as int?;
            return RedirectToAction("index", new { id = banBe.IdUser1, checkLogin = 1 , idUser = banBe.IdUser2 });
        }

        [HttpPost]
        public IActionResult Refuse(BanBe banBe)
        {
            var madeFriend = db.BanBes.FirstOrDefault(p => p.IdUser1 == banBe.IdUser1 && p.IdUser2 == banBe.IdUser2);
            if(madeFriend != null )
            {
                db.BanBes.Remove(madeFriend);
                db.SaveChanges();
            }
            var idUser = TempData.Peek("UserId") as int?;
            return RedirectToAction("index", new { id = banBe.IdUser1, checkLogin = 1, idUser = banBe.IdUser2 });
        }
        [HttpPost]
        public IActionResult RoomChat(RoomChat roomChat)
        {
            var room = db.RoomChats.FirstOrDefault(p => (p.MaUser1 == roomChat.MaUser1 && p.MaUser2 == roomChat.MaUser2)
            || (p.MaUser1 == roomChat.MaUser2 && p.MaUser2 == roomChat.MaUser1));
            if( room == null )
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
                return RedirectToAction("index");
            }
            return RedirectToAction("index");
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
    }
}
