namespace api.DTOs.Stock
{
    /// <summary>
    /// Class mainly for creating stock requests, ID will not be included
    /// First you do save the DTO into the DB which will generate the ID
    /// then get the ID from the DB by querying the DB by ID
    /// then package all that and send it back to the user
    /// </summary>
    public class CreateStockRequestDto
    {

        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

        public decimal Purchase { get; set; }

        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }

    }

}