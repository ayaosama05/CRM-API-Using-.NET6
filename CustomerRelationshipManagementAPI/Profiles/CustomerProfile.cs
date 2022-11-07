using AutoMapper;

namespace CustomerRelationshipManagementAPI.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto> ();
            CreateMap<CustomerDto, Customer> ();
            CreateMap<Customer, CustomerWithRequestsDto>()
                .ForMember(dest => dest.CustomerRequestDtos,
                ops => ops.MapFrom(src => src.Requests));
            CreateMap<Request, CustomerRequestDto>()
                .ForMember(dest => dest.RequestType, opts =>
                opts.MapFrom(
                   src => src.RequestType.Type
                   ))
                .ReverseMap();
        }
    }
}
