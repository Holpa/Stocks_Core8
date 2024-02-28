using api.Data;
using api.DTOs.Stock;
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
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.id == id);
            if (stockModel == null)
            {
                return null;
            }
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.
            Include(c => c.Comments).ToListAsync();
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