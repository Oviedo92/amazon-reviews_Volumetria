## 📸 Galería Paso a Paso del Proyecto

A continuación, se documenta visualmente todo el proceso, desde la limpieza de datos en Python, pasando por ETL, arquitectura de base de datos, hasta las pruebas de estrés en los contenedores:

### Fase 1 a Fase 6: Documentación Visual

<details>
  <summary><b>👉 Clic aquí para expandir la Galería Paso a Paso (39 imágenes) 📸</b></summary>
  
![Paso 1](assets/captura1.png)
![Paso 2](assets/captura2.png)
![Paso 3](assets/captura3.png)
![Paso 4](assets/captura4.png)
![Paso 5](assets/captura5.png)
![Paso 6](assets/captura6.png)
![Paso 7](assets/captura7.png)
![Paso 8](assets/captura8.png)
![Paso 9](assets/captura9.png)
![Paso 10](assets/captura10.png)
![Paso 11](assets/captura11.png)
![Paso 12](assets/captura12.png)
![Paso 13](assets/captura13.png)
![Paso 14](assets/captura14.png)
![Paso 15](assets/captura15.png)
![Paso 16](assets/captura16.png)
![Paso 17](assets/captura17.png)
![Paso 18](assets/captura18.png)
![Paso 19](assets/captura19.png)
![Paso 20](assets/captura20.png)
![Paso 21](assets/captura21.png)
![Paso 22](assets/captura22.png)
![Paso 23](assets/captura23.png)
![Paso 24](assets/captura24.png)
![Paso 25](assets/captura25.png)
![Paso 26](assets/captura26.png)
![Paso 27](assets/captura27.png)
![Paso 28](assets/captura28.png)
![Paso 29](assets/captura29.png)
![Paso 30](assets/captura30.png)
![Paso 31](assets/captura31.png)
![Paso 32](assets/captura32.png)
![Paso 33](assets/captura33.png)
![Paso 34](assets/captura34.png)
![Paso 35](assets/captura35.png)
![Paso 36](assets/captura36.png)
![Paso 37](assets/captura37.png)
![Paso 38](assets/captura38.png)
![Paso 39](assets/captura39.png)

</details>


# 🚀 Taller de Volumetría, ETL y Arquitectura Distribuida - Amazon Data

Este repositorio documenta el ciclo de vida completo de un proyecto de alta volumetría de datos. Abarca desde la extracción y limpieza de datos crudos (EDA), carga masiva mediante ETL, optimización de base de datos relacional, hasta la construcción de una API distribuida balanceada y sometida a pruebas de estrés extremas.

## 🧠 Arquitectura y Pipeline del Proyecto (Data Pipeline)
El flujo de los datos desde su origen hasta el monitoreo en tiempo real:
`Archivos Crudos -> Jupyter (Python) -> SSIS (ETL) -> SQL Server -> .NET Core API -> Nginx (Load Balancer) -> 3 Nodos Docker -> Prometheus/Grafana -> JMeter (Estrés)`

![Arquitectura Cloud SCHEMA](assets/reduce-latency-schema.jpg)


![.NET Core API](assets/captura14.png)
![.NET Core API](assets/captura15.png)
![.NET Core API](assets/captura16.png)
![Estructura/Arquitectura .NET Core API](assets/captura17.png)


![Nginx (Load Balancer)](assets/captura20.png)
![JMeter (Estrés)](assets/captura27.png)
![JMeter (Estrés)](assets/captura28.png)
![JMeter (Estrés)](assets/captura34.png)
![JMeter (Estrés)](assets/captura35.png)
![JMeter (Estrés)](assets/captura36.png)
![JMeter (Estrés)](assets/captura37.png)



![Archivos Tabla Metadata Limpio](assets/captura38.png)
![Archivos Tabla Reseñas Limpio](assets/captura39.png)

---

## 🛠️ Fase 1: Análisis Exploratorio de Datos (EDA)
Se utilizó **Python** y **Jupyter Notebooks** dentro de un entorno virtual (`venv`) para procesar los datasets originales de Amazon.
* Se eliminaron campos nulos y se estandarizaron formatos (tratamiento de comillas y caracteres especiales).
* El resultado fueron dos datasets optimizados: `Metadata_Limpio` y `Reviews_Limpio`.

![EDA y Limpieza de Datos](assets/captura12.png)
*(Arriba: Visualización de columnas y filas limpias listas para su ingesta).*

---

## 🗄️ Fase 2: Ingesta Masiva (ETL) con SSIS
Para manejar la carga masiva sin saturar la memoria local, se construyó un pipeline de integración utilizando **SQL Server Integration Services (SSIS)** (vía Visual Studio) y **SQL Server Management Studio (SSMS) v21**.
* Se inyectaron más de **8 millones de registros** limpios distribuidos en `Tabla_Metadata` y `Tabla_Resenas`.

![Carga de SSIS y Volumetría](assets/captura8.png)
![Carga de SSIS y Volumetría](assets/captura9.png)
![Registro de filas luego de la Integracion del SSIS(Pipeline)](assets/captura12.png)
![Registro de filas luego de la Integracion del SSIS(Pipeline)](assets/captura13.png)

---

