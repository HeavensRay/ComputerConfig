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
using api.Dto.Ram;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/Ram")]
    [ApiController]
    public class RamController : ControllerBase
    {
        private readonly IRamRepo _RamRepo;
        public RamController(IRamRepo RamRepo)
        {
            _RamRepo = RamRepo;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dbRams = await _RamRepo.GetAllAsync(); //coming from db

            var dbDto = dbRams.Select(static s => s.ToGetDto()); //mapper turns to dto

            return Ok(dbDto);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var dbRam = await _RamRepo.GetByIdAsync(id);
            
            if(dbRam == null)
            {
                return NotFound();
            }
            
            return Ok(dbRam.ToGetDto());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(PostRamDto RamDto)
        {
            var RamEntity = RamDto.ToCreateEntity();
            await _RamRepo.CreateAsync(RamEntity);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BaseDtoCreate updateDto)
        {
            var RamEntity = await _RamRepo.UpdateAsync(id, updateDto);
            if(RamEntity == null)
            {
                return NotFound();
            }

            return Ok(RamEntity.ToGetDto());
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //find
            var RamEntity = await _RamRepo.DeleteAsync(id);
            if (RamEntity == null)
            {
                return NotFound();
            }

            return NoContent(); // 200 for delete
        }
    }
}