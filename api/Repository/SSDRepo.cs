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
    public class SSDRepo : ISSDRepo
    {
        private readonly AppDbContext _context;
        public SSDRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SSD> CreateAsync(SSD EntitiesSD)
        {
            // adds a Entity to db
            await _context.SSDs.AddAsync(EntitiesSD);
            await _context.SaveChangesAsync();
            return EntitiesSD;
        }

        public async Task<SSD?> DeleteAsync(int id)
        {
            var ssdEntity = await _context.SSDs.FindAsync(id);
            
            if(ssdEntity == null)
            {
                return null;
            }

            _context.SSDs.Remove(ssdEntity);
            await _context.SaveChangesAsync();
            return ssdEntity;
        }

        public async Task<List<SSD>> GetAllAsync()
        {
            return await _context.SSDs.ToListAsync();
        }

        public async Task<SSD?> GetByIdAsync(int id)
        {
            return await _context.SSDs.FindAsync(id);
        }

        public Task<bool> Exists(int id)
        {
            // check if exists if it does return true
            return _context.SSDs.AnyAsync(s => s.Id == id);
        }

        public async Task<SSD?> UpdateAsync(int id, BaseDtoCreate updateDto)
        {
            var exists  = await _context.SSDs.FindAsync(id);
            if (exists == null)
            {
                return null;
            }
            exists.Price = updateDto.Price;
            exists.Photo = updateDto.Photo;

            await _context.SaveChangesAsync();

            return exists;
        }

        public async Task<SSD?> FindBestForPrice(decimal budget, int capacity)
        {
            var found =  await _context.SSDs
                .Where(p => p.Price < budget && p.Capacity >= capacity) 
                .OrderBy(p => p.SPD)
                .FirstOrDefaultAsync(); 
            if (found == null)
            {
                return null;
            }

            return found;
        }
    }
}