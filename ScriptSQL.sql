CREATE DATABASE PRUEBA_MAKERS

USE PRUEBA_MAKERS

-- Roles
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL UNIQUE
);

INSERT INTO Roles (Nombre) VALUES ('Usuario');
INSERT INTO Roles (Nombre) VALUES ('Administrador');

-- Usuarios
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);

-- Relación muchos a muchos: Usuarios - Roles
CREATE TABLE UsuariosRoles (
    UsuarioId INT,
    RolId INT,
    PRIMARY KEY (UsuarioId, RolId),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE CASCADE,
    FOREIGN KEY (RolId) REFERENCES Roles(Id) ON DELETE CASCADE
);

-- Estados del préstamo
CREATE TABLE EstadosPrestamo (
    Id INT PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL
);

INSERT INTO EstadosPrestamo (Id, Nombre) VALUES (1, 'Pendiente');
INSERT INTO EstadosPrestamo (Id, Nombre) VALUES (2, 'Aprobado');
INSERT INTO EstadosPrestamo (Id, Nombre) VALUES (3, 'Rechazado');

-- Prestamos
CREATE TABLE Prestamos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UsuarioId INT NOT NULL,
    Monto DECIMAL(18,2) NOT NULL,
    PlazoEnMeses INT NOT NULL,
    EstadoId INT NOT NULL DEFAULT 1,
    FechaSolicitud DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NULL,

    CONSTRAINT FK_Prestamos_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    CONSTRAINT FK_Prestamos_Estado FOREIGN KEY (EstadoId) REFERENCES EstadosPrestamo(Id)
);

select * from Usuarios
select * from UsuariosRoles

select * from Prestamos
select * from EstadosPrestamo