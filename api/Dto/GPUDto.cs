using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Dto.GPU
{
    public class GetGPUDto : BaseDto // w id
    {
        public required string Brand {get; set;}
        public required string Model {get; set;}
        public int Volts {get; set;}

        
    }
    public class PostGPUDto : BaseDtoCreate // no Id
    {
        public required string Brand {get; set;}
        public required string Model {get; set;}
        public int Power {get; set;}
        public int Volts {get; set;}

        
    }
}