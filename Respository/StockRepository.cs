using api.Interfaces;
using api.Models;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        public Task<List<Stock>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}