using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZwalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await regionRepository.GetAllAsync();

            // var regionsDto = new List<RegionDto>();
            //
            // foreach (var regionDomain in regionsDomain)
            // {
            //     regionsDto.Add(new RegionDto()
            //     {
            //         Id = regionDomain.Id,
            //         Code = regionDomain.Code,
            //         Name = regionDomain.Name,
            //         RegionImageUrl = regionDomain.RegionImageUrl,
            //     });
            // }

            // var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
            // return Ok(regionsDto);

            // 使用AutoMapper
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);
            // var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // var regionDto = new RegionDto()
            // {
            //     Id = regionDomain.Id,
            //     Code = regionDomain.Code,
            //     Name = regionDomain.Name,
            //     RegionImageUrl = regionDomain.RegionImageUrl,
            // };
            // return Ok(regionDto);

            // 使用AutoMapper
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // var regionDomainModel = new Region
            // {
            //     Code = addRegionRequestDto.Code,
            //     Name = addRegionRequestDto.Name,
            //     RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            // };
            //
            // regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
            //
            // var regionDto = new RegionDto
            // {
            //     Id = regionDomainModel.Id,
            //     Code = regionDomainModel.Code,
            //     Name = regionDomainModel.Name,
            //     RegionImageUrl = regionDomainModel.RegionImageUrl,
            // };
            //
            // return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);

            // 使用AutoMapper
            if (ModelState.IsValid) // 未使用ValidateModelAttribute
            {
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id,
            [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // var regionDomainModel = new Region
            // {
            //     Code = updateRegionRequestDto.Code,
            //     Name = updateRegionRequestDto.Name,
            //     RegionImageUrl = updateRegionRequestDto.RegionImageUrl,
            // };
            //
            // regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            //
            // if (regionDomainModel == null)
            // {
            //     return NotFound();
            // }
            //
            // var regionDto = new RegionDto
            // {
            //     Id = regionDomainModel.Id,
            //     Code = regionDomainModel.Code,
            //     Name = regionDomainModel.Name,
            //     RegionImageUrl = regionDomainModel.RegionImageUrl,
            // };
            // return Ok(regionDto);

            // 使用AutoMapper
            if (ModelState.IsValid)
            {
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
                return Ok(mapper.Map<RegionDto>(regionDomainModel));
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            // var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // var regionDto = new RegionDto
            // {
            //     Id = regionDomainModel.Id,
            //     Code = regionDomainModel.Code,
            //     Name = regionDomainModel.Name,
            //     RegionImageUrl = regionDomainModel.RegionImageUrl,
            // };

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}