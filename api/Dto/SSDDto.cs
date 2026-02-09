using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Dto.SSD
{
    public class GetSSDDto : BaseDto // w id
    {
        public required string Brand {get; set;}
        public required string Model {get; set;}
        public int SPD {get; set;}
        public int Capacity {get; set;}

        
    }
    public class PostSSDDto : BaseDtoCreate // no Id
    {
        public required string Brand {get; set;}
        public required string Model {get; set;}
        public int SPD {get; set;}
        public int Capacity {get; set;}

        
    }
}