CREATE DATABASE HieuThuocDB
    COLLATE Vietnamese_CI_AS;
GO
USE HieuThuocDB;
GO

-- ------------------------------------------------------------
-- Bảng 1: VaiTro
-- ------------------------------------------------------------
CREATE TABLE VaiTro (
    MaVaiTro    TINYINT       NOT NULL IDENTITY(1,1),
    TenVaiTro   NVARCHAR(30)  NOT NULL,
    MoTa        NVARCHAR(100) NULL,
    CONSTRAINT PK_VaiTro PRIMARY KEY (MaVaiTro),
    CONSTRAINT UQ_VaiTro_Ten UNIQUE (TenVaiTro)
);
GO

-- ------------------------------------------------------------
-- Bảng 2: NhanVien
-- ------------------------------------------------------------
CREATE TABLE NhanVien (
    MaNV          INT           NOT NULL IDENTITY(1,1),
    HoTen         NVARCHAR(60)  NOT NULL,
    GioiTinh      NCHAR(3)      NOT NULL CHECK (GioiTinh IN (N'Nam', N'Nữ')),
    NgaySinh      DATE          NOT NULL,
    SoDienThoai   NVARCHAR(15)  NOT NULL,
    Email         NVARCHAR(80)  NULL,
    MaVaiTro      TINYINT       NOT NULL,
    NgayVaoLam    DATE          NOT NULL DEFAULT GETDATE(),
    TrangThai     BIT           NOT NULL DEFAULT 1,    -- 1=Đang làm, 0=Nghỉ
    CONSTRAINT PK_NhanVien PRIMARY KEY (MaNV),
    CONSTRAINT FK_NV_VaiTro FOREIGN KEY (MaVaiTro)
        REFERENCES VaiTro(MaVaiTro),
    CONSTRAINT UQ_NV_SDT UNIQUE (SoDienThoai)
);
GO

-- ------------------------------------------------------------
-- Bảng 3: TaiKhoan
-- ------------------------------------------------------------
CREATE TABLE TaiKhoan (
    MaTK        INT          NOT NULL IDENTITY(1,1),
    TenDangNhap NVARCHAR(40) NOT NULL,
    MatKhau     NVARCHAR(256) NOT NULL,   -- lưu hash bcrypt
    NgayTao     DATETIME     NOT NULL DEFAULT GETDATE(),
    LanDangNhapCuoi DATETIME NULL,
    MaNV        INT          NOT NULL,
    CONSTRAINT PK_TaiKhoan PRIMARY KEY (MaTK),
    CONSTRAINT UQ_TK_TenDangNhap UNIQUE (TenDangNhap),
    CONSTRAINT UQ_TK_MaNV UNIQUE (MaNV),  -- 1 NV chỉ có 1 TK
    CONSTRAINT FK_TK_NhanVien FOREIGN KEY (MaNV)
        REFERENCES NhanVien(MaNV)
);
GO

-- ------------------------------------------------------------
-- Bảng 4: LoaiThuoc
-- ------------------------------------------------------------
CREATE TABLE LoaiThuoc (
    MaLoai   INT           NOT NULL IDENTITY(1,1),
    TenLoai  NVARCHAR(50)  NOT NULL,
    MoTa     NVARCHAR(200) NULL,
    CONSTRAINT PK_LoaiThuoc PRIMARY KEY (MaLoai),
    CONSTRAINT UQ_Loai_Ten UNIQUE (TenLoai)
);
GO

-- ------------------------------------------------------------
-- Bảng 5: NhaCungCap
-- ------------------------------------------------------------
CREATE TABLE NhaCungCap (
    MaNCC       INT          NOT NULL IDENTITY(1,1),
    TenNCC      NVARCHAR(100) NOT NULL,
    DiaChi      NVARCHAR(150) NULL,
    TinhThanh   NVARCHAR(50)  NULL,
    SoDienThoai NVARCHAR(15)  NULL,
    Email       NVARCHAR(80)  NULL,
    TrangThai   BIT           NOT NULL DEFAULT 1,
    CONSTRAINT PK_NhaCungCap PRIMARY KEY (MaNCC),
    CONSTRAINT UQ_NCC_Ten UNIQUE (TenNCC)
);
GO

