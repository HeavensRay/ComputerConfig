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
using System.Text.RegularExpressions;

namespace api.Repository
{
    public class GPURepo : IGPURepo
    {
        private readonly AppDbContext _context;
        public GPURepo(AppDbContext context)
        {
            _context = context;
            
        }

        public async Task<GPU> CreateAsync(GPU EntitieGPU)
        {
            // adds a Entity to db
            await _context.GPUs.AddAsync(EntitieGPU);
            await _context.SaveChangesAsync();
            return EntitieGPU;
        }

        public async Task<GPU?> DeleteAsync(int id)
        {
            var GPUEntity = await _context.GPUs.FindAsync(id);
            
            if(GPUEntity == null)
            {
                return null;
            }

            _context.GPUs.Remove(GPUEntity);
            await _context.SaveChangesAsync();
            return GPUEntity;
        }

        public async Task<List<GPU>> GetAllAsync()
        {
            return await _context.GPUs.ToListAsync();
        }

        public async Task<GPU?> GetByIdAsync(int id)
        {
            return await _context.GPUs.FindAsync(id);
        }

        public Task<bool> Exists(int id)
        {
            // check if exists if it does return true
            return _context.GPUs.AnyAsync(s => s.Id == id);
        }

        public async Task<GPU?> UpdateAsync(int id, BaseDtoCreate updateDto)
        {
            var exists  = await _context.GPUs.FindAsync(id);
            if (exists == null)
            {
                return null;
            }

            exists.Price = updateDto.Price;
            exists.Photo = updateDto.Photo;

            await _context.SaveChangesAsync();

            return exists;
        }

        public async Task<GPU?> FindBestForPrice(decimal budget, int power)
        {
           var found =  await _context.GPUs
                .Where(p => p.Price < budget)       // filter "less expensive than budget"
                .OrderByDescending(p => p.Power)      // sort by Power descending
                .FirstOrDefaultAsync(); 

            if (found == null)
            {
                return null;
            }

            return found;

        }
    }
}