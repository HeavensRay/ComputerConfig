using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Patterns;
using System.ComponentModel.DataAnnotations.Schema; // cus of column...

namespace api.Entities
{
    public class Base
    {
        public int Id { get; set; }

    // like the gpt said: money + silent rounding = lawsuits 😬
        [Column(TypeName = "decimal(18,2)")] 
        public decimal Price {get; set;}
        
        public string? Photo{get; set;}
        
        //hmm
        public List<Comment> comments {get;set;} = new List<Comment>();
        
    }
}