-- ------------------------------------------------------------
-- Bảng 6: DonViTinh
-- ------------------------------------------------------------
CREATE TABLE DonViTinh (
    MaDVT   TINYINT      NOT NULL IDENTITY(1,1),
    TenDVT  NVARCHAR(20) NOT NULL,
    CONSTRAINT PK_DonViTinh PRIMARY KEY (MaDVT),
    CONSTRAINT UQ_DVT_Ten UNIQUE (TenDVT)
);
GO

-- ------------------------------------------------------------
-- Bảng 7: Thuoc
-- ------------------------------------------------------------
CREATE TABLE Thuoc (
    MaThuoc     INT            NOT NULL IDENTITY(1,1),
    TenThuoc    NVARCHAR(100)  NOT NULL,
    MaDVT       TINYINT        NOT NULL,
    GiaBan      DECIMAL(12,0)  NOT NULL CHECK (GiaBan >= 0),
    SoLuongTon  INT            NOT NULL DEFAULT 0 CHECK (SoLuongTon >= 0),
    MuongCanhBao INT           NOT NULL DEFAULT 20, -- cảnh báo khi tồn < ngưỡng
    MaLoai      INT            NOT NULL,
    CanToa      BIT            NOT NULL DEFAULT 0,  -- 1=cần toa bác sĩ
    GhiChu      NVARCHAR(200)  NULL,
    TrangThai   BIT            NOT NULL DEFAULT 1,
    CONSTRAINT PK_Thuoc PRIMARY KEY (MaThuoc),
    CONSTRAINT FK_Thuoc_LoaiThuoc FOREIGN KEY (MaLoai)
        REFERENCES LoaiThuoc(MaLoai),
    CONSTRAINT FK_Thuoc_DVT FOREIGN KEY (MaDVT)
        REFERENCES DonViTinh(MaDVT)
);
GO

-- ------------------------------------------------------------
-- Bảng 8: PhieuNhap
-- ------------------------------------------------------------
CREATE TABLE PhieuNhap (
    MaPhieuNhap INT           NOT NULL IDENTITY(1,1),
    NgayNhap    DATETIME      NOT NULL DEFAULT GETDATE(),
    MaNCC       INT           NOT NULL,
    MaNV        INT           NOT NULL,
    TongTien    DECIMAL(15,0) NOT NULL DEFAULT 0,
    GhiChu      NVARCHAR(200) NULL,
    CONSTRAINT PK_PhieuNhap PRIMARY KEY (MaPhieuNhap),
    CONSTRAINT FK_PN_NCC FOREIGN KEY (MaNCC)
        REFERENCES NhaCungCap(MaNCC),
    CONSTRAINT FK_PN_NV FOREIGN KEY (MaNV)
        REFERENCES NhanVien(MaNV)
);
GO

-- ------------------------------------------------------------
-- Bảng 9: ChiTietNhap
-- ------------------------------------------------------------
CREATE TABLE ChiTietNhap (
    MaCTN        INT           NOT NULL IDENTITY(1,1),
    MaPhieuNhap  INT           NOT NULL,
    MaThuoc      INT           NOT NULL,
    SoLuong      INT           NOT NULL CHECK (SoLuong > 0),
    GiaNhap      DECIMAL(12,0) NOT NULL CHECK (GiaNhap >= 0),
    SoLo         NVARCHAR(30)  NULL,
    HanSuDung    DATE          NULL,
    ThanhTien    AS (SoLuong * GiaNhap) PERSISTED,  -- computed column
    CONSTRAINT PK_ChiTietNhap PRIMARY KEY (MaCTN),
    CONSTRAINT UQ_CTN UNIQUE (MaPhieuNhap, MaThuoc, SoLo), -- cùng phiếu+thuốc+lô là duy nhất
    CONSTRAINT FK_CTN_PhieuNhap FOREIGN KEY (MaPhieuNhap)
        REFERENCES PhieuNhap(MaPhieuNhap),
    CONSTRAINT FK_CTN_Thuoc FOREIGN KEY (MaThuoc)
        REFERENCES Thuoc(MaThuoc)
);
GO

