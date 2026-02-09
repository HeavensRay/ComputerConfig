using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Entities;
using Microsoft.Extensions.Caching.Memory;

public interface IMoboRepo : IBase
    {
        Task<List<Mobo>> GetAllAsync();
        Task<Mobo?> GetByIdAsync(int id);
        Task<Mobo> CreateAsync(Mobo EntitiesSD);
        // for all components all that can be updated is price, and photo
        Task<Mobo?> UpdateAsync(int id, BaseDtoCreate updateDto);

        Task<Mobo?> DeleteAsync(int id);

        Task<Mobo?> FindBestForPrice(decimal budget, string rating);
    }