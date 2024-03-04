using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        [MinLength(1, ErrorMessage = "Symbol has to have 3 or more characters")]
        [MaxLength(10, ErrorMessage = "Title can't be over 10 characters")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MinLength(1, ErrorMessage = "Company Name has to have 1 or more characters")]
        [MaxLength(60, ErrorMessage = "Company Name can't be over 60 characters")]
        public string CompanyName { get; set; } = string.Empty;
        [DefaultValue(0)]
        public decimal Purchase { get; set; }

        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }

    }

}