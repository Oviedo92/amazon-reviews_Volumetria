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


-- Le cambiamos la ropa al ASIN para que coincida con Metadata y SSIS
ALTER TABLE Tabla_Resenas
ALTER COLUMN asin NVARCHAR(50);

-- Actualizamos las que faltan para que sean Unicode (NVARCHAR)
ALTER TABLE Tabla_Resenas
ALTER COLUMN reviewerID NVARCHAR(100);


SELECT COUNT(*) AS Total_Registros 
FROM Tabla_Metadata;

TRUNCATE TABLE Tabla_Resenas;

SELECT COUNT(*) FROM Tabla_Resenas

SELECT TOP 5000 * FROM Tabla_Metadata;

SELECT TOP 10000 * FROM Tabla_Resenas;

SELECT TOP 10000 * FROM Tabla_Resenas WHERE asin IN (SELECT asin FROM Tabla_Metadata)

-- 1. Borramos hasta que queden 5 millones exactos
WHILE (SELECT COUNT(*) FROM Tabla_Resenas) > 5000000
BEGIN
    DELETE TOP (500000) FROM Tabla_Resenas;
END
GO

-- 2. OBLIGATORIO: Le decimos a SQL que nos devuelva ese espacio físico
DBCC SHRINKDATABASE (AmazonVolumetriaDB);
GO

-- 1. Le ponemos su Llave Primaria (Obligatorio para paginación en C#)
-- Le ponemos su Llave Primaria
ALTER TABLE Tabla_Resenas 
ADD IdResena INT IDENTITY(1,1) PRIMARY KEY;
GO

-- 2. Le creamos el Índice al ASIN para que las búsquedas vuelen ?
-- Creamos el Índice para el ASIN
CREATE NONCLUSTERED INDEX IX_TablaResenas_ASIN 
ON Tabla_Resenas (asin);
GO

SELECT TOP 8000000 R.asin, M.title 
FROM Tabla_Resenas R
INNER JOIN Tabla_Metadata M ON R.asin = M.asin