using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Patterns;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // cus of column decimal...

namespace api.Entities
{
    public class EntityConfig
    {
        // thats the FK for one to many relation\
        [Key]
        [MaxLength(50)]
        public required string ConfigName{get; set;} //PK key
        [Key]
        public required string Username { get; set; } //PK key+foreign

        public required string Purpose{get; set;} //1080p... work

        public int? SsdId {get; set; } 
        public int? CpuId {get; set; }
        public int? GpuId {get; set; } 
        public int? MoboId {get; set; } 
        public int? PcuId {get; set; } 
        public int? RamId {get; set; } 

        // nav property back to parent/part
        public User? User { get; set; }
        public SSD? Ssd{get; set;}
        public GPU? Gpu{get; set;}
        public CPU? Cpu{get; set;}
        public Mobo? Mobo{get; set;}
        public Pcu? Pcu{get; set;}
        public Ram? Ram{get; set;}

        [Column(TypeName = "decimal(18,2)")] 
        public decimal Price {get; set;}

    }
}
