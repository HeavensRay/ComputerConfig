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
    public class PcuRepo : IPcuRepo
    {
        private readonly AppDbContext _context;
        public PcuRepo(AppDbContext context)
        {
            _context = context;
            
        }

        public async Task<Pcu> CreateAsync(Pcu EntitiePcu)
        {
            // adds a Entity to db
            await _context.Pcus.AddAsync(EntitiePcu);
            await _context.SaveChangesAsync();
            return EntitiePcu;
        }

        public async Task<Pcu?> DeleteAsync(int id)
        {
            var PcuEntity = await _context.Pcus.FindAsync(id);
            
            if(PcuEntity == null)
            {
                return null;
            }

            _context.Pcus.Remove(PcuEntity);
            await _context.SaveChangesAsync();
            return PcuEntity;
        }

        public async Task<List<Pcu>> GetAllAsync()
        {
            return await _context.Pcus.ToListAsync();
        }

        public async Task<Pcu?> GetByIdAsync(int id)
        {
            return await _context.Pcus.FindAsync(id);
        }

        public Task<bool> Exists(int id)
        {
            // check if exists if it does return true
            return _context.Pcus.AnyAsync(s => s.Id == id);
        }

        public async Task<Pcu?> UpdateAsync(int id, BaseDtoCreate updateDto)
        {
            var exists  = await _context.Pcus.FindAsync(id);
            if (exists == null)
            {
                return null;
            }

            exists.Price = updateDto.Price;
            exists.Photo = updateDto.Photo;

            await _context.SaveChangesAsync();

            return exists;
        }

        public async Task<Pcu?> FindBestForPrice(int volts, string rating)
        {
            var found =  await _context.Pcus
                .Where(p => p.Volts >= volts && p.Rating == rating)       
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