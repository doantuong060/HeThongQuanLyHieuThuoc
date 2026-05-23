IF DB_ID('HieuThuocDB') IS NOT NULL
    DROP DATABASE HieuThuocDB;
GO

CREATE DATABASE HieuThuocDB
    COLLATE Vietnamese_CI_AS;
GO

USE HieuThuocDB;
GO

-- ============================================================
-- BẢNG DANH MỤC CƠ BẢN
-- ============================================================

-- 1. VaiTro
CREATE TABLE VaiTro (
    MaVaiTro  VARCHAR(10)   NOT NULL,
    TenVaiTro NVARCHAR(30)  NOT NULL,
    MoTa      NVARCHAR(100) NULL,
    CONSTRAINT PK_VaiTro     PRIMARY KEY (MaVaiTro),
    CONSTRAINT UQ_VaiTro_Ten UNIQUE (TenVaiTro)
);
GO

-- 2. NhanVien
CREATE TABLE NhanVien (
    MaNV        VARCHAR(10)  NOT NULL,
    HoTen       NVARCHAR(60) NOT NULL,
    GioiTinh    NVARCHAR(3)  NOT NULL CHECK (GioiTinh IN (N'Nam', N'Nữ')), 
    NgaySinh    DATE         NOT NULL,
    SoDienThoai NVARCHAR(15) NOT NULL,
    Email       NVARCHAR(80) NULL,
    NgayVaoLam  DATE         NOT NULL DEFAULT GETDATE(),
    TrangThai   BIT          NOT NULL DEFAULT 1,
    CONSTRAINT PK_NhanVien PRIMARY KEY (MaNV),
    CONSTRAINT UQ_NV_SDT   UNIQUE (SoDienThoai)
);

-- 3. TaiKhoan
CREATE TABLE TaiKhoan (
    MaNV            VARCHAR(10)   NOT NULL,
    TenDangNhap     NVARCHAR(40)  NOT NULL,
    MatKhau         NVARCHAR(256) NOT NULL,
    NgayTao         DATETIME      NOT NULL DEFAULT GETDATE(),
    LanDangNhapCuoi DATETIME      NULL,
    MaVaiTro        VARCHAR(10)   NOT NULL,
    CONSTRAINT PK_TaiKhoan       PRIMARY KEY (MaNV),
    CONSTRAINT UQ_TK_TenDangNhap UNIQUE (TenDangNhap),
    CONSTRAINT FK_TK_NhanVien    FOREIGN KEY (MaNV)     REFERENCES NhanVien(MaNV),
    CONSTRAINT FK_TK_VaiTro      FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro),
    CONSTRAINT CK_TK_TenDangNhap CHECK (TenDangNhap NOT LIKE '%[^a-zA-Z0-9]%'),
    CONSTRAINT CK_TK_MatKhau     CHECK (
        MatKhau COLLATE Latin1_General_BIN LIKE '%[A-Z]%' AND
        MatKhau COLLATE Latin1_General_BIN LIKE '%[a-z]%' AND
        MatKhau LIKE '%@%'
    )
);
GO

-- 4. LoaiThuoc
CREATE TABLE LoaiThuoc (
    MaLoai  VARCHAR(10)   NOT NULL,
    TenLoai NVARCHAR(50)  NOT NULL,
    MoTa    NVARCHAR(200) NULL,
    CONSTRAINT PK_LoaiThuoc  PRIMARY KEY (MaLoai),
    CONSTRAINT UQ_Loai_Ten   UNIQUE (TenLoai)
);
GO

-- 5. LoaiVatTu
CREATE TABLE LoaiVatTu (
    MaLoaiVT  VARCHAR(10)   NOT NULL,
    TenLoaiVT NVARCHAR(50)  NOT NULL,
    MoTa      NVARCHAR(200) NULL,
    CONSTRAINT PK_LoaiVatTu     PRIMARY KEY (MaLoaiVT),
    CONSTRAINT UQ_LoaiVatTu_Ten UNIQUE (TenLoaiVT)
);
GO

-- 6. DonViTinh
CREATE TABLE DonViTinh ( 
    MaDVT  VARCHAR(10)  NOT NULL,
    TenDVT NVARCHAR(20) NOT NULL,
    CONSTRAINT PK_DonViTinh PRIMARY KEY (MaDVT),
    CONSTRAINT UQ_DVT_Ten   UNIQUE (TenDVT)
);
GO

-- 7. TinhThanh
CREATE TABLE TinhThanh (
    MaTinhThanh  VARCHAR(10)  NOT NULL,
    TenTinhThanh NVARCHAR(60) NOT NULL,
    CONSTRAINT PK_TinhThanh     PRIMARY KEY (MaTinhThanh),
    CONSTRAINT UQ_TinhThanh_Ten UNIQUE (TenTinhThanh)
);
GO

