using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergyP2.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; } // e.g., Grain, Vegetable
        [Required]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        public int FarmerId { get; set; }

        [ForeignKey("FarmerId")]
        public Farmer Farmer { get; set; }
    }
}
