using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Identity.Client;
using Microsoft.Net.Http.Headers;

namespace api.Entities
{
    public class Pcu : Base
    {
        public required string Model{get; set;}
        public int Volts{get; set;}
        public required string  Rating{get; set;}

    }
}