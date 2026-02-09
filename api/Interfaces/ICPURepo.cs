using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Entities;
using Microsoft.Extensions.Caching.Memory;

public interface ICPURepo : IBase
    {
        Task<List<CPU>> GetAllAsync();
        Task<CPU?> GetByIdAsync(int id);
        Task<CPU> CreateAsync(CPU EntitiesSD);
        // for all components all that can be updated is price, and photo
        Task<CPU?> UpdateAsync(int id, BaseDtoCreate updateDto);

        Task<CPU?> DeleteAsync(int id);

        Task<CPU?> FindBestForPrice(decimal budget, int power);

    }