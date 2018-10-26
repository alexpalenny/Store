--dotnet ef database update --project SeaStore.Entities --startup-project SeaStore.Api
--dotnet ef  migrations add 10262018_DbChange --project SeaStore.Entities --startup-project SeaStore.Api

--"DefaultConnection": "Server=.\\;Database=SeaStore;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=False;User ID=sa;Password=alexpass12!;",
--"Server=.\;Database=SeaStore;Trusted_Connection=True;MultipleActiveResultSets=true"


--Connection string
--Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;



USE [yachts]
GO

/****** Object:  Table [dbo].[BoatTypes]  Script Date: 10/14/2018 11:07:34 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BoatTypes](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL
 CONSTRAINT [PK_BoatTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[PayTypes]  Script Date: 10/14/2018 11:07:34 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PayTypes](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL
 CONSTRAINT [PK_PayTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Boats]  Script Date: 10/14/2018 11:07:34 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Boats](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[TypeId] [int] NOT NULL,
	[Capacity] [int] NULL,
	[Length] [decimal](8,2) NULL,
	[Beam] [decimal](8,2) NULL,
	[Descritption] [nvarchar](500) NULL,
	[Price] [decimal](18,2) NULL,
	[PayTypeId] [int] NULL,
	[MinOrder] [decimal](8,2) NULL,
	[Default] [bit] NOT NULL DEFAULT(0)
 CONSTRAINT [PK_Boats] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Boats]  WITH CHECK ADD  CONSTRAINT [FK_Boats_BoatTypes] FOREIGN KEY([TypeId])
REFERENCES [dbo].[BoatTypes] ([Id])
GO

ALTER TABLE [dbo].[Boats] CHECK CONSTRAINT [FK_Boats_BoatTypes]
GO

ALTER TABLE [dbo].[Boats]  WITH CHECK ADD  CONSTRAINT [FK_Boats_PayTypes] FOREIGN KEY([PayTypeId])
REFERENCES [dbo].[PayTypes] ([Id])
GO

ALTER TABLE [dbo].[Boats] CHECK CONSTRAINT [FK_Boats_PayTypes]
GO


USE [SeaStore]
GO

SET IDENTITY_INSERT [dbo].[BoatTypes] ON 
GO
INSERT [dbo].[BoatTypes] ([Id], [Name]) VALUES (1, N'Парусная яхта')
GO
INSERT [dbo].[BoatTypes] ([Id], [Name]) VALUES (2, N'Моторная яхта')
GO
INSERT [dbo].[BoatTypes] ([Id], [Name]) VALUES (3, N'Катер')
GO
INSERT [dbo].[BoatTypes] ([Id], [Name]) VALUES (4, N'SB-20')
GO
INSERT [dbo].[BoatTypes] ([Id], [Name]) VALUES (5, N'Каяк')
GO
INSERT [dbo].[BoatTypes] ([Id], [Name]) VALUES (6, N'Лодка')
GO
INSERT [dbo].[BoatTypes] ([Id], [Name]) VALUES (7, N'Гидроцикл')
GO

SET IDENTITY_INSERT [dbo].[BoatTypes] OFF 
GO

SET IDENTITY_INSERT [dbo].[PayTypes] ON 
GO

INSERT [dbo].[PayTypes] ([Id], [Name]) VALUES (1, N'Наличными')
GO
INSERT [dbo].[PayTypes] ([Id], [Name]) VALUES (2, N'Картой')
GO

SET IDENTITY_INSERT [dbo].[PayTypes] OFF 
GO

USE [SeaStore]
GO
SET IDENTITY_INSERT [dbo].[Boats] ON 
GO
INSERT [dbo].[Boats] ([Id], [Beam], [BoatTypeId], [Capacity], [Default], [Descritption], [Length], [MinOrder], [Name], [PayTypeId], [Price], [TypeId]) VALUES (1, CAST(3.50 AS Decimal(18, 2)), 1, 10, 1, N'На борту спальня и каюткомпания, гальюн, холодильник, газовая плита, телевизор, музыка. Имеются удочки для рыбалки, ласты и маска, для желающих нырнуть.', CAST(14.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), N'Форт', 1, CAST(2500.00 AS Decimal(18, 2)), 1)
GO
INSERT [dbo].[Boats] ([Id], [Beam], [BoatTypeId], [Capacity], [Default], [Descritption], [Length], [MinOrder], [Name], [PayTypeId], [Price], [TypeId]) VALUES (2, CAST(3.00 AS Decimal(18, 2)), 1, 6, 1, N'На борту спальня и каюткомпания, гальюн, холодильник, газовая плита, музыка. Ласты и маска, для желающих нырнуть.', CAST(10.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), N'Нир', 1, CAST(1500.00 AS Decimal(18, 2)), 1)
GO
INSERT [dbo].[Boats] ([Id], [Beam], [BoatTypeId], [Capacity], [Default], [Descritption], [Length], [MinOrder], [Name], [PayTypeId], [Price], [TypeId]) VALUES (3, CAST(2.10 AS Decimal(18, 2)), 1, 4, 1, N'На борту спальня гальюн, холодильник, газовая плита, музыка. Ласты и маска, для желающих нырнуть.', CAST(7.50 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), N'Ник', 1, CAST(1000.00 AS Decimal(18, 2)), 1)
GO
SET IDENTITY_INSERT [dbo].[Boats] OFF
GO


SELECT TOP (1000) b.[Id]
      ,b.[Name]
      ,b.[TypeId]
	  ,bt.Name AS Type
      ,b.[Capacity]
      ,b.[Length]
      ,b.[Beam]
      ,b.[Descritption]
      ,b.[Price]
      ,b.[PayTypeId]
	  ,pt.Name AS PayType
      ,b.[MinOrder]
      ,b.[Default]
  FROM [yachts].[dbo].[Boats] b
  INNER JOIN [yachts].[dbo].[BoatTypes] bt ON bt.Id = b.TypeId
  INNER JOIN [yachts].[dbo].[PayTypes] pt ON pt.Id = b.TypeId