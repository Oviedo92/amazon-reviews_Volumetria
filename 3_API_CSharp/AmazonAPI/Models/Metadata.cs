using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonAPI.Models
{
    [Table("Tabla_Metadata")] // Pon el nombre exacto de tu tabla de SQL
    public class Metadata
    {
        [Key]
        public string asin { get; set; } = null!; // Llave primaria de Metadata

        public string? title { get; set; }
        public string? brand { get; set; }
        public string? category { get; set; }
        public double? price { get; set; }
    }
}