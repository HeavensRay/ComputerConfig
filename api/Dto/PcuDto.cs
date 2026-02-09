using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Dto.Pcu
{
    public class GetPcuDto : BaseDto // w id
    {

        public required string Model {get; set;}
        public int Volts {get; set;}
        // Hidden
        //public char Rating{get; set;}

        
    }
    public class PostPcuDto : BaseDtoCreate // no Id
    {
        public required string Model {get; set;}
        public int Volts {get; set;}
        public string Rating{get; set;}
        
    }
}