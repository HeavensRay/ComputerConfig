using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Identity.Client;
using Microsoft.Net.Http.Headers;

namespace api.Entities
{
    public class SSD : Base
    {
        public required string Brand {get; set;}
        public required string Model {get; set;}
        public int SPD {get; set;}
        public int Capacity {get; set;}


    }
}