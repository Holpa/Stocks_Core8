using api.Models;
using api.DTOs.Stock;

namespace api.Mappers
{
    //extention methods
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                id = stockModel.id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
            };

        }

        /// <summary>
        /// hast to be in a form of Stock to be able to SAVE from a post request
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static Stock ToStockFromCreateDTO(this CreateStockRequestDto stockModel)
        {

            return new Stock
            {
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap
            };


        }

    }


}