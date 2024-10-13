USE master;
GO

-- Xóa cơ sở dữ liệu nếu tồn tại
DROP DATABASE IF EXISTS TravelVN;
GO

-- Tạo cơ sở dữ liệu TravelVN
CREATE DATABASE TravelVN;
GO

USE TravelVN;
GO

-- Bảng quản lý loại phòng khách sạn
CREATE TABLE LoaiPhong (
    MaLoai NVARCHAR(10) PRIMARY KEY,
    TenLoai NVARCHAR(MAX) NULL
);

-- Bảng quản lý trạng thái phòng khách sạn
CREATE TABLE TrangThaiPhong (
    MaTrangThai NVARCHAR(10) PRIMARY KEY,
    TenTrangThai NVARCHAR(50) NOT NULL
);

-- Bảng quản lý tỉnh thành
CREATE TABLE TinhThanh (
    MaTinh NVARCHAR(10) PRIMARY KEY,
    TenTinh NVARCHAR(255) NOT NULL
);

-- Bảng quản lý khách sạn
CREATE TABLE Hotels (
    MaKhachSan NVARCHAR(10) PRIMARY KEY,
    TenKhachSan NVARCHAR(MAX) NOT NULL,
    DiaChi NVARCHAR(MAX),
    MoTa NVARCHAR(MAX),
    HinhAnh NVARCHAR(255),
    MaTinh NVARCHAR(10) NULL,
    FOREIGN KEY (MaTinh) REFERENCES TinhThanh(MaTinh) 
);

-- Bảng quản lý người dùng (Users)
CREATE TABLE Users (
    MaUser NVARCHAR(10) PRIMARY KEY,
    TenUser NVARCHAR(50) NOT NULL,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100),
    DienThoai NVARCHAR(15)
);

-- Bảng quản lý phòng khách sạn
CREATE TABLE Phong (
    MaPhong NVARCHAR(10) PRIMARY KEY,
    MaKhachSan NVARCHAR(10) NOT NULL, 
    MaLoai NVARCHAR(10) NOT NULL,
    MaTrangThai NVARCHAR(10) NOT NULL,
    Gia DECIMAL(18, 2) NOT NULL,
    Hinh NVARCHAR(255),
    FOREIGN KEY (MaKhachSan) REFERENCES Hotels(MaKhachSan),
    FOREIGN KEY (MaLoai) REFERENCES LoaiPhong(MaLoai),
    FOREIGN KEY (MaTrangThai) REFERENCES TrangThaiPhong(MaTrangThai)
);

-- Bảng quản lý loại tour
CREATE TABLE LoaiTour (
    MaLoaiTour NVARCHAR(10) PRIMARY KEY,
    TenLoaiTour NVARCHAR(MAX) NOT NULL
);

-- Bảng quản lý tour
CREATE TABLE Tour (
    MaTour NVARCHAR(10) PRIMARY KEY,
    TenTour NVARCHAR(MAX) NOT NULL,
    MoTa NVARCHAR(MAX),
    NgayKhoiHanh DATE NOT NULL,
    ThoiGian INT NOT NULL,  -- Số ngày của tour
    Gia DECIMAL(18, 2) NOT NULL,
    MaLoaiTour NVARCHAR(10) NOT NULL,
    MaTinh NVARCHAR(10) NULL,  
    FOREIGN KEY (MaLoaiTour) REFERENCES LoaiTour(MaLoaiTour),
    FOREIGN KEY (MaTinh) REFERENCES TinhThanh(MaTinh)  
);

-- Bảng quản lý chuyến bay
CREATE TABLE Flight (
    MaFlight NVARCHAR(10) PRIMARY KEY,
    TenFlight NVARCHAR(100) NOT NULL,
    HangBay NVARCHAR(50),
    LoaiMayBay NVARCHAR(50)
);

