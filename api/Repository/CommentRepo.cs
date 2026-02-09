using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepo : IComment
    {
        private readonly AppDbContext _context;
        public CommentRepo (AppDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> Delete(int id)
        {
            var commEntity = await _context.Comments.FindAsync(id);
            
            if(commEntity == null)
            {
                return null;
            }

            _context.Comments.Remove(commEntity);
            await _context.SaveChangesAsync();
            return commEntity;
        }

        public async Task<List<Comment>> GetAllAsync(int baseid)
        {
            return await _context.Comments.Where(c => c.BaseId == baseid).ToListAsync();
        }
    }
}