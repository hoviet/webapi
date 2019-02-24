USE [master]
GO
/****** Object:  Database [QL_BH]    Script Date: 2/24/2019 9:32:19 AM ******/
CREATE DATABASE [QL_BH]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QL_BH', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\QL_BH.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QL_BH_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\QL_BH_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [QL_BH] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QL_BH].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QL_BH] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QL_BH] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QL_BH] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QL_BH] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QL_BH] SET ARITHABORT OFF 
GO
ALTER DATABASE [QL_BH] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QL_BH] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QL_BH] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QL_BH] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QL_BH] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QL_BH] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QL_BH] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QL_BH] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QL_BH] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QL_BH] SET  DISABLE_BROKER 
GO
ALTER DATABASE [QL_BH] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QL_BH] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QL_BH] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QL_BH] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QL_BH] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QL_BH] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QL_BH] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QL_BH] SET RECOVERY FULL 
GO
ALTER DATABASE [QL_BH] SET  MULTI_USER 
GO
ALTER DATABASE [QL_BH] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QL_BH] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QL_BH] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QL_BH] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QL_BH] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'QL_BH', N'ON'
GO
ALTER DATABASE [QL_BH] SET QUERY_STORE = OFF
GO
USE [QL_BH]
GO
/****** Object:  Table [dbo].[ChiTietDonHang]    Script Date: 2/24/2019 9:32:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietDonHang](
	[id_don_hang] [int] NOT NULL,
	[id_san_pham] [int] NOT NULL,
	[gia_km] [float] NOT NULL,
	[so_luong] [int] NOT NULL,
	[tong_tien] [float] NOT NULL,
	[thoi_gian_lap] [date] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DanhMucSanPham]    Script Date: 2/24/2019 9:32:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhMucSanPham](
	[id_danh_muc] [int] IDENTITY(1,1) NOT NULL,
	[ten_danh_muc] [nvarchar](max) NOT NULL,
	[url_hinh] [nvarchar](max) NULL,
 CONSTRAINT [PK_DanhMucSanPham] PRIMARY KEY CLUSTERED 
(
	[id_danh_muc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DanhSachKhuyenMai]    Script Date: 2/24/2019 9:32:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhSachKhuyenMai](
	[id_khuyen_mai] [int] IDENTITY(1,1) NOT NULL,
	[ten_km] [nvarchar](max) NOT NULL,
	[phan_tram_km] [float] NOT NULL,
	[t_bat_dau] [date] NOT NULL,
	[t_ket_thuc] [date] NOT NULL,
	[url_hinh] [nvarchar](max) NULL,
 CONSTRAINT [PK_DanhSachKhuyenMai] PRIMARY KEY CLUSTERED 
(
	[id_khuyen_mai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonDatHang]    Script Date: 2/24/2019 9:32:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonDatHang](
	[id_don_hang] [int] IDENTITY(1,1) NOT NULL,
	[id_khach_hang] [int] NOT NULL,
	[id_tinh_trang] [int] NOT NULL,
	[ngay_lap] [date] NOT NULL,
	[tong_tien] [float] NOT NULL,
	[so_dt_nguoi_nhan] [int] NOT NULL,
	[noi_nhan] [nvarchar](max) NOT NULL,
	[ghi_chu] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_DonDatHang] PRIMARY KEY CLUSTERED 
(
	[id_don_hang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HinhSP]    Script Date: 2/24/2019 9:32:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HinhSP](
	[id_hinh] [int] NOT NULL,
	[id_sp] [int] NOT NULL,
	[url_hinh] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_HinhSP] PRIMARY KEY CLUSTERED 
(
	[id_hinh] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhachHang]    Script Date: 2/24/2019 9:32:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhachHang](
	[id_khach_hang] [int] IDENTITY(1,1) NOT NULL,
	[tai_khoan] [nvarchar](550) NOT NULL,
	[mat_khau] [nvarchar](50) NOT NULL,
	[ten_nguoi_dung] [nvarchar](50) NOT NULL,
	[so_dt] [nchar](10) NOT NULL,
	[email] [nvarchar](500) NOT NULL,
	[gioi_tinh] [nvarchar](10) NOT NULL,
	[ngay_sinh] [date] NULL,
	[t_dang_ky] [date] NOT NULL,
 CONSTRAINT [PK_KhachHang] PRIMARY KEY CLUSTERED 
(
	[id_khach_hang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SanPham]    Script Date: 2/24/2019 9:32:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanPham](
	[id_san_pham] [int] IDENTITY(1,1) NOT NULL,
	[id_danh_muc] [int] NOT NULL,
	[ten_sp] [nvarchar](500) NOT NULL,
	[gia_sp] [float] NOT NULL,
	[phan_tram_km] [float] NOT NULL,
	[gia_km] [float] NOT NULL,
	[so_luong] [int] NOT NULL,
	[mo_ta] [nvarchar](max) NOT NULL,
	[url_hinh_chinh] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_SanPham] PRIMARY KEY CLUSTERED 
(
	[id_san_pham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SanPhamYeuThich]    Script Date: 2/24/2019 9:32:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanPhamYeuThich](
	[id_yeu_thich] [int] IDENTITY(1,1) NOT NULL,
	[id_san_pham] [int] NOT NULL,
	[id_khach_hang] [int] NOT NULL,
 CONSTRAINT [PK_SanPhamYeuThich] PRIMARY KEY CLUSTERED 
(
	[id_yeu_thich] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TinhTrangDonHang]    Script Date: 2/24/2019 9:32:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TinhTrangDonHang](
	[id_tinh_trang] [int] IDENTITY(1,1) NOT NULL,
	[tinh_trang_don_hang] [nvarchar](500) NOT NULL,
	[ghi_chu] [nvarchar](max) NULL,
 CONSTRAINT [PK_TinhTrangDonHang] PRIMARY KEY CLUSTERED 
(
	[id_tinh_trang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDonHang_DonDatHang] FOREIGN KEY([id_don_hang])
REFERENCES [dbo].[DonDatHang] ([id_don_hang])
GO
ALTER TABLE [dbo].[ChiTietDonHang] CHECK CONSTRAINT [FK_ChiTietDonHang_DonDatHang]
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDonHang_SanPham] FOREIGN KEY([id_san_pham])
REFERENCES [dbo].[SanPham] ([id_san_pham])
GO
ALTER TABLE [dbo].[ChiTietDonHang] CHECK CONSTRAINT [FK_ChiTietDonHang_SanPham]
GO
ALTER TABLE [dbo].[DonDatHang]  WITH CHECK ADD  CONSTRAINT [FK_DonDatHang_KhachHang] FOREIGN KEY([id_khach_hang])
REFERENCES [dbo].[KhachHang] ([id_khach_hang])
GO
ALTER TABLE [dbo].[DonDatHang] CHECK CONSTRAINT [FK_DonDatHang_KhachHang]
GO
ALTER TABLE [dbo].[DonDatHang]  WITH CHECK ADD  CONSTRAINT [FK_DonDatHang_TinhTrangDonHang] FOREIGN KEY([id_tinh_trang])
REFERENCES [dbo].[TinhTrangDonHang] ([id_tinh_trang])
GO
ALTER TABLE [dbo].[DonDatHang] CHECK CONSTRAINT [FK_DonDatHang_TinhTrangDonHang]
GO
ALTER TABLE [dbo].[HinhSP]  WITH CHECK ADD  CONSTRAINT [FK_HinhSP_SanPham] FOREIGN KEY([id_sp])
REFERENCES [dbo].[SanPham] ([id_san_pham])
GO
ALTER TABLE [dbo].[HinhSP] CHECK CONSTRAINT [FK_HinhSP_SanPham]
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD  CONSTRAINT [FK_SanPham_DanhMucSanPham] FOREIGN KEY([id_danh_muc])
REFERENCES [dbo].[DanhMucSanPham] ([id_danh_muc])
GO
ALTER TABLE [dbo].[SanPham] CHECK CONSTRAINT [FK_SanPham_DanhMucSanPham]
GO
ALTER TABLE [dbo].[SanPhamYeuThich]  WITH CHECK ADD  CONSTRAINT [FK_SanPhamYeuThich_KhachHang] FOREIGN KEY([id_khach_hang])
REFERENCES [dbo].[KhachHang] ([id_khach_hang])
GO
ALTER TABLE [dbo].[SanPhamYeuThich] CHECK CONSTRAINT [FK_SanPhamYeuThich_KhachHang]
GO
ALTER TABLE [dbo].[SanPhamYeuThich]  WITH CHECK ADD  CONSTRAINT [FK_SanPhamYeuThich_SanPham] FOREIGN KEY([id_san_pham])
REFERENCES [dbo].[SanPham] ([id_san_pham])
GO
ALTER TABLE [dbo].[SanPhamYeuThich] CHECK CONSTRAINT [FK_SanPhamYeuThich_SanPham]
GO
USE [master]
GO
ALTER DATABASE [QL_BH] SET  READ_WRITE 
GO
