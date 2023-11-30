using System.Drawing;
using AutoMapper;
using NZwalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Region = NZwalks.API.Models.Domain.Region;

namespace NZWalks.API.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Region, RegionDto>().ReverseMap();
        CreateMap<AddRegionRequestDto, Region>().ReverseMap();
        CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
        CreateMap<Walk, WalkDto>().ReverseMap();
        CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
        CreateMap<UpdateWalkRequestDto,Walk >().ReverseMap();
        CreateMap<Difficulty, DifficultyDto>().ReverseMap();
    }
}