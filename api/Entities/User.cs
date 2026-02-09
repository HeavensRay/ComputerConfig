using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace api.Entities
{
    [Index(nameof(UserName), IsUnique = true)]
    public class User : IdentityUser
    {
        // parent-child configs
        public List<EntityConfig> Configs {get;set;} = new List<EntityConfig>(); // if null make it 
    }
}