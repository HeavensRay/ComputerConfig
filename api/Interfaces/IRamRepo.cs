using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Entities;
using Microsoft.Extensions.Caching.Memory;

public interface IRamRepo : IBase
    {
        Task<List<Ram>> GetAllAsync();
        Task<Ram?> GetByIdAsync(int id);
        Task<Ram> CreateAsync(Ram EntitiesSD);
        // for all components all that can be updated is price, and photo
        Task<Ram?> UpdateAsync(int id, BaseDtoCreate updateDto);

        Task<Ram?> DeleteAsync(int id);

        Task<Ram?> FindBestForPrice(decimal budget, int gigs);
    }