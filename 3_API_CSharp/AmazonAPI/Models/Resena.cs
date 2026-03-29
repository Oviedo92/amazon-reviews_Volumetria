using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonAPI.Models
{
    [Table("Tabla_Resenas")] // Asegúrate de que sea el nombre exacto de tu tabla en SQL
    public class Resena
    {
        [Key]
        public int IdResena { get; set; }

        public string? asin { get; set; }
        public string? reviewerID { get; set; }
        public double? overall { get; set; } // SQL float -> C# double
        public bool? verified { get; set; }  // SQL bit -> C# bool
        public string? reviewText { get; set; }
        public string? summary { get; set; }
        public int? vote { get; set; }
        public DateTime? reviewDate { get; set; } // SQL date -> C# DateTime
    }
}