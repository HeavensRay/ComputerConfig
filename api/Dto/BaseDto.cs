using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto
{
    public class BaseDto
    {
        public int Id { get; set; }
        public decimal Price {get; set;}
        public string? Photo{get; set;}
    }

    public class BaseDtoCreate
    {
        public decimal Price {get; set;}
        public string? Photo{get; set;}
    }
}