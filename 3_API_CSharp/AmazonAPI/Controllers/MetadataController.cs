using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmazonAPI.Data;
using AmazonAPI.Models;

namespace AmazonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetadataController : ControllerBase
    {
        private readonly AmazonDbContext _context;

        public MetadataController(AmazonDbContext context)
        {
            _context = context;
        }

        // 1. ENDPOINT DE PAGINACIÓN GENERAL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Metadata>>> GetMetadata(int pagina = 1, int tamano = 50)
        {
            if (tamano > 500) tamano = 500;

            var metadatas = await _context.Metadatas
                .Skip((pagina - 1) * tamano)
                .Take(tamano)
                .ToListAsync();

            return Ok(metadatas);
        }

        // 2. ENDPOINT DE BÚSQUEDA RÁPIDA POR ASIN
        [HttpGet("{asin}")]
        public async Task<ActionResult<Metadata>> GetMetadataByAsin(string asin)
        {
            // FindAsync va directo a la Llave Primaria, es el método más rápido de EF Core
            var metadata = await _context.Metadatas.FindAsync(asin);

            if (metadata == null)
            {
                return NotFound(new { mensaje = $"No se encontró el producto con ASIN: {asin}" });
            }

            return Ok(metadata);
        }
    }
}