-- Bảng quản lý đặt dịch vụ (Booking)
CREATE TABLE Booking (
    MaBooking NVARCHAR(10) PRIMARY KEY,
    MaUser NVARCHAR(10) NOT NULL,
    LoaiDichVu NVARCHAR(20) NOT NULL, -- 'Hotel', 'Tour', 'Flight'
    NgayDat DATE NOT NULL,
    TongTien DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (MaUser) REFERENCES Users(MaUser)
);

-- Bảng chi tiết đặt phòng khách sạn
CREATE TABLE BookingHotelDetails (
    MaBooking NVARCHAR(10) PRIMARY KEY,
    MaPhong NVARCHAR(10) NOT NULL,
    NgayNhanPhong DATE NOT NULL,
    NgayTraPhong DATE NOT NULL,
    FOREIGN KEY (MaBooking) REFERENCES Booking(MaBooking),
    FOREIGN KEY (MaPhong) REFERENCES Phong(MaPhong)
);

-- Bảng chi tiết đặt tour
CREATE TABLE TourDetails (
    MaBooking NVARCHAR(10) PRIMARY KEY,
    MaTour NVARCHAR(10) NOT NULL,
    SoLuongNguoi INT NOT NULL,
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE NOT NULL,
    FOREIGN KEY (MaBooking) REFERENCES Booking(MaBooking),
    FOREIGN KEY (MaTour) REFERENCES Tour(MaTour)
);

-- Bảng chi tiết đặt chuyến bay (Flight Details)
CREATE TABLE FlightDetails (
    MaBooking NVARCHAR(10) PRIMARY KEY,
    MaFlight NVARCHAR(10) NOT NULL,
    SoHieuChuyenBay NVARCHAR(10) NOT NULL,
    NgayBay DATE NOT NULL,
    DiemDi NVARCHAR(50) NOT NULL,
    DiemDen NVARCHAR(50) NOT NULL,
    GioKhoiHanh TIME NOT NULL,
    GioDen TIME NOT NULL,
    GiaVe DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (MaBooking) REFERENCES Booking(MaBooking),
    FOREIGN KEY (MaFlight) REFERENCES Flight(MaFlight)
);

-- Bảng quản lý combo
CREATE TABLE Combo (
    MaCombo INT PRIMARY KEY IDENTITY(1,1),
    TenCombo NVARCHAR(255) NOT NULL,
    GiaCombo DECIMAL(18, 2) NOT NULL
);

-- Bảng quản lý thanh toán (Payment)
CREATE TABLE Payment (
    MaPayment NVARCHAR(10) PRIMARY KEY,
    MaBooking NVARCHAR(10) NOT NULL,
    NgayThanhToan DATE NOT NULL,
    SoTien DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (MaBooking) REFERENCES Booking(MaBooking)
);

-- Bảng liên kết Tour với Combo (ComboTour)
CREATE TABLE ComboTour (
    MaCombo INT NOT NULL,
    MaTour NVARCHAR(10) NOT NULL,
    PRIMARY KEY (MaCombo, MaTour),
    FOREIGN KEY (MaCombo) REFERENCES Combo(MaCombo),
    FOREIGN KEY (MaTour) REFERENCES Tour(MaTour)
);

-- Bảng liên kết Khách sạn với Combo (ComboKhachSan)
CREATE TABLE ComboKhachSan (
    MaCombo INT NOT NULL,
    MaKhachSan NVARCHAR(10) NOT NULL,
    PRIMARY KEY (MaCombo, MaKhachSan),
    FOREIGN KEY (MaCombo) REFERENCES Combo(MaCombo),
    FOREIGN KEY (MaKhachSan) REFERENCES Hotels(MaKhachSan)
);

-- Bảng liên kết Chuyến bay với Combo (ComboChuyenBay)
CREATE TABLE ComboChuyenBay (
    MaCombo INT NOT NULL,
    MaFlight NVARCHAR(10) NOT NULL,
    PRIMARY KEY (MaCombo, MaFlight),
    FOREIGN KEY (MaCombo) REFERENCES Combo(MaCombo),
    FOREIGN KEY (MaFlight) REFERENCES Flight(MaFlight)
);