-- 8. PhuongXa
CREATE TABLE PhuongXa (
    MaPhuongXa  VARCHAR(10)  NOT NULL,
    TenPhuongXa NVARCHAR(80) NOT NULL,
    MaTinhThanh VARCHAR(10)  NOT NULL,
    CONSTRAINT PK_PhuongXa           PRIMARY KEY (MaPhuongXa),
    CONSTRAINT FK_PhuongXa_TinhThanh FOREIGN KEY (MaTinhThanh) REFERENCES TinhThanh(MaTinhThanh)
);
GO

-- 9. NhaCungCap
CREATE TABLE NhaCungCap (
    MaNCC       VARCHAR(10)   NOT NULL,
    TenNCC      NVARCHAR(100) NOT NULL,
    DiaChi      NVARCHAR(150) NULL,
    MaPhuongXa  VARCHAR(10)   NULL,
    SoDienThoai NVARCHAR(15)  NULL,
    Email       NVARCHAR(80)  NULL,
    TrangThai   BIT           NOT NULL DEFAULT 1,
    CONSTRAINT PK_NhaCungCap   PRIMARY KEY (MaNCC),
    CONSTRAINT UQ_NCC_Ten      UNIQUE (TenNCC),
    CONSTRAINT FK_NCC_PhuongXa FOREIGN KEY (MaPhuongXa) REFERENCES PhuongXa(MaPhuongXa)
);
GO

-- 10. KhachHang
CREATE TABLE KhachHang (
    MaKH         VARCHAR(10)   NOT NULL,
    HoTen        NVARCHAR(60)  NOT NULL,
    SoDienThoai  NVARCHAR(15)  NOT NULL,
    DiaChi       NVARCHAR(150) NULL,
    MaPhuongXa   VARCHAR(10)   NULL,
    GhiChuBenhLy NVARCHAR(500) NULL,
    NgayDangKy   DATE          NOT NULL DEFAULT GETDATE(),
    TrangThai    BIT           NOT NULL DEFAULT 1,
    CONSTRAINT PK_KhachHang   PRIMARY KEY (MaKH),
    CONSTRAINT UQ_KH_SDT      UNIQUE (SoDienThoai),
    CONSTRAINT FK_KH_PhuongXa FOREIGN KEY (MaPhuongXa) REFERENCES PhuongXa(MaPhuongXa)
);
GO

-- 11. SanPham
CREATE TABLE SanPham (
    MaSP       VARCHAR(10)   NOT NULL,
    TenSP      NVARCHAR(100) NOT NULL,
    MaDVT      VARCHAR(10)   NOT NULL,
    GiaBan     DECIMAL(15,0) NOT NULL CHECK (GiaBan > 0),
    MucCanhBao INT           NOT NULL DEFAULT 20,
    LoaiSP     VARCHAR(10)   NOT NULL CHECK (LoaiSP IN ('THUOC', 'VATTU')),
    TrangThai  BIT           NOT NULL DEFAULT 1,
    CONSTRAINT PK_SanPham           PRIMARY KEY (MaSP),
    CONSTRAINT FK_SanPham_DonViTinh FOREIGN KEY (MaDVT) REFERENCES DonViTinh(MaDVT)
);
GO

-- 12. Thuoc
CREATE TABLE Thuoc (
    MaSP   VARCHAR(10)   NOT NULL,
    MaLoai VARCHAR(10)   NOT NULL,
    CanToa BIT           NOT NULL DEFAULT 0,
    GhiChu NVARCHAR(200) NULL,
    CONSTRAINT PK_Thuoc         PRIMARY KEY (MaSP),
    CONSTRAINT FK_Thuoc_SanPham FOREIGN KEY (MaSP)   REFERENCES SanPham(MaSP),
    CONSTRAINT FK_Thuoc_Loai    FOREIGN KEY (MaLoai) REFERENCES LoaiThuoc(MaLoai)
);
GO

-- 13. VatTuYTe
CREATE TABLE VatTuYTe (
    MaSP       VARCHAR(10)   NOT NULL,
    MaLoaiVT   VARCHAR(10)   NOT NULL,
    NhaSanXuat NVARCHAR(100) NULL,
    CONSTRAINT PK_VatTuYTe    PRIMARY KEY (MaSP),
    CONSTRAINT FK_VT_SanPham  FOREIGN KEY (MaSP)      REFERENCES SanPham(MaSP),
    CONSTRAINT FK_VT_LoaiVT   FOREIGN KEY (MaLoaiVT)  REFERENCES LoaiVatTu(MaLoaiVT)
);
GO

-- ============================================================
-- BẢNG NGHIỆP VỤ
-- ============================================================

-- 14. PhieuNhap
CREATE TABLE PhieuNhap (
    MaPhieuNhap VARCHAR(10)   NOT NULL,
    NgayNhap    DATETIME      NOT NULL DEFAULT GETDATE(),
    MaNCC       VARCHAR(10)   NOT NULL,
    MaNV        VARCHAR(10)   NOT NULL,
    GhiChu      NVARCHAR(200) NULL,
    CONSTRAINT PK_PhieuNhap PRIMARY KEY (MaPhieuNhap),
    CONSTRAINT FK_PN_NCC    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
    CONSTRAINT FK_PN_NV     FOREIGN KEY (MaNV)  REFERENCES NhanVien(MaNV)
);
GO

