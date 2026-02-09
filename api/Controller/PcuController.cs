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
using api.Dto.Pcu;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/Pcu")]
    [ApiController]
    public class PcuController : ControllerBase
    {
        private readonly IPcuRepo _PcuRepo;
        public PcuController(IPcuRepo PcuRepo)
        {
            _PcuRepo = PcuRepo;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dbPcus = await _PcuRepo.GetAllAsync(); //coming from db

            var dbDto = dbPcus.Select(static s => s.ToGetDto()); //mapper turns to dto

            return Ok(dbDto);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var dbPcu = await _PcuRepo.GetByIdAsync(id);
            
            if(dbPcu == null)
            {
                return NotFound();
            }

            return Ok(dbPcu.ToGetDto());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(PostPcuDto PcuDto)
        {
            var PcuEntity = PcuDto.ToCreateEntity();
            await _PcuRepo.CreateAsync(PcuEntity);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BaseDtoCreate updateDto)
        {
            var PcuEntity = await _PcuRepo.UpdateAsync(id, updateDto);
            if(PcuEntity == null)
            {
                return NotFound();
            }

            return Ok(PcuEntity.ToGetDto());
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //find
            var PcuEntity = await _PcuRepo.DeleteAsync(id);
            if (PcuEntity == null)
            {
                return NotFound();
            }

            return NoContent(); // 200 for delete
        }
    }
}