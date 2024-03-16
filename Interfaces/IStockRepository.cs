using api.DTOs.Stock;
using api.Helpers;
using api.Models;


///purpose is to move away our code away from logic
namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject queryObject);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task<Stock?> DeleteAsync(int id);
        Task<Stock?> GetBySymbol(string symbol);
        Task<Stock?> GetBySymbolAsync(string symbol);
    }
}