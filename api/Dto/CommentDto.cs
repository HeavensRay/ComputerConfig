using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Dto.Comment
{
    public class GetCommentDto  // w id
    {
        public int Id{get;set;}
        public required string Username{get; set;}
        public required string Writing{get;set;}
        public string CreatedOn { get; set; } // fixing cursed date
        
    }
    public class MakeCommentDto
    {

        public int BaseId{get;set;}
        public required string Username{get; set;}
        public required string Writing{get;set;}
    }
}