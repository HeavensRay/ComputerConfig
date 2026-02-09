
using System;
using api.Data;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Entities;
using api.Repository;
using api.Interfaces;
using System.Runtime.InteropServices;
using api.Dto.User;
using api.Dto.Config;
using api.Services;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/Config")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigRepo _configRepo;
        private readonly ISSDRepo _ssdRepo;
        private readonly IGenConfig _genRepo;
        public ConfigController(IConfigRepo configRepo, ISSDRepo ssdRepo, IGenConfig genRepo)
        { 
            _configRepo = configRepo;
            _ssdRepo = ssdRepo;
            _genRepo = genRepo;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var username = User.GetUserName();
            var dbConf = await _configRepo.GetAllAsync(username); //coming from db

            var dbDto = dbConf.Select(static c => c.ToGetDto()); //mapper turns to dto

            return Ok(dbDto);
        }
        
        // [HttpPost("manual/{userId}/{ssdId}")]
        // public async Task<IActionResult> Create(
        //             [FromRoute] string userId,
        //             [FromRoute] int ssdId, 
        //             PostConfigDto configDto ){
        //     if(!await _userRepo.Exists(userId))
        //     {
        //         return BadRequest("User does not exist");
        //     }
        //     if(!await _ssdRepo.Exists(ssdId))
        //     {
        //         return BadRequest("SSD does not exist");
        //     }

        //     var configEntity = configDto.ToCreateEntity(userId,ssdId);
        //     await _configRepo.CreateAsync(configEntity);
        //     return Ok();
        // }
        [Authorize]
        [HttpPost("{ssdSize}")]
        public async Task<IActionResult> GenFromPrice(
                    [FromBody] PostConfigDto dto,
                    [FromRoute] int ssdSize)
                    
        {
            var username = User.GetUserName();
            var genned = await _genRepo.CreateFromBudget(dto.Price, dto, ssdSize);
            if (genned == null)
            {
                return NotFound("Ur Poor lol get a job");
            }
            
            var entity = genned.ToEntityGen(username);
            await _configRepo.CreateAsync(entity);
            return Ok(genned);
        }
        [Authorize]
        [HttpGet("{configName}")]
        public async Task<IActionResult> GetById([FromRoute] string configName)

        {
            var username = User.GetUserName();
            var config = await _configRepo.GetByIdAsync(configName , username);
            // just check dont display the entire component
            if(config == null)
            {
                return NotFound();
            }


            return Ok();
        }
        [Authorize]
        [HttpDelete]
        [Route("{configName}")]

        public async Task<IActionResult> Delete([FromRoute] string configName)
        {
            var username = User.GetUserName();   
            var Exists = await _configRepo.Delete(configName, username);
            if(Exists == null)
            {
                return NotFound("Config not found");
            }
            return Ok();
        }
    }
}