using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.Mobo; //EXPLICITLY STATE .component
using api.Entities;

namespace api.Mappers
{
    public static class  MoboMapper
    {
        public static GetMoboDto ToGetDto(this Mobo Entity)
        /// <summary>
        /// Takes in a model and returns it as dto(only the fields needed)
        /// </summary>
        {
            return new GetMoboDto
            {
                Id = Entity.Id,
                Model = Entity.Model,
                Chip = Entity.Chip,
                Rating = Entity.Rating,
                Price = Entity.Price,
                Photo = Entity.Photo
                
            };
        }

        public static Mobo ToCreateEntity(this PostMoboDto Dto)
        { //from dto to model that can be stored in db
            return new Mobo
            {
                Model = Dto.Model,
                Chip = Dto.Chip,
                Rating = Dto.Rating,
                Price = Dto.Price,
                Photo = Dto.Photo

            };
        }

    }
}