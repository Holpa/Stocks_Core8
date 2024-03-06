using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {

            // First, find and delete any comments associated with the stock
            var comments = await _context.Comments
                                         .Where(c => c.StockId == id)
                                         .ToListAsync();

            if (comments.Any())
            {
                _context.Comments.RemoveRange(comments);
            }

            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.id == id);
            if (stockModel == null)
            {
                return null;
            }
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject queryObject)
        {
            var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(queryObject.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(queryObject.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(queryObject.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
            {
                if (queryObject.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = queryObject.IsDecsending ?
                     stocks.OrderByDescending(s => s.Symbol) :
                      stocks.OrderBy(s => s.Symbol);
                }
            }
            // pagination
            /*
            using Skip and take makes magic where you can slice the enteries into portions then smaller portions 

            .Skip(2) and .Take(2)

            will do the following 

            1 ,2 3, 4, 5, 6, 7, 8

            will take (1,2) and skip the rest

            but next query is going to consider the sub set  (3,4,5,6,7,8)

            then take will do (3,4) this time ...etc
            */
            var _skipNumer = (queryObject.PageNumber - 1) * queryObject.PageSize;

            return await stocks.Skip(_skipNumer).Take(queryObject.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.
            Include(c => c.Comments).FirstOrDefaultAsync(i => i.id == id);
        }

        public async Task<Stock?> GetBySymbol(string symbol)
        {
            return await _context.Stocks.
            Include(s => s.Comments).
            FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var _existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.id == id);
            if (_existingStock == null)
            {
                return null;
            }

            _existingStock.Symbol = stockDto.Symbol;
            _existingStock.CompanyName = stockDto.CompanyName;
            _existingStock.LastDiv = stockDto.LastDiv;
            _existingStock.Industry = stockDto.Industry;
            _existingStock.MarketCap = stockDto.MarketCap;

            await _context.SaveChangesAsync();
            return _existingStock;
        }
    }
}