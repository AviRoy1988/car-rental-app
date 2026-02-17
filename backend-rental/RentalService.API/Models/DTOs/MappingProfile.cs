using AutoMapper;
using RentalService.API.Models;

namespace RentalService.API.Models.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Rental, RentalDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src => 
                src.ReturnDateTime.HasValue 
                    ? (int)Math.Ceiling((src.ReturnDateTime.Value - src.PickupDateTime).TotalDays)
                    : (int?)null))
            .ForMember(dest => dest.NumberOfKm, opt => opt.MapFrom(src => 
                src.ReturnMeterReading.HasValue 
                    ? src.ReturnMeterReading.Value - src.PickupMeterReading
                    : (int?)null));
    }
}
