using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto.CPU; //EXPLICITLY STATE .component
using api.Entities;

namespace api.Mappers
{
    public static class  CPUMapper
    {
        public static GetCPUDto ToGetDto(this CPU Entity)
        /// <summary>
        /// Takes in a model and returns it as dto(only the fields needed)
        /// </summary>
        {
            return new GetCPUDto
            {
                Id = Entity.Id,
                Model = Entity.Model,
                //Power = Entity.Power,
                Cores = Entity.Cores,
                Price = Entity.Price,
                Photo = Entity.Photo
                
                
            };
        }

        public static CPU ToCreateEntity(this PostCPUDto Dto)
        { //from dto to model that can be stored in db
            return new CPU
            {
                Model = Dto.Model,
                Power = Dto.Power,
                Cores = Dto.Cores,
                Price = Dto.Price,
                Photo = Dto.Photo

            };
        }

    }
}