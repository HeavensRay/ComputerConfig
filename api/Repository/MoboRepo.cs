using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using api.Dto;

namespace api.Repository
{
    public class MoboRepo : IMoboRepo
    {
        private readonly AppDbContext _context;
        public MoboRepo(AppDbContext context)
        {
            _context = context;
            
        }

        public async Task<Mobo> CreateAsync(Mobo EntitieMobo)
        {
            // adds a Entity to db
            await _context.Motherboards.AddAsync(EntitieMobo);
            await _context.SaveChangesAsync();
            return EntitieMobo;
        }

        public async Task<Mobo?> DeleteAsync(int id)
        {
            var MoboEntity = await _context.Motherboards.FindAsync(id);
            
            if(MoboEntity == null)
            {
                return null;
            }

            _context.Motherboards.Remove(MoboEntity);
            await _context.SaveChangesAsync();
            return MoboEntity;
        }

        public async Task<List<Mobo>> GetAllAsync()
        {
            return await _context.Motherboards.ToListAsync();
        }

        public async Task<Mobo?> GetByIdAsync(int id)
        {
            return await _context.Motherboards.FindAsync(id);
        }

        public Task<bool> Exists(int id)
        {
            // check if exists if it does return true
            return _context.Motherboards.AnyAsync(s => s.Id == id);
        }

        public async Task<Mobo?> UpdateAsync(int id, BaseDtoCreate updateDto)
        {
            var exists  = await _context.Motherboards.FindAsync(id);
            if (exists == null)
            {
                return null;
            }

            exists.Price = updateDto.Price;
            exists.Photo = updateDto.Photo;

            await _context.SaveChangesAsync();

            return exists;
        }

        public async Task<Mobo?> FindBestForPrice(decimal budget, string rating)
        {
            var found =  await _context.Motherboards
                .Where(p => p.Price < budget && p.Rating == rating) 
                .OrderBy(p => p.Price)
                .FirstOrDefaultAsync(); 
            if (found == null)
            {
                return null;
            }

            return found;

        }
    }
}