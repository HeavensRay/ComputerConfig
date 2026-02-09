using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.Config;
using api.Entities;

namespace api.Mappers
{
    public static class  ConfigMapper
    {
        public static GetConfigDto ToGetDto(this EntityConfig Entity)
        {
            // read 
            return new GetConfigDto
            {
                ConfigName = Entity.ConfigName,
                Username = Entity.Username,
                Purpose = Entity.Purpose,
                SsdId = Entity.SsdId,
                GpuId = Entity.GpuId,
                CpuId = Entity.CpuId,
                MoboId = Entity.MoboId,
                PcuId = Entity.PcuId,
                RamId = Entity.RamId,
                Price = Entity.Price
            };
        }
        public static EntityConfig ToCreateEntity(this PostConfigDto Dto, string username, int ssdId)
        {
            // user inputs these, incomplete model
            return new EntityConfig
            {
                ConfigName = Dto.ConfigName,
                Username = username,
                Purpose = Dto.Purpose,
                Price = Dto.Price

                


            };
        }
        public static EntityConfig ToEntityGen(this GennedConfigDto Dto, string username)
        {
            // algorithm fills these, complete model
            return new EntityConfig
            {
                ConfigName = Dto.ConfigName,
                Username = username,
                Purpose = Dto.Purpose,
                SsdId = Dto.SsdId,
                GpuId = Dto.GpuId,
                CpuId = Dto.CpuId,
                MoboId = Dto.MoboId,
                PcuId = Dto.PcuId,
                RamId = Dto.RamId,
                Price = Dto.Price
                


            };
        }
    }
}