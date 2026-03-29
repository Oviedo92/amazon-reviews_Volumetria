using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmazonAPI.Data;
using AmazonAPI.Models;

namespace AmazonAPI.Controllers
{
    // Esta es la ruta base: http://localhost:puerto/api/Resenas
    [Route("api/[controller]")]
    [ApiController]
    public class ResenasController : ControllerBase
    {
        private readonly AmazonDbContext _context;

        // INYECCIÓN DE DEPENDENCIAS: Le pasamos el puente de la BD al mesero
        public ResenasController(AmazonDbContext context)
        {
            _context = context;
        }

        // 1. ENDPOINT PARA PAGINACIÓN (Para no explotar la memoria)
        // URL de ejemplo: GET api/Resenas?pagina=1&tamano=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resena>>> GetResenas(int pagina = 1, int tamano = 50)
        {
            // Validamos que no pidan locuras que tumben el servidor
            if (tamano > 500) tamano = 500;

            var resenas = await _context.Resenas
                .Skip((pagina - 1) * tamano) // Aquí usa la Llave Primaria que creamos
                .Take(tamano)
                .ToListAsync();

            return Ok(resenas);
        }

        // 2. ENDPOINT DE ALTO RENDIMIENTO (Usando el Índice de SQL Server)
        // URL de ejemplo: GET api/Resenas/asin/B00005N7P0
        [HttpGet("asin/{asin}")]
        public async Task<ActionResult<IEnumerable<Resena>>> GetResenasByAsin(string asin)
        {
            // Gracias al NONCLUSTERED INDEX que creamos en SQL, esta consulta volará ⚡
            var resenas = await _context.Resenas
                .Where(r => r.asin == asin)
                .ToListAsync();

            if (!resenas.Any())
            {
                return NotFound(new { mensaje = $"No se encontraron reseñas para el ASIN: {asin}" });
            }

            return Ok(resenas);
        }
    }
}