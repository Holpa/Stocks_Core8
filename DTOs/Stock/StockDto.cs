namespace api.DTOs.Stock
{
    /// <summary>
    /// this class mainly for GET as it mimics the original STOCK object
    /// Comments will be moved on its own DTO class for now
    /// </summary>
    public class StockDto
    {
        public int id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

        public decimal Purchase { get; set; }

        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }
        //public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}