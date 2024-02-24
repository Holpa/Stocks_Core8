using api.Models;


///purpose is to move away our code away from logic
namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();
    }
}