## ⚡ Fase 3: Optimización y Tuning de Base de Datos
Debido a la inmensa cantidad de registros, se detectaron cuellos de botella y errores de tipos de datos durante las pruebas. Se implementaron las siguientes sentencias SQL para sanear la data, crear índices de alto rendimiento y purgar el espacio físico:

![Sentencias SQL](assets/captura3.png)
![Sentencias SQL](assets/captura4.png)
![Sentencias SQL](assets/captura5.png)
![Estructura de tablas Resenas y Metadata SQL](assets/captura6.png)
![Sentencias SQL](assets/captura11.png)


```sql
-- 1. Ajuste de Tipos de Datos (Unicode) y Primary Keys
ALTER TABLE Tabla_Metadata DROP CONSTRAINT PK__Tabla_Me__5B79DA86340C2919;
ALTER TABLE Tabla_Metadata ALTER COLUMN asin NVARCHAR(50) NOT NULL;
ALTER TABLE Tabla_Metadata ADD PRIMARY KEY (asin);

ALTER TABLE Tabla_Resenas ALTER COLUMN asin NVARCHAR(50);
ALTER TABLE Tabla_Resenas ALTER COLUMN reviewerID NVARCHAR(100);
ALTER TABLE Tabla_Resenas ADD IdResena INT IDENTITY(1,1) PRIMARY KEY;

-- 2. Creación de Índices (Non-Clustered) para acelerar consultas de la API
CREATE NONCLUSTERED INDEX IX_Amazon_Asin_Fast ON Tabla_Resenas (asin) INCLUDE (reviewText, overall, summary);
CREATE NONCLUSTERED INDEX IX_TablaResenas_ASIN ON Tabla_Resenas (asin);

-- 3. Reducción de Volumetría (A 5 Millones exactos) y liberación de memoria
WHILE (SELECT COUNT(*) FROM Tabla_Resenas) > 5000000
BEGIN
    DELETE TOP (500000) FROM Tabla_Resenas;
END
GO
DBCC SHRINKDATABASE (AmazonVolumetriaDB);

⚙️ Fase 4: Backend y Arquitectura de 3 Capas (.NET Core)
Se desarrolló una Web API en C# .NET 8 Core estructurada en 3 capas (Models, Controllers, Data) utilizando Entity Framework Core.

Controllers: Se definieron endpoints específicos para Metadata y Reseñas, implementando validaciones de seguridad y paginación para evitar desbordamientos de memoria (Out of Memory).

Configuración (Program.cs): Implementación de CORS para habilitar peticiones HTTP/HTTPS de forma segura.

🐳 Fase 5: Dockerización y Balanceo de Carga (HA)
Para simular un entorno productivo real, se utilizó WSL2 y se contenerizó la solución:

Nginx (nginx.conf): Configurado como Reverse Proxy utilizando el algoritmo Round Robin para distribuir equitativamente el tráfico.

Clúster de APIs: Se levantaron 3 Nodos (contenedores) idénticos de la API para garantizar Alta Disponibilidad.

Infraestructura as Code: Definición de la topología en Dockerfile y docker-compose.yaml.

📈 Fase 6: Pruebas de Estrés (JMeter) y Monitoreo (Grafana)
Se configuró Prometheus para extraer métricas de los contenedores y Grafana para su visualización. Utilizando Apache JMeter, se simularon cargas escalonadas (100, 300, hasta 2000 usuarios concurrentes).

Resultados del Test:

El balanceador (Nginx) distribuyó las peticiones perfectamente entre los 3 nodos.

Al inyectar 2000 usuarios, el clúster toleró la carga inicial, pero el sistema local colapsó (CPU al 100% y saturación de RAM), demostrando el límite físico de la infraestructura local frente a consultas pesadas a una base de datos de 5M de registros.

Test de Resiliencia: Se apagaron nodos aleatoriamente durante la prueba, comprobando que Nginx redirigía el tráfico exitosamente a los nodos vivos.

🚀 Cómo ejecutar este proyecto localmente
Prerrequisitos
Docker Desktop (con WSL2 habilitado).

.NET 8 SDK.

SQL Server Management Studio.

Pasos de Ejecución
Clonar el repositorio:

Bash
git clone https://github.com/tu-usuario/tu-repo.git
cd tu-repo
Restaurar la base de datos: Ejecutar los scripts SQL en tu instancia local de SQL Server.

Levantar la infraestructura (API, Nginx, Monitoreo):

Bash
docker compose up -d --build
Verificar servicios:

API Balanceada: http://localhost:8080/swagger

Grafana (Dashboard): http://localhost:3000

Prometheus: http://localhost:9090

Ejecutar Pruebas de Carga: Abrir el archivo .jmx con Apache JMeter y ejecutar el Thread Group.

📝 Resumen del Pipeline:
Extract & Clean (Jupyter + Python): Limpieza de archivos de texto sucio, nulos y comillas en un entorno virtual aislado.

Transform & Load (SSIS + SSMS): Inyección masiva a base de datos relacional.

Optimize (SQL): Reducción de volumetría, liberación de espacio (SHRINK) e indexación.

Serve (.NET API): Creación de endpoints paginados.

Distribute & Balance (Docker + Nginx): Contenerización de la API en 3 nodos con balanceo Round Robin.

Monitor & Break (Grafana + JMeter): Monitoreo en tiempo re