-- ------------------------------------------------------------
-- Bảng 10: HoaDon
-- ------------------------------------------------------------
CREATE TABLE HoaDon (
    MaHD      INT           NOT NULL IDENTITY(1,1),
    NgayBan   DATETIME      NOT NULL DEFAULT GETDATE(),
    MaNV      INT           NOT NULL,
    TongTien  DECIMAL(15,0) NOT NULL DEFAULT 0,
    GhiChu    NVARCHAR(200) NULL,
    CONSTRAINT PK_HoaDon PRIMARY KEY (MaHD),
    CONSTRAINT FK_HD_NV FOREIGN KEY (MaNV)
        REFERENCES NhanVien(MaNV)
);
GO

-- ------------------------------------------------------------
-- Bảng 11: ChiTietHoaDon
-- ------------------------------------------------------------
CREATE TABLE ChiTietHoaDon (
    MaCTHD    INT           NOT NULL IDENTITY(1,1),
    MaHD      INT           NOT NULL,
    MaThuoc   INT           NOT NULL,
    SoLuong   INT           NOT NULL CHECK (SoLuong > 0),
    DonGia    DECIMAL(12,0) NOT NULL CHECK (DonGia >= 0),
    ThanhTien AS (SoLuong * DonGia) PERSISTED,
    CONSTRAINT PK_ChiTietHoaDon PRIMARY KEY (MaCTHD),
    CONSTRAINT UQ_CTHD UNIQUE (MaHD, MaThuoc),
    CONSTRAINT FK_CTHD_HoaDon FOREIGN KEY (MaHD)
        REFERENCES HoaDon(MaHD),
    CONSTRAINT FK_CTHD_Thuoc FOREIGN KEY (MaThuoc)
        REFERENCES Thuoc(MaThuoc)
);
GO

-- ============================================================
-- PHẦN 3: INDEX (tối ưu truy vấn)
-- ============================================================
CREATE INDEX IX_Thuoc_MaLoai    ON Thuoc(MaLoai);
CREATE INDEX IX_Thuoc_TonKho    ON Thuoc(SoLuongTon);
CREATE INDEX IX_CTN_PhieuNhap   ON ChiTietNhap(MaPhieuNhap);
CREATE INDEX IX_CTN_Thuoc       ON ChiTietNhap(MaThuoc);
CREATE INDEX IX_CTHD_HoaDon     ON ChiTietHoaDon(MaHD);
CREATE INDEX IX_CTHD_Thuoc      ON ChiTietHoaDon(MaThuoc);
CREATE INDEX IX_HoaDon_NgayBan  ON HoaDon(NgayBan);
CREATE INDEX IX_PN_NgayNhap     ON PhieuNhap(NgayNhap);
GO

-- ============================================================
-- PHẦN 4: DỮ LIỆU MẪU (INSERT)
-- ============================================================

-- VaiTro
INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES
(N'QuanLy',       N'Quản lý / Chủ hiệu thuốc — toàn quyền'),
(N'ThuNgan',      N'Thu ngân / Dược sĩ bán hàng'),
(N'NhanVienKho',  N'Nhân viên kho — nhập hàng, kiểm kho');
GO

-- NhanVien
INSERT INTO NhanVien (HoTen, GioiTinh, NgaySinh, SoDienThoai, Email, MaVaiTro, NgayVaoLam) VALUES
(N'Nguyễn Thị Mai',   N'Nữ',  '1990-05-15', '0901234567', N'mai.nt@thuoc.vn',    1, '2020-01-01'),
(N'Trần Văn Hùng',    N'Nam', '1995-08-20', '0912345678', N'hung.tv@thuoc.vn',   2, '2021-03-15'),
(N'Lê Thị Hoa',       N'Nữ',  '1993-03-10', '0923456789', N'hoa.lt@thuoc.vn',    2, '2021-06-01'),
(N'Phạm Văn Bình',    N'Nam', '1988-11-25', '0934567890', N'binh.pv@thuoc.vn',   3, '2020-08-10'),
(N'Võ Thị Lan',       N'Nữ',  '1997-07-04', '0945678901', N'lan.vt@thuoc.vn',    2, '2022-01-20');
GO

