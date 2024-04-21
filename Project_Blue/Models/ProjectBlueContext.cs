using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project_Blue.Models;

public partial class ProjectBlueContext : DbContext
{
    public ProjectBlueContext()
    {
    }

    public ProjectBlueContext(DbContextOptions<ProjectBlueContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<BaiPost> BaiPosts { get; set; }

    public virtual DbSet<BanBe> BanBes { get; set; }

    public virtual DbSet<BinhLuan> BinhLuans { get; set; }

    public virtual DbSet<BinhLuanShare> BinhLuanShares { get; set; }

    public virtual DbSet<GioiTinh> GioiTinhs { get; set; }

    public virtual DbSet<GroupChat> GroupChats { get; set; }

    public virtual DbSet<GroupMember> GroupMembers { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MessageGroup> MessageGroups { get; set; }

    public virtual DbSet<ReactionPost> ReactionPosts { get; set; }

    public virtual DbSet<ReactionSharePost> ReactionSharePosts { get; set; }

    public virtual DbSet<RoomChat> RoomChats { get; set; }

    public virtual DbSet<SavePost> SavePosts { get; set; }

    public virtual DbSet<SharePost> SharePosts { get; set; }

    public virtual DbSet<ThongTinCaNhan> ThongTinCaNhans { get; set; }

    public virtual DbSet<TrangThai> TrangThais { get; set; }

    public virtual DbSet<TrangThaiBanBe> TrangThaiBanBes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=GIA-KHIEM\\MSSQLSERVER01;Initial Catalog=Project_Blue;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Idadmin);

            entity.ToTable("Admin");

            entity.Property(e => e.Gmail)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Image).HasMaxLength(259);
            entity.Property(e => e.Name).HasMaxLength(43);
            entity.Property(e => e.Password)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Username)
                .HasMaxLength(75)
                .IsUnicode(false);

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.Admins)
                .HasForeignKey(d => d.Gender)
                .HasConstraintName("FK_Admin_GioiTinh");
        });

        modelBuilder.Entity<BaiPost>(entity =>
        {
            entity.HasKey(e => e.MaBaiPost);

            entity.ToTable("BaiPost");

            entity.Property(e => e.AnhBaiPost).HasMaxLength(259);
            entity.Property(e => e.Caption).HasMaxLength(40);
            entity.Property(e => e.MoTa).HasMaxLength(125);
            entity.Property(e => e.PostedTime).HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiPostNavigation).WithMany(p => p.BaiPosts)
                .HasForeignKey(d => d.MaNguoiPost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BaiPost_ThongTinCaNhan");
        });

        modelBuilder.Entity<BanBe>(entity =>
        {
            entity.HasKey(e => e.IdBanBe);

            entity.ToTable("BanBe");

            entity.HasOne(d => d.IdUser1Navigation).WithMany(p => p.BanBeIdUser1Navigations)
                .HasForeignKey(d => d.IdUser1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BanBe_ThongTinCaNhan");

            entity.HasOne(d => d.IdUser2Navigation).WithMany(p => p.BanBeIdUser2Navigations)
                .HasForeignKey(d => d.IdUser2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BanBe_ThongTinCaNhan1");

            entity.HasOne(d => d.TrangThaiNavigation).WithMany(p => p.BanBes)
                .HasForeignKey(d => d.TrangThai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BanBe_TrangThaiBanBe");
        });

        modelBuilder.Entity<BinhLuan>(entity =>
        {
            entity.HasKey(e => e.MaCmt);

            entity.ToTable("BinhLuan");

            entity.Property(e => e.NoiDungCmt).HasMaxLength(300);

            entity.HasOne(d => d.IdUserCmtNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.IdUserCmt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BinhLuan_ThongTinCaNhan");
        });

        modelBuilder.Entity<BinhLuanShare>(entity =>
        {
            entity.HasKey(e => e.MaCmtShare);

            entity.ToTable("BinhLuanShare");

            entity.Property(e => e.NoiDungCmtShare).HasMaxLength(300);

            entity.HasOne(d => d.IdUserCmtShareNavigation).WithMany(p => p.BinhLuanShares)
                .HasForeignKey(d => d.IdUserCmtShare)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BinhLuanShare_ThongTinCaNhan");
        });

        modelBuilder.Entity<GioiTinh>(entity =>
        {
            entity.HasKey(e => e.IdGioiTinh);

            entity.ToTable("GioiTinh");

            entity.Property(e => e.GioiTinh1)
                .HasMaxLength(5)
                .HasColumnName("GioiTinh");
        });

        modelBuilder.Entity<GroupChat>(entity =>
        {
            entity.HasKey(e => e.GroupId);

            entity.ToTable("Group_chat");

            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.GroupImage)
                .HasMaxLength(259)
                .HasColumnName("group_image");
            entity.Property(e => e.GroupName)
                .HasMaxLength(65)
                .HasColumnName("group_name");
        });

        modelBuilder.Entity<GroupMember>(entity =>
        {
            entity.ToTable("Group_member");

            entity.Property(e => e.GroupMemberId).HasColumnName("group_member_id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupMembers)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Group_member_Group_chat");

            entity.HasOne(d => d.User).WithMany(p => p.GroupMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Group_member_ThongTinCaNhan");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.IdMessage);

            entity.ToTable("Message");

            entity.Property(e => e.NoiDung).HasMaxLength(160);
        });

        modelBuilder.Entity<MessageGroup>(entity =>
        {
            entity.HasKey(e => e.MessageId);

            entity.ToTable("Message_group");

            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.Text)
                .HasMaxLength(160)
                .HasColumnName("text");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Group).WithMany(p => p.MessageGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Message_group_Group_chat");

            entity.HasOne(d => d.User).WithMany(p => p.MessageGroups)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Message_group_ThongTinCaNhan");
        });

        modelBuilder.Entity<ReactionPost>(entity =>
        {
            entity.HasKey(e => e.IdReaction);

            entity.ToTable("ReactionPost");
        });

        modelBuilder.Entity<ReactionSharePost>(entity =>
        {
            entity.HasKey(e => e.IdReactionShare);

            entity.ToTable("ReactionSharePost");
        });

        modelBuilder.Entity<RoomChat>(entity =>
        {
            entity.HasKey(e => e.MaPhong);

            entity.ToTable("RoomChat");

            entity.Property(e => e.ImageUser).HasMaxLength(259);
            entity.Property(e => e.NoiDung).HasMaxLength(160);
            entity.Property(e => e.TenPhong).HasMaxLength(43);

            entity.HasOne(d => d.MaUser1Navigation).WithMany(p => p.RoomChatMaUser1Navigations)
                .HasForeignKey(d => d.MaUser1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomChat_ThongTinCaNhan2");

            entity.HasOne(d => d.MaUser2Navigation).WithMany(p => p.RoomChatMaUser2Navigations)
                .HasForeignKey(d => d.MaUser2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomChat_ThongTinCaNhan3");
        });

        modelBuilder.Entity<SavePost>(entity =>
        {
            entity.HasKey(e => e.IdSave);

            entity.ToTable("SavePost");

            entity.Property(e => e.IdSave).HasColumnName("idSave");
        });

        modelBuilder.Entity<SharePost>(entity =>
        {
            entity.HasKey(e => e.IdPostShare);

            entity.ToTable("SharePost");

            entity.Property(e => e.NoiDungShare).HasMaxLength(125);
            entity.Property(e => e.SharedTime).HasColumnType("datetime");

            entity.HasOne(d => d.IdBaiPostNavigation).WithMany(p => p.SharePosts)
                .HasForeignKey(d => d.IdBaiPost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SharePost_BaiPost");

            entity.HasOne(d => d.IdUserShareNavigation).WithMany(p => p.SharePosts)
                .HasForeignKey(d => d.IdUserShare)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SharePost_ThongTinCaNhan");
        });

        modelBuilder.Entity<ThongTinCaNhan>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang);

            entity.ToTable("ThongTinCaNhan");

            entity.Property(e => e.AnhDaiDien).HasMaxLength(259);
            entity.Property(e => e.Gmail)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SDT");
            entity.Property(e => e.TenKhachHang).HasMaxLength(43);
            entity.Property(e => e.TieuSu).HasMaxLength(101);
            entity.Property(e => e.UserName)
                .HasMaxLength(75)
                .IsUnicode(false);

            entity.HasOne(d => d.GioiTinhNavigation).WithMany(p => p.ThongTinCaNhans)
                .HasForeignKey(d => d.GioiTinh)
                .HasConstraintName("FK_ThongTinCaNhan_GioiTinh");

            entity.HasOne(d => d.TrangThaiNavigation).WithMany(p => p.ThongTinCaNhans)
                .HasForeignKey(d => d.TrangThai)
                .HasConstraintName("FK_ThongTinCaNhan_TrangThai");
        });

        modelBuilder.Entity<TrangThai>(entity =>
        {
            entity.HasKey(e => e.IdTrangThai);

            entity.ToTable("TrangThai");

            entity.Property(e => e.TrangThai1)
                .HasMaxLength(20)
                .HasColumnName("TrangThai");
        });

        modelBuilder.Entity<TrangThaiBanBe>(entity =>
        {
            entity.HasKey(e => e.IdtrangThai);

            entity.ToTable("TrangThaiBanBe");

            entity.Property(e => e.IdtrangThai).HasColumnName("IDTrangThai");
            entity.Property(e => e.TenTrangThai).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