-- 15. LoHang
CREATE TABLE LoHang (
    SoLo          NVARCHAR(30)  NOT NULL,
    MaSP          VARCHAR(10)   NOT NULL,
    MaPhieuNhap   VARCHAR(10)   NOT NULL,
    GiaNhap       DECIMAL(15,0) NOT NULL CHECK (GiaNhap > 0),
    HanSuDung     DATE          NOT NULL,
    SoLuongNhap   INT           NOT NULL CHECK (SoLuongNhap > 0),
    SoLuongConLai INT           NOT NULL CHECK (SoLuongConLai >= 0),
    CONSTRAINT PK_LoHang           PRIMARY KEY (SoLo),
    CONSTRAINT FK_LH_SanPham       FOREIGN KEY (MaSP)        REFERENCES SanPham(MaSP),
    CONSTRAINT FK_LH_PhieuNhap     FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhap(MaPhieuNhap),
    CONSTRAINT CK_LH_ConLai_Le_Nhap CHECK (SoLuongConLai <= SoLuongNhap)
);
GO

CREATE INDEX IX_LH_HanSuDung ON LoHang(HanSuDung, SoLuongConLai);
GO

CREATE INDEX IX_LH_MaSP ON LoHang(MaSP);
GO

-- 16. HoaDon
CREATE TABLE HoaDon (
    MaHD    VARCHAR(10)   NOT NULL,
    NgayBan DATETIME      NOT NULL DEFAULT GETDATE(),
    MaNV    VARCHAR(10)   NOT NULL,
    MaKH    VARCHAR(10)   NULL,
    GhiChu  NVARCHAR(200) NULL,
    CONSTRAINT PK_HoaDon PRIMARY KEY (MaHD),
    CONSTRAINT FK_HD_NV  FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    CONSTRAINT FK_HD_KH  FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);
GO

-- 17. ChiTietHoaDon
CREATE TABLE ChiTietHoaDon (
    MaCTHD  VARCHAR(10)   NOT NULL,
    MaHD    VARCHAR(10)   NOT NULL,
    SoLo    NVARCHAR(30)  NOT NULL,
    SoLuong INT           NOT NULL CHECK (SoLuong > 0),
    DonGia  DECIMAL(15,0) NOT NULL CHECK (DonGia > 0),
    CONSTRAINT PK_ChiTietHoaDon PRIMARY KEY (MaCTHD),
    CONSTRAINT UQ_CTHD          UNIQUE (MaHD, SoLo),
    CONSTRAINT FK_CTHD_HoaDon   FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
    CONSTRAINT FK_CTHD_LoHang FOREIGN KEY (SoLo) REFERENCES LoHang(SoLo)
);
GO

CREATE INDEX IX_CTHD_MaHD ON ChiTietHoaDon(MaHD);
GO

CREATE INDEX IX_CTHD_SoLo ON ChiTietHoaDon(SoLo);
GO

-- ============================================================
-- TRIGGER: Tự động cập nhật TongTien vào HoaDon
-- ============================================================

ALTER TABLE HoaDon ADD TongTien DECIMAL(15,0) NULL;
GO

CREATE OR ALTER TRIGGER trg_HoaDon_UpdateTongTien
ON ChiTietHoaDon
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE hd
    SET TongTien = (
        SELECT ISNULL(SUM(SoLuong * DonGia), 0)
        FROM ChiTietHoaDon ct
        WHERE ct.MaHD = hd.MaHD
    )
    FROM HoaDon hd
    WHERE hd.MaHD IN (
        SELECT MaHD FROM inserted
        UNION
        SELECT MaHD FROM deleted
    );
END;
GO

-- ============================================================
-- TRIGGER: Tự động cập nhật TongTien vào PhieuNhap
-- ============================================================
ALTER TABLE PhieuNhap ADD TongTien DECIMAL(15,0) NULL;
GO

CREATE OR ALTER TRIGGER trg_PhieuNhap_UpdateTongTien
ON LoHang
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE pn
    SET pn.TongTien = (
        SELECT ISNULL(SUM(lh.GiaNhap * lh.SoLuongNhap), 0)
        FROM LoHang lh
        WHERE lh.MaPhieuNhap = pn.MaPhieuNhap
    )
    FROM PhieuNhap pn
    WHERE pn.MaPhieuNhap IN (
        SELECT MaPhieuNhap FROM inserted
        UNION
        SELECT MaPhieuNhap FROM deleted
    );
END;
GO

