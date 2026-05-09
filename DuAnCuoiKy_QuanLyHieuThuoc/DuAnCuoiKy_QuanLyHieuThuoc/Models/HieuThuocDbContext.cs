using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class HieuThuocDbContext : DbContext
{
    public HieuThuocDbContext()
    {
    }

    public HieuThuocDbContext(DbContextOptions<HieuThuocDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<ChiTietNhap> ChiTietNhaps { get; set; }

    public virtual DbSet<DonViTinh> DonViTinhs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<LoaiThuoc> LoaiThuocs { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<Thuoc> Thuocs { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    public virtual DbSet<VwCanhBao> VwCanhBaos { get; set; }

    public virtual DbSet<VwDanhSachThuoc> VwDanhSachThuocs { get; set; }

    public virtual DbSet<VwDoanhThuThang> VwDoanhThuThangs { get; set; }

    public virtual DbSet<VwHoaDonChiTiet> VwHoaDonChiTiets { get; set; }

    public virtual DbSet<VwTopThuocBanChay> VwTopThuocBanChays { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DoanNgocTuong;Database=HieuThuocDB;User Id=sa;Password=12345;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Vietnamese_CI_AS");

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => e.MaCthd);

            entity.ToTable("ChiTietHoaDon");

            entity.HasIndex(e => e.MaHd, "IX_CTHD_HoaDon");

            entity.HasIndex(e => e.MaThuoc, "IX_CTHD_Thuoc");

            entity.HasIndex(e => new { e.MaHd, e.MaThuoc }, "UQ_CTHD").IsUnique();

            entity.Property(e => e.MaCthd).HasColumnName("MaCTHD");
            entity.Property(e => e.DonGia).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.ThanhTien)
                .HasComputedColumnSql("([SoLuong]*[DonGia])", true)
                .HasColumnType("decimal(23, 0)");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHD_HoaDon");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHD_Thuoc");
        });

        modelBuilder.Entity<ChiTietNhap>(entity =>
        {
            entity.HasKey(e => e.MaCtn);

            entity.ToTable("ChiTietNhap");

            entity.HasIndex(e => e.MaPhieuNhap, "IX_CTN_PhieuNhap");

            entity.HasIndex(e => e.MaThuoc, "IX_CTN_Thuoc");

            entity.HasIndex(e => new { e.MaPhieuNhap, e.MaThuoc, e.SoLo }, "UQ_CTN").IsUnique();

            entity.Property(e => e.MaCtn).HasColumnName("MaCTN");
            entity.Property(e => e.GiaNhap).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.SoLo).HasMaxLength(30);
            entity.Property(e => e.ThanhTien)
                .HasComputedColumnSql("([SoLuong]*[GiaNhap])", true)
                .HasColumnType("decimal(23, 0)");

            entity.HasOne(d => d.MaPhieuNhapNavigation).WithMany(p => p.ChiTietNhaps)
                .HasForeignKey(d => d.MaPhieuNhap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTN_PhieuNhap");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.ChiTietNhaps)
                .HasForeignKey(d => d.MaThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTN_Thuoc");
        });

        modelBuilder.Entity<DonViTinh>(entity =>
        {
            entity.HasKey(e => e.MaDvt);

            entity.ToTable("DonViTinh");

            entity.HasIndex(e => e.TenDvt, "UQ_DVT_Ten").IsUnique();

            entity.Property(e => e.MaDvt)
                .ValueGeneratedOnAdd()
                .HasColumnName("MaDVT");
            entity.Property(e => e.TenDvt)
                .HasMaxLength(20)
                .HasColumnName("TenDVT");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHd);

            entity.ToTable("HoaDon");

            entity.HasIndex(e => e.NgayBan, "IX_HoaDon_NgayBan");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.GhiChu).HasMaxLength(200);
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.NgayBan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(15, 0)");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HD_NV");
        });

        modelBuilder.Entity<LoaiThuoc>(entity =>
        {
            entity.HasKey(e => e.MaLoai);

            entity.ToTable("LoaiThuoc");

            entity.HasIndex(e => e.TenLoai, "UQ_Loai_Ten").IsUnique();

            entity.Property(e => e.MoTa).HasMaxLength(200);
            entity.Property(e => e.TenLoai).HasMaxLength(50);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNcc);

            entity.ToTable("NhaCungCap");

            entity.HasIndex(e => e.TenNcc, "UQ_NCC_Ten").IsUnique();

            entity.Property(e => e.MaNcc).HasColumnName("MaNCC");
            entity.Property(e => e.DiaChi).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(80);
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.TenNcc)
                .HasMaxLength(100)
                .HasColumnName("TenNCC");
            entity.Property(e => e.TinhThanh).HasMaxLength(50);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv);

            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.SoDienThoai, "UQ_NV_SDT").IsUnique();

            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.Email).HasMaxLength(80);
            entity.Property(e => e.GioiTinh)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.HoTen).HasMaxLength(60);
            entity.Property(e => e.NgayVaoLam).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NV_VaiTro");
        });

        modelBuilder.Entity<PhieuNhap>(entity =>
        {
            entity.HasKey(e => e.MaPhieuNhap);

            entity.ToTable("PhieuNhap");

            entity.HasIndex(e => e.NgayNhap, "IX_PN_NgayNhap");

            entity.Property(e => e.GhiChu).HasMaxLength(200);
            entity.Property(e => e.MaNcc).HasColumnName("MaNCC");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.NgayNhap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(15, 0)");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.MaNcc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PN_NCC");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PN_NV");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTk);

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.MaNv, "UQ_TK_MaNV").IsUnique();

            entity.HasIndex(e => e.TenDangNhap, "UQ_TK_TenDangNhap").IsUnique();

            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.LanDangNhapCuoi).HasColumnType("datetime");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.MatKhau).HasMaxLength(256);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenDangNhap).HasMaxLength(40);

            entity.HasOne(d => d.MaNvNavigation).WithOne(p => p.TaiKhoan)
                .HasForeignKey<TaiKhoan>(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TK_NhanVien");
        });

        modelBuilder.Entity<Thuoc>(entity =>
        {
            entity.HasKey(e => e.MaThuoc);

            entity.ToTable("Thuoc");

            entity.HasIndex(e => e.MaLoai, "IX_Thuoc_MaLoai");

            entity.HasIndex(e => e.SoLuongTon, "IX_Thuoc_TonKho");

            entity.Property(e => e.GhiChu).HasMaxLength(200);
            entity.Property(e => e.GiaBan).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.MaDvt).HasColumnName("MaDVT");
            entity.Property(e => e.MuongCanhBao).HasDefaultValue(20);
            entity.Property(e => e.TenThuoc).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaDvtNavigation).WithMany(p => p.Thuocs)
                .HasForeignKey(d => d.MaDvt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Thuoc_DVT");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.Thuocs)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Thuoc_LoaiThuoc");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro);

            entity.ToTable("VaiTro");

            entity.HasIndex(e => e.TenVaiTro, "UQ_VaiTro_Ten").IsUnique();

            entity.Property(e => e.MaVaiTro).ValueGeneratedOnAdd();
            entity.Property(e => e.MoTa).HasMaxLength(100);
            entity.Property(e => e.TenVaiTro).HasMaxLength(30);
        });

        modelBuilder.Entity<VwCanhBao>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_CanhBao");

            entity.Property(e => e.LoaiCanhBao).HasMaxLength(22);
            entity.Property(e => e.TenThuoc).HasMaxLength(100);
        });

        modelBuilder.Entity<VwDanhSachThuoc>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_DanhSachThuoc");

            entity.Property(e => e.GiaBan).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.TenDvt)
                .HasMaxLength(20)
                .HasColumnName("TenDVT");
            entity.Property(e => e.TenLoai).HasMaxLength(50);
            entity.Property(e => e.TenThuoc).HasMaxLength(100);
            entity.Property(e => e.TrangThaiTon).HasMaxLength(11);
        });

        modelBuilder.Entity<VwDoanhThuThang>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_DoanhThuThang");

            entity.Property(e => e.TongDoanhThu).HasColumnType("decimal(38, 0)");
            entity.Property(e => e.TrungBinhHoaDon).HasColumnType("decimal(38, 6)");
        });

        modelBuilder.Entity<VwHoaDonChiTiet>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_HoaDonChiTiet");

            entity.Property(e => e.DonGia).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.GhiChu).HasMaxLength(200);
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.NgayBan).HasColumnType("datetime");
            entity.Property(e => e.TenDvt)
                .HasMaxLength(20)
                .HasColumnName("TenDVT");
            entity.Property(e => e.TenNhanVien).HasMaxLength(60);
            entity.Property(e => e.TenThuoc).HasMaxLength(100);
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(23, 0)");
            entity.Property(e => e.TongTien).HasColumnType("decimal(15, 0)");
        });

        modelBuilder.Entity<VwTopThuocBanChay>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TopThuocBanChay");

            entity.Property(e => e.TenLoai).HasMaxLength(50);
            entity.Property(e => e.TenThuoc).HasMaxLength(100);
            entity.Property(e => e.TongDoanhThu).HasColumnType("decimal(38, 0)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
