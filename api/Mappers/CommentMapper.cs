using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.Comment; //EXPLICITLY STATE .component
using api.Entities;

namespace api.Mappers
{
    public static class  CommentMapper
    {
        public static GetCommentDto ToGetDto(this Comment comment)
        {
            return new GetCommentDto
            {
                Id = comment.Id,
                Username = comment.Username,
                CreatedOn = comment.CreatedOn.ToString("dd-MM-yy"),
                Writing = comment.Writing
            };
        }
        public static Comment ToCreateEntity(int baseID,string username, string writing)
        {
            return new Comment
            {
                BaseId = baseID,
                Username = username,
                Writing = writing
            };
        }
    }
}
