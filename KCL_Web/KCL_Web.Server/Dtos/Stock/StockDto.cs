namespace KCL_Web.Server.Dtos.Stock
{
    public class StockDto
    {
        public int Id { get; set; }

        public string Synbol { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public decimal Purchase { get; set; }

        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = null!;

        public string MarketCup { get; set; } = null!;

    }
}