using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Stock
{
    public class UpdateStockRequestDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Symbol Name has to have 1 or more characters")]
        [MaxLength(10, ErrorMessage = "Symbol Name can't be over 10 characters")]
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