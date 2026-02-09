using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Dto.Mobo
{
    public class GetMoboDto : BaseDto // w id
    {

        public required string Model {get; set;}
        public required string Chip {get; set;}

        public string Rating{get; set;}

        
    }
    public class PostMoboDto : BaseDtoCreate // no Id
    {
        public required string Model {get; set;}
        public required string Chip {get; set;}
        public string Rating{get; set;}
        
    }
}