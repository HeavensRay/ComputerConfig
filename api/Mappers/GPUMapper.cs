using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.GPU; //EXPLICITLY STATE .component
using api.Entities;

namespace api.Mappers
{
    public static class  GPUMapper
    {
        public static GetGPUDto ToGetDto(this GPU Entity)
        /// <summary>
        /// Takes in a model and returns it as dto(only the fields needed)
        /// </summary>
        {
            return new GetGPUDto
            {
                Id = Entity.Id,
                Brand = Entity.Brand,
                Model = Entity.Model,
                //Power = Entity.Power,
                Volts = Entity.Volts,
                Price = Entity.Price,
                Photo = Entity.Photo
                
            };
        }

        public static GPU ToCreateEntity(this PostGPUDto Dto)
        { //from dto to model that can be stored in db
            return new GPU
            {
                Brand = Dto.Brand,
                Model = Dto.Model,
                Power = Dto.Power,
                Volts = Dto.Volts,
                Price = Dto.Price,
                Photo = Dto.Photo

            };
        }

    }
}