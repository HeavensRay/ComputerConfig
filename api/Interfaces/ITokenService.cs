using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace api.Interfaces
{
    public interface ITokenService
    {
        public Task<string> CreateToken(User user);
    }
}