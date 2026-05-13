CREATE DATABASE HieuThuocDB
    COLLATE Vietnamese_CI_AS;
GO
USE HieuThuocDB;
GO

-- ------------------------------------------------------------
-- Bảng 1: VaiTro
-- ------------------------------------------------------------
CREATE TABLE VaiTro (
    MaVaiTro  VARCHAR(10)   NOT NULL, -- Đổi sang VARCHAR, định dạng VT01
    TenVaiTro NVARCHAR(30)  NOT NULL,
    MoTa      NVARCHAR(100) NULL,
    CONSTRAINT PK_VaiTro      PRIMARY KEY (MaVaiTro),
    CONSTRAINT UQ_VaiTro_Ten  UNIQUE (TenVaiTro)
);
GO

-- ------------------------------------------------------------
-- Bảng 2: NhanVien
-- ------------------------------------------------------------
CREATE TABLE NhanVien (
    MaNV        VARCHAR(10)  NOT NULL, -- Đổi sang VARCHAR, định dạng NV0001
    HoTen       NVARCHAR(60) NOT NULL,
    GioiTinh    NCHAR(3)     NOT NULL CHECK (GioiTinh IN (N'Nam', N'Nữ')),
    NgaySinh    DATE         NOT NULL,
    SoDienThoai NVARCHAR(15) NOT NULL,
    Email       NVARCHAR(80) NULL,
    MaVaiTro    VARCHAR(10)  NOT NULL,
    NgayVaoLam  DATE         NOT NULL DEFAULT GETDATE(),
    TrangThai   BIT          NOT NULL DEFAULT 1,   -- 1=Đang làm, 0=Nghỉ
    CONSTRAINT PK_NhanVien   PRIMARY KEY (MaNV),
    CONSTRAINT FK_NV_VaiTro  FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro),
    CONSTRAINT UQ_NV_SDT     UNIQUE (SoDienThoai)
);
GO

-- ------------------------------------------------------------
-- Bảng 3: TaiKhoan  (MaNV là PK — 1 nhân viên 1 tài khoản)
-- ------------------------------------------------------------
CREATE TABLE TaiKhoan (
    MaNV            VARCHAR(10)   NOT NULL,   -- PK đồng thời là FK
    TenDangNhap     NVARCHAR(40)  NOT NULL,
    MatKhau         NVARCHAR(256) NOT NULL,   
    NgayTao         DATETIME      NOT NULL DEFAULT GETDATE(),
    LanDangNhapCuoi DATETIME      NULL,
    CONSTRAINT PK_TaiKhoan          PRIMARY KEY (MaNV),
    CONSTRAINT UQ_TK_TenDangNhap    UNIQUE (TenDangNhap),
    CONSTRAINT FK_TK_NhanVien       FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    -- Tài khoản không chứa kí tự đặc biệt (chỉ chữ và số)
    CONSTRAINT CK_TK_TenDangNhap    CHECK (TenDangNhap NOT LIKE '%[^a-zA-Z0-9]%'),
    -- Mật khẩu phải có chữ in hoa, chữ thường và kí tự @
    CONSTRAINT CK_TK_MatKhau        CHECK (
        MatKhau COLLATE Latin1_General_BIN LIKE '%[A-Z]%' AND  -- Có chữ in hoa
        MatKhau COLLATE Latin1_General_BIN LIKE '%[a-z]%' AND  -- Có chữ thường
        MatKhau LIKE '%@%'                                     -- Có kí tự @
    )
);
GO

-- ------------------------------------------------------------
-- Bảng 4: LoaiThuoc
-- ------------------------------------------------------------
CREATE TABLE LoaiThuoc (
    MaLoai  VARCHAR(10)   NOT NULL, -- Định dạng LT001
    TenLoai NVARCHAR(50)  NOT NULL,
    MoTa    NVARCHAR(200) NULL,
    CONSTRAINT PK_LoaiThuoc   PRIMARY KEY (MaLoai),
    CONSTRAINT UQ_Loai_Ten    UNIQUE (TenLoai)
);
GO

-- ------------------------------------------------------------
-- Bảng 5: TinhThanh
-- ------------------------------------------------------------
CREATE TABLE TinhThanh (
    MaTinhThanh  VARCHAR(10)   NOT NULL, -- Định dạng TT001
    TenTinhThanh NVARCHAR(60) NOT NULL,
    CONSTRAINT PK_TinhThanh      PRIMARY KEY (MaTinhThanh),
    CONSTRAINT UQ_TinhThanh_Ten  UNIQUE (TenTinhThanh)
);
GO