-- Bảng Contact
CREATE TABLE Contact (
    MaContact INT PRIMARY KEY IDENTITY(1,1),
    MaUser NVARCHAR(10) NULL,
    TenUser NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Noidung NVARCHAR(MAX) NOT NULL,
    NgayTao DATE DEFAULT GETDATE(),
    FOREIGN KEY (MaUser) REFERENCES Users(MaUser)
);

-- Insert sample data for LoaiPhong
INSERT INTO LoaiPhong (MaLoai, TenLoai)
VALUES 
    (N'C1', N'Phòng đơn'), 
    (N'C2', N'Phòng đôi'), 
    (N'C3', N'Phòng gia đình');

-- Insert sample data for TrangThaiPhong
INSERT INTO TrangThaiPhong (MaTrangThai, TenTrangThai)
VALUES 
    (N'TT1', N'Đang trống'), 
    (N'TT2', N'Đang sử dụng'), 
    (N'TT3', N'Đã đặt');

-- Insert sample data for TinhThanh

INSERT INTO TinhThanh (MaTinh, TenTinh) VALUES
    (N'TT01', N'An Giang'),
    (N'TT02', N'Bà Rịa - Vũng Tàu'),
    (N'TT03', N'Bạc Liêu'),
    (N'TT04', N'Bắc Giang'),
    (N'TT05', N'Bắc Kạn'),
    (N'TT06', N'Bến Tre'),
    (N'TT07', N'Bình Định'),
    (N'TT08', N'Bình Dương'),
    (N'TT09', N'Bình Phước'),
    (N'TT10', N'Bình Thuận'),
    (N'TT11', N'Cà Mau'),
    (N'TT12', N'Các Huyện Của HCM'),
    (N'TT13', N'Đà Nẵng'),
    (N'TT14', N'Đắk Lắk'),
    (N'TT15', N'Đắk Nông'),
    (N'TT16', N'Hà Giang'),
    (N'TT17', N'Hà Nam'),
    (N'TT18', N'Hà Nội'),
    (N'TT19', N'Hà Tĩnh'),
    (N'TT20', N'Hải Dương'),
    (N'TT21', N'Hải Phòng'),
    (N'TT22', N'Hòa Bình'),
    (N'TT23', N'Hồ Chí Minh'),
    (N'TT24', N'Hưng Yên'),
    (N'TT25', N'Khánh Hòa'),
    (N'TT26', N'Kiên Giang'),
    (N'TT27', N'Kon Tum'),
    (N'TT28', N'Lai Châu'),
    (N'TT29', N'Lào Cai'),
    (N'TT30', N'Lâm Đồng'),
    (N'TT31', N'Long An'),
    (N'TT32', N'Nghệ An'),
    (N'TT33', N'Ninh Bình'),
    (N'TT34', N'Ninh Thuận'),
    (N'TT35', N'Phú Thọ'),
    (N'TT36', N'Phú Yên'),
    (N'TT37', N'Quảng Bình'),
    (N'TT38', N'Quảng Nam'),
    (N'TT39', N'Quảng Ngãi'),
    (N'TT40', N'Quảng Ninh'),
    (N'TT41', N'Sóc Trăng'),
    (N'TT42', N'Son La'),
    (N'TT43', N'Tây Ninh'),
    (N'TT44', N'Thái Bình'),
    (N'TT45', N'Thái Nguyên'),
    (N'TT46', N'Thành phố Hồ Chí Minh'),
    (N'TT47', N'Thừa Thiên - Huế'),
    (N'TT48', N'Tiền Giang'),
    (N'TT49', N'Tiền Giang'),
    (N'TT50', N'Tuyên Quang'),
    (N'TT51', N'Vĩnh Long'),
    (N'TT52', N'Vĩnh Phúc'),
    (N'TT53', N'Yên Bái'),
    (N'TT54', N'Điện Biên'),
    (N'TT55', N'Hà Tĩnh'),
    (N'TT56', N'Hà Giang'),
    (N'TT57', N'Kon Tum'),
    (N'TT58', N'Lai Châu'),
    (N'TT59', N'Nam Định'),
    (N'TT60', N'Ninh Bình'),
    (N'TT61', N'Quảng Trị'),
    (N'TT62', N'Bắc Ninh'),
    (N'TT63', N'Hải Phòng');


