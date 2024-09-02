USE [master]
GO
/****** Object:  Database [BDVISITAS]    Script Date: 01/09/2024 20:55:34 ******/
CREATE DATABASE [BDVISITAS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BDVISITAS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\BDVISITAS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BDVISITAS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\BDVISITAS_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [BDVISITAS] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BDVISITAS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BDVISITAS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BDVISITAS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BDVISITAS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BDVISITAS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BDVISITAS] SET ARITHABORT OFF 
GO
ALTER DATABASE [BDVISITAS] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BDVISITAS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BDVISITAS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BDVISITAS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BDVISITAS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BDVISITAS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BDVISITAS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BDVISITAS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BDVISITAS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BDVISITAS] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BDVISITAS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BDVISITAS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BDVISITAS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BDVISITAS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BDVISITAS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BDVISITAS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BDVISITAS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BDVISITAS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BDVISITAS] SET  MULTI_USER 
GO
ALTER DATABASE [BDVISITAS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BDVISITAS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BDVISITAS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BDVISITAS] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BDVISITAS] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BDVISITAS] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [BDVISITAS] SET QUERY_STORE = ON
GO
ALTER DATABASE [BDVISITAS] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BDVISITAS]
GO
/****** Object:  Table [dbo].[Aulas]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aulas](
	[idAula] [int] IDENTITY(1,1) NOT NULL,
	[NombreAula] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[idAula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Edificios]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Edificios](
	[idEdificio] [int] IDENTITY(1,1) NOT NULL,
	[NombreEdificio] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[idEdificio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[idUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NULL,
	[Apellido] [varchar](50) NULL,
	[FechaNacimiento] [date] NULL,
	[TipoUsuario] [varchar](20) NULL,
	[Usuario] [varchar](50) NULL,
	[Contraseña] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[idUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Visitas]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Visitas](
	[idVisita] [int] IDENTITY(1,1) NOT NULL,
	[NombreAula] [varchar](100) NULL,
	[NombreEdificio] [varchar](100) NULL,
	[NombreVisitante] [varchar](50) NULL,
	[ApellidoVisitante] [varchar](50) NULL,
	[Carrera] [varchar](100) NULL,
	[Correo] [varchar](100) NULL,
	[HoraEntrada] [datetime] NULL,
	[HoraSalida] [datetime] NULL,
	[MotivoVisita] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[idVisita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Aulas] ON 

INSERT [dbo].[Aulas] ([idAula], [NombreAula]) VALUES (1, N'1A')
INSERT [dbo].[Aulas] ([idAula], [NombreAula]) VALUES (2, N'1B')
INSERT [dbo].[Aulas] ([idAula], [NombreAula]) VALUES (3, N'1C')
INSERT [dbo].[Aulas] ([idAula], [NombreAula]) VALUES (4, N'1D')
INSERT [dbo].[Aulas] ([idAula], [NombreAula]) VALUES (5, N'2A')
INSERT [dbo].[Aulas] ([idAula], [NombreAula]) VALUES (6, N'2B')
INSERT [dbo].[Aulas] ([idAula], [NombreAula]) VALUES (7, N'2C')
INSERT [dbo].[Aulas] ([idAula], [NombreAula]) VALUES (8, N'2D')
INSERT [dbo].[Aulas] ([idAula], [NombreAula]) VALUES (9, N'Registro')
INSERT [dbo].[Aulas] ([idAula], [NombreAula]) VALUES (11, N'Auditorio')
SET IDENTITY_INSERT [dbo].[Aulas] OFF
GO
SET IDENTITY_INSERT [dbo].[Edificios] ON 

INSERT [dbo].[Edificios] ([idEdificio], [NombreEdificio]) VALUES (1, N'Edificio 1')
INSERT [dbo].[Edificios] ([idEdificio], [NombreEdificio]) VALUES (2, N'Edificio 2')
INSERT [dbo].[Edificios] ([idEdificio], [NombreEdificio]) VALUES (3, N'Edificio 3')
INSERT [dbo].[Edificios] ([idEdificio], [NombreEdificio]) VALUES (6, N'Edificio 4')
INSERT [dbo].[Edificios] ([idEdificio], [NombreEdificio]) VALUES (7, N'Edificio 5')
INSERT [dbo].[Edificios] ([idEdificio], [NombreEdificio]) VALUES (9, N'Edificio 6')
SET IDENTITY_INSERT [dbo].[Edificios] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 

INSERT [dbo].[Usuarios] ([idUsuario], [Nombre], [Apellido], [FechaNacimiento], [TipoUsuario], [Usuario], [Contraseña]) VALUES (1, N'Juan', N'Guevara', CAST(N'1980-05-15' AS Date), N'Administrador', N'Admin1', N'A123')
INSERT [dbo].[Usuarios] ([idUsuario], [Nombre], [Apellido], [FechaNacimiento], [TipoUsuario], [Usuario], [Contraseña]) VALUES (2, N'Nicole', N'Suarez', CAST(N'1995-10-25' AS Date), N'General', N'Empleado1', N'U2121')
INSERT [dbo].[Usuarios] ([idUsuario], [Nombre], [Apellido], [FechaNacimiento], [TipoUsuario], [Usuario], [Contraseña]) VALUES (6, N'Julian', N'Lovera', CAST(N'1986-11-07' AS Date), N'General', N'Empleado3', N'1234')
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
GO
SET IDENTITY_INSERT [dbo].[Visitas] ON 

INSERT [dbo].[Visitas] ([idVisita], [NombreAula], [NombreEdificio], [NombreVisitante], [ApellidoVisitante], [Carrera], [Correo], [HoraEntrada], [HoraSalida], [MotivoVisita]) VALUES (1, N'1C', N'Edificio 3', N'Leslie', N'Lora', N'Multimedia', N'LL2@gmail.com', CAST(N'2024-04-09T01:39:12.000' AS DateTime), CAST(N'2024-04-10T01:39:12.000' AS DateTime), N'Esta estudiante tiene problemas con su cuenta en la plataforma virtual.')
INSERT [dbo].[Visitas] ([idVisita], [NombreAula], [NombreEdificio], [NombreVisitante], [ApellidoVisitante], [Carrera], [Correo], [HoraEntrada], [HoraSalida], [MotivoVisita]) VALUES (2, N'1C', N'Edificio 2', N'Anderson', N'Sosa', N'Multimedia', N'As19@gmail.com', CAST(N'2024-08-08T22:07:31.000' AS DateTime), CAST(N'2024-08-10T22:07:31.000' AS DateTime), N'No recibe las notificaciones de la institucion.')
INSERT [dbo].[Visitas] ([idVisita], [NombreAula], [NombreEdificio], [NombreVisitante], [ApellidoVisitante], [Carrera], [Correo], [HoraEntrada], [HoraSalida], [MotivoVisita]) VALUES (3, N'2A', N'Edificio 2', N'Julian', N'Alvarez', N'Multimedia', N'JL12@gmail.com', CAST(N'2024-04-20T23:27:27.000' AS DateTime), CAST(N'2024-04-28T23:27:27.000' AS DateTime), N'Problemas con su correo.')
INSERT [dbo].[Visitas] ([idVisita], [NombreAula], [NombreEdificio], [NombreVisitante], [ApellidoVisitante], [Carrera], [Correo], [HoraEntrada], [HoraSalida], [MotivoVisita]) VALUES (4, N'2B', N'Edificio 3', N'Jason', N'Guevara', N'Software', N'Jg123@gmail.com', CAST(N'2024-09-11T20:50:43.000' AS DateTime), CAST(N'2024-09-12T20:50:43.000' AS DateTime), N'Problemas con la plataforma.')
SET IDENTITY_INSERT [dbo].[Visitas] OFF
GO
/****** Object:  StoredProcedure [dbo].[EditarEdificio]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EditarEdificio]
    @idEdificio INT,
    @NuevoNombreEdificio VARCHAR(100)
AS
BEGIN
    UPDATE Edificios
    SET NombreEdificio = @NuevoNombreEdificio
    WHERE idEdificio = @idEdificio;
END;
GO
/****** Object:  StoredProcedure [dbo].[EditarNombreAula]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EditarNombreAula]
    @idAula INT,
    @nuevoNombreAula VARCHAR(100)
AS
BEGIN
    UPDATE Aulas
    SET NombreAula = @nuevoNombreAula
    WHERE idAula = @idAula;
END;
GO
/****** Object:  StoredProcedure [dbo].[EliminarAula]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EliminarAula]
    @ID INT
AS
BEGIN
    DELETE FROM Aulas WHERE idAula = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[EliminarEdificio]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EliminarEdificio]
    @idEdificio INT
AS
BEGIN
    DELETE FROM Edificios
    WHERE idEdificio = @idEdificio;
END;
GO
/****** Object:  StoredProcedure [dbo].[EliminarUsuario]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EliminarUsuario]
    @idUsuario INT
AS
BEGIN
    DELETE FROM Usuarios
    WHERE idUsuario = @idUsuario;
END;
GO
/****** Object:  StoredProcedure [dbo].[InsertarAula]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertarAula]
    @NombreAula VARCHAR(100)
AS
BEGIN
    INSERT INTO Aulas (NombreAula) VALUES (@NombreAula);
END;
GO
/****** Object:  StoredProcedure [dbo].[InsertarEdificio]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertarEdificio]
    @NombreEdificio VARCHAR(100)
AS
BEGIN
    INSERT INTO Edificios (NombreEdificio)
    VALUES (@NombreEdificio);
END;
GO
/****** Object:  StoredProcedure [dbo].[InsertarUsuario]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertarUsuario]
    @Nombre VARCHAR(50),
    @Apellido VARCHAR(50),
    @FechaNacimiento DATE,
    @TipoUsuario VARCHAR(20),
    @Usuario VARCHAR(50),
    @Contraseña VARCHAR(100)
AS
BEGIN
    INSERT INTO Usuarios (Nombre, Apellido, FechaNacimiento, TipoUsuario, Usuario, Contraseña)
    VALUES (@Nombre, @Apellido, @FechaNacimiento, @TipoUsuario, @Usuario, @Contraseña);
END;
GO
/****** Object:  StoredProcedure [dbo].[ListarAulas]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ListarAulas]
AS
BEGIN
    -- Seleccionar el ID y el nombre de todas las aulas
    SELECT idAula, NombreAula FROM Aulas;
END;
GO
/****** Object:  StoredProcedure [dbo].[ListarEdificios]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ListarEdificios]
AS
BEGIN
    SELECT * FROM Edificios;
END;
GO
/****** Object:  StoredProcedure [dbo].[ListarUsuarios]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ListarUsuarios]
AS
BEGIN
    SELECT idUsuario, Nombre, Apellido, TipoUsuario, Usuario, Contraseña
    FROM Usuarios;
END;
GO
/****** Object:  StoredProcedure [dbo].[ModificarUsuario]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ModificarUsuario]
    @idUsuario INT,
    @Nombre VARCHAR(50),
    @Apellido VARCHAR(50),
    @FechaNacimiento DATE,
    @TipoUsuario VARCHAR(20),
    @Usuario VARCHAR(50),
    @Contraseña VARCHAR(100)
AS
BEGIN
    UPDATE Usuarios
    SET Nombre = @Nombre,
        Apellido = @Apellido,
        FechaNacimiento = @FechaNacimiento,
        TipoUsuario = @TipoUsuario,
        Usuario = @Usuario,
        Contraseña = @Contraseña
    WHERE idUsuario = @idUsuario;
END;
GO
/****** Object:  StoredProcedure [dbo].[ObtenerDatosUsuarios]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ObtenerDatosUsuarios]
AS
BEGIN
    SELECT idUsuario,
           Nombre,
           Apellido,
           FechaNacimiento,
           TipoUsuario,
           Usuario,
		   Contraseña
    FROM Usuarios;
END;
GO
/****** Object:  StoredProcedure [dbo].[ObtenerDatosVisitas]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ObtenerDatosVisitas]
AS
BEGIN
    SELECT NombreAula,
           NombreEdificio,
           NombreVisitante,
           Correo,
           HoraEntrada,
           HoraSalida,
           MotivoVisita
    FROM Visitas;
END;
GO
/****** Object:  StoredProcedure [dbo].[RegistrarVisita]    Script Date: 01/09/2024 20:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RegistrarVisita]
    @NombreAula VARCHAR(100),
    @NombreEdificio VARCHAR(100),
    @NombreVisitante VARCHAR(50),
    @ApellidoVisitante VARCHAR(50),
    @Carrera VARCHAR(100),
    @Correo VARCHAR(100),
    @HoraEntrada DATETIME,
    @HoraSalida DATETIME,
    @MotivoVisita VARCHAR(200)
AS
BEGIN
    INSERT INTO Visitas (NombreAula, NombreEdificio, NombreVisitante, ApellidoVisitante, Carrera, Correo, HoraEntrada, HoraSalida, MotivoVisita)
    VALUES (@NombreAula, @NombreEdificio, @NombreVisitante, @ApellidoVisitante, @Carrera, @Correo, @HoraEntrada, @HoraSalida, @MotivoVisita);
END;
GO
USE [master]
GO
ALTER DATABASE [BDVISITAS] SET  READ_WRITE 
GO
