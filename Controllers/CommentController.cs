using System.Drawing;
using api.Data;
using api.DTOs;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commnetContext, IStockRepository stockContext)
        {
            _commentRepo = commnetContext;
            _stockRepo = stockContext;
        }


        [HttpGet]
        public async Task<IActionResult> getAllComments()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comments = await _commentRepo.GetAllAsync();
            // add mapper between comment and commentDTO
            var commentDTO = comments.Select(s => s.ToCommentDto());
            return Ok(commentDTO);
        }

        [HttpPost("{symbol:alpha:minlength(1):maxlength(10)}")]
        public async Task<IActionResult> InsertComment(string symbol,
         [FromBody] CreateCommentDTO createCommentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Ensure symbol and comment exist before proceeding
            if (string.IsNullOrWhiteSpace(symbol) || string.IsNullOrWhiteSpace(createCommentDTO.Content) ||
             string.IsNullOrWhiteSpace(createCommentDTO.Title))
            {
                return BadRequest("Symbol and content are required.");
            }

            // Sanitize the input
            var cleanSymbol = symbol.Trim();
            // Find the stock by symbol

            var _stock = await _stockRepo.GetBySymbol(cleanSymbol);

            if (_stock == null)
            {
                return NotFound($"Stock with symbol {cleanSymbol} not found.");
            }
            //extra step because we do not have mapper from createCommentDTO to comment
            var comment = new CommentDTO
            {
                Content = createCommentDTO.Content.Trim(),
                Title = createCommentDTO.Title.Trim(),
                StockId = _stock.id
            };

            var _comm = comment.ToComment();
            return Ok(await _commentRepo.CreateComment(_comm));
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _comment = await _commentRepo.GetByIdAsync(id);
            if (_comment == null)
            {

                return NotFound();
            }

            return Ok(_comment.ToCommentDto());
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _result = await _commentRepo.DeleteByIdAsync(id);
            if (_result == null)
            {
                return NotFound("Comment was not found");
            }
            return Ok("Comment deleted");
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id,
         [FromBody] UpdateCommentDTO updateCommentDto)
        {
            var _comment = await _commentRepo.UpdateCommentAsync(id, updateCommentDto.UpdateToComment());

            if (_comment == null)
            {
                return NotFound("Comment not found");
            }
            return Ok(_comment);
        }

    }


}