-- Insert sample data for Hotels
INSERT INTO Hotels (MaKhachSan, TenKhachSan, DiaChi, MoTa, HinhAnh, MaTinh)
VALUES 
    (N'KS1', N'Khách sạn A', N'123 Đường ABC, Hà Nội', N'Khách sạn sang trọng', N'hotelA.jpg', N'TT1'),
    (N'KS2', N'Khách sạn B', N'456 Đường DEF, Hồ Chí Minh', N'Khách sạn giá rẻ', N'hotelB.jpg', N'TT2');

-- Insert sample data for Phong
INSERT INTO Phong (MaPhong, MaKhachSan, MaLoai, MaTrangThai, Gia, Hinh)
VALUES 
    (N'P001', N'HS01', N'C1', N'S1',  1000000, 'room1.jpg'),
    (N'P002', N'HS02', N'C2', N'S2',  1500000, 'room2.jpg'),
    (N'P003', N'HS03', N'C3', N'S3',  2000000, 'room3.jpg'),
    (N'P004', N'HS04', N'C1', N'S1',  1200000, 'room4.jpg'),
    (N'P005', N'HS05', N'C2', N'S2',  1700000, 'room5.jpg'),
    (N'P006', N'HS06', N'C3', N'S3',  2500000, 'room6.jpg'),
    (N'P007', N'HS07', N'C1', N'S1',  900000, 'room7.jpg'),
    (N'P008', N'HS08', N'C2', N'S2',  1600000, 'room8.jpg'),
    (N'P009', N'HS09', N'C3', N'S3',  2100000, 'room9.jpg'),
    (N'P010', N'HS10', N'C1', N'S1',  1100000, 'room10.jpg'),
    (N'P011', N'HS11', N'C2', N'S2',  1300000, 'room11.jpg');


-- Insert sample data for LoaiTour
INSERT INTO LoaiTour (MaLoaiTour, TenLoaiTour)
VALUES 
    (N'LT1', N'Tour khám phá'), 
    (N'LT2', N'Tour nghỉ dưỡng');

-- Insert sample data for Tour
INSERT INTO Tour (MaTour, TenTour, MoTa, NgayKhoiHanh, ThoiGian, Gia, MaLoaiTour, MaTinh)
VALUES 

    (N'T002', N'Tour Sapa 2 ngày', N'Tour khám phá Sapa, phong cảnh thiên nhiên tuyệt đẹp', '2024-11-10', 2, 3000000, N'LT1', N'TT1'), -- Thay T1 và T12 thành LT1 và TT1
    (N'T003', N'Tour Phú Quốc 3 ngày', N'Tour tham quan đảo Phú Quốc, tắm biển và thưởng thức hải sản', '2024-12-05', 3, 5000000, N'LT1', N'TT2'), -- Thay T1 và T44 thành LT1 và TT2
    (N'T004', N'Tour Nha Trang 4 ngày', N'Tour nghỉ dưỡng tại Nha Trang, tham gia các hoạt động thể thao dưới nước', '2024-11-15', 4, 7000000, N'LT1', N'TT2'), -- Thay T1 và T30 thành LT1 và TT2
    (N'T005', N'Tour Hội An 2 ngày', N'Tour khám phá phố cổ Hội An và các làng nghề truyền thống', '2024-11-20', 2, 2500000, N'LT1', N'TT1'), -- Thay T1 và T4 thành LT1 và TT1
    (N'T006', N'Tour Đà Lạt 3 ngày', N'Tour khám phá Đà Lạt, thành phố ngàn hoa', '2024-12-12', 3, 3500000, N'LT1', N'TT1'), -- Thay T1 và T33 thành LT1 và TT1
    (N'T007', N'Tour Quy Nhơn 2 ngày', N'Tour tham quan các điểm du lịch nổi tiếng tại Quy Nhơn', '2024-11-25', 2, 2800000, N'LT1', N'TT2'), -- Thay T1 và T27 thành LT1 và TT2
    (N'T008', N'Tour Hạ Long 1 ngày', N'Tour tham quan vịnh Hạ Long, thưởng thức hải sản', '2024-11-30', 1, 1500000, N'LT1', N'TT2'), -- Thay T1 và T9 thành LT1 và TT2
    (N'T009', N'Tour Mộc Châu 2 ngày', N'Tour khám phá vùng cao nguyên Mộc Châu', '2024-12-01', 2, 2200000, N'LT1', N'TT1'), -- Thay T1 và T14 thành LT1 và TT1
    (N'T010', N'Tour Bình Ba 3 ngày', N'Tour tham quan đảo Bình Ba, tắm biển và thưởng thức hải sản', '2024-12-10', 3, 4500000, N'LT1', N'TT1'), -- Thay T1 và T29 thành LT1 và TT1
    (N'T011', N'Tour Đồng Hới 2 ngày', N'Tour khám phá các điểm du lịch tại Đồng Hới', '2024-12-15', 2, 2000000, N'LT1', N'TT1'); -- Thay T1 và T51 thành LT1 và TT1


-- Insert sample data for Users
INSERT INTO Users (MaUser, TenUser, Username, Password, Email, DienThoai)
VALUES 
    (N'U1', N'Nguyễn Văn A', N'nguenvana', N'password1', N'email1@example.com', N'0123456789'),
    (N'U2', N'Trần Thị B', N'tranthib', N'password2', N'email2@example.com', N'0987654321');

-- Insert sample data for Booking
INSERT INTO Booking (MaBooking, MaUser, LoaiDichVu, NgayDat, TongTien)
VALUES 
    (N'B1', N'U1', N'Hotel', '2024-10-05', 3000),
    (N'B2', N'U2', N'Tour', '2024-10-06', 5000);

-- Insert sample data for BookingHotelDetails
INSERT INTO BookingHotelDetails (MaBooking, MaPhong, NgayNhanPhong, NgayTraPhong)
VALUES 
    (N'B1', N'PH1', '2024-10-10', '2024-10-12'),
    (N'B2', N'PH3', '2024-10-11', '2024-10-15');

-- Insert sample data for Flight
INSERT INTO Flight (MaFlight, TenFlight, HangBay, LoaiMayBay)
VALUES 
    (N'FL1', N'Chuyến bay A', N'Vietnam Airlines', N'Boeing 777'),
    (N'FL2', N'Chuyến bay B', N'VietJet', N'Airbus A320');

-- Insert sample data for Combo
INSERT INTO Combo (TenCombo, GiaCombo)
VALUES 
    (N'Combo A', 2000000),
    (N'Combo B', 3500000);

-- Insert sample data for Payment
INSERT INTO Payment (MaPayment, MaBooking, NgayThanhToan, SoTien)
VALUES 
    (N'PM1', N'B1', '2024-10-05', 3000),
    (N'PM2', N'B2', '2024-10-06', 5000);

-- Insert sample data for TourDetails
INSERT INTO TourDetails (MaBooking, MaTour, SoLuongNguoi, NgayBatDau, NgayKetThuc)
VALUES 
    (N'B2', N'T1', 2, '2024-10-01', '2024-10-04');

-- Insert sample data for FlightDetails
INSERT INTO FlightDetails (MaBooking, MaFlight, SoHieuChuyenBay, NgayBay, DiemDi, DiemDen, GioKhoiHanh, GioDen, GiaVe)
VALUES 
    (N'B1', N'FL1', N'VN123', '2024-10-12', N'Hà Nội', N'Sài Gòn', '10:00', '12:00', 1000000);

-- Insert sample data for Contact
INSERT INTO Contact (MaUser, TenUser, Email, Noidung)
VALUES 
    (N'U1', N'Nguyễn Văn A', N'email1@example.com', N'Thắc mắc về dịch vụ'),
    (N'U2', N'Trần Thị B', N'email2@example.com', N'Đề xuất cải tiến dịch vụ');
