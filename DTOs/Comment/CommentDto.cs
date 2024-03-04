using System.ComponentModel.DataAnnotations;
using api.Models;
namespace api.DTOs

{
    public class CommentDTO
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Title has to have 5 or more characters")]
        [MaxLength(280, ErrorMessage = "Title can't be over 280 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Content has to have 5 or more characters")]
        [MaxLength(280, ErrorMessage = "Content can't be over 280 characters")]
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public int? StockId { get; set; }
    }


    public class CreateCommentDTO
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title has to have 5 or more characters")]
        [MaxLength(280, ErrorMessage = "Title can't be over 280 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Content has to have 5 or more characters")]
        [MaxLength(280, ErrorMessage = "Content can't be over 280 characters")]
        public string Content { get; set; } = string.Empty;
        public int? StockId { get; set; }
    }
}