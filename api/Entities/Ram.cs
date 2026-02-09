
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Identity.Client;
using Microsoft.Net.Http.Headers;

namespace api.Entities
{
    public class Ram : Base
    {
        public required string Model{get; set;}
        public int Gigabytes{get; set;}
        public int Speed{get; set;}

    }
}