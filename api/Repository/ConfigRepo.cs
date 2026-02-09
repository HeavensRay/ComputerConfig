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
using api.Dto.Config;

namespace api.Repository
{
    public class ConfigRepo : IConfigRepo
    {
        private readonly AppDbContext _context;
        public ConfigRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<EntityConfig> CreateAsync(EntityConfig configModel)
        {
            await _context.Configurations.AddAsync(configModel);
            await _context.SaveChangesAsync();
            return configModel;
        }

        public async Task<EntityConfig?> Delete(string username, string configName)
        {
            var Exists = await _context.Configurations.FindAsync(configName, username);
            if(Exists == null)
            {
                return null;
            }

            _context.Configurations.Remove(Exists);
            await _context.SaveChangesAsync();
            return Exists;
        }

        public async Task<List<EntityConfig>> GetAllAsync(string username)
        {
            return await _context.Configurations.Where(c => c.Username == username).ToListAsync();
        }

        public async Task<EntityConfig?> GetByIdAsync(string username, string configName)
        {
            return await _context.Configurations.FindAsync(configName, username);
        }

    }
}