using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Mappers;
using api.Interfaces;
using api.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using api.Dto.GPU;
using api.Repository;
using api.Dto.Config;
using System.Security.Cryptography;

namespace api.Services
{
    public class GenConfig : IGenConfig
    {
        private ISSDRepo _ssdRepo;
        private ICPURepo _cpuRepo;
        private IGPURepo _gpuRepo;
        private IMoboRepo _moboRepo;
        private IPcuRepo _pcuRepo;
        private IRamRepo _ramRepo;

        public GenConfig(ISSDRepo sSDRepo, ICPURepo cpuRepo, 
        IGPURepo gpuRepo,IMoboRepo moboRepo, IPcuRepo pcuRepo, IRamRepo ramRepo )
        {
            _ssdRepo = sSDRepo;
            _cpuRepo = cpuRepo;
            _gpuRepo = gpuRepo;
            _moboRepo = moboRepo;
            _pcuRepo = pcuRepo;
            _ramRepo = ramRepo;
        }

        public async Task<GennedConfigDto?> CreateFromBudget(decimal budget, PostConfigDto criteria, int ssdSize)
        {
            // saving for ssd+ram bare minimum
            decimal ssdBudget = 150m;
            // 1 gpu
            decimal gpuBudget = (budget-(ssdBudget+150)) * 0.6m; //150 for ram
            
            var gpuEntity = await _gpuRepo.FindBestForPrice(gpuBudget, int.Parse(criteria.Purpose));
            if (gpuEntity == null)
            {
                return null;
            }

            gpuEntity.ToGetDto();

            // we have gpu now
            budget = budget -gpuEntity.Price;


            // pcu find cheapest pcu where volts = gpu.volts
            string rating = gpuEntity.Volts switch
            {
                >= 850 => "A",
                >= 500 => "B",
    _           => "C"
            };

            var pcuEntity = await _pcuRepo.FindBestForPrice( gpuEntity.Volts, rating );
            if (pcuEntity == null)
            {
                return null;
            }

            pcuEntity.ToGetDto();

            budget = budget -pcuEntity.Price;

            //  cpu
            decimal cpuBudget = budget * 0.4m;
            var cpuEntity = await _cpuRepo.FindBestForPrice(cpuBudget ,gpuEntity.Power - 200);
            if (cpuEntity == null)
            {
                return null;
            }

            cpuEntity.ToGetDto();

            budget = budget -cpuEntity.Price;

            // mobo
            string moboRating = cpuEntity.Power switch
            {
                > 2000 => "S",
                >= 1400 => "A",
                > 1000 => "B",
    _           => "C"
            };

            var MoboEntity = await _moboRepo.FindBestForPrice(budget, moboRating);
            if (MoboEntity == null)
            {
                return null;
            }
            budget = budget -MoboEntity.Price;

            // ssdBudget

            decimal ssdBMaybe = budget * 0.5m;
            if (ssdBMaybe > ssdBudget) // if leftover is more than 200 take it
            {
                ssdBudget = ssdBMaybe;
            }

            var ssdEntity = await _ssdRepo.FindBestForPrice(ssdBudget, ssdSize);

            if (ssdEntity == null)
            {
                return null;
            }
            budget = budget -ssdEntity.Price;


            // ram

            int capacity = 16;
            if (criteria.Purpose != "1080")
            {
                capacity = 32;
            }

            var ramEntity = await _ramRepo.FindBestForPrice(budget, capacity);

            if (ramEntity == null)
            {
                return null;
            }

            return new GennedConfigDto
            {
                ConfigName = criteria.ConfigName,
                Purpose = criteria.Purpose,
                SsdId = ssdEntity.Id,
                CpuId = cpuEntity.Id,
                GpuId = gpuEntity.Id,
                MoboId = MoboEntity.Id,
                PcuId = pcuEntity.Id,
                RamId = ramEntity.Id,
                Price = ssdEntity.Price + cpuEntity.Price + gpuEntity.Price +
                                    MoboEntity.Price + pcuEntity.Price + ramEntity.Price
            };

        }
    }
}