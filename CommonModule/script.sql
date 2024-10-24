USE [master]
GO
/****** Object:  Database [BritishGarments]    Script Date: 10/24/2024 6:13:26 PM ******/
CREATE DATABASE [BritishGarments]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BritishGarments', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\BritishGarments.mdf' , SIZE = 3136KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BritishGarments_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\BritishGarments_log.ldf' , SIZE = 784KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [BritishGarments] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BritishGarments].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BritishGarments] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BritishGarments] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BritishGarments] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BritishGarments] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BritishGarments] SET ARITHABORT OFF 
GO
ALTER DATABASE [BritishGarments] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BritishGarments] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BritishGarments] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BritishGarments] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BritishGarments] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BritishGarments] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BritishGarments] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BritishGarments] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BritishGarments] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BritishGarments] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BritishGarments] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BritishGarments] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BritishGarments] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BritishGarments] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BritishGarments] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BritishGarments] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [BritishGarments] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BritishGarments] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BritishGarments] SET  MULTI_USER 
GO
ALTER DATABASE [BritishGarments] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BritishGarments] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BritishGarments] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BritishGarments] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [BritishGarments]
GO
/****** Object:  Table [dbo].[Colors]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Colors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[RGB Color] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_Colors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Discount]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discount](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[Discount Amount] [int] NULL,
 CONSTRAINT [PK_Discount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductColor]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductColor](
	[ProductId] [int] NOT NULL,
	[ColorId] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_ProductColor_1] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[ColorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductImage]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductImage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[Image] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_ProductImage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[Description] [nvarchar](1000) NULL,
	[IsActive] [bit] NOT NULL,
	[Gender] [varchar](100) NULL,
	[Weight] [int] NULL,
	[Length] [int] NULL,
	[Height] [int] NULL,
	[InStock] [int] NULL,
	[Discount] [int] NULL,
 CONSTRAINT [PK__Products__B40CC6ED8A743232] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductSize]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductSize](
	[ProductId] [int] NOT NULL,
	[SizeId] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_ProductSize_1] PRIMARY KEY CLUSTERED 
(
	[SizeId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseDetails]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseDetails](
	[PurchaseDetailId] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseId] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PurchaseDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Purchases]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purchases](
	[PurchaseId] [int] IDENTITY(1,1) NOT NULL,
	[VendorId] [int] NOT NULL,
	[PurchaseDate] [datetime] NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PurchaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[isActive] [bit] NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SaleDetails]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaleDetails](
	[SaleDetailID] [int] IDENTITY(1,1) NOT NULL,
	[SaleID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[TotalPrice]  AS ([Quantity]*[UnitPrice]),
PRIMARY KEY CLUSTERED 
(
	[SaleDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sales]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sales](
	[SaleID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[SaleDate] [datetime2](7) NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[PaymentMethod] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[SaleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sizes]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sizes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_SizeIs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/24/2024 6:13:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](40) NULL,
	[Email] [nvarchar](100) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[PasswordHash] [varbinary](max) NULL,
	[PasswordSalt] [varbinary](max) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[CreatedBy] [datetime2](7) NULL,
	[RoleId] [int] NULL,
	[PasswordResetToken] [nvarchar](max) NULL,
	[ResetTokenExpires] [datetime2](7) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Colors] ON 

INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (2, N'Red', N'255,0,0', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (3, N'Green', N'0,255,0', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (4, N'Blue', N'0,0,255', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (5, N'Yellow', N'255,255,0', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (6, N'Cyan', N'0,255,255', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (8, N'Magenta', N'255,0,255', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (9, N'Orange', N'255,165,0', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (10, N'Purple', N'128,0,128', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (11, N'Pink', N'255,192,203', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (12, N'Black', N'0,0,0', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (13, N'White', N'255,255,255', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (14, N'Gray', N'128,128,128', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (15, N'Brown', N'165,42,42', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (16, N'Lime', N'0,255,0', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (17, N'Teal', N'0,128,128', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (18, N'Navy', N'0,0,128', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (19, N'Olive', N'128,128,0', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (20, N'Maroon', N'128,0,0', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (21, N'Gold', N'255,215,0', NULL)
INSERT [dbo].[Colors] ([Id], [Name], [RGB Color], [Description]) VALUES (22, N'Silver', N'192,192,192', NULL)
SET IDENTITY_INSERT [dbo].[Colors] OFF
GO
SET IDENTITY_INSERT [dbo].[Discount] ON 

INSERT [dbo].[Discount] ([Id], [Name], [Description], [IsActive], [Discount Amount]) VALUES (2, N'New Year Discount', NULL, 1, 5)
INSERT [dbo].[Discount] ([Id], [Name], [Description], [IsActive], [Discount Amount]) VALUES (4, N'Chrismtas discount', NULL, 1, 10)
SET IDENTITY_INSERT [dbo].[Discount] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductCategory] ON 

INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (12, N'Boats', N'Footwear', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (13, N'Socks', N'Footwear', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (16, N'Sports Shoes', N'Footwear', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (18, N'Sandal', N'Footwear', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (21, N'Slippers', N'Footwear', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (23, N'Casual Shoes', N'Footwear', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (26, N'Formal Shoes
', N'Footwear', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (27, N'
Heels', N'Footwear', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (28, N'Cap', N'Accessories', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (30, N'Bras', N'Accessories', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (31, N'Panty', N'Accessories', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (32, N'Boxers', N'Accessories', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (33, N'Gloves', N'Accessories', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (34, N'Bags', N'Accessories', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (35, N'Coat', N'Clothing', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (36, N'Jackets', N'Clothing', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (37, N'Trousers', N'Clothing', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (38, N'Leatherwear', N'Clothing', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (39, N'Sleepwear', N'Clothing', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (40, N'Swimwear', N'Clothing', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (41, N'Security uniform', N'Clothing', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (42, N'Sports uniform', N'Clothing', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (43, N'Belts', N'Accessories', 1)
INSERT [dbo].[ProductCategory] ([CategoryId], [Name], [Description], [isActive]) VALUES (44, N'Ties', N'Accessories', 1)
SET IDENTITY_INSERT [dbo].[ProductCategory] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleId], [Name], [Description], [isActive]) VALUES (1, N'Admin', N'Full access', 1)
INSERT [dbo].[Roles] ([RoleId], [Name], [Description], [isActive]) VALUES (2, N'Customer', N'Limited access', 1)
INSERT [dbo].[Roles] ([RoleId], [Name], [Description], [isActive]) VALUES (3, N'Vendor', N'Seller person, from whom we purchase the items', 1)
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Sizes] ON 

INSERT [dbo].[Sizes] ([Id], [Name], [Description]) VALUES (1, N'XS', N'Xttra Small')
INSERT [dbo].[Sizes] ([Id], [Name], [Description]) VALUES (2, N'S', N'Small')
INSERT [dbo].[Sizes] ([Id], [Name], [Description]) VALUES (3, N'M', N'Medium')
INSERT [dbo].[Sizes] ([Id], [Name], [Description]) VALUES (4, N'L', N'Large')
INSERT [dbo].[Sizes] ([Id], [Name], [Description]) VALUES (5, N'XL', N'Xtra Large ')
INSERT [dbo].[Sizes] ([Id], [Name], [Description]) VALUES (6, N'XXL', N'Double Xtra Large')
SET IDENTITY_INSERT [dbo].[Sizes] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [Username], [Email], [FirstName], [LastName], [PasswordHash], [PasswordSalt], [CreatedAt], [UpdatedAt], [CreatedBy], [RoleId], [PasswordResetToken], [ResetTokenExpires]) VALUES (1, N'aaa', N'kashifajmal.ksk@gmail.com', N'aaaaaaa', N'bbbbbbbb', 0x4CE150E53F3E22C785F60FDD662EA429CB1FA8EA62E6096583C674DD7A762621F683FC50419B3751046A03F99E77CC44F82D76567DD20258BAE0E8FA5B884BCE, 0xE8998FFAE0FA41F03FF7FE4F0194DF84377A2DA4A010F8458BCE1136EF27CFDBE638945D588B2708429EBD9952ECD8D38E8810D4F68633D425D6C17E68A9515A7257396EEFAAD370259F85CC334171DEB9E4B5C55638B4E9C6EFA06A30932A25E77017C1B41689187D60FBDBC3396BA0D462589D1BA60A3EB33FBDA06ABE27EE, CAST(N'2024-09-29T15:32:48.1854186' AS DateTime2), CAST(N'2024-09-29T15:32:48.1948696' AS DateTime2), NULL, 1, N'x81s3OB2YXg0kIWGv5ekgIhhCR1q/zsOITV6ahQkH46M+Hn389IaHx8WUGnwUWW9WCXtLpP90uo1Ys+QYD6eAg==', NULL)
INSERT [dbo].[Users] ([UserId], [Username], [Email], [FirstName], [LastName], [PasswordHash], [PasswordSalt], [CreatedAt], [UpdatedAt], [CreatedBy], [RoleId], [PasswordResetToken], [ResetTokenExpires]) VALUES (2, N'abc', N'ajmalshahkashif@gmail.com', N'aa', N'bb', 0xB0345D79E6043C8A052B3BF7E79987A68A17D7B1CBCC0140414BBF7948AE3CB8B92D684AEA6B2EF4173FABF7AA030404D3DBB7C2CA295E276750655015BD4E49, 0x39CDE4407A63F73805D116E34D43282DE3C23B7BEC23D4F0B648401595D4774AE447989A09E01DEEFAECE0973A170F360791841E24A7538B0A8B4B90CFD5586260C9300D881D601AB101A77864EAE8159299CA782FF94921A854C7D2FB1CCF89751006A897A9590DE4457EA9CB429729B0F56C85FC2B6DA25239E67A004DB2B8, CAST(N'2024-09-02T10:58:43.9798291' AS DateTime2), CAST(N'2024-09-02T10:58:43.9799792' AS DateTime2), NULL, 1, NULL, NULL)
INSERT [dbo].[Users] ([UserId], [Username], [Email], [FirstName], [LastName], [PasswordHash], [PasswordSalt], [CreatedAt], [UpdatedAt], [CreatedBy], [RoleId], [PasswordResetToken], [ResetTokenExpires]) VALUES (5, NULL, N'vendor1@gmail.com', NULL, N'Vendor1', NULL, NULL, NULL, NULL, NULL, 3, NULL, NULL)
INSERT [dbo].[Users] ([UserId], [Username], [Email], [FirstName], [LastName], [PasswordHash], [PasswordSalt], [CreatedAt], [UpdatedAt], [CreatedBy], [RoleId], [PasswordResetToken], [ResetTokenExpires]) VALUES (7, NULL, N'vendor2@gmail.com', NULL, N'Vendor2', NULL, NULL, NULL, NULL, NULL, 3, NULL, NULL)
INSERT [dbo].[Users] ([UserId], [Username], [Email], [FirstName], [LastName], [PasswordHash], [PasswordSalt], [CreatedAt], [UpdatedAt], [CreatedBy], [RoleId], [PasswordResetToken], [ResetTokenExpires]) VALUES (11, NULL, N'vendor3@gamil.com', NULL, N'Vendor3', NULL, NULL, NULL, NULL, NULL, 3, NULL, NULL)
INSERT [dbo].[Users] ([UserId], [Username], [Email], [FirstName], [LastName], [PasswordHash], [PasswordSalt], [CreatedAt], [UpdatedAt], [CreatedBy], [RoleId], [PasswordResetToken], [ResetTokenExpires]) VALUES (13, NULL, N'vendor4@gamil.com', NULL, N'Vendor4', NULL, NULL, NULL, NULL, NULL, 3, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_User_RoleId]    Script Date: 10/24/2024 6:13:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_User_RoleId] ON [dbo].[Users]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ProductCategory] ADD  CONSTRAINT [DF_ProductCategory_isActive]  DEFAULT ((1)) FOR [isActive]
GO
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF__Products__Create__6E01572D]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Purchases] ADD  DEFAULT (getdate()) FOR [PurchaseDate]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_isActive]  DEFAULT ((1)) FOR [isActive]
GO
ALTER TABLE [dbo].[Sales] ADD  DEFAULT (getdate()) FOR [SaleDate]
GO
ALTER TABLE [dbo].[Sales] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ProductColor]  WITH CHECK ADD  CONSTRAINT [FK_ProductColor_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[ProductColor] CHECK CONSTRAINT [FK_ProductColor_Products]
GO
ALTER TABLE [dbo].[ProductImage]  WITH CHECK ADD  CONSTRAINT [FK_ProductImage_ProductImage] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[ProductImage] CHECK CONSTRAINT [FK_ProductImage_ProductImage]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Products] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[ProductCategory] ([CategoryId])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Products]
GO
ALTER TABLE [dbo].[ProductSize]  WITH CHECK ADD  CONSTRAINT [FK_ProductSize_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[ProductSize] CHECK CONSTRAINT [FK_ProductSize_Products]
GO
ALTER TABLE [dbo].[ProductSize]  WITH CHECK ADD  CONSTRAINT [FK_ProductSize_SizeIs] FOREIGN KEY([SizeId])
REFERENCES [dbo].[Sizes] ([Id])
GO
ALTER TABLE [dbo].[ProductSize] CHECK CONSTRAINT [FK_ProductSize_SizeIs]
GO
ALTER TABLE [dbo].[PurchaseDetails]  WITH CHECK ADD  CONSTRAINT [FK__PurchaseD__Produ__08B54D69] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[PurchaseDetails] CHECK CONSTRAINT [FK__PurchaseD__Produ__08B54D69]
GO
ALTER TABLE [dbo].[PurchaseDetails]  WITH CHECK ADD FOREIGN KEY([PurchaseId])
REFERENCES [dbo].[Purchases] ([PurchaseId])
GO
ALTER TABLE [dbo].[Purchases]  WITH CHECK ADD FOREIGN KEY([VendorId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[SaleDetails]  WITH CHECK ADD  CONSTRAINT [FK__SaleDetai__Produ__7B5B524B] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[SaleDetails] CHECK CONSTRAINT [FK__SaleDetai__Produ__7B5B524B]
GO
ALTER TABLE [dbo].[SaleDetails]  WITH CHECK ADD FOREIGN KEY([SaleID])
REFERENCES [dbo].[Sales] ([SaleID])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_User_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_User_Roles_RoleId]
GO
USE [master]
GO
ALTER DATABASE [BritishGarments] SET  READ_WRITE 
GO
