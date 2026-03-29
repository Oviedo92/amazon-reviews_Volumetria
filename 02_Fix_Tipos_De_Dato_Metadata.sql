-- 1. Crear la Base de Datos para tu Taller
CREATE DATABASE AmazonVolumetriaDB;
GO

-- 2. Usar esa base de datos
USE AmazonVolumetriaDB;
GO

-- 3. Crear la tabla de METADATA (Los Productos)
-- Nota del Senior: El 'asin' es la llave primaria (Primary Key)
CREATE TABLE Tabla_Metadata (
    asin VARCHAR(50) PRIMARY KEY,
    title NVARCHAR(MAX),      -- NVARCHAR soporta emojis y caracteres raros
    brand NVARCHAR(255),
    category NVARCHAR(MAX),
    price FLOAT               -- Decimal para los precios
);
GO

-- 4. Crear la tabla de RESEÑAS (Los Comentarios)
-- Nota del Senior: Aquí NO ponemos Primary Key al 'asin' porque un producto puede tener miles de reseñas.
CREATE TABLE Tabla_Resenas (
    asin VARCHAR(50),
    reviewerID VARCHAR(100),
    overall FLOAT,            -- Estrellas de 1 a 5
    verified BIT,             -- 1 = True (Compró), 0 = False
    reviewText NVARCHAR(MAX), -- El comentario largo
    summary NVARCHAR(MAX),    -- El título del comentario
    vote INT,                 -- Cantidad de likes (Los que arreglamos de NaN a 0)
    reviewDate DATE           -- Fecha limpia
);
GO

-- 1. Le quitamos la corona (Borramos la Llave Primaria temporalmente)
ALTER TABLE Tabla_Metadata
DROP CONSTRAINT PK__Tabla_Me__5B79DA86340C2919;

-- 2. Le cambiamos la ropa al formato moderno (Unicode) y nos aseguramos de que no acepte vacíos
ALTER TABLE Tabla_Metadata
ALTER COLUMN asin NVARCHAR(50) NOT NULL;

-- 3. Le devolvemos la corona (Volvemos a hacerla Llave Primaria)
ALTER TABLE Tabla_Metadata
ADD PRIMARY KEY (asin);

SELECT COUNT(*) AS Total_Registros 
FROM Tabla_Metadata;

SELECT TOP 10 * FROM Tabla_Metadata;