-- ------------------------------------------------------------
-- Bảng 6: PhuongXa
-- ------------------------------------------------------------
CREATE TABLE PhuongXa (
    MaPhuongXa   VARCHAR(10)   NOT NULL, -- Định dạng PX001
    TenPhuongXa  NVARCHAR(80) NOT NULL,
    MaTinhThanh  VARCHAR(10)  NOT NULL,
    CONSTRAINT PK_PhuongXa         PRIMARY KEY (MaPhuongXa),
    CONSTRAINT FK_PhuongXa_TinhThanh FOREIGN KEY (MaTinhThanh) REFERENCES TinhThanh(MaTinhThanh)
);
GO

-- ------------------------------------------------------------
-- Bảng 7: NhaCungCap
-- ------------------------------------------------------------
CREATE TABLE NhaCungCap (
    MaNCC       VARCHAR(10)   NOT NULL, -- Định dạng NCC0001
    TenNCC      NVARCHAR(100) NOT NULL,
    DiaChi      NVARCHAR(150) NULL,   
    MaPhuongXa  VARCHAR(10)   NULL,   
    SoDienThoai NVARCHAR(15)  NULL,
    Email       NVARCHAR(80)  NULL,
    TrangThai   BIT           NOT NULL DEFAULT 1,
    CONSTRAINT PK_NhaCungCap  PRIMARY KEY (MaNCC),
    CONSTRAINT UQ_NCC_Ten     UNIQUE (TenNCC),
    CONSTRAINT FK_NCC_PhuongXa FOREIGN KEY (MaPhuongXa) REFERENCES PhuongXa(MaPhuongXa)
);
GO

-- ------------------------------------------------------------
-- Bảng 8: DonViTinh
-- ------------------------------------------------------------
CREATE TABLE DonViTinh (
    MaDVT  VARCHAR(10)   NOT NULL, -- Định dạng DVT001
    TenDVT NVARCHAR(20) NOT NULL,
    CONSTRAINT PK_DonViTinh  PRIMARY KEY (MaDVT),
    CONSTRAINT UQ_DVT_Ten    UNIQUE (TenDVT)
);
GO

-- ------------------------------------------------------------
-- Bảng 9: Thuoc
-- ------------------------------------------------------------
CREATE TABLE Thuoc (
    MaThuoc      VARCHAR(10)   NOT NULL, -- Định dạng TH0001
    TenThuoc     NVARCHAR(100) NOT NULL,
    MaDVT        VARCHAR(10)   NOT NULL,
    GiaBan       DECIMAL(12,0) NOT NULL CHECK (GiaBan >= 0),
    SoLuongTon   INT           NOT NULL DEFAULT 0 CHECK (SoLuongTon >= 0),
    MuongCanhBao INT           NOT NULL DEFAULT 20,   
    MaLoai       VARCHAR(10)   NOT NULL,
    CanToa       BIT           NOT NULL DEFAULT 0,    
    GhiChu       NVARCHAR(200) NULL,
    TrangThai    BIT           NOT NULL DEFAULT 1,
    CONSTRAINT PK_Thuoc          PRIMARY KEY (MaThuoc),
    CONSTRAINT FK_Thuoc_LoaiThuoc FOREIGN KEY (MaLoai) REFERENCES LoaiThuoc(MaLoai),
    CONSTRAINT FK_Thuoc_DVT       FOREIGN KEY (MaDVT)  REFERENCES DonViTinh(MaDVT)
);
GO

-- ------------------------------------------------------------
-- Bảng 10: PhieuNhap
-- ------------------------------------------------------------
CREATE TABLE PhieuNhap (
    MaPhieuNhap VARCHAR(10)   NOT NULL, -- Định dạng PN0001
    NgayNhap    DATETIME      NOT NULL DEFAULT GETDATE(),
    MaNCC       VARCHAR(10)   NOT NULL,
    MaNV        VARCHAR(10)   NOT NULL,
    TongTien    DECIMAL(15,0) NOT NULL DEFAULT 0,
    GhiChu      NVARCHAR(200) NULL,
    CONSTRAINT PK_PhieuNhap PRIMARY KEY (MaPhieuNhap),
    CONSTRAINT FK_PN_NCC    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
    CONSTRAINT FK_PN_NV     FOREIGN KEY (MaNV)  REFERENCES NhanVien(MaNV)
);
GO

-- ------------------------------------------------------------
-- Bảng 11: ChiTietNhap
-- ------------------------------------------------------------
CREATE TABLE ChiTietNhap (
    MaCTN       VARCHAR(10)   NOT NULL, -- Định dạng CTN0001
    MaPhieuNhap VARCHAR(10)   NOT NULL,
    MaThuoc     VARCHAR(10)   NOT NULL,
    SoLuong     INT           NOT NULL CHECK (SoLuong > 0),
    GiaNhap     DECIMAL(12,0) NOT NULL CHECK (GiaNhap >= 0),
    SoLo        NVARCHAR(30)  NULL,
    HanSuDung   DATE          NULL,
    ThanhTien   AS (SoLuong * GiaNhap) PERSISTED,   
    CONSTRAINT PK_ChiTietNhap PRIMARY KEY (MaCTN),
    CONSTRAINT UQ_CTN         UNIQUE (MaPhieuNhap, MaThuoc, SoLo),
    CONSTRAINT FK_CTN_PhieuNhap FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhap(MaPhieuNhap),
    CONSTRAINT FK_CTN_Thuoc     FOREIGN KEY (MaThuoc)     REFERENCES Thuoc(MaThuoc)
);
GO

