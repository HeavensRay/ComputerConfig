using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Entities;
using Microsoft.Extensions.Caching.Memory;

public interface IBase
{
    // so i dont forget to add an exists method
    Task<bool> Exists(int id);
}