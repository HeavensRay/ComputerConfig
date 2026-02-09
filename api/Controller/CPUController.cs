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

namespace api.Controllers
{
    [Route("api/CPU")]
    [ApiController]
    public class CPUController : ControllerBase
    {
        private readonly ICPURepo _CPURepo;
        public CPUController(ICPURepo CPURepo)
        {
            _CPURepo = CPURepo;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dbCPUs = await _CPURepo.GetAllAsync(); //coming from db

            var dbDto = dbCPUs.Select(static s => s.ToGetDto()); //mapper turns to dto

            return Ok(dbDto);
        }
        [Authorize]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var dbCPU = await _CPURepo.GetByIdAsync(id);
            
            if(dbCPU == null)
            {
                return NotFound();
            }
            
            return Ok(dbCPU.ToGetDto());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(PostCPUDto CPUDto)
        {
            var CPUEntity = CPUDto.ToCreateEntity();
            await _CPURepo.CreateAsync(CPUEntity);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BaseDtoCreate updateDto)
        {
            var CPUEntity = await _CPURepo.UpdateAsync(id, updateDto);
            if(CPUEntity == null)
            {
                return NotFound();
            }

            return Ok(CPUEntity.ToGetDto());
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //find
            var CPUEntity = await _CPURepo.DeleteAsync(id);
            if (CPUEntity == null)
            {
                return NotFound();
            }

            return NoContent(); // 200 for delete
        }
    }
}