-- ------------------------------------------------------------
-- Bảng 12: HoaDon
-- ------------------------------------------------------------
CREATE TABLE HoaDon (
    MaHD     VARCHAR(10)   NOT NULL, -- Định dạng HD0001
    NgayBan  DATETIME      NOT NULL DEFAULT GETDATE(),
    MaNV     VARCHAR(10)   NOT NULL,
    TongTien DECIMAL(15,0) NOT NULL DEFAULT 0,
    GhiChu   NVARCHAR(200) NULL,
    CONSTRAINT PK_HoaDon PRIMARY KEY (MaHD),
    CONSTRAINT FK_HD_NV  FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO

-- ------------------------------------------------------------
-- Bảng 13: ChiTietHoaDon
-- ------------------------------------------------------------
CREATE TABLE ChiTietHoaDon (
    MaCTHD    VARCHAR(10)   NOT NULL, -- Định dạng CTHD0001
    MaHD      VARCHAR(10)   NOT NULL,
    MaThuoc   VARCHAR(10)   NOT NULL,
    SoLuong   INT           NOT NULL CHECK (SoLuong > 0),
    DonGia    DECIMAL(12,0) NOT NULL CHECK (DonGia >= 0),
    ThanhTien AS (SoLuong * DonGia) PERSISTED,
    CONSTRAINT PK_ChiTietHoaDon PRIMARY KEY (MaCTHD),
    CONSTRAINT UQ_CTHD          UNIQUE (MaHD, MaThuoc),
    CONSTRAINT FK_CTHD_HoaDon   FOREIGN KEY (MaHD)    REFERENCES HoaDon(MaHD),
    CONSTRAINT FK_CTHD_Thuoc    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc)
);
GO

-- ============================================================
-- PHẦN 2: INDEX (tối ưu truy vấn)
-- ============================================================
CREATE INDEX IX_PhuongXa_MaTinhThanh  ON PhuongXa(MaTinhThanh);
CREATE INDEX IX_NCC_MaPhuongXa         ON NhaCungCap(MaPhuongXa);
CREATE INDEX IX_Thuoc_MaLoai     ON Thuoc(MaLoai);
CREATE INDEX IX_Thuoc_TonKho     ON Thuoc(SoLuongTon);
CREATE INDEX IX_CTN_PhieuNhap    ON ChiTietNhap(MaPhieuNhap);
CREATE INDEX IX_CTN_Thuoc        ON ChiTietNhap(MaThuoc);
CREATE INDEX IX_CTHD_HoaDon      ON ChiTietHoaDon(MaHD);
CREATE INDEX IX_CTHD_Thuoc       ON ChiTietHoaDon(MaThuoc);
CREATE INDEX IX_HoaDon_NgayBan   ON HoaDon(NgayBan);
CREATE INDEX IX_PN_NgayNhap      ON PhieuNhap(NgayNhap);
GO

-- ============================================================
-- PHẦN 3: DỮ LIỆU MẪU (INSERT)
-- ============================================================

-- VaiTro
INSERT INTO VaiTro (MaVaiTro, TenVaiTro, MoTa) VALUES
('VT01', N'QuanLy',       N'Quản lý / Chủ hiệu thuốc — toàn quyền'),
('VT02', N'DuocSi',       N'Dược sĩ bán hàng'),
('VT03', N'NhanVienKho',  N'Nhân viên kho — nhập hàng, kiểm kho');
GO

-- NhanVien
INSERT INTO NhanVien (MaNV, HoTen, GioiTinh, NgaySinh, SoDienThoai, Email, MaVaiTro, NgayVaoLam) VALUES
('NV0001', N'Nguyễn Thị Mai',  N'Nữ',  '1990-05-15', '0901234567', N'maint@gmail.com',   'VT01', '2020-01-01'),
('NV0002', N'Trần Văn Hùng',   N'Nam', '1995-08-20', '0912345678', N'hungtv@gmail.com',  'VT02', '2021-03-15'),
('NV0003', N'Lê Thị Hoa',      N'Nữ',  '1993-03-10', '0923456789', N'hoalt@gmail.com',   'VT02', '2021-06-01'),
('NV0004', N'Phạm Văn Bình',   N'Nam', '1988-11-25', '0934567890', N'binhpv@gmail.com',  'VT03', '2020-08-10'),
('NV0005', N'Võ Thị Lan',      N'Nữ',  '1997-07-04', '0945678901', N'lanvt@gmail.com',   'VT02', '2022-01-20');
GO

