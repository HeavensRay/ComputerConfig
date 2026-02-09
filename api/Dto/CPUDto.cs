using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Dto.CPU  // change here 2
{
    public class GetCPUDto : BaseDto // w id
    {
        public required string Model {get; set;}
        // Hidden from user
        //public int Power {get; set;}
        public int Cores {get; set;}

        
    }
    public class PostCPUDto : BaseDtoCreate // no Id
    {

        public required string Model {get; set;}
        public int Power {get; set;}
        public int Cores {get; set;}

        
    }
}