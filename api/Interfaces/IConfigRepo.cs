using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Config;
using api.Entities;
namespace api.Interfaces
{
    public interface IConfigRepo
    {
        Task<List<EntityConfig>> GetAllAsync(string username);
        Task<EntityConfig?> GetByIdAsync(string username, string configName);
        Task<EntityConfig> CreateAsync(EntityConfig configModel);
        Task<EntityConfig?> Delete(string username, string configName);

    }
}