-- TaiKhoan (Mật khẩu sửa thành chuỗi có chữ hoa, chữ thường và @ để thỏa mãn CHECK constraint)
INSERT INTO TaiKhoan (MaNV, TenDangNhap, MatKhau) VALUES
('NV0001', 'maiAdmin',    'Mai@123'),
('NV0002', 'hungCashier', 'Hung@123'),
('NV0003', 'hoaCashier',  'Hoa@123'),
('NV0004', 'binhKho',     'Binh@123'),
('NV0005', 'lanCashier',  'Lan@123');
GO

-- LoaiThuoc
INSERT INTO LoaiThuoc (MaLoai, TenLoai, MoTa) VALUES
('LT01', N'Kháng sinh',            N'Thuốc kháng khuẩn, diệt vi khuẩn'),
('LT02', N'Giảm đau - Hạ sốt',    N'Paracetamol, Ibuprofen và các loại tương tự'),
('LT03', N'Vitamin & Khoáng chất', N'Bổ sung dinh dưỡng, tăng đề kháng'),
('LT04', N'Tim mạch',              N'Thuốc huyết áp, tim mạch, mỡ máu'),
('LT05', N'Tiêu hóa',              N'Thuốc dạ dày, tiêu hóa, nhuận tràng'),
('LT06', N'Hô hấp',                N'Thuốc ho, long đờm, hen suyễn'),
('LT07', N'Ngoài da',              N'Kem bôi da, thuốc mỡ, dung dịch sát khuẩn'),
('LT08', N'Mắt - Tai - Mũi',       N'Nhỏ mắt, nhỏ tai, xịt mũi');
GO

-- DonViTinh
INSERT INTO DonViTinh (MaDVT, TenDVT) VALUES
('DVT01', N'Viên'), ('DVT02', N'Viên sủi'), ('DVT03', N'Chai'), ('DVT04', N'Lọ'),
('DVT05', N'Ống'),  ('DVT06', N'Gói'),      ('DVT07', N'Hộp'),  ('DVT08', N'Tuýp');
GO

-- TinhThanh
INSERT INTO TinhThanh (MaTinhThanh, TenTinhThanh) VALUES
('TT01', N'Hà Nội'),
('TT02', N'TP. Hồ Chí Minh'),
('TT03', N'Đà Nẵng'),
('TT04', N'Hải Phòng'),
('TT05', N'Quảng Nam');
GO

-- PhuongXa  
INSERT INTO PhuongXa (MaPhuongXa, TenPhuongXa, MaTinhThanh) VALUES
('PX01', N'Phường Hàng Bạc', 'TT01'),
('PX02', N'Phường Tràng Tiền', 'TT01'),
('PX03', N'Phường Thảo Điền', 'TT02'),
('PX04', N'Phường Mỹ An', 'TT03'),
('PX05', N'Phường Hiệp Bình Chánh', 'TT02');
GO

-- NhaCungCap  
INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, MaPhuongXa, SoDienThoai, Email) VALUES
('NCC0001', N'Dược phẩm Trung ương 1', N'Số 15 Hàng Bạc', 'PX01', '02438255140', N'contact@cgmail.com'),
('NCC0002', N'Nhà thuốc Tràng Tiền', N'Số 2 Tràng Tiền', 'PX02', '02438242485', N'info@gmail.com'),
('NCC0003', N'Dược phẩm An Khang', N'Số 10 Xuân Thủy', 'PX03', '02838994567', N'cskh@gmail.com'),
('NCC0004', N'Pymepharco Đà Nẵng', N'170 Trần Hưng Đạo', 'PX04', '02363842364', N'danang@gmail.com'),
('NCC0005', N'Công ty Dược OPV', N'Số 12 Đường số 23', 'PX05', '02839302345', N'opv@gmail.com');
GO

