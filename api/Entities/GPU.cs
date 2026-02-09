using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Identity.Client;
using Microsoft.Net.Http.Headers;

namespace api.Entities
{
    public class GPU : Base
    {
        public required string Brand{get; set;}
        public required string Model{get; set;}
        public int  Power{get; set;}
        public int Volts{get; set;}


        


        

    }
}