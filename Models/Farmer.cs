using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergyP2.Models
{
    public class Farmer
    {
        [Key]
        public int FarmerId { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        // Farmer-specific properties
        public string FarmName { get; set; }
        public string Location { get; set; }
        public DateTime DateJoined { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
