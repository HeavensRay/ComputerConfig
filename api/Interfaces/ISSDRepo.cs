using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Entities;
using Microsoft.Extensions.Caching.Memory;

public interface ISSDRepo : IBase
    {
        Task<List<SSD>> GetAllAsync();
        Task<SSD?> GetByIdAsync(int id);
        Task<SSD> CreateAsync(SSD EntitiesSD);
        Task<SSD?> UpdateAsync(int id, BaseDtoCreate updateDto);

        Task<SSD?> DeleteAsync(int id);

        Task<SSD?> FindBestForPrice(decimal budget, int capacity);
    }