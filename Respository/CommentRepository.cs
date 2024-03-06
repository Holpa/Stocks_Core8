using api.Data;
using api.DTOs;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<String> CreateComment(Comment comment)
        {
            // Add new comment
            await _context.Comments.AddAsync(comment);

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return "Update Success";
        }

        public async Task<Comment?> DeleteByIdAsync(int id)
        {
            var _comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (_comment == null)
            {
                return null;
            }
            _context.Comments.Remove(_comment);

            await _context.SaveChangesAsync();
            return _comment;
        }

        public async Task<Comment?> UpdateCommentAsync(int id, Comment comment)
        {
            var _comment = await _context.Comments.FindAsync(id);

            if (_comment == null)
            {
                return null;
            }

            _comment.Title = comment.Title;
            _comment.Content = comment.Content;

            await _context.SaveChangesAsync();
            return _comment;
        }
    }
}