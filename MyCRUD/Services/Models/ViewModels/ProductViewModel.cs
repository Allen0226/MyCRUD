namespace Services.Models.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public short? Stock { get; set; }
        public string LaunchDate { get; set; }
    }
}
