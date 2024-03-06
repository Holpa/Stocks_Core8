using api.DTOs;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static CommentDTO ToCommentDto(this Comment commentModel)
        {
            return new CommentDTO
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId
            };

        }

        public static Comment ToComment(this CommentDTO commentDTO)
        {
            return new Comment
            {
                Id = commentDTO.Id,
                Title = commentDTO.Title,
                Content = commentDTO.Content,
                CreatedOn = commentDTO.CreatedOn,
                StockId = commentDTO.StockId
            };
        }

        public static Comment UpdateToComment(this UpdateCommentDTO updateCommentDTO)
        {
            return new Comment
            {
                Title = updateCommentDTO.Title,
                Content = updateCommentDTO.Content,
            };
        }
    }
}