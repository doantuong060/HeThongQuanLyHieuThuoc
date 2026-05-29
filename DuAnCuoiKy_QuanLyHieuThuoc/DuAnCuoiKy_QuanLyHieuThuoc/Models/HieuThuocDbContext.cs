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

    public virtual DbSet<DonViTinh> DonViTinhs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LoHang> LoHangs { get; set; }

    public virtual DbSet<LoaiThuoc> LoaiThuocs { get; set; }

    public virtual DbSet<LoaiVatTu> LoaiVatTus { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; }

    public virtual DbSet<PhuongXa> PhuongXas { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<Thuoc> Thuocs { get; set; }

    public virtual DbSet<TinhThanh> TinhThanhs { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    public virtual DbSet<VatTuYte> VatTuYtes { get; set; }

    public virtual DbSet<VwDiemTichLuyKhachHang> VwDiemTichLuyKhachHangs { get; set; }

    public virtual DbSet<VwKiemTraHanDung> VwKiemTraHanDungs { get; set; }

    public virtual DbSet<VwLichSuMuaHang> VwLichSuMuaHangs { get; set; }

    public virtual DbSet<VwTonKhoSanPham> VwTonKhoSanPhams { get; set; }

    public virtual DbSet<VwTongTienHoaDon> VwTongTienHoaDons { get; set; }

    public virtual DbSet<VwTongTienPhieuNhap> VwTongTienPhieuNhaps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=HieuThuocDB;User Id=sa;Password=12345;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Vietnamese_CI_AS");

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => e.MaCthd);

            entity.ToTable("ChiTietHoaDon", tb =>
                {
                    tb.HasTrigger("trg_CTHD_TruKho");
                    tb.HasTrigger("trg_HoaDon_UpdateTongTien");
                });

            entity.HasIndex(e => e.MaHd, "IX_CTHD_MaHD");

            entity.HasIndex(e => e.SoLo, "IX_CTHD_SoLo");

            entity.HasIndex(e => new { e.MaHd, e.SoLo }, "UQ_CTHD").IsUnique();

            entity.Property(e => e.MaCthd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaCTHD");
            entity.Property(e => e.DonGia).HasColumnType("decimal(15, 0)");
            entity.Property(e => e.MaHd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaHD");
            entity.Property(e => e.SoLo).HasMaxLength(30);

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHD_HoaDon");

            entity.HasOne(d => d.SoLoNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.SoLo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHD_LoHang");
        });

        modelBuilder.Entity<DonViTinh>(entity =>
        {
            entity.HasKey(e => e.MaDvt);

            entity.ToTable("DonViTinh");

            entity.HasIndex(e => e.TenDvt, "UQ_DVT_Ten").IsUnique();

            entity.Property(e => e.MaDvt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaDVT");
            entity.Property(e => e.TenDvt)
                .HasMaxLength(20)
                .HasColumnName("TenDVT");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHd);

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaHD");
            entity.Property(e => e.GhiChu).HasMaxLength(200);
            entity.Property(e => e.MaKh)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaNv)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaNV");
            entity.Property(e => e.NgayBan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(15, 0)");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK_HD_KH");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HD_NV");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh);

            entity.ToTable("KhachHang");

            entity.HasIndex(e => e.SoDienThoai, "UQ_KH_SDT").IsUnique();

            entity.Property(e => e.MaKh)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaKH");
            entity.Property(e => e.DiaChi).HasMaxLength(150);
            entity.Property(e => e.GhiChuBenhLy).HasMaxLength(500);
            entity.Property(e => e.HoTen).HasMaxLength(60);
            entity.Property(e => e.MaPhuongXa)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.NgayDangKy).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaPhuongXaNavigation).WithMany(p => p.KhachHangs)
                .HasForeignKey(d => d.MaPhuongXa)
                .HasConstraintName("FK_KH_PhuongXa");
        });

        modelBuilder.Entity<LoHang>(entity =>
        {
            entity.HasKey(e => e.SoLo);

            entity.ToTable("LoHang", tb => tb.HasTrigger("trg_PhieuNhap_UpdateTongTien"));

            entity.HasIndex(e => new { e.HanSuDung, e.SoLuongConLai }, "IX_LH_HanSuDung");

            entity.HasIndex(e => e.MaSp, "IX_LH_MaSP");

            entity.Property(e => e.SoLo).HasMaxLength(30);
            entity.Property(e => e.GiaNhap).HasColumnType("decimal(15, 0)");
            entity.Property(e => e.MaPhieuNhap)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MaSp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaSP");

            entity.HasOne(d => d.MaPhieuNhapNavigation).WithMany(p => p.LoHangs)
                .HasForeignKey(d => d.MaPhieuNhap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LH_PhieuNhap");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.LoHangs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LH_SanPham");
        });

        modelBuilder.Entity<LoaiThuoc>(entity =>
        {
            entity.HasKey(e => e.MaLoai);

            entity.ToTable("LoaiThuoc");

            entity.HasIndex(e => e.TenLoai, "UQ_Loai_Ten").IsUnique();

            entity.Property(e => e.MaLoai)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MoTa).HasMaxLength(200);
            entity.Property(e => e.TenLoai).HasMaxLength(50);
        });

        modelBuilder.Entity<LoaiVatTu>(entity =>
        {
            entity.HasKey(e => e.MaLoaiVt);

            entity.ToTable("LoaiVatTu");

            entity.HasIndex(e => e.TenLoaiVt, "UQ_LoaiVatTu_Ten").IsUnique();

            entity.Property(e => e.MaLoaiVt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaLoaiVT");
            entity.Property(e => e.MoTa).HasMaxLength(200);
            entity.Property(e => e.TenLoaiVt)
                .HasMaxLength(50)
                .HasColumnName("TenLoaiVT");
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNcc);

            entity.ToTable("NhaCungCap");

            entity.HasIndex(e => e.TenNcc, "UQ_NCC_Ten").IsUnique();

            entity.Property(e => e.MaNcc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaNCC");
            entity.Property(e => e.DiaChi).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(80);
            entity.Property(e => e.MaPhuongXa)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.TenNcc)
                .HasMaxLength(100)
                .HasColumnName("TenNCC");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaPhuongXaNavigation).WithMany(p => p.NhaCungCaps)
                .HasForeignKey(d => d.MaPhuongXa)
                .HasConstraintName("FK_NCC_PhuongXa");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv);

            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.SoDienThoai, "UQ_NV_SDT").IsUnique();

            entity.Property(e => e.MaNv)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaNV");
            entity.Property(e => e.Email).HasMaxLength(80);
            entity.Property(e => e.GioiTinh).HasMaxLength(3);
            entity.Property(e => e.HoTen).HasMaxLength(60);
            entity.Property(e => e.NgayVaoLam).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<PhieuNhap>(entity =>
        {
            entity.HasKey(e => e.MaPhieuNhap);

            entity.ToTable("PhieuNhap");

            entity.Property(e => e.MaPhieuNhap)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.GhiChu).HasMaxLength(200);
            entity.Property(e => e.MaNcc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaNCC");
            entity.Property(e => e.MaNv)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaNV");
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

        modelBuilder.Entity<PhuongXa>(entity =>
        {
            entity.HasKey(e => e.MaPhuongXa);

            entity.ToTable("PhuongXa");

            entity.Property(e => e.MaPhuongXa)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MaTinhThanh)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TenPhuongXa).HasMaxLength(80);

            entity.HasOne(d => d.MaTinhThanhNavigation).WithMany(p => p.PhuongXas)
                .HasForeignKey(d => d.MaTinhThanh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhuongXa_TinhThanh");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSp);

            entity.ToTable("SanPham");

            entity.Property(e => e.MaSp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaSP");
            entity.Property(e => e.GiaBan).HasColumnType("decimal(15, 0)");
            entity.Property(e => e.LoaiSp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("LoaiSP");
            entity.Property(e => e.MaDvt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaDVT");
            entity.Property(e => e.MucCanhBao).HasDefaultValue(20);
            entity.Property(e => e.TenSp)
                .HasMaxLength(100)
                .HasColumnName("TenSP");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaDvtNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaDvt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SanPham_DonViTinh");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaNv);

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.TenDangNhap, "UQ_TK_TenDangNhap").IsUnique();

            entity.Property(e => e.MaNv)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaNV");
            entity.Property(e => e.LanDangNhapCuoi).HasColumnType("datetime");
            entity.Property(e => e.MaVaiTro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MatKhau).HasMaxLength(256);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenDangNhap).HasMaxLength(40);

            entity.HasOne(d => d.MaNvNavigation).WithOne(p => p.TaiKhoan)
                .HasForeignKey<TaiKhoan>(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TK_NhanVien");

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TK_VaiTro");
        });

        modelBuilder.Entity<Thuoc>(entity =>
        {
            entity.HasKey(e => e.MaSp);

            entity.ToTable("Thuoc");

            entity.Property(e => e.MaSp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaSP");
            entity.Property(e => e.GhiChu).HasMaxLength(200);
            entity.Property(e => e.MaLoai)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.Thuocs)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Thuoc_Loai");

            entity.HasOne(d => d.MaSpNavigation).WithOne(p => p.Thuoc)
                .HasForeignKey<Thuoc>(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Thuoc_SanPham");
        });

        modelBuilder.Entity<TinhThanh>(entity =>
        {
            entity.HasKey(e => e.MaTinhThanh);

            entity.ToTable("TinhThanh");

            entity.HasIndex(e => e.TenTinhThanh, "UQ_TinhThanh_Ten").IsUnique();

            entity.Property(e => e.MaTinhThanh)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TenTinhThanh).HasMaxLength(60);
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro);

            entity.ToTable("VaiTro");

            entity.HasIndex(e => e.TenVaiTro, "UQ_VaiTro_Ten").IsUnique();

            entity.Property(e => e.MaVaiTro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MoTa).HasMaxLength(100);
            entity.Property(e => e.TenVaiTro).HasMaxLength(30);
        });

        modelBuilder.Entity<VatTuYte>(entity =>
        {
            entity.HasKey(e => e.MaSp);

            entity.ToTable("VatTuYTe");

            entity.Property(e => e.MaSp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaSP");
            entity.Property(e => e.MaLoaiVt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaLoaiVT");
            entity.Property(e => e.NhaSanXuat).HasMaxLength(100);

            entity.HasOne(d => d.MaLoaiVtNavigation).WithMany(p => p.VatTuYtes)
                .HasForeignKey(d => d.MaLoaiVt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VT_LoaiVT");

            entity.HasOne(d => d.MaSpNavigation).WithOne(p => p.VatTuYte)
                .HasForeignKey<VatTuYte>(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VT_SanPham");
        });

        modelBuilder.Entity<VwDiemTichLuyKhachHang>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_DiemTichLuyKhachHang");

            entity.Property(e => e.DiaChi).HasMaxLength(150);
            entity.Property(e => e.DiemTichLuy).HasColumnType("decimal(38, 6)");
            entity.Property(e => e.GhiChuBenhLy).HasMaxLength(500);
            entity.Property(e => e.HoTen).HasMaxLength(60);
            entity.Property(e => e.MaKh)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaKH");
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.TenPhuongXa).HasMaxLength(80);
            entity.Property(e => e.TenTinhThanh).HasMaxLength(60);
            entity.Property(e => e.TongChiTieu).HasColumnType("decimal(38, 0)");
        });

        modelBuilder.Entity<VwKiemTraHanDung>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_KiemTraHanDung");

            entity.Property(e => e.LoaiSp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("LoaiSP");
            entity.Property(e => e.MaSp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaSP");
            entity.Property(e => e.SoLo).HasMaxLength(30);
            entity.Property(e => e.TenDvt)
                .HasMaxLength(20)
                .HasColumnName("TenDVT");
            entity.Property(e => e.TenSp)
                .HasMaxLength(100)
                .HasColumnName("TenSP");
            entity.Property(e => e.TrangThaiHan).HasMaxLength(22);
        });

        modelBuilder.Entity<VwLichSuMuaHang>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_LichSuMuaHang");

            entity.Property(e => e.DonGia).HasColumnType("decimal(15, 0)");
            entity.Property(e => e.GhiChu).HasMaxLength(200);
            entity.Property(e => e.LoaiSp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("LoaiSP");
            entity.Property(e => e.MaHd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaHD");
            entity.Property(e => e.MaSp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaSP");
            entity.Property(e => e.NgayBan).HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.SoLo).HasMaxLength(30);
            entity.Property(e => e.TenKhachHang).HasMaxLength(60);
            entity.Property(e => e.TenNhanVienBan).HasMaxLength(60);
            entity.Property(e => e.TenSp)
                .HasMaxLength(100)
                .HasColumnName("TenSP");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(26, 0)");
            entity.Property(e => e.TongTienHoaDon).HasColumnType("decimal(15, 0)");
        });

        modelBuilder.Entity<VwTonKhoSanPham>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TonKhoSanPham");

            entity.Property(e => e.GiaBan).HasColumnType("decimal(15, 0)");
            entity.Property(e => e.LoaiSp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("LoaiSP");
            entity.Property(e => e.MaDvt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaDVT");
            entity.Property(e => e.MaSp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaSP");
            entity.Property(e => e.TenDvt)
                .HasMaxLength(20)
                .HasColumnName("TenDVT");
            entity.Property(e => e.TenSp)
                .HasMaxLength(100)
                .HasColumnName("TenSP");
            entity.Property(e => e.TrangThaiTon).HasMaxLength(15);
        });

        modelBuilder.Entity<VwTongTienHoaDon>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TongTienHoaDon");

            entity.Property(e => e.GhiChu).HasMaxLength(200);
            entity.Property(e => e.MaHd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaHD");
            entity.Property(e => e.MaKh)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaNv)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaNV");
            entity.Property(e => e.NgayBan).HasColumnType("datetime");
            entity.Property(e => e.TenKhachHang).HasMaxLength(60);
            entity.Property(e => e.TenNhanVien).HasMaxLength(60);
            entity.Property(e => e.TongTien).HasColumnType("decimal(15, 0)");
        });

        modelBuilder.Entity<VwTongTienPhieuNhap>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TongTienPhieuNhap");

            entity.Property(e => e.GhiChu).HasMaxLength(200);
            entity.Property(e => e.MaNcc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaNCC");
            entity.Property(e => e.MaNv)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaNV");
            entity.Property(e => e.MaPhieuNhap)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.NgayNhap).HasColumnType("datetime");
            entity.Property(e => e.TenNcc)
                .HasMaxLength(100)
                .HasColumnName("TenNCC");
            entity.Property(e => e.TenNhanVien).HasMaxLength(60);
            entity.Property(e => e.TongTien).HasColumnType("decimal(15, 0)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
