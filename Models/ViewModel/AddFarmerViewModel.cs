namespace AgriEnergyP2.Models.ViewModel
{
    public class AddFarmerViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; } // Optional: if you're creating users here

        public string FarmName { get; set; }
        public string Location { get; set; }
    }
}
