using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Dto.Config;
using api.Entities;
using Microsoft.Extensions.Caching.Memory;

public interface IGenConfig
{
    Task<GennedConfigDto?> CreateFromBudget(decimal budget, PostConfigDto criteria, int ssdSize);
}