-- TaiKhoan
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, MaNV) VALUES
('mai.admin',    '$2b$12$LKabc...hash_mai',   1),
('hung.cashier', '$2b$12$LKabc...hash_hung',  2),
('hoa.cashier',  '$2b$12$LKabc...hash_hoa',   3),
('binh.kho',     '$2b$12$LKabc...hash_binh',  4),
('lan.cashier',  '$2b$12$LKabc...hash_lan',   5);
GO

-- LoaiThuoc
INSERT INTO LoaiThuoc (TenLoai, MoTa) VALUES
(N'Kháng sinh',             N'Thuốc kháng khuẩn, diệt vi khuẩn'),
(N'Giảm đau - Hạ sốt',     N'Paracetamol, Ibuprofen và các loại tương tự'),
(N'Vitamin & Khoáng chất',  N'Bổ sung dinh dưỡng, tăng đề kháng'),
(N'Tim mạch',               N'Thuốc huyết áp, tim mạch, mỡ máu'),
(N'Tiêu hóa',               N'Thuốc dạ dày, tiêu hóa, nhuận tràng'),
(N'Hô hấp',                 N'Thuốc ho, long đờm, hen suyễn'),
(N'Ngoài da',               N'Kem bôi da, thuốc mỡ, dung dịch sát khuẩn'),
(N'Mắt - Tai - Mũi',        N'Nhỏ mắt, nhỏ tai, xịt mũi');
GO

-- DonViTinh
INSERT INTO DonViTinh (TenDVT) VALUES
(N'Viên'), (N'Viên sủi'), (N'Chai'), (N'Lọ'),
(N'Ống'), (N'Gói'), (N'Hộp'), (N'Tuýp');
GO

-- NhaCungCap
INSERT INTO NhaCungCap (TenNCC, DiaChi, TinhThanh, SoDienThoai, Email) VALUES
(N'Công ty Dược Hậu Giang',   N'KCN Hậu Giang',              N'Hậu Giang',     '02923891433', N'info@dhg.com.vn'),
(N'Công ty Pymepharco',        N'170 Trần Hưng Đạo',          N'Phú Yên',       '02573842364', N'contact@pymepharco.vn'),
(N'Công ty Dược Bình Định',    N'498 Nguyễn Thái Học',        N'Bình Định',     '02563822041', N'binhdinh@pharma.vn'),
(N'Công ty Zuellig Pharma',    N'Lô B2-3 KCN Nomura',         N'Hải Phòng',     '02253736789', N'vn@zuelligpharma.com'),
(N'Công ty Cổ phần OPV',       N'8 Cao Thắng, Q.3',           N'TP. Hồ Chí Minh','02839302345',N'opv@opv.vn');
GO

