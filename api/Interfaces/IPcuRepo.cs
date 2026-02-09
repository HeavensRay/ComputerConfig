using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Entities;
using Microsoft.Extensions.Caching.Memory;

public interface IPcuRepo : IBase
    {
        Task<List<Pcu>> GetAllAsync();
        Task<Pcu?> GetByIdAsync(int id);
        Task<Pcu> CreateAsync(Pcu EntitiesSD);
        // for all components all that can be updated is price, and photo
        Task<Pcu?> UpdateAsync(int id, BaseDtoCreate updateDto);

        Task<Pcu?> DeleteAsync(int id);
        Task<Pcu?> FindBestForPrice(int volts, string rating);

    }