-- ====================================================================
-- TRIGGER: Tự động cập nhật SoLuongConLai khi tạo hóa đơn, hủy hóa đơn
-- ====================================================================
CREATE OR ALTER TRIGGER trg_CTHD_TruKho
ON ChiTietHoaDon
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Cộng lại kho cho các dòng bị xóa hoặc sửa (dòng cũ)
    UPDATE lh
    SET lh.SoLuongConLai = lh.SoLuongConLai + d.SoLuong
    FROM LoHang lh
    INNER JOIN deleted d ON d.SoLo = lh.SoLo;

    -- Trừ kho cho các dòng mới INSERT hoặc sau UPDATE
    UPDATE lh
    SET lh.SoLuongConLai = lh.SoLuongConLai - i.SoLuong
    FROM LoHang lh
    INNER JOIN inserted i ON i.SoLo = lh.SoLo;
END;
GO

-- ============================================================
-- VIEW: Tổng tiền PhieuNhap
-- ============================================================
CREATE OR ALTER VIEW vw_TongTienPhieuNhap AS
SELECT
    pn.MaPhieuNhap,
    pn.NgayNhap,
    pn.MaNCC,
    ncc.TenNCC,
    pn.MaNV,
    nv.HoTen   AS TenNhanVien,
    pn.GhiChu,
    ISNULL(pn.TongTien, 0) AS TongTien
FROM PhieuNhap pn
INNER JOIN NhaCungCap ncc ON ncc.MaNCC = pn.MaNCC
INNER JOIN NhanVien   nv  ON nv.MaNV   = pn.MaNV;
GO

-- ============================================================
-- VIEW: Tổng tiền HoaDon (sử dụng TongTien từ bảng HoaDon)
-- ============================================================
CREATE OR ALTER VIEW vw_TongTienHoaDon AS
SELECT
    hd.MaHD,
    hd.NgayBan,
    hd.MaNV,
    nv.HoTen  AS TenNhanVien,
    hd.MaKH,
    kh.HoTen  AS TenKhachHang,
    hd.GhiChu,
    ISNULL(hd.TongTien, 0) AS TongTien
FROM HoaDon hd
INNER JOIN NhanVien       nv ON nv.MaNV  = hd.MaNV
LEFT  JOIN KhachHang      kh ON kh.MaKH  = hd.MaKH;
GO

-- ============================================================
-- VIEW: Tồn kho sản phẩm 
-- ============================================================
CREATE OR ALTER VIEW vw_TonKhoSanPham AS
SELECT
    sp.MaSP,
    sp.TenSP,
    sp.MaDVT,
    dvt.TenDVT,
    sp.GiaBan,
    sp.MucCanhBao,
    sp.LoaiSP,
    sp.TrangThai,
    ISNULL(SUM(lh.SoLuongConLai), 0) AS SoLuongTon,
    CASE
        WHEN ISNULL(SUM(lh.SoLuongConLai), 0) <= sp.MucCanhBao THEN N'⚠ Cần nhập thêm'
        ELSE N'Bình thường'
    END AS TrangThaiTon
FROM SanPham sp
LEFT JOIN LoHang    lh  ON lh.MaSP  = sp.MaSP
LEFT JOIN DonViTinh dvt ON dvt.MaDVT = sp.MaDVT
GROUP BY sp.MaSP, sp.TenSP, sp.MaDVT, dvt.TenDVT,
         sp.GiaBan, sp.MucCanhBao, sp.LoaiSP, sp.TrangThai;
GO

-- ============================================================
-- VIEW: Điểm tích lũy khách hàng
-- ============================================================
CREATE OR ALTER VIEW vw_DiemTichLuyKhachHang AS
SELECT
    kh.MaKH,
    kh.HoTen,
    kh.SoDienThoai,
    kh.DiaChi,
    p.TenPhuongXa,
    t.TenTinhThanh,
    kh.GhiChuBenhLy,
    kh.NgayDangKy,
    kh.TrangThai,
    ISNULL(SUM(hd.TongTien) / 100000, 0) AS DiemTichLuy,
    ISNULL(SUM(hd.TongTien), 0)           AS TongChiTieu
FROM KhachHang kh
LEFT JOIN PhuongXa    p  ON p.MaPhuongXa  = kh.MaPhuongXa
LEFT JOIN TinhThanh   t  ON t.MaTinhThanh = p.MaTinhThanh
LEFT JOIN HoaDon      hd ON hd.MaKH       = kh.MaKH
GROUP BY kh.MaKH, kh.HoTen, kh.SoDienThoai, kh.DiaChi,
         p.TenPhuongXa, t.TenTinhThanh,
         kh.GhiChuBenhLy, kh.NgayDangKy, kh.TrangThai;
GO

-- ============================================================
-- VIEW: Kiểm tra hạn dùng (FEFO)
-- ============================================================
CREATE OR ALTER VIEW vw_KiemTraHanDung AS
SELECT
    sp.MaSP,
    sp.TenSP,
    sp.LoaiSP,
    dvt.TenDVT,
    lh.SoLo,
    lh.HanSuDung,
    lh.SoLuongConLai,
    DATEDIFF(DAY, GETDATE(), lh.HanSuDung) AS SoNgayConLai,
    CASE
        WHEN DATEDIFF(DAY, GETDATE(), lh.HanSuDung) < 0  THEN N'Quá hạn'
        WHEN DATEDIFF(DAY, GETDATE(), lh.HanSuDung) <= 30 THEN N'Sắp hết hạn (≤30 ngày)'
        WHEN DATEDIFF(DAY, GETDATE(), lh.HanSuDung) <= 60 THEN N'Cận hạn (≤60 ngày)'
        ELSE N'Còn dài hạn'
    END AS TrangThaiHan
