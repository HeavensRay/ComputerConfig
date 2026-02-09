using System;
using api.Data;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using api.Dto;
using Microsoft.EntityFrameworkCore;
using api.Entities;
using api.Repository;
using api.Interfaces;
using System.Runtime.InteropServices;
using api.Dto.CPU;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using api.Dto.Comment;
using api.Services;

namespace api.Controllers
{
    [Route("api/Comments")]
    [ApiController]
    public class CommController : ControllerBase
    {
        private readonly IComment _commRepo;
        public CommController(IComment commRepo)
        {
            _commRepo = commRepo;
        }

        [Authorize]
        [HttpGet("{baseId}")]
        public async Task<IActionResult> GetAll( int baseId)
        {
            var comments = await _commRepo.GetAllAsync(baseId); //coming from db

            var dbDto = comments.Select(static s => s.ToGetDto()); //mapper turns to dto

            return Ok(dbDto);
        }

        [Authorize]
        [HttpPost("{baseId}")]
        public async Task<IActionResult> Create([FromRoute] int baseId, string writing)
        {
            var username = User.GetUserName();
            var commEntity = Mappers.CommentMapper.ToCreateEntity(baseId,username,writing);
            await _commRepo.CreateAsync(commEntity);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //find
            var commEntity = await _commRepo.Delete(id);
            if (commEntity == null)
            {
                return NotFound();
            }

            return NoContent(); // 200 for delete
        }
    }
}