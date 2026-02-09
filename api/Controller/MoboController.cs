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
using api.Dto.Mobo;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/Mobo")]
    [ApiController]
    public class MoboController : ControllerBase
    {
        private readonly IMoboRepo _MoboRepo;
        public MoboController(IMoboRepo MoboRepo)
        {
            _MoboRepo = MoboRepo;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dbMobos = await _MoboRepo.GetAllAsync(); //coming from db

            var dbDto = dbMobos.Select(static s => s.ToGetDto()); //mapper turns to dto

            return Ok(dbDto);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var dbMobo = await _MoboRepo.GetByIdAsync(id);
            
            if(dbMobo == null)
            {
                return NotFound();
            }
            
            return Ok(dbMobo.ToGetDto());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostMoboDto MoboDto)
        {
            var MoboEntity = MoboDto.ToCreateEntity();
            await _MoboRepo.CreateAsync(MoboEntity);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BaseDtoCreate updateDto)
        {
            var MoboEntity = await _MoboRepo.UpdateAsync(id, updateDto);
            if(MoboEntity == null)
            {
                return NotFound();
            }

            return Ok(MoboEntity.ToGetDto());
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //find
            var MoboEntity = await _MoboRepo.DeleteAsync(id);
            if (MoboEntity == null)
            {
                return NotFound();
            }

            return NoContent(); // 200 for delete
        }
    }
}