-- Thuoc 
INSERT INTO Thuoc (MaThuoc, TenThuoc, MaDVT, GiaBan, SoLuongTon, MuongCanhBao, MaLoai, CanToa, GhiChu) VALUES
('TH0001', N'Amoxicillin 500mg',           'DVT01',  4500, 200, 30, 'LT01', 1, N'Kháng sinh phổ rộng'),
('TH0002', N'Paracetamol 500mg',           'DVT01',  1200, 500, 50, 'LT02', 0, N'Hạ sốt giảm đau thông dụng'),
('TH0003', N'Ibuprofen 400mg',             'DVT01',  2500, 300, 30, 'LT02', 0, N'Kháng viêm, giảm đau'),
('TH0004', N'Vitamin C 1000mg',            'DVT02',  3500, 400, 40, 'LT03', 0, N'Tăng sức đề kháng'),
('TH0005', N'Atorvastatin 20mg',           'DVT01',  8500, 150, 20, 'LT04', 1, N'Hạ cholesterol'),
('TH0006', N'Omeprazole 20mg',             'DVT01',  6000, 180, 25, 'LT05', 0, N'Giảm acid dạ dày'),
('TH0007', N'Salbutamol 2mg',              'DVT01',  3200, 120, 20, 'LT06', 1, N'Giãn phế quản'),
('TH0008', N'Metronidazole 250mg',         'DVT01',  2800, 250, 30, 'LT01', 1, N'Kháng khuẩn kị khí'),
('TH0009', N'Cetirizine 10mg',             'DVT01',  5500, 180, 20, 'LT06', 0, N'Kháng histamin, dị ứng'),
('TH0010', N'Vitamin B Complex',           'DVT01',  4200, 350, 40, 'LT03', 0, N'Bổ sung vitamin nhóm B'),
('TH0011', N'Losartan 50mg',               'DVT01',  9500,  90, 15, 'LT04', 1, N'Hạ huyết áp'),
('TH0012', N'Loperamide 2mg',              'DVT01',  3800, 200, 25, 'LT05', 0, N'Trị tiêu chảy'),
('TH0013', N'Betadine 10% 30ml',           'DVT03', 28000,  60, 10, 'LT07', 0, N'Sát khuẩn vết thương'),
('TH0014', N'Natri clorid 0.9% nhỏ mắt',  'DVT04', 18000,  80, 15, 'LT08', 0, N'Rửa mắt, dưỡng ẩm'),
('TH0015', N'Augmentin 625mg',             'DVT01', 12000, 100, 20, 'LT01', 1, N'Amoxicillin + acid clavulanic');
GO

-- PhieuNhap
INSERT INTO PhieuNhap (MaPhieuNhap, NgayNhap, MaNCC, MaNV, GhiChu) VALUES
('PN0001', '2025-01-05', 'NCC0001', 'NV0004', N'Nhập kỳ tháng 1 đợt 1'),
('PN0002', '2025-01-12', 'NCC0002', 'NV0004', N'Nhập kỳ tháng 1 đợt 2'),
('PN0003', '2025-02-03', 'NCC0003', 'NV0004', N'Nhập tháng 2'),
('PN0004', '2025-02-20', 'NCC0001', 'NV0004', N'Bổ sung tháng 2'),
('PN0005', '2025-03-10', 'NCC0004', 'NV0004', N'Nhập tháng 3 đợt 1'),
('PN0006', '2025-03-25', 'NCC0005', 'NV0004', N'Nhập tháng 3 đợt 2'),
('PN0007', '2025-04-08', 'NCC0002', 'NV0004', N'Nhập tháng 4 đợt 1'),
('PN0008', '2025-04-22', 'NCC0001', 'NV0004', N'Nhập tháng 4 đợt 2');
GO

-- ChiTietNhap
INSERT INTO ChiTietNhap (MaCTN, MaPhieuNhap, MaThuoc, SoLuong, GiaNhap, SoLo, HanSuDung) VALUES
('CTN0001', 'PN0001', 'TH0001', 100, 3500, 'LOT-2501A', '2026-12-31'),
('CTN0002', 'PN0001', 'TH0002', 200,  900, 'LOT-2501B', '2027-06-30'),
('CTN0003', 'PN0001', 'TH0004', 150, 2800, 'LOT-2501C', '2027-03-31'),
('CTN0004', 'PN0002', 'TH0003', 100, 1800, 'LOT-2502A', '2026-09-30'),
('CTN0005', 'PN0002', 'TH0006',  80, 4500, 'LOT-2502B', '2026-11-30'),
('CTN0006', 'PN0003', 'TH0005',  50, 6500, 'LOT-2503A', '2027-01-31'),
('CTN0007', 'PN0003', 'TH0007',  60, 2400, 'LOT-2503B', '2026-08-31'),
('CTN0008', 'PN0003', 'TH0011',  40, 7800, 'LOT-2503C', '2027-02-28'),
('CTN0009', 'PN0004', 'TH0008', 100, 2100, 'LOT-2504A', '2026-10-31'),
('CTN0010', 'PN0004', 'TH0009',  80, 4200, 'LOT-2504B', '2027-04-30'),
('CTN0011', 'PN0005', 'TH0010', 120, 3200, 'LOT-2505A', '2027-05-31'),
('CTN0012', 'PN0005', 'TH0012',  90, 2900, 'LOT-2505B', '2026-07-31'),
('CTN0013', 'PN0005', 'TH0013',  30,22000, 'LOT-2505C', '2026-06-30'),
('CTN0014', 'PN0006', 'TH0014',  50,14000, 'LOT-2506A', '2026-05-31'),
('CTN0015', 'PN0006', 'TH0015',  40, 9500, 'LOT-2506B', '2027-03-31'),
('CTN0016', 'PN0007', 'TH0001',  80, 3500, 'LOT-2507A', '2027-01-31'),
('CTN0017', 'PN0007', 'TH0002', 150,  900, 'LOT-2507B', '2027-08-30'),
('CTN0018', 'PN0007', 'TH0003',  70, 1800, 'LOT-2507C', '2026-12-31'),
('CTN0019', 'PN0008', 'TH0004', 100, 2800, 'LOT-2508A', '2027-06-30'),
('CTN0020', 'PN0008', 'TH0005',  60, 6500, 'LOT-2508B', '2027-04-30');
GO

