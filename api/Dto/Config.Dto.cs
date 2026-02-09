using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Dto.Config
{
    public class GetConfigDto
    {
        public required string ConfigName{get; set;} //PK key
        // DELETE LATER
        public string? Username { get; set; } //PK key+foreign

        public required string Purpose{get; set;} //1080p... work

        public int? SsdId {get; set; } 
        public int? CpuId {get; set; }
        public int? GpuId {get; set; } 
        public int? MoboId {get; set;} 
        public int? PcuId {get; set; } 
        public int? RamId {get; set; } 

        public decimal Price{get; set;}

    }

        public class PostConfigDto
    {
        public required string ConfigName{get; set;} //PK key

        public required string Purpose{get; set;} //1080p... work
        public decimal Price{get; set;} // budget

        
    }
    public class GennedConfigDto
    {
        public required string ConfigName{get; set;} //PK key
        public string? Username { get; set; } //PK key+foreign
        public required string Purpose{get; set;} //1080p... work
        public int? SsdId {get; set; } 
        public int? CpuId {get; set; }
        public int? GpuId {get; set; } 
        public int? MoboId {get; set; } 
        public int? PcuId {get; set; } 
        public int? RamId {get; set; } 

        public decimal Price{get; set;}
    }
}