-- Thuoc (MaDVT: 1=Viên, 2=Viên sủi, 3=Chai, 4=Lọ)
INSERT INTO Thuoc (TenThuoc, MaDVT, GiaBan, SoLuongTon, MuongCanhBao, MaLoai, CanToa, GhiChu) VALUES
(N'Amoxicillin 500mg',          1,  4500,  200, 30, 1, 1, N'Kháng sinh phổ rộng'),
(N'Paracetamol 500mg',          1,  1200,  500, 50, 2, 0, N'Hạ sốt giảm đau thông dụng'),
(N'Ibuprofen 400mg',            1,  2500,  300, 30, 2, 0, N'Kháng viêm, giảm đau'),
(N'Vitamin C 1000mg',           2,  3500,  400, 40, 3, 0, N'Tăng sức đề kháng'),
(N'Atorvastatin 20mg',          1,  8500,  150, 20, 4, 1, N'Hạ cholesterol'),
(N'Omeprazole 20mg',            1,  6000,  180, 25, 5, 0, N'Giảm acid dạ dày'),
(N'Salbutamol 2mg',             1,  3200,  120, 20, 6, 1, N'Giãn phế quản'),
(N'Metronidazole 250mg',        1,  2800,  250, 30, 1, 1, N'Kháng khuẩn kị khí'),
(N'Cetirizine 10mg',            1,  5500,  180, 20, 6, 0, N'Kháng histamin, dị ứng'),
(N'Vitamin B Complex',          1,  4200,  350, 40, 3, 0, N'Bổ sung vitamin nhóm B'),
(N'Losartan 50mg',              1,  9500,   90, 15, 4, 1, N'Hạ huyết áp'),
(N'Loperamide 2mg',             1,  3800,  200, 25, 5, 0, N'Trị tiêu chảy'),
(N'Betadine 10% 30ml',          3, 28000,   60, 10, 7, 0, N'Sát khuẩn vết thương'),
(N'Natri clorid 0.9% nhỏ mắt', 4, 18000,   80, 15, 8, 0, N'Rửa mắt, dưỡng ẩm'),
(N'Augmentin 625mg',            1, 12000,  100, 20, 1, 1, N'Amoxicillin + acid clavulanic');
GO

-- PhieuNhap
INSERT INTO PhieuNhap (NgayNhap, MaNCC, MaNV, GhiChu) VALUES
('2025-01-05', 1, 4, N'Nhập kỳ tháng 1 đợt 1'),
('2025-01-12', 2, 4, N'Nhập kỳ tháng 1 đợt 2'),
('2025-02-03', 3, 4, N'Nhập tháng 2'),
('2025-02-20', 1, 4, N'Bổ sung tháng 2'),
('2025-03-10', 4, 4, N'Nhập tháng 3 đợt 1'),
('2025-03-25', 5, 4, N'Nhập tháng 3 đợt 2'),
('2025-04-08', 2, 4, N'Nhập tháng 4 đợt 1'),
('2025-04-22', 1, 4, N'Nhập tháng 4 đợt 2');
GO

-- ChiTietNhap
INSERT INTO ChiTietNhap (MaPhieuNhap, MaThuoc, SoLuong, GiaNhap, SoLo, HanSuDung) VALUES
(1,  1, 100, 3500, 'LOT-2501A', '2026-12-31'),
(1,  2, 200,  900, 'LOT-2501B', '2027-06-30'),
(1,  4, 150, 2800, 'LOT-2501C', '2027-03-31'),
(2,  3, 100, 1800, 'LOT-2502A', '2026-09-30'),
(2,  6,  80, 4500, 'LOT-2502B', '2026-11-30'),
(3,  5,  50, 6500, 'LOT-2503A', '2027-01-31'),
(3,  7,  60, 2400, 'LOT-2503B', '2026-08-31'),
(3, 11,  40, 7800, 'LOT-2503C', '2027-02-28'),
(4,  8, 100, 2100, 'LOT-2504A', '2026-10-31'),
(4,  9,  80, 4200, 'LOT-2504B', '2027-04-30'),
(5, 10, 120, 3200, 'LOT-2505A', '2027-05-31'),
(5, 12,  90, 2900, 'LOT-2505B', '2026-07-31'),
(5, 13,  30,22000, 'LOT-2505C', '2026-06-30'),
(6, 14,  50,14000, 'LOT-2506A', '2026-05-31'),
(6, 15,  40, 9500, 'LOT-2506B', '2027-03-31'),
(7,  1,  80, 3500, 'LOT-2507A', '2027-01-31'),
(7,  2, 150,  900, 'LOT-2507B', '2027-08-30'),
(7,  3,  70, 1800, 'LOT-2507C', '2026-12-31'),
(8,  4, 100, 2800, 'LOT-2508A', '2027-06-30'),
(8,  5,  60, 6500, 'LOT-2508B', '2027-04-30');
GO

-- Cập nhật TongTien PhieuNhap sau khi insert ChiTietNhap
UPDATE PhieuNhap SET TongTien = (
    SELECT ISNULL(SUM(ThanhTien),0)
    FROM ChiTietNhap
    WHERE ChiTietNhap.MaPhieuNhap = PhieuNhap.MaPhieuNhap
);
GO

