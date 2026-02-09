using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.SSD;
using api.Entities;

namespace api.Mappers
{
    public static class  SSDMapper
    {
        public static GetSSDDto ToGetDto(this SSD EntitiesSD)
        /// <summary>
        /// Takes in a model and returns it as dto(only the fields needed)
        /// </summary>
        {
            return new GetSSDDto
            {
                Id = EntitiesSD.Id,
                Brand = EntitiesSD.Brand,
                Model = EntitiesSD.Model,
                SPD = EntitiesSD.SPD,
                Capacity = EntitiesSD.Capacity,
                Price = EntitiesSD.Price,
                Photo = EntitiesSD.Photo
                
            };
        }

        public static SSD ToCreateEntity(this PostSSDDto ssdDto)
        { //from dto to model that can be stored in db
            return new SSD
            {
                Brand = ssdDto.Brand,
                Model = ssdDto.Model,
                SPD = ssdDto.SPD,
                Capacity = ssdDto.Capacity,
                Price = ssdDto.Price,
                Photo = ssdDto.Photo

            };
        }

    }
}