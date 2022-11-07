using AutoMapper;

namespace CustomerRelationshipManagementAPI.Profiles
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<RequestForCreationDto, Request>();
            CreateMap<RequestForUpdateDto, Request>();
            CreateMap<Request,RequestDto>()
                .ForMember(dest => dest.CustomerName ,
                opts => 
                opts.MapFrom(
                    src => string.Concat(src.Customer.FirstName, " " , src.Customer.LastName)))
                .ForMember(dest => dest.CustomerPhone,
                opts => 
                opts.MapFrom(
                    src => src.Customer.PhoneNumber))
                .ForMember(dest => dest.RequestType , opts => 
                opts.MapFrom(
                    src => src.RequestType.Type
                    ))
                ;
        }
    }
}