FROM LoHang  lh
INNER JOIN SanPham    sp  ON sp.MaSP  = lh.MaSP
INNER JOIN DonViTinh  dvt ON dvt.MaDVT = sp.MaDVT
WHERE lh.SoLuongConLai > 0
  AND sp.TrangThai = 1;
GO

-- ============================================================
-- VIEW: Lịch sử mua hàng theo số điện thoại
-- ============================================================
CREATE OR ALTER VIEW vw_LichSuMuaHang AS
SELECT
    kh.SoDienThoai,
    kh.HoTen        AS TenKhachHang,
    hd.MaHD,
    hd.NgayBan,
    nv.HoTen        AS TenNhanVienBan,
    sp.MaSP,
    sp.TenSP,
    sp.LoaiSP,
    ct.SoLuong,
    ct.DonGia,
    (ct.SoLuong * ct.DonGia) AS ThanhTien,
    lh.SoLo,
    lh.HanSuDung,
    hd.TongTien     AS TongTienHoaDon,
    hd.GhiChu
FROM HoaDon hd
LEFT JOIN KhachHang         kh  ON kh.MaKH  = hd.MaKH
INNER JOIN NhanVien          nv  ON nv.MaNV  = hd.MaNV
INNER JOIN ChiTietHoaDon     ct  ON ct.MaHD  = hd.MaHD
INNER JOIN LoHang            lh  ON lh.SoLo  = ct.SoLo
INNER JOIN SanPham           sp  ON sp.MaSP  = lh.MaSP;
GO

-- ============================================================
-- DỮ LIỆU MẪU
-- ============================================================

INSERT INTO VaiTro (MaVaiTro, TenVaiTro, MoTa) VALUES
('VT01', N'QuanLy',      N'Quản lý / Chủ hiệu thuốc — toàn quyền'),
('VT02', N'DuocSi',      N'Dược sĩ bán hàng'),
('VT03', N'NhanVienKho', N'Nhân viên kho — nhập hàng, kiểm kho');
GO

INSERT INTO NhanVien (MaNV, HoTen, GioiTinh, NgaySinh, SoDienThoai, Email, NgayVaoLam) VALUES
('NV0001', N'Nguyễn Thị Mai', N'Nữ',  '1990-05-15', '0901234567', N'maint@gmail.com',  '2020-01-01'),
('NV0002', N'Trần Văn Hùng',  N'Nam', '1995-08-20', '0912345678', N'hungtv@gmail.com', '2021-03-15'),
('NV0003', N'Lê Thị Hoa',     N'Nữ',  '1993-03-10', '0923456789', N'hoalt@gmail.com',  '2021-06-01'),
('NV0004', N'Phạm Văn Bình',  N'Nam', '1988-11-25', '0934567890', N'binhpv@gmail.com', '2020-08-10'),
('NV0005', N'Võ Thị Lan',     N'Nữ',  '1997-07-04', '0945678901', N'lanvt@gmail.com',  '2022-01-20');
GO

INSERT INTO TaiKhoan (MaNV, TenDangNhap, MatKhau, MaVaiTro) VALUES
('NV0001', 'maiAdmin',    'Mai@123',  'VT01'),
('NV0002', 'hungCashier', 'Hung@123', 'VT02'),
('NV0003', 'hoaCashier',  'Hoa@123',  'VT02'),
('NV0004', 'binhKho',     'Binh@123', 'VT03'),
('NV0005', 'lanCashier',  'Lan@123',  'VT02');
GO

INSERT INTO LoaiThuoc (MaLoai, TenLoai, MoTa) VALUES
('LT01', N'Kháng sinh',          N'Thuốc kháng khuẩn'),
('LT02', N'Giảm đau - Hạ sốt',   NULL),
('LT03', N'Vitamin & Khoáng chất',NULL),
('LT04', N'Tim mạch',             NULL),
('LT05', N'Tiêu hóa',             NULL),
('LT06', N'Hô hấp',               NULL),
('LT07', N'Ngoài da',             NULL),
('LT08', N'Mắt - Tai - Mũi',      NULL);
GO

INSERT INTO LoaiVatTu (MaLoaiVT, TenLoaiVT, MoTa) VALUES
('LVT01', N'Băng gạc',           N'Băng cuộn, gạc y tế'),
('LVT02', N'Dụng cụ tiêu hao',   N'Kim tiêm, dây truyền'),
('LVT03', N'Thiết bị nhỏ',       N'Nhiệt kế, huyết áp kế'),
('LVT04', N'Dung dịch sát khuẩn',N'Cồn, nước muối');
GO

