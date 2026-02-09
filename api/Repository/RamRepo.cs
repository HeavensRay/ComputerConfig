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
    public class RamRepo : IRamRepo
    {
        private readonly AppDbContext _context;
        public RamRepo(AppDbContext context)
        {
            _context = context;
            
        }

        public async Task<Ram> CreateAsync(Ram EntitieRam)
        {
            // adds a Entity to db
            await _context.Rams.AddAsync(EntitieRam);
            await _context.SaveChangesAsync();
            return EntitieRam;
        }

        public async Task<Ram?> DeleteAsync(int id)
        {
            var RamEntity = await _context.Rams.FindAsync(id);
            
            if(RamEntity == null)
            {
                return null;
            }

            _context.Rams.Remove(RamEntity);
            await _context.SaveChangesAsync();
            return RamEntity;
        }

        public async Task<List<Ram>> GetAllAsync()
        {
            return await _context.Rams.ToListAsync();
        }

        public async Task<Ram?> GetByIdAsync(int id)
        {
            return await _context.Rams.FindAsync(id);
        }

        public Task<bool> Exists(int id)
        {
            // check if exists if it does return true
            return _context.Rams.AnyAsync(s => s.Id == id);
        }

        public async Task<Ram?> UpdateAsync(int id, BaseDtoCreate updateDto)
        {
            var exists  = await _context.Rams.FindAsync(id);
            if (exists == null)
            {
                return null;
            }

            exists.Price = updateDto.Price;
            exists.Photo = updateDto.Photo;

            await _context.SaveChangesAsync();

            return exists;
        }

        public async Task<Ram?> FindBestForPrice(decimal budget, int gigs)
        {
            var found =  await _context.Rams
                .Where(p => p.Price <= budget && p.Gigabytes >= gigs)       
                .OrderBy(p => p.Speed)     
                .FirstOrDefaultAsync(); 
            
            if (found == null)
            {
                return null;
            }

            return found;
        }
    }
}