using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Identity.Client;
using Microsoft.Net.Http.Headers;

namespace api.Entities
{
    public class Comment
    {
        [Key]
        public int Id{get; set;}
        public int BaseId { get; set; }     // <-- only one FK

        // so if user deleted it cascades
        public required string Username{get; set;}
        public required string Writing{get;set;}
        public DateTime CreatedOn { get; set; } = DateTime.Now; // will put in the actual time now

        [ForeignKey("Username")]
        public User? User {get; set;}

        [ForeignKey("BaseId")]
        public Base? Base { get; set; }
    }
}