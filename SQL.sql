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


USE [yachts]
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
INSERT [dbo].[PayTypes] ([Id], [Name]) VALUES (1, N'Наличными')
GO
INSERT [dbo].[PayTypes] ([Id], [Name]) VALUES (2, N'Картой')
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