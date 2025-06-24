namespace AgriEnergyP2.Models.ViewModel
{
    public class ProductFilterViewModel
    {
        public string ProductType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public List<Product> FilteredProducts { get; set; }
    }
}