INSERT INTO DonViTinh (MaDVT, TenDVT) VALUES
('DVT01', N'Viên'), ('DVT02', N'Viên sủi'), ('DVT03', N'Chai'), ('DVT04', N'Lọ'),
('DVT05', N'Ống'),  ('DVT06', N'Gói'),      ('DVT07', N'Hộp'),  ('DVT08', N'Tuýp');
GO

INSERT INTO TinhThanh (MaTinhThanh, TenTinhThanh) VALUES
('TT01', N'Hà Nội'), ('TT02', N'TP. Hồ Chí Minh'), ('TT03', N'Đà Nẵng'),
('TT04', N'Hải Phòng'), ('TT05', N'Quảng Nam');
GO

INSERT INTO PhuongXa (MaPhuongXa, TenPhuongXa, MaTinhThanh) VALUES
('PX01', N'Phường Hàng Bạc',        'TT01'),
('PX02', N'Phường Tràng Tiền',       'TT01'),
('PX03', N'Phường Thảo Điền',        'TT02'),
('PX04', N'Phường Mỹ An',            'TT03'),
('PX05', N'Phường Hiệp Bình Chánh',  'TT02');
GO

INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, MaPhuongXa, SoDienThoai, Email) VALUES
('NCC0001', N'Dược phẩm Trung ương 1', N'Số 15 Hàng Bạc',       'PX01', '02438255140', N'contact@cgmail.com'),
('NCC0002', N'Nhà thuốc Tràng Tiền',   N'Số 2 Tràng Tiền',       'PX02', '02438242485', N'info@gmail.com'),
('NCC0003', N'Dược phẩm An Khang',     N'Số 10 Xuân Thủy',       'PX03', '02838994567', N'cskh@gmail.com'),
('NCC0004', N'Pymepharco Đà Nẵng',     N'170 Trần Hưng Đạo',     'PX04', '02363842364', N'danang@gmail.com'),
('NCC0005', N'Công ty Dược OPV',       N'Số 12 Đường số 23',     'PX05', '02839302345', N'opv@gmail.com');
GO

INSERT INTO KhachHang (MaKH, HoTen, SoDienThoai, DiaChi, MaPhuongXa, GhiChuBenhLy) VALUES
('KH0001', N'Nguyễn Văn An',  '0987654321', N'Số 123 Hàng Bạc',  'PX01', N'Dị ứng Penicillin'),
('KH0002', N'Trần Thị Bình',  '0978123456', N'Số 45 Tràng Tiền', 'PX02', N'Tiền sử hen suyễn'),
('KH0003', N'Lê Văn Cường',   '0965234789', N'Số 78 Thảo Điền',  'PX03', N'Cao huyết áp'),
('KH0004', N'Phạm Thị Dung',  '0956347812', N'Số 90 Mỹ An',      'PX04', NULL);
GO

INSERT INTO SanPham (MaSP, TenSP, MaDVT, GiaBan, MucCanhBao, LoaiSP, TrangThai) VALUES
('TH0001', N'Amoxicillin 500mg',      'DVT01',   4500, 30, 'THUOC', 1),
('TH0002', N'Paracetamol 500mg',      'DVT01',   1200, 50, 'THUOC', 1),
('TH0003', N'Ibuprofen 400mg',        'DVT01',   2500, 30, 'THUOC', 1),
('TH0004', N'Vitamin C 1000mg',       'DVT02',   3500, 40, 'THUOC', 1),
('TH0005', N'Atorvastatin 20mg',      'DVT01',   8500, 20, 'THUOC', 1),
('TH0006', N'Omeprazole 20mg',        'DVT01',   6000, 25, 'THUOC', 1),
('TH0007', N'Salbutamol 2mg',         'DVT01',   3200, 20, 'THUOC', 1),
('TH0008', N'Metronidazole 250mg',    'DVT01',   2800, 30, 'THUOC', 1),
('TH0009', N'Cetirizine 10mg',        'DVT01',   5500, 20, 'THUOC', 1),
('TH0010', N'Vitamin B Complex',      'DVT01',   4200, 40, 'THUOC', 1),
('TH0011', N'Losartan 50mg',          'DVT01',   9500, 15, 'THUOC', 1),
('TH0012', N'Loperamide 2mg',         'DVT01',   3800, 25, 'THUOC', 1),
('TH0013', N'Betadine 10% 30ml',      'DVT03',  28000, 10, 'THUOC', 1),
('TH0014', N'Natri clorid 0.9%',      'DVT04',  18000, 15, 'THUOC', 1),
('TH0015', N'Augmentin 625mg',        'DVT01',  12000, 20, 'THUOC', 1),
('VT0001', N'Gạc y tế 10x10cm',       'DVT06',   2000, 50, 'VATTU', 1),
('VT0002', N'Băng cuộn 6cm x 5m',     'DVT07',   8000, 30, 'VATTU', 1),
('VT0003', N'Bông y tế 100g',         'DVT06',  15000, 20, 'VATTU', 1),
('VT0004', N'Kim tiêm 5ml',           'DVT07',   1200,100, 'VATTU', 1),
('VT0005', N'Dây truyền dịch',        'DVT07',   5500, 50, 'VATTU', 1),
('VT0006', N'Nhiệt kế điện tử',       'DVT07',  75000, 10, 'VATTU', 1),
('VT0007', N'Máy đo huyết áp',        'DVT07', 450000,  5, 'VATTU', 1),
('VT0008', N'Cồn 70° 100ml',          'DVT03',  22000, 30, 'VATTU', 1),
('VT0009', N'Nước muối sinh lý 10ml', 'DVT04',   8000, 50, 'VATTU', 1);
GO