-- HoaDon
INSERT INTO HoaDon (NgayBan, MaNV, GhiChu) VALUES
('2025-01-08', 2, N'Khách lẻ'),
('2025-01-09', 3, N'Khách lẻ'),
('2025-01-15', 2, N'Có toa bác sĩ'),
('2025-02-05', 5, NULL),
('2025-02-10', 3, N'Khách quen'),
('2025-02-18', 2, N'Khách lẻ'),
('2025-03-02', 5, NULL),
('2025-03-14', 3, N'Có toa — kháng sinh'),
('2025-03-20', 2, NULL),
('2025-04-01', 5, N'Khách lẻ'),
('2025-04-07', 3, NULL),
('2025-04-15', 2, N'Có toa');
GO

-- ChiTietHoaDon (DonGia = snapshot giá tại thời điểm bán)
INSERT INTO ChiTietHoaDon (MaHD, MaThuoc, SoLuong, DonGia) VALUES
(1,  2, 2,  1200),
(1,  4, 1,  3500),
(2,  1, 1,  4500),
(2,  3, 2,  2500),
(3,  5, 1,  8500),
(3,  1, 2,  4500),
(4,  9, 1,  5500),
(4, 10, 2,  4200),
(5,  2, 3,  1200),
(5,  6, 1,  6000),
(6, 13, 1, 28000),
(6, 14, 2, 18000),
(7, 12, 1,  3800),
(7,  4, 2,  3500),
(8,  8, 1,  2800),
(8,  7, 1,  3200),
(9, 11, 1,  9500),
(9,  2, 5,  1200),
(10,15, 1, 12000),
(10, 9, 2,  5500),
(11, 3, 2,  2500),
(11, 6, 1,  6000),
(12, 5, 1,  8500),
(12, 1, 1,  4500);
GO

-- Cập nhật TongTien HoaDon
UPDATE HoaDon SET TongTien = (
    SELECT ISNULL(SUM(ThanhTien),0)
    FROM ChiTietHoaDon
    WHERE ChiTietHoaDon.MaHD = HoaDon.MaHD
);
GO

-- ============================================================
-- PHẦN 5: STORED PROCEDURES
-- ============================================================

-- SP 1: Bán hàng — tạo hóa đơn + cập nhật tồn kho
CREATE OR ALTER PROCEDURE sp_BanHang
    @MaNV     INT,
    @DanhSach NVARCHAR(MAX),  -- JSON: [{"MaThuoc":1,"SoLuong":2},...]
    @GhiChu   NVARCHAR(200) = NULL,
    @MaHD_Out INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Tạo hóa đơn
        INSERT INTO HoaDon (NgayBan, MaNV, GhiChu)
        VALUES (GETDATE(), @MaNV, @GhiChu);
        SET @MaHD_Out = SCOPE_IDENTITY();

        -- Parse JSON và insert chi tiết
        INSERT INTO ChiTietHoaDon (MaHD, MaThuoc, SoLuong, DonGia)
        SELECT @MaHD_Out,
               j.MaThuoc,
               j.SoLuong,
               t.GiaBan
        FROM OPENJSON(@DanhSach)
             WITH (MaThuoc INT, SoLuong INT) AS j
        INNER JOIN Thuoc t ON t.MaThuoc = j.MaThuoc;

        -- Trừ tồn kho
        UPDATE t
        SET t.SoLuongTon = t.SoLuongTon - j.SoLuong
        FROM Thuoc t
        INNER JOIN (
            SELECT MaThuoc, SoLuong FROM ChiTietHoaDon WHERE MaHD = @MaHD_Out
        ) j ON j.MaThuoc = t.MaThuoc;

        -- Kiểm tra tồn kho không âm
        IF EXISTS (SELECT 1 FROM Thuoc WHERE SoLuongTon < 0)
        BEGIN
            RAISERROR(N'Tồn kho không đủ cho một hoặc nhiều sản phẩm', 16, 1);
        END

        -- Cập nhật tổng tiền hóa đơn
        UPDATE HoaDon
        SET TongTien = (SELECT SUM(ThanhTien) FROM ChiTietHoaDon WHERE MaHD = @MaHD_Out)
        WHERE MaHD = @MaHD_Out;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- SP 2: Nhập hàng — tạo phiếu nhập + cập nhật tồn kho