-- Cập nhật TongTien PhieuNhap
UPDATE PhieuNhap SET TongTien = (
    SELECT ISNULL(SUM(ThanhTien), 0)
    FROM ChiTietNhap
    WHERE ChiTietNhap.MaPhieuNhap = PhieuNhap.MaPhieuNhap
);
GO

-- HoaDon
INSERT INTO HoaDon (MaHD, NgayBan, MaNV, GhiChu) VALUES
('HD0001', '2025-01-08', 'NV0002', N'Khách lẻ'),
('HD0002', '2025-01-09', 'NV0003', N'Khách lẻ'),
('HD0003', '2025-01-15', 'NV0002', N'Có toa bác sĩ'),
('HD0004', '2025-02-05', 'NV0005', NULL),
('HD0005', '2025-02-10', 'NV0003', N'Khách quen'),
('HD0006', '2025-02-18', 'NV0002', N'Khách lẻ'),
('HD0007', '2025-03-02', 'NV0005', NULL),
('HD0008', '2025-03-14', 'NV0003', N'Có toa — kháng sinh'),
('HD0009', '2025-03-20', 'NV0002', NULL),
('HD0010', '2025-04-01', 'NV0005', N'Khách lẻ'),
('HD0011', '2025-04-07', 'NV0003', NULL),
('HD0012', '2025-04-15', 'NV0002', N'Có toa');
GO

-- ChiTietHoaDon
INSERT INTO ChiTietHoaDon (MaCTHD, MaHD, MaThuoc, SoLuong, DonGia) VALUES
('CTHD0001', 'HD0001', 'TH0002', 2,  1200),
('CTHD0002', 'HD0001', 'TH0004', 1,  3500),
('CTHD0003', 'HD0002', 'TH0001', 1,  4500),
('CTHD0004', 'HD0002', 'TH0003', 2,  2500),
('CTHD0005', 'HD0003', 'TH0005', 1,  8500),
('CTHD0006', 'HD0003', 'TH0001', 2,  4500),
('CTHD0007', 'HD0004', 'TH0009', 1,  5500),
('CTHD0008', 'HD0004', 'TH0010', 2,  4200),
('CTHD0009', 'HD0005', 'TH0002', 3,  1200),
('CTHD0010', 'HD0005', 'TH0006', 1,  6000),
('CTHD0011', 'HD0006', 'TH0013', 1, 28000),
('CTHD0012', 'HD0006', 'TH0014', 2, 18000),
('CTHD0013', 'HD0007', 'TH0012', 1,  3800),
('CTHD0014', 'HD0007', 'TH0004', 2,  3500),
('CTHD0015', 'HD0008', 'TH0008', 1,  2800),
('CTHD0016', 'HD0008', 'TH0007', 1,  3200),
('CTHD0017', 'HD0009', 'TH0011', 1,  9500),
('CTHD0018', 'HD0009', 'TH0002', 5,  1200),
('CTHD0019', 'HD0010', 'TH0015', 1, 12000),
('CTHD0020', 'HD0010', 'TH0009', 2,  5500),
('CTHD0021', 'HD0011', 'TH0003', 2,  2500),
('CTHD0022', 'HD0011', 'TH0006', 1,  6000),
('CTHD0023', 'HD0012', 'TH0005', 1,  8500),
('CTHD0024', 'HD0012', 'TH0001', 1,  4500);
GO

-- Cập nhật TongTien HoaDon
UPDATE HoaDon SET TongTien = (
    SELECT ISNULL(SUM(ThanhTien), 0)
    FROM ChiTietHoaDon
    WHERE ChiTietHoaDon.MaHD = HoaDon.MaHD
);
GO

-- ============================================================
-- PHẦN 4: STORED PROCEDURES
-- ============================================================

