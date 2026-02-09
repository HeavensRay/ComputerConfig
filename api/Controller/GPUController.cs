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
using api.Dto.GPU;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/GPU")]
    [ApiController]
    public class GPUController : ControllerBase
    {
        private readonly IGPURepo _GPURepo;
        public GPUController(IGPURepo GPURepo)
        {
            _GPURepo = GPURepo;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dbGPUs = await _GPURepo.GetAllAsync(); //coming from db

            var dbDto = dbGPUs.Select(static s => s.ToGetDto()); //mapper turns to dto

            return Ok(dbDto);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var dbGPU = await _GPURepo.GetByIdAsync(id);

            if(dbGPU == null)
            {
                return NotFound();
            }

            return Ok(dbGPU.ToGetDto());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(PostGPUDto GPUDto)
        {
            var GPUEntity = GPUDto.ToCreateEntity();
            await _GPURepo.CreateAsync(GPUEntity);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BaseDtoCreate updateDto)
        {
            var GPUEntity = await _GPURepo.UpdateAsync(id, updateDto);
            if(GPUEntity == null)
            {
                return NotFound();
            }

            return Ok(GPUEntity.ToGetDto());
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //find
            var GPUEntity = await _GPURepo.DeleteAsync(id);
            if (GPUEntity == null)
            {
                return NotFound();
            }

            return NoContent(); // 200 for delete
        }
    }
}