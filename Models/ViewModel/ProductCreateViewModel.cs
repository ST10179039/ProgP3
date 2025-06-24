using System.ComponentModel.DataAnnotations;


namespace AgriEnergyP2.Models.ViewModel
{
    public class ProductCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }
    }
}