INSERT INTO Thuoc (MaSP, MaLoai, CanToa, GhiChu) VALUES
('TH0001','LT01',1,N'Kháng sinh phổ rộng'),
('TH0002','LT02',0,N'Hạ sốt giảm đau'),
('TH0003','LT02',0,N'Kháng viêm'),
('TH0004','LT03',0,N'Tăng đề kháng'),
('TH0005','LT04',1,N'Hạ cholesterol'),
('TH0006','LT05',0,N'Giảm acid dạ dày'),
('TH0007','LT06',1,N'Giãn phế quản'),
('TH0008','LT01',1,N'Kháng khuẩn kị khí'),
('TH0009','LT06',0,N'Kháng histamin'),
('TH0010','LT03',0,N'Vitamin nhóm B'),
('TH0011','LT04',1,N'Hạ huyết áp'),
('TH0012','LT05',0,N'Trị tiêu chảy'),
('TH0013','LT07',0,N'Sát khuẩn'),
('TH0014','LT08',0,N'Rửa mắt'),
('TH0015','LT01',1,N'Kháng sinh kết hợp');
GO

INSERT INTO VatTuYTe (MaSP, MaLoaiVT, NhaSanXuat) VALUES
('VT0001','LVT01',N'Thăng Long'),
('VT0002','LVT01',N'B.Braun'),
('VT0003','LVT01',N'Việt Nam'),
('VT0004','LVT02',N'Mediplast'),
('VT0005','LVT02',N'B.Braun'),
('VT0006','LVT03',N'Omron'),
('VT0007','LVT03',N'Omron'),
('VT0008','LVT04',N'Phúc An'),
('VT0009','LVT04',N'Medisafe');
GO

INSERT INTO PhieuNhap (MaPhieuNhap, NgayNhap, MaNCC, MaNV, GhiChu) VALUES
('PN0001','2025-01-05','NCC0001','NV0004',N'Nhập kỳ tháng 1'),
('PN0002','2025-01-12','NCC0002','NV0004',N'Nhập kỳ tháng 1 đợt 2'),
('PN0003','2025-02-03','NCC0003','NV0004',N'Nhập tháng 2'),
('PN0004','2025-02-20','NCC0001','NV0004',N'Bổ sung tháng 2'),
('PN0005','2025-03-10','NCC0004','NV0004',N'Nhập tháng 3'),
('PN0006','2025-03-25','NCC0005','NV0004',N'Nhập tháng 3 đợt 2'),
('PN0007','2025-04-08','NCC0002','NV0004',N'Nhập tháng 4'),
('PN0008','2025-04-22','NCC0001','NV0004',N'Nhập tháng 4 đợt 2');
GO

INSERT INTO LoHang (SoLo, MaSP, MaPhieuNhap, GiaNhap, HanSuDung, SoLuongNhap, SoLuongConLai) VALUES
('LOT-2501A','TH0001','PN0001',  3500,'2026-12-31',100, 100),
('LOT-2501B','TH0002','PN0001',   900,'2027-06-30',200,200),
('LOT-2501C','TH0004','PN0001',  2800,'2027-03-31',150,150),
('LOT-2502A','TH0003','PN0002',  1800,'2026-09-30',100, 100),
('LOT-2502B','TH0006','PN0002',  4500,'2026-11-30', 80, 80),
('LOT-2503A','TH0005','PN0003',  6500,'2027-01-31', 50, 50),
('LOT-2503B','TH0007','PN0003',  2400,'2026-08-31', 60, 60),
('LOT-2503C','TH0011','PN0003',  7800,'2027-02-28', 40, 40),
('LOT-2504A','TH0008','PN0004',  2100,'2026-10-31',100,100),
('LOT-2504B','TH0009','PN0004',  4200,'2027-04-30', 80, 80),
('LOT-2505A','TH0010','PN0005',  3200,'2027-05-31',120,120),
('LOT-2505B','TH0012','PN0005',  2900,'2026-07-31', 90, 90),
('LOT-2505C','TH0013','PN0005', 22000,'2026-06-30', 30, 30),
('LOT-2506A','TH0014','PN0006', 14000,'2026-05-31', 50, 50),
('LOT-2506B','TH0015','PN0006',  9500,'2027-03-31', 40, 40),
('LOT-2507A','TH0001','PN0007',  3500,'2027-01-31', 80, 80),
('LOT-2507B','TH0002','PN0007',   900,'2027-08-30',150,150),
('LOT-2507C','TH0003','PN0007',  1800,'2026-12-31', 70, 70),
('LOT-2508A','TH0004','PN0008',  2800,'2027-06-30',100,100),
('LOT-2508B','TH0005','PN0008',  6500,'2027-04-30', 60, 60),
('LOT-VT01A','VT0001','PN0001',  1500,'2027-12-31',300,300),
('LOT-VT01B','VT0002','PN0001',  6000,'2027-06-30',100, 100),
('LOT-VT01C','VT0004','PN0001',   900,'2026-09-30',500,500),
('LOT-VT02A','VT0005','PN0002',  4200,'2027-03-31',200,200),
('LOT-VT02B','VT0008','PN0002', 18000,'2027-10-31',100, 100),
('LOT-VT03A','VT0006','PN0003', 58000,'2027-12-31', 30, 30),
('LOT-VT03B','VT0007','PN0003',380000,'2027-12-31', 10, 10),
('LOT-VT03C','VT0009','PN0003',  6000,'2026-12-31',200,200);
GO

