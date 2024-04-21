using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics;
using Project_Blue.Models;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Azure.Messaging;
using Microsoft.IdentityModel.Tokens;
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
                            .ToList();
                        var listSharePost = db.SharePosts
                            .Include(p => p.IdBaiPostNavigation)
                            .Include(p => p.IdUserShareNavigation)
                            .ToList();
                        var listSAP = new List<dynamic>();
                        listSAP.AddRange(listBaiPost);
                        listSAP.AddRange(listSharePost);
                        listSAP = listSAP.OrderByDescending(item => item is BaiPost ? ((BaiPost)item).PostedTime : ((SharePost)item).SharedTime).ToList();
                        var listCmt = db.BinhLuans
                            .Include(p => p.IdUserCmtNavigation)
                            .ToList();
                        var listTTCN = db.ThongTinCaNhans.ToList();                 
                        var listBanbe = db.BanBes
                            .Include(p => p.IdUser1Navigation)
                            .Include(p => p.IdUser2Navigation)
                            .ToList();
                        var reactionPosts = db.ReactionPosts.ToList();
                        var binhluanShares = db.BinhLuanShares
                            .Include(p => p.IdUserCmtShareNavigation)
                            .ToList();
                        var reactionShares = db.ReactionSharePosts.ToList();
                        SavePost savePost = new SavePost();
                        BanBe banBe = new BanBe();
                        RoomChat roomChat = new RoomChat();
                        BinhLuan binhLuan = new BinhLuan();
                        ReactionPost reactionPost = new ReactionPost();
                        ReactionSharePost reactionSharePost = new ReactionSharePost();
                        SharePost sharePost = new SharePost();  
                        ViewBag.IdinforUser = UserId;
                        ViewBag.IdUser = UserId;

                        var PostACmt = new PostACmt
                        {
                            BinhLuanList = listCmt,
                            thongTinCaNhans = listTTCN,
                            reactionPosts = reactionPosts,
                            BanBes = listBanbe,
                            SAPList = listSAP,
                            BinhLuanShares = binhluanShares,
                            BanBe = banBe,
                            RoomChat = roomChat,
                            binhLuan = binhLuan,
                            ReactionPost = reactionPost,
                            reactionSharesPosts = reactionShares,
                            ReactionSharePost = reactionSharePost,
                            SharePost = sharePost,
                            savePost = savePost,
                            groupChats = db.GroupChats.ToList(),
                            groupMembers = db.GroupMembers
                                    .OrderByDescending(time => time.CreateAt)
                                    .Include(p => p.Group)
                                    .ToList(),
                        };
                        TempData["UserId"] = UserId;
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
                if (!ModelState.IsValid)
                {
                var UserId = HttpContext.Session.GetInt32("userId_" + baiPost.MaNguoiPost);
                var User = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == UserId);
                if (User != null)
                {
                    ViewBag.IdinforUser = User.MaKhachHang;
                    ViewBag.TenUser = User.TenKhachHang;
                    ViewBag.AnhDaiDien = User.AnhDaiDien;
                }
                ViewBag.idUser = UserId;
                return View("PostStatus");
                }
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
        [HttpPost]
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
                TempData["UserId"] = reaction.IdUser;
                db.ReactionPosts.Remove(reaction);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                TempData["UserId"] = reactionPost.IdUser;
                db.ReactionPosts.Add(reactionPost);
                db.SaveChanges();
                return Ok();
            }
        }

        [HttpGet]
        public IActionResult LoadLike(int postId)
        {
            ViewBag.postId = postId;
            var react = db.ReactionPosts.ToList();
            var idUser = TempData.Peek("UserId") as int?;
            ViewBag.IdUser = idUser;
            ReactionPost reactionPost = new ReactionPost();
            PostACmt postACmt = new PostACmt
            {
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
                var baiPost = db.BaiPosts.FirstOrDefault(p => p.MaBaiPost == maBaiPost);
                if(baiPost != null)
                {
                    ViewBag.maNguoiPost = baiPost.MaNguoiPost;
                }
                ViewBag.maBaiPost = maBaiPost;
                ViewBag.IdUser = idUser;
                var loadCmt = db.BinhLuans
                    .Include(p => p.IdUserCmtNavigation)
                    .ToList();
                BinhLuan binhLuan = new BinhLuan();
                PostACmt postACmt = new PostACmt
                {
                    binhLuan = binhLuan,
                    BinhLuanList = loadCmt,
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

        //Share Post---------------------------------------------------

        public IActionResult DisplayShare(int idUser, int idPost)
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
            ViewBag.idPost = idPost;
            var baiPostList = db.BaiPosts
                .Include(p => p.MaNguoiPostNavigation)
                .ToList();
            SharePost sharePost = new SharePost();
            PostACmt postACmt = new PostACmt()
            {
                BaiPostList = baiPostList,
                SharePost = sharePost
            };
            return View(postACmt);
        }

        [HttpPost]
        public IActionResult BaiPostShared(SharePost sharePost)
        {
            db.SharePosts.Add(sharePost);
            db.SaveChanges();
            return RedirectToAction("Home", "Home", new { id = sharePost.IdUserShare, checkLogin = 1 });
        }

        [HttpPost]
        public IActionResult SavePost(SavePost savePost)
        {
            db.SavePosts.Add(savePost);
            db.SaveChanges();
            return Json(new { message = "Lưu bài viết thành công!" });
        }

        public IActionResult DisPlaySavePost()
        {

            var idUser = TempData.Peek("UserId") as int?;
            var User = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == idUser);
            if (User != null)
            {
                ViewBag.TenUser = User.TenKhachHang;
                ViewBag.AnhDaiDien = User.AnhDaiDien;
            }
            ViewBag.IdinforUser = idUser;
            ViewBag.IdUser = idUser;
            var listBaiPost = db.BaiPosts
                            .Include(p => p.MaNguoiPostNavigation)
                            .ToList();
            var listSharePost = db.SharePosts
                .Include(p => p.IdBaiPostNavigation)
                .Include(p => p.IdUserShareNavigation)
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
            var binhluanShares = db.BinhLuanShares
                .Include(p => p.IdUserCmtShareNavigation)
                .ToList();
            var reactionShares = db.ReactionSharePosts.ToList();
            var savePosts = db.SavePosts
                .OrderByDescending(p=>p.IdSave)
                .ToList();
            var baiPostList = db.BaiPosts
                .Include(p => p.MaNguoiPostNavigation)
                .ToList();
            var sharePosts = db.SharePosts
                .Include(p=>p.IdBaiPostNavigation)
                .Include(p =>p.IdUserShareNavigation)
                .ToList();
            BanBe banBe = new BanBe();
            RoomChat roomChat = new RoomChat();
            ReactionPost reactionPost = new ReactionPost();
            ReactionSharePost reactionSharePost = new ReactionSharePost();
            var PostACmt = new PostACmt
            {
                BinhLuanList = listCmt,
                thongTinCaNhans = listTTCN,
                reactionPosts = reactionPosts,
                BanBes = listBanbe,
                BinhLuanShares = binhluanShares,
                BanBe = banBe,
                RoomChat = roomChat,
                ReactionPost = reactionPost,
                reactionSharesPosts = reactionShares,
                ReactionSharePost = reactionSharePost,
                SavePosts = savePosts,
                BaiPostList = baiPostList,
                sharePosts = sharePosts,
                groupChats = db.GroupChats.ToList(),
                groupMembers = db.GroupMembers
                                    .OrderByDescending(time => time.CreateAt)
                                    .Include(p => p.Group)
                                    .ToList(),
            };
            return View(PostACmt);
        }

        [HttpPost]
        public IActionResult DeleteSavePost(int idSavePost, int idUser)
        {
            var savePost = db.SavePosts.FirstOrDefault(p=>p.IdSave == idSavePost);
            if (savePost == null)
                return BadRequest();
            db.SavePosts.Remove(savePost);
            db.SaveChanges();
            return Json(new { messageDelete = "Xóa bài lưu thành công!" }); ;
        }

        [HttpPost]
        public IActionResult DeleteShare(int maBaiShare, int idUser)
        {
            var baiShare = db.SharePosts.FirstOrDefault(p => p.IdPostShare == maBaiShare);
            if (baiShare != null)
            {
                db.SharePosts.Remove(baiShare);
                db.SaveChanges();
                return RedirectToAction("home", new { id = idUser, checkLogin = 1 });
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult ReactionPostShared(ReactionSharePost reactionSharePost)
        {
            var reactionShare = db.ReactionSharePosts.FirstOrDefault(p => p.IdPostShare == reactionSharePost.IdPostShare && p.IdUser == reactionSharePost.IdUser);
            if (reactionShare != null)
            {
                TempData["UserShareId"] = reactionShare.IdUser;
                db.ReactionSharePosts.Remove(reactionShare);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                TempData["UserShareId"] = reactionSharePost.IdUser;
                db.ReactionSharePosts.Add(reactionSharePost);
                db.SaveChanges();
                return Ok();
            }
        }

        [HttpGet]
        public IActionResult LoadLikeShared(int postId)
        {
            ViewBag.postId = postId;
            var react = db.ReactionSharePosts.ToList();
            var idUser = TempData.Peek("UserShareId") as int?;
            ViewBag.IdUser = idUser;
            ReactionSharePost reactionShare = new ReactionSharePost();
            PostACmt postACmt = new PostACmt
            {
                reactionSharesPosts = react,
                ReactionSharePost = reactionShare
            };
            return View(postACmt);
        }

        [HttpPost]
        public IActionResult PostCmtShared(BinhLuanShare binhLuanShare)
        {
            TempData["UserShareId"] = binhLuanShare.IdUserCmtShare;
            HttpContext.Session.SetInt32("ShareId_" + binhLuanShare.IdUserCmtShare, binhLuanShare.MaShare);
            db.BinhLuanShares.Add(binhLuanShare);
            db.SaveChanges();
            return RedirectToAction("LoadCmtShared");

        }
        [HttpGet]
        public IActionResult LoadCmtShared()
        {
            var idUser = TempData.Peek("UserShareId") as int?;
            var maBaiShare = HttpContext.Session.GetInt32("ShareId_" + idUser);
            if (maBaiShare != null)
            {
                var UserCmt = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == idUser);
                if (UserCmt != null)
                {
                    ViewBag.AnhUser = UserCmt.AnhDaiDien;
                }
                var baiShare = db.SharePosts.FirstOrDefault(p => p.IdPostShare == maBaiShare);
                if (baiShare != null)
                {
                    ViewBag.maNguoiPost = baiShare.IdUserShare;
                }
                ViewBag.maBaiPost = maBaiShare;
                ViewBag.IdUser = idUser;
                var loadCmt = db.BinhLuanShares
                    .Include(p => p.IdUserCmtShareNavigation)
                    .ToList();
                BinhLuanShare binhLuanShare = new BinhLuanShare();
                PostACmt postACmt = new PostACmt
                {
                    BinhLuanShare = binhLuanShare,
                    BinhLuanShares = loadCmt,
                };
                return View(postACmt);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult DeleteCmtShared(int id, int idCmt)
        {
            var cmtShare = db.BinhLuanShares.FirstOrDefault(p => p.MaCmtShare == idCmt);
            if (cmtShare != null)
            {
                HttpContext.Session.SetInt32("ShareId_" + id, cmtShare.MaShare);
                db.BinhLuanShares.Remove(cmtShare);
                db.SaveChanges();
                return RedirectToAction("Home", new { id = id, checkLogin = 1 });
            }
            return NotFound();
        }
        // -----------------------
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
        private static List<ThongTinCaNhan> list_friends = new List<ThongTinCaNhan>();
        [HttpPost]
        public IActionResult Added_Friends(int id)
        {
            var ttcn = db.ThongTinCaNhans.FirstOrDefault(p => p.MaKhachHang == id);
            foreach(var items in list_friends)
            {
                if(items.MaKhachHang == ttcn.MaKhachHang)
                {
                    list_friends.Remove(items);
                    return Ok();
                }
            }
            list_friends.Add(ttcn);

            return Ok();
        }
        [HttpPost]
        public void clearList()
        {
            list_friends.Clear();
        }
        [HttpGet]
        public IActionResult displayFriends()
        {
            return View(list_friends);
        }
        [HttpPost]
        public IActionResult confirm_addGroup()
        {
            GroupChat group = new GroupChat();
            group.GroupName = "New_Group";
            group.GroupImage = "/ImagesUser/User.png";
            db.GroupChats.Add(group);
            db.SaveChanges();
            var idUser = TempData.Peek("UserId") as int?;

            List<GroupMember> groupMembers = new List<GroupMember>();

            GroupMember addMe = new GroupMember();
            addMe.UserId = (int) idUser;
            addMe.GroupId = group.GroupId;
            addMe.CreateAt = DateTime.Now;
            groupMembers.Add(addMe);
            foreach (var item in list_friends)
            {
                GroupMember groupMember = new GroupMember();
                groupMember.GroupId = group.GroupId;
                groupMember.UserId = item.MaKhachHang;
                groupMember.CreateAt = DateTime.Now;
                groupMembers.Add(groupMember);
            }
            TempData["idGr"] = group.GroupId;
            db.GroupMembers.AddRange(groupMembers);
            db.SaveChanges();
            list_friends.Clear();
            return Ok();        
        }
        [HttpPost]
        public IActionResult MessageGroup(MessageGroup messageGroup)
        {
            db.MessageGroups.Add(messageGroup);
            db.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public IActionResult DisplayMessageGroupFirst()
        {

            PostACmt model = new PostACmt
            {
                groupMembers = db.GroupMembers.ToList(),
                groupChats = db.GroupChats.ToList(),
                messageGroups = db.MessageGroups.Include(p => p.User).ToList(),
                MessageGroup = new MessageGroup(),
            };

            var idUser = TempData.Peek("UserId") as int?;
            var idGr = TempData.Peek("idGr") as int?;
            ViewBag.IdUser = idUser;
            ViewBag.idGr = idGr;
            return View(model);
        }
        [HttpGet]
        public IActionResult LoadMessageGroup()
        {
            PostACmt model = new PostACmt
            {
                groupMembers = db.GroupMembers.Include(p => p.Group).ToList(),
                groupChats = db.GroupChats.ToList(),
                messageGroups = db.MessageGroups.Include(p=>p.User).ToList(),
                MessageGroup = new MessageGroup(),
            };

            var idUser = TempData.Peek("UserId") as int?;
            var idGr = TempData.Peek("idGr") as int?;
            ViewBag.IdUser = idUser;
            ViewBag.idGr = idGr;
            return View(model);
        }
        [HttpGet]
        public IActionResult LoadMessageGroupforMess(int idGr)
        {
            PostACmt model = new PostACmt
            {
                groupMembers = db.GroupMembers.Include(p => p.Group).ToList(),
                groupChats = db.GroupChats.ToList(),
                messageGroups = db.MessageGroups.Include(p => p.User).ToList(),
                MessageGroup = new MessageGroup(),
            };

            var idUser = TempData.Peek("UserId") as int?;
            ViewBag.IdUser = idUser;
            ViewBag.idGr = idGr;
            return View(model);
        }

        [HttpGet]
        public IActionResult loadGroup()
        {
            var groupMembers = db.GroupMembers.OrderByDescending(time => time.CreateAt).Include(p => p.Group).ToList();
            ViewBag.IdUser = TempData.Peek("UserId") as int?;
            return View(groupMembers);
        }

        [HttpGet]
        public IActionResult DisplayMessageGroup(int grId)
        {

            PostACmt model = new PostACmt
            {
                groupMembers = db.GroupMembers.Include(p=>p.Group).ToList(),
                groupChats = db.GroupChats.ToList(),
                messageGroups = db.MessageGroups.Include(p => p.User).ToList(),
                MessageGroup = new MessageGroup(),
            };

            var idUser = TempData.Peek("UserId") as int?;
            ViewBag.IdUser = idUser;
            ViewBag.idGr = grId;
            return View(model);
        }
    }
}
