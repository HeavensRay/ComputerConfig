using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Config;
using api.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace api.Interfaces
{
    public interface IComment
    {
        Task<List<Comment>> GetAllAsync(int baseid);
        Task<Comment> CreateAsync(Comment comment);
        Task<Comment?> Delete(int id);
    }
}