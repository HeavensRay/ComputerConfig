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
    public class CPURepo : ICPURepo
    {
        private readonly AppDbContext _context;
        public CPURepo(AppDbContext context)
        {
            _context = context;
            
        }

        public async Task<CPU> CreateAsync(CPU EntitieCPU)
        {
            // adds a Entity to db
            await _context.CPUs.AddAsync(EntitieCPU);
            await _context.SaveChangesAsync();
            return EntitieCPU;
        }

        public async Task<CPU?> DeleteAsync(int id)
        {
            var CPUEntity = await _context.CPUs.FindAsync(id);
            
            if(CPUEntity == null)
            {
                return null;
            }

            _context.CPUs.Remove(CPUEntity);
            await _context.SaveChangesAsync();
            return CPUEntity;
        }

        public async Task<List<CPU>> GetAllAsync()
        {
            return await _context.CPUs.ToListAsync();
        }

        public async Task<CPU?> GetByIdAsync(int id)
        {
            return await _context.CPUs.FindAsync(id);
        }

        public Task<bool> Exists(int id)
        {
            // check if exists if it does return true
            return _context.CPUs.AnyAsync(s => s.Id == id);
        }

        public async Task<CPU?> UpdateAsync(int id, BaseDtoCreate updateDto)
        {
            var exists  = await _context.CPUs.FindAsync(id);
            if (exists == null)
            {
                return null;
            }

            exists.Price = updateDto.Price;
            exists.Photo = updateDto.Photo;

            await _context.SaveChangesAsync();

            return exists;
        }
        public async Task<CPU?> FindBestForPrice(decimal budget, int power)
        {
           var found =  await _context.CPUs
                .Where(p => p.Price < budget && p.Power <= power)       // filter "less expensive than budget"
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