-- SP 1: Bán hàng — tự động sinh mã HD000xx
CREATE OR ALTER PROCEDURE sp_BanHang
    @MaNV     VARCHAR(10),
    @DanhSach NVARCHAR(MAX),   -- JSON: [{"MaThuoc":"TH0001","SoLuong":2},...]
    @GhiChu   NVARCHAR(200) = NULL,
    @MaHD_Out VARCHAR(10) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Tự tạo mã Hóa Đơn
        DECLARE @MaxID INT;
        SELECT @MaxID = ISNULL(MAX(CAST(RIGHT(MaHD, 4) AS INT)), 0) FROM HoaDon;
        SET @MaHD_Out = 'HD' + RIGHT('0000' + CAST(@MaxID + 1 AS VARCHAR(4)), 4);

        -- Tạo hóa đơn
        INSERT INTO HoaDon (MaHD, NgayBan, MaNV, GhiChu)
        VALUES (@MaHD_Out, GETDATE(), @MaNV, @GhiChu);

        -- Parse JSON và insert chi tiết
        INSERT INTO ChiTietHoaDon (MaHD, MaThuoc, SoLuong, DonGia)
        SELECT @MaHD_Out,
               j.MaThuoc,
               j.SoLuong,
               t.GiaBan
        FROM OPENJSON(@DanhSach)
             WITH (MaThuoc VARCHAR(10), SoLuong INT) AS j
        INNER JOIN Thuoc t ON t.MaThuoc = j.MaThuoc;

        -- Trừ tồn kho
        UPDATE t
        SET t.SoLuongTon = t.SoLuongTon - j.SoLuong
        FROM Thuoc t
        INNER JOIN (
            SELECT MaThuoc, SoLuong
            FROM ChiTietHoaDon
            WHERE MaHD = @MaHD_Out
        ) j ON j.MaThuoc = t.MaThuoc;

        -- Kiểm tra tồn kho không âm
        IF EXISTS (SELECT 1 FROM Thuoc WHERE SoLuongTon < 0)
        BEGIN
            RAISERROR(N'Tồn kho không đủ cho một hoặc nhiều sản phẩm', 16, 1);
        END

        -- Cập nhật tổng tiền hóa đơn
        UPDATE HoaDon
        SET TongTien = (
            SELECT SUM(ThanhTien)
            FROM ChiTietHoaDon
            WHERE MaHD = @MaHD_Out
        )
        WHERE MaHD = @MaHD_Out;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- SP 2: Nhập hàng — tự động sinh mã PN000xx
CREATE OR ALTER PROCEDURE sp_NhapHang
    @MaNCC    VARCHAR(10),
    @MaNV     VARCHAR(10),
    @DanhSach NVARCHAR(MAX),   -- JSON: [{"MaThuoc":"TH0001","SoLuong":100,"GiaNhap":3500,"SoLo":"LOT","HanSuDung":"2027-01-01"},...]
    @GhiChu   NVARCHAR(200) = NULL,
    @MaPN_Out VARCHAR(10) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Tự tạo mã Phiếu Nhập
        DECLARE @MaxID INT;
        SELECT @MaxID = ISNULL(MAX(CAST(RIGHT(MaPhieuNhap, 4) AS INT)), 0) FROM PhieuNhap;
        SET @MaPN_Out = 'PN' + RIGHT('0000' + CAST(@MaxID + 1 AS VARCHAR(4)), 4);

        INSERT INTO PhieuNhap (MaPhieuNhap, NgayNhap, MaNCC, MaNV, GhiChu)
        VALUES (@MaPN_Out, GETDATE(), @MaNCC, @MaNV, @GhiChu);

        INSERT INTO ChiTietNhap (MaPhieuNhap, MaThuoc, SoLuong, GiaNhap, SoLo, HanSuDung)
        SELECT @MaPN_Out, MaThuoc, SoLuong, GiaNhap, SoLo, HanSuDung
        FROM OPENJSON(@DanhSach)
             WITH (MaThuoc   VARCHAR(10),
                   SoLuong   INT,
                   GiaNhap   DECIMAL(12,0),
                   SoLo      NVARCHAR(30),
                   HanSuDung DATE);

        -- Cộng tồn kho
        UPDATE t
        SET t.SoLuongTon = t.SoLuongTon + j.SoLuong
        FROM Thuoc t
        INNER JOIN (
            SELECT MaThuoc, SoLuong
            FROM ChiTietNhap
            WHERE MaPhieuNhap = @MaPN_Out
        ) j ON j.MaThuoc = t.MaThuoc;

        -- Cập nhật tổng tiền phiếu nhập
        UPDATE PhieuNhap
        SET TongTien = (
            SELECT SUM(ThanhTien)
            FROM ChiTietNhap
            WHERE MaPhieuNhap = @MaPN_Out
        )
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
-- PHẦN 5: VIEWS
-- ============================================================

-- View 1: Danh sách thuốc đầy đủ
CREATE OR ALTER VIEW vw_DanhSachThuoc AS
    SELECT
        t.MaThuoc,
        t.TenThuoc,
        d.TenDVT,
        l.TenLoai,
        t.GiaBan,
        t.SoLuongTon,
        t.MuongCanhBao,
        CASE WHEN t.SoLuongTon <= t.MuongCanhBao
             THEN N'⚠ Sắp hết'
             ELSE N'Bình thường'
        END AS TrangThaiTon,
        t.CanToa,
        t.TrangThai
    FROM Thuoc t
    INNER JOIN LoaiThuoc l ON l.MaLoai = t.MaLoai
    INNER JOIN DonViTinh d ON d.MaDVT  = t.MaDVT;
