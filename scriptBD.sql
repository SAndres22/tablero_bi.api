USE [TABLEROBI]
GO
/****** Object:  Table [dbo].[Empresas]    Script Date: 25/06/2024 11:43:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresas](
	[EmpresaId] [int] NOT NULL,
	[Nit] [nvarchar](20) NULL,
	[NombreEmpresa] [nvarchar](120) NULL,
PRIMARY KEY CLUSTERED 
(
	[EmpresaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductosMasVendidos]    Script Date: 25/06/2024 11:43:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductosMasVendidos](
	[ProdMasVendidosId] [int] NOT NULL,
	[ProductoId] [int] NULL,
	[AnioMes] [char](7) NULL,
	[Descripcion] [nvarchar](120) NULL,
	[CantidadVendida] [float] NULL,
	[SucursalId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProdMasVendidosId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductosMenosVendidos]    Script Date: 25/06/2024 11:43:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductosMenosVendidos](
	[ProdMenosVendidosId] [int] NOT NULL,
	[ProductoId] [int] NULL,
	[AnioMes] [char](7) NULL,
	[Descripcion] [nvarchar](120) NULL,
	[CantidadVendida] [float] NULL,
	[SucursalId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProdMenosVendidosId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sucursales]    Script Date: 25/06/2024 11:43:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sucursales](
	[SucursalId] [int] NOT NULL,
	[NombreSucursal] [nvarchar](120) NULL,
	[EmpresaId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[SucursalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 25/06/2024 11:43:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[UsuarioId] [int] NOT NULL,
	[Username] [varchar](100) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[ImagenUrl] [nvarchar](max) NULL,
	[EmpresaId] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VentasMensuales]    Script Date: 25/06/2024 11:43:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VentasMensuales](
	[VentaId] [int] NOT NULL,
	[AnioMes] [varchar](7) NULL,
	[TotalVentas] [int] NULL,
	[TotalPrecioVentas] [float] NULL,
	[TotalValorUtilidad] [float] NULL,
	[MargenDeGanancia] [float] NULL,
	[ValorTicketPromedio] [float] NULL,
	[SucursalId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[VentaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ProductosMasVendidos]  WITH CHECK ADD FOREIGN KEY([SucursalId])
REFERENCES [dbo].[Sucursales] ([SucursalId])
GO
ALTER TABLE [dbo].[ProductosMenosVendidos]  WITH CHECK ADD FOREIGN KEY([SucursalId])
REFERENCES [dbo].[Sucursales] ([SucursalId])
GO
ALTER TABLE [dbo].[Sucursales]  WITH CHECK ADD FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([EmpresaId])
GO
ALTER TABLE [dbo].[VentasMensuales]  WITH CHECK ADD FOREIGN KEY([SucursalId])
REFERENCES [dbo].[Sucursales] ([SucursalId])
GO
