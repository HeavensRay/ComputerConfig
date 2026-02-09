using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.Pcu; //EXPLICITLY STATE .component
using api.Entities;

namespace api.Mappers
{
    public static class  PcuMapper
    {
        public static GetPcuDto ToGetDto(this Pcu Entity)
        /// <summary>
        /// Takes in a model and returns it as dto(only the fields needed)
        /// </summary>
        {
            return new GetPcuDto
            {
                Id = Entity.Id,
                Model = Entity.Model,
                Volts = Entity.Volts,
                Price = Entity.Price,
                Photo = Entity.Photo
                
            };
        }

        public static Pcu ToCreateEntity(this PostPcuDto Dto)
        { //from dto to model that can be stored in db
            return new Pcu
            {
                Model = Dto.Model,
                Volts = Dto.Volts,
                Rating = Dto.Rating,
                Price = Dto.Price,
                Photo = Dto.Photo

            };
        }

    }
}