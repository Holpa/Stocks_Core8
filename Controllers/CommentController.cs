using api.Data;
using api.DTOs;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public CommentController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost("{symbol}")]
        public IActionResult UpdateComment(string symbol, [FromBody] CommentDTO commentDTO)
        {
            // Ensure symbol and comment exist before proceeding
            if (string.IsNullOrWhiteSpace(symbol) || string.IsNullOrWhiteSpace(commentDTO.Content))
            {
                return BadRequest("Symbol and content are required.");
            }

            // Sanitize the input
            var cleanSymbol = symbol.Trim();
            var cleanContent = commentDTO.Content.Trim();

            // Find the stock by symbol
            var stock = _context.Stocks.Include(s => s.Comments).FirstOrDefault(s => s.Symbol == cleanSymbol);

            if (stock == null)
            {
                return NotFound($"Stock with symbol {cleanSymbol} not found.");
            }

            // Assuming we're adding a new comment to the stock
            var comment = new Comment
            {
                Title = commentDTO.Title,
                Content = cleanContent,
                StockId = stock.id
            };

            // Add new comment
            _context.Comments.Add(comment);

            // Save the changes to the database
            _context.SaveChanges();

            return Ok("Comment added successfully.");
        }

    }


}