INSERT INTO HoaDon (MaHD, NgayBan, MaNV, MaKH, GhiChu) VALUES
('HD0001','2025-01-08','NV0002','KH0001',N'Khách quen - có toa'),
('HD0002','2025-01-09','NV0003',NULL,    N'Khách lẻ'),
('HD0003','2025-01-15','NV0002','KH0002',N'Có toa bác sĩ'),
('HD0004','2025-02-05','NV0005',NULL,    NULL),
('HD0005','2025-02-10','NV0003','KH0003',N'Khách quen'),
('HD0006','2025-02-18','NV0002',NULL,    N'Khách lẻ'),
('HD0007','2025-03-02','NV0005','KH0004',NULL),
('HD0008','2025-03-14','NV0003',NULL,    N'Có toa — kháng sinh'),
('HD0009','2025-03-20','NV0002','KH0001',N'Tái khám'),
('HD0010','2025-04-01','NV0005',NULL,    N'Khách lẻ'),
('HD0011','2025-04-07','NV0003','KH0002',NULL),
('HD0012','2025-04-15','NV0002',NULL,    N'Có toa');
GO

INSERT INTO ChiTietHoaDon (MaCTHD, MaHD, SoLo, SoLuong, DonGia) VALUES
('CTHD0001','HD0001','LOT-2501B', 2, 1200),
('CTHD0002','HD0001','LOT-2501C', 1, 3500),
('CTHD0003','HD0001','LOT-VT01A',10, 2000),
('CTHD0004','HD0001','LOT-VT01B', 2, 8000),
('CTHD0005','HD0002','LOT-2501A', 1, 4500),
('CTHD0006','HD0002','LOT-2502A', 2, 2500),
('CTHD0007','HD0003','LOT-2503A', 1, 8500),
('CTHD0008','HD0003','LOT-2501A', 2, 4500),
('CTHD0009','HD0004','LOT-2504B', 1, 5500),
('CTHD0010','HD0004','LOT-2505A', 2, 4200),
('CTHD0011','HD0005','LOT-2501B', 3, 1200),
('CTHD0012','HD0005','LOT-2502B', 1, 6000),
('CTHD0013','HD0005','LOT-VT03A', 1, 75000),
('CTHD0014','HD0005','LOT-VT02B', 2, 22000),
('CTHD0015','HD0006','LOT-2505C', 1, 28000),
('CTHD0016','HD0006','LOT-2506A', 2, 18000),
('CTHD0017','HD0007','LOT-2505B', 1, 3800),
('CTHD0018','HD0007','LOT-2501C', 2, 3500),
('CTHD0019','HD0008','LOT-2504A', 1, 2800),
('CTHD0020','HD0008','LOT-2503B', 1, 3200),
('CTHD0021','HD0009','LOT-2503C', 1, 9500),
('CTHD0022','HD0009','LOT-2501B', 5, 1200),
('CTHD0023','HD0010','LOT-2506B', 1, 12000),
('CTHD0024','HD0010','LOT-2504B', 2, 5500),
('CTHD0025','HD0010','LOT-VT03C', 5, 8000),
('CTHD0026','HD0010','LOT-VT01C',20, 1200),
('CTHD0027','HD0011','LOT-2502A', 2, 2500),
('CTHD0028','HD0011','LOT-2502B', 1, 6000),
('CTHD0029','HD0012','LOT-2503A', 1, 8500),
('CTHD0030','HD0012','LOT-2501A', 1, 4500);
GO

UPDATE lh
SET lh.SoLuongConLai = lh.SoLuongConLai - (
    SELECT ISNULL(SUM(ct.SoLuong), 0)
    FROM ChiTietHoaDon ct
    WHERE ct.SoLo = lh.SoLo
)
FROM LoHang lh;
GO