GO

-- View 2: Cảnh báo tồn kho thấp và sắp hết hạn (≤60 ngày)
CREATE OR ALTER VIEW vw_CanhBao AS
    SELECT
        t.MaThuoc, t.TenThuoc, t.SoLuongTon, t.MuongCanhBao,
        N'Tồn kho thấp' AS LoaiCanhBao,
        NULL            AS HanSuDung
    FROM Thuoc t
    WHERE t.SoLuongTon <= t.MuongCanhBao
      AND t.TrangThai = 1
    UNION ALL
    SELECT DISTINCT
        t.MaThuoc, t.TenThuoc, t.SoLuongTon, t.MuongCanhBao,
        N'Sắp hết hạn (≤60 ngày)',
        c.HanSuDung
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
        nv.HoTen         AS TenNhanVien,
        t.TenThuoc,
        d.TenDVT,
        ct.SoLuong,
        ct.DonGia,
        ct.ThanhTien,
        hd.TongTien,
        hd.GhiChu
    FROM HoaDon hd
    INNER JOIN NhanVien      nv ON nv.MaNV   = hd.MaNV
    INNER JOIN ChiTietHoaDon ct ON ct.MaHD   = hd.MaHD
    INNER JOIN Thuoc          t ON t.MaThuoc = ct.MaThuoc
    INNER JOIN DonViTinh      d ON d.MaDVT   = t.MaDVT;
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
    INNER JOIN Thuoc     t ON t.MaThuoc = ct.MaThuoc
    INNER JOIN LoaiThuoc l ON l.MaLoai  = t.MaLoai
    GROUP BY t.MaThuoc, t.TenThuoc, l.TenLoai;
GO

-- View 6: Địa chỉ đầy đủ nhà cung cấp (join PhuongXa, TinhThanh)
CREATE OR ALTER VIEW vw_NhaCungCapDiaChi AS
    SELECT
        n.MaNCC,
        n.TenNCC,
        n.DiaChi,
        p.TenPhuongXa,
        t.TenTinhThanh,
        n.SoDienThoai,
        n.Email,
        n.TrangThai,
        -- Địa chỉ gộp đầy đủ
        CONCAT(n.DiaChi, N', ', p.TenPhuongXa, N', ', t.TenTinhThanh) AS DiaChiDayDu
    FROM NhaCungCap n
    LEFT JOIN PhuongXa  p ON p.MaPhuongXa   = n.MaPhuongXa
    LEFT JOIN TinhThanh t ON t.MaTinhThanh = p.MaTinhThanh;
GO

-- ============================================================
-- PHẦN 6: KIỂM TRA KẾT QUẢ
-- ============================================================
SELECT N'=== KIỂM TRA SỐ DÒNG CÁC BẢNG ===' AS ThongTin;
SELECT N'VaiTro'          AS Bang, COUNT(*) AS SoBan FROM VaiTro          UNION ALL
SELECT N'NhanVien',                COUNT(*) FROM NhanVien                  UNION ALL
SELECT N'TaiKhoan',                COUNT(*) FROM TaiKhoan                  UNION ALL
SELECT N'LoaiThuoc',               COUNT(*) FROM LoaiThuoc                 UNION ALL
SELECT N'DonViTinh',               COUNT(*) FROM DonViTinh                 UNION ALL
SELECT N'TinhThanh',               COUNT(*) FROM TinhThanh                 UNION ALL
SELECT N'PhuongXa',                COUNT(*) FROM PhuongXa                  UNION ALL
SELECT N'NhaCungCap',              COUNT(*) FROM NhaCungCap                UNION ALL
SELECT N'Thuoc',                   COUNT(*) FROM Thuoc                     UNION ALL
SELECT N'PhieuNhap',               COUNT(*) FROM PhieuNhap                 UNION ALL
SELECT N'ChiTietNhap',             COUNT(*) FROM ChiTietNhap               UNION ALL
SELECT N'HoaDon',                  COUNT(*) FROM HoaDon                    UNION ALL
SELECT N'ChiTietHoaDon',           COUNT(*) FROM ChiTietHoaDon;

SELECT N'=== ĐỊA CHỈ NHÀ CUNG CẤP ĐẦY ĐỦ ===' AS ThongTin;
SELECT * FROM vw_NhaCungCapDiaChi;

SELECT N'=== CẢNH BÁO TỒN KHO / HẾT HẠN ===' AS ThongTin;
SELECT * FROM vw_CanhBao ORDER BY LoaiCanhBao;

SELECT N'=== DOANH THU THEO THÁNG ===' AS ThongTin;
SELECT * FROM vw_DoanhThuThang ORDER BY Nam, Thang;

SELECT N'=== TOP 5 THUỐC BÁN CHẠY ===' AS ThongTin;
SELECT TOP 5 * FROM vw_TopThuocBanChay ORDER BY TongSoLuongBan DESC;
GO