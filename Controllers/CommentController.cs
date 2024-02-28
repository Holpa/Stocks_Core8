using System.Drawing;
using api.Data;
using api.DTOs;
using api.Interfaces;
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
        private readonly ICommentRepository _commentContext;
        private readonly IStockRepository _stockContext;

        public CommentController(ICommentRepository commnetContext, IStockRepository stockContext)
        {
            _commentContext = commnetContext;
            _stockContext = stockContext;
        }


        [HttpGet]
        public async Task<IActionResult> getAllComments()
        {
            var comments = await _commentContext.GetAllAsync();
            // add mapper between comment and commentDTO
            var commentDTO = comments.Select(s => s.ToCommentDto());
            return Ok(commentDTO);
        }

        [HttpPost("{symbol}")]
        public async Task<IActionResult> UpdateComment(string symbol,
         [FromBody] CommentDTO commentDTO)
        {
            // Ensure symbol and comment exist before proceeding
            if (string.IsNullOrWhiteSpace(symbol) || string.IsNullOrWhiteSpace(commentDTO.Content) ||
             string.IsNullOrWhiteSpace(commentDTO.Title))
            {
                return BadRequest("Symbol and content are required.");
            }

            // Sanitize the input
            var cleanSymbol = symbol.Trim();
            // Find the stock by symbol

            var _stock = await _stockContext.GetBySymbol(cleanSymbol);

            if (_stock == null)
            {
                return NotFound($"Stock with symbol {cleanSymbol} not found.");
            }
            commentDTO.Content = commentDTO.Content.Trim();
            commentDTO.Title = commentDTO.Title.Trim();
            commentDTO.StockId = _stock.id;

            var _comm = commentDTO.ToComment();
            return Ok(await _commentContext.UpdateComment(_comm));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var _comment = await _commentContext.GetByIdAsync(id);
            if (_comment == null)
            {

                return NotFound();
            }

            return Ok(_comment.ToCommentDto());
        }

    }


}