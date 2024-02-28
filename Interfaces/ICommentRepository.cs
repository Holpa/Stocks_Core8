using api.DTOs;
using api.Models;


///purpose is to move away our code away from logic
namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();

        Task<String> UpdateComment(Comment comment);

        Task<Comment?> GetByIdAsync(int id);
    }
}