CREATE OR ALTER PROCEDURE sp_NhapHang
    @MaNCC      INT,
    @MaNV       INT,
    @DanhSach   NVARCHAR(MAX), -- JSON: [{"MaThuoc":1,"SoLuong":100,"GiaNhap":3500,"SoLo":"LOT","HanSuDung":"2027-01-01"},...]
    @GhiChu     NVARCHAR(200) = NULL,
    @MaPN_Out   INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO PhieuNhap (NgayNhap, MaNCC, MaNV, GhiChu)
        VALUES (GETDATE(), @MaNCC, @MaNV, @GhiChu);
        SET @MaPN_Out = SCOPE_IDENTITY();

        INSERT INTO ChiTietNhap (MaPhieuNhap, MaThuoc, SoLuong, GiaNhap, SoLo, HanSuDung)
        SELECT @MaPN_Out, MaThuoc, SoLuong, GiaNhap, SoLo, HanSuDung
        FROM OPENJSON(@DanhSach)
             WITH (MaThuoc INT, SoLuong INT, GiaNhap DECIMAL(12,0),
                   SoLo NVARCHAR(30), HanSuDung DATE);

        -- Cộng tồn kho
        UPDATE t
        SET t.SoLuongTon = t.SoLuongTon + j.SoLuong
        FROM Thuoc t
        INNER JOIN (
            SELECT MaThuoc, SoLuong FROM ChiTietNhap WHERE MaPhieuNhap = @MaPN_Out
        ) j ON j.MaThuoc = t.MaThuoc;

        -- Cập nhật tổng tiền phiếu nhập
        UPDATE PhieuNhap
        SET TongTien = (SELECT SUM(ThanhTien) FROM ChiTietNhap WHERE MaPhieuNhap = @MaPN_Out)
        WHERE MaPhieuNhap = @MaPN_Out;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- ============================================================
-- PHẦN 6: VIEWS
-- ============================================================

-- View 1: Danh sách thuốc đầy đủ (join tên loại, đơn vị)
CREATE OR ALTER VIEW vw_DanhSachThuoc AS
    SELECT
        t.MaThuoc,
        t.TenThuoc,
        d.TenDVT,
        l.TenLoai,
        t.GiaBan,
        t.SoLuongTon,
        t.MuongCanhBao,
        CASE WHEN t.SoLuongTon <= t.MuongCanhBao THEN N'⚠ Sắp hết' ELSE N'Bình thường' END AS TrangThaiTon,
        t.CanToa,
        t.TrangThai
    FROM Thuoc t
    INNER JOIN LoaiThuoc l ON l.MaLoai = t.MaLoai
    INNER JOIN DonViTinh d ON d.MaDVT = t.MaDVT;
GO

-- View 2: Thuốc cần cảnh báo (tồn thấp hoặc sắp hết hạn 60 ngày)
CREATE OR ALTER VIEW vw_CanhBao AS
    SELECT
        t.MaThuoc, t.TenThuoc, t.SoLuongTon, t.MuongCanhBao,
        N'Tồn kho thấp' AS LoaiCanhBao, NULL AS HanSuDung
    FROM Thuoc t
    WHERE t.SoLuongTon <= t.MuongCanhBao AND t.TrangThai = 1
    UNION ALL
    SELECT DISTINCT
        t.MaThuoc, t.TenThuoc, t.SoLuongTon, t.MuongCanhBao,
        N'Sắp hết hạn (≤60 ngày)', c.HanSuDung
    FROM Thuoc t
    INNER JOIN ChiTietNhap c ON c.MaThuoc = t.MaThuoc
    WHERE c.HanSuDung IS NOT NULL
      AND c.HanSuDung <= DATEADD(DAY, 60, GETDATE())
      AND t.TrangThai = 1;
