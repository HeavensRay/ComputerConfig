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
using api.Dto.SSD;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/SSD")]
    [ApiController]
    public class SSDController : ControllerBase
    {
        private readonly ISSDRepo _ssdRepo;
        public SSDController(ISSDRepo ssdRepo)
        {
            _ssdRepo = ssdRepo;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dbSsds = await _ssdRepo.GetAllAsync(); //coming from db

            var dbDto = dbSsds.Select(static s => s.ToGetDto()); //mapper turns to dto

            return Ok(dbDto);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var dbSsd = await _ssdRepo.GetByIdAsync(id);

            if(dbSsd == null)
            {
                return NotFound();
            }

            return Ok(dbSsd.ToGetDto());
        }[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(PostSSDDto SSDDto)
        {
            var ssdEntity = SSDDto.ToCreateEntity();
            await _ssdRepo.CreateAsync(ssdEntity);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BaseDtoCreate updateDto)
        {
            var ssdEntity = await _ssdRepo.UpdateAsync(id, updateDto);
            if(ssdEntity == null)
            {
                return NotFound();
            }

            return Ok(ssdEntity.ToGetDto());
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //find
            var ssdEntity = await _ssdRepo.DeleteAsync(id);
            if (ssdEntity == null)
            {
                return NotFound();
            }

            return NoContent(); // 200 for delete
        }
    }
}