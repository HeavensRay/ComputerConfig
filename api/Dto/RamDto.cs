using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Dto.Ram
{
    public class GetRamDto : BaseDto // w id
    {

        public required string Model{get; set;}
        public int Gigabytes{get; set;}
        public int Speed{get; set;}

        
    }
    public class PostRamDto : BaseDtoCreate // no Id
    {
        public required string Model{get; set;}
        public int Gigabytes{get; set;}
        public int Speed{get; set;}
        
    }
}