GO

-- View 3: Báo cáo doanh thu theo tháng
CREATE OR ALTER VIEW vw_DoanhThuThang AS
    SELECT
        YEAR(NgayBan)  AS Nam,
        MONTH(NgayBan) AS Thang,
        COUNT(*)       AS SoHoaDon,
        SUM(TongTien)  AS TongDoanhThu,
        AVG(TongTien)  AS TrungBinhHoaDon
    FROM HoaDon
    GROUP BY YEAR(NgayBan), MONTH(NgayBan);
GO

-- View 4: Hóa đơn chi tiết đầy đủ
CREATE OR ALTER VIEW vw_HoaDonChiTiet AS
    SELECT
        hd.MaHD,
        hd.NgayBan,
        nv.HoTen          AS TenNhanVien,
        t.TenThuoc,
        d.TenDVT,
        ct.SoLuong,
        ct.DonGia,
        ct.ThanhTien,
        hd.TongTien,
        hd.GhiChu
    FROM HoaDon hd
    INNER JOIN NhanVien nv      ON nv.MaNV     = hd.MaNV
    INNER JOIN ChiTietHoaDon ct ON ct.MaHD     = hd.MaHD
    INNER JOIN Thuoc t          ON t.MaThuoc   = ct.MaThuoc
    INNER JOIN DonViTinh d      ON d.MaDVT     = t.MaDVT;
GO

-- View 5: Top thuốc bán chạy
CREATE OR ALTER VIEW vw_TopThuocBanChay AS
    SELECT
        t.MaThuoc,
        t.TenThuoc,
        l.TenLoai,
        SUM(ct.SoLuong)   AS TongSoLuongBan,
        SUM(ct.ThanhTien) AS TongDoanhThu
    FROM ChiTietHoaDon ct
    INNER JOIN Thuoc t    ON t.MaThuoc = ct.MaThuoc
    INNER JOIN LoaiThuoc l ON l.MaLoai = t.MaLoai
    GROUP BY t.MaThuoc, t.TenThuoc, l.TenLoai;
GO

-- ============================================================
-- PHẦN 7: KIỂM TRA KẾT QUẢ
-- ============================================================
SELECT N'=== KIỂM TRA DỮ LIỆU ===' AS ThongTin;
SELECT N'VaiTro'      AS Bang, COUNT(*) AS SoBan FROM VaiTro      UNION ALL
SELECT N'NhanVien',            COUNT(*) FROM NhanVien              UNION ALL
SELECT N'TaiKhoan',            COUNT(*) FROM TaiKhoan              UNION ALL
SELECT N'LoaiThuoc',           COUNT(*) FROM LoaiThuoc             UNION ALL
SELECT N'DonViTinh',           COUNT(*) FROM DonViTinh             UNION ALL
SELECT N'NhaCungCap',          COUNT(*) FROM NhaCungCap            UNION ALL
SELECT N'Thuoc',               COUNT(*) FROM Thuoc                 UNION ALL
SELECT N'PhieuNhap',           COUNT(*) FROM PhieuNhap             UNION ALL
SELECT N'ChiTietNhap',         COUNT(*) FROM ChiTietNhap           UNION ALL
SELECT N'HoaDon',              COUNT(*) FROM HoaDon                UNION ALL
SELECT N'ChiTietHoaDon',       COUNT(*) FROM ChiTietHoaDon;

SELECT N'=== CẢNH BÁO TỒN KHO / HẾT HẠN ===' AS ThongTin;
SELECT * FROM vw_CanhBao ORDER BY LoaiCanhBao;

SELECT N'=== DOANH THU THEO THÁNG ===' AS ThongTin;
SELECT * FROM vw_DoanhThuThang ORDER BY Nam, Thang;

SELECT N'=== TOP THUỐC BÁN CHẠY ===' AS ThongTin;
SELECT TOP 5 * FROM vw_TopThuocBanChay ORDER BY TongSoLuongBan DESC;
GO
