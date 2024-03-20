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

    public virtual DbSet<BaiPost> BaiPosts { get; set; }

    public virtual DbSet<BanBe> BanBes { get; set; }

    public virtual DbSet<BinhLuan> BinhLuans { get; set; }

    public virtual DbSet<RoomChat> RoomChats { get; set; }

    public virtual DbSet<ThongTinCaNhan> ThongTinCaNhans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=GIA-KHIEM\\MSSQLSERVER01;Initial Catalog=Project_Blue;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaiPost>(entity =>
        {
            entity.HasKey(e => e.MaBaiPost);

            entity.ToTable("BaiPost");

            entity.Property(e => e.AnhBaiPost).HasMaxLength(50);
            entity.Property(e => e.AnhNguoiPost).HasMaxLength(50);
            entity.Property(e => e.Caption).HasMaxLength(50);
            entity.Property(e => e.MoTa).HasMaxLength(50);
            entity.Property(e => e.TenNguoiPost).HasMaxLength(50);
        });

        modelBuilder.Entity<BanBe>(entity =>
        {
            entity.HasKey(e => e.MaBanBe);

            entity.ToTable("BanBe");

            entity.HasOne(d => d.MaUser1Navigation).WithMany(p => p.BanBeMaUser1Navigations)
                .HasForeignKey(d => d.MaUser1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BanBe_ThongTinCaNhan");

            entity.HasOne(d => d.MaUser2Navigation).WithMany(p => p.BanBeMaUser2Navigations)
                .HasForeignKey(d => d.MaUser2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BanBe_ThongTinCaNhan1");
        });

        modelBuilder.Entity<BinhLuan>(entity =>
        {
            entity.HasKey(e => e.MaCmt);

            entity.ToTable("BinhLuan");

            entity.Property(e => e.AnhNguoiCmt).HasMaxLength(50);
            entity.Property(e => e.NoiDungCmt).HasMaxLength(50);
            entity.Property(e => e.TenNguoiCmt).HasMaxLength(50);
        });

        modelBuilder.Entity<RoomChat>(entity =>
        {
            entity.HasKey(e => e.MaPhong);

            entity.ToTable("RoomChat");

            entity.Property(e => e.NoiDung).HasMaxLength(50);
            entity.Property(e => e.TenPhong).HasMaxLength(50);

            entity.HasOne(d => d.MaUser1Navigation).WithMany(p => p.RoomChatMaUser1Navigations)
                .HasForeignKey(d => d.MaUser1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomChat_ThongTinCaNhan");

            entity.HasOne(d => d.MaUser2Navigation).WithMany(p => p.RoomChatMaUser2Navigations)
                .HasForeignKey(d => d.MaUser2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomChat_ThongTinCaNhan1");
        });

        modelBuilder.Entity<ThongTinCaNhan>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang);

            entity.ToTable("ThongTinCaNhan");

            entity.Property(e => e.AnhDaiDien).HasMaxLength(50);
            entity.Property(e => e.Password)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Sdt)
                .HasMaxLength(50)
                .HasColumnName("SDT");
            entity.Property(e => e.TenKhachHang).HasMaxLength(50);
            entity.Property(e => e.TieuSu).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
