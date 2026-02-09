using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Entities;
using Microsoft.Extensions.Caching.Memory;

public interface IGPURepo : IBase
    {
        Task<List<GPU>> GetAllAsync();
        Task<GPU?> GetByIdAsync(int id);
        Task<GPU> CreateAsync(GPU EntitiesSD);
        // for all components all that can be updated is price, and photo
        Task<GPU?> UpdateAsync(int id, BaseDtoCreate updateDto);

        Task<GPU?> DeleteAsync(int id);

        Task<GPU?> FindBestForPrice(decimal budget, int power);

    }