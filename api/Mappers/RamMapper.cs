using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.Ram; //EXPLICITLY STATE .component
using api.Entities;

namespace api.Mappers
{
    public static class  RamMapper
    {
        public static GetRamDto ToGetDto(this Ram Entity)
        /// <summary>
        /// Takes in a model and returns it as dto(only the fields needed)
        /// </summary>
        {
            return new GetRamDto
            {
                Id = Entity.Id,
                Model = Entity.Model,
                Gigabytes = Entity.Gigabytes,
                Price = Entity.Price,
                Photo = Entity.Photo
                
            };
        }

        public static Ram ToCreateEntity(this PostRamDto Dto)
        { //from dto to model that can be stored in db
            return new Ram
            {
                Model = Dto.Model,
                Gigabytes = Dto.Gigabytes,
                Price = Dto.Price,
                Photo = Dto.Photo,
                Speed = Dto.Speed

            };
        }

    }
}