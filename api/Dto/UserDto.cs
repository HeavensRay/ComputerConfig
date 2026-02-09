using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using api.Dto.Config;

namespace api.Dto.User
{
    public class UserLogged
    {
        public required string Username{get;set;}
        public string Token{get; set;}

        // parent-child configs - deprecated
        //public List<GetConfigDto>? Configs {get;set;}
    }
    public class RegisterDto
    {
        [Required]
        public string? Username{get;set;}
        [Required]
        public string? Password{get;set;}
    }
    public class UserTokenDto
    {
        public required string Username{get;set;}
        public string Token{get; set;}
    }
}