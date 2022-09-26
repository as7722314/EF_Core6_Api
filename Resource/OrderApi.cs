using AutoMapper;
using CoreApiTest.Models;

namespace CoreApiTest.Resource
{
    public class OrderApi : Profile
    {
        public OrderApi()
        {
            CreateMap<Order, OrderApiResource>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => $"{src.Id}")
                )
                .ForMember(
                    dest => dest.Total,
                    opt => opt.MapFrom(src => $"{src.Total}")
                )
                .ForMember(
                    dest => dest.OrderDate,
                    opt => opt.MapFrom(src => $"{src.OrderDate}")
                )
                .ForMember(
                    dest => dest.User,
                    opt =>
                        CreateMap<User, UserApiResource>()
                            .ForMember(
                                dest => dest.Id,
                                opt => opt.MapFrom(src => $"{src.Id}")
                            )
                            .ForMember(
                                dest => dest.Name,
                                opt => opt.MapFrom(src => $"{src.Name}")
                            )
                            .ForMember(
                                dest => dest.Email,
                                opt => opt.MapFrom(src => $"{src.Email}")
                            )
                );
        }
    }
}
