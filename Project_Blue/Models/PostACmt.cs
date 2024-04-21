namespace Project_Blue.Models
{
    public class PostACmt
    {
        public List<BaiPost> BaiPostList { get; set; }
        public List<BinhLuan> BinhLuanList { get; set; }
        public List<ThongTinCaNhan> thongTinCaNhans { get; set; }
        public List<BanBe> BanBes {  get; set; }
        public List<RoomChat> roomChats { get; set; }
        public List<Message> messages { get; set; }
        public List<Admin> Admins { get; set; }
        public List<ReactionPost> reactionPosts { get; set; }
        public List<SharePost> sharePosts { get; set; }
        public List<ReactionSharePost> reactionSharesPosts { get; set; }
        public List<BinhLuanShare> BinhLuanShares { get; set; }
        public List<dynamic> SAPList { get; set; }
        public List<SavePost> SavePosts { get; set; }
        public List<GroupMember> groupMembers { get; set; }
        public List<GroupChat> groupChats { get; set; }
        public List<MessageGroup> messageGroups { get; set; }
        public BaiPost BaiPost { get; set; }
        public BanBe BanBe { get; set; }
        public ThongTinCaNhan ThongTinCaNhan { get; set; }
        public RoomChat RoomChat { get; set; }
        public Message Message { get; set; }
        public BinhLuan binhLuan { get; set; }
        public Admin Admin { get; set; }
        public SavePost savePost { get; set; }
        public ReactionPost ReactionPost { get; set; }
        public ReactionSharePost ReactionSharePost { get; set; }
        public BinhLuanShare BinhLuanShare { get; set; }
        public SharePost SharePost { get; set; }
        public MessageGroup MessageGroup { get; set; }
    }
}
