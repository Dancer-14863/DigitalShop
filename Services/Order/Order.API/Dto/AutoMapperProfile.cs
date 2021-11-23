using AutoMapper;

namespace Order.API.Dto
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.Order, GetOrderDto>();
            CreateMap<CreateOrderDto, Models.Order>();
        }
    }
}