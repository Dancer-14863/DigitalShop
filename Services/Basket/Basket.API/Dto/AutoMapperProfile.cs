using AutoMapper;

namespace Basket.API.Dto
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.Basket, GetBasketDto>();
            CreateMap<Models.Basket, UpdateBasketDto>();
            CreateMap<UpdateBasketDto, Models.Basket>();
            CreateMap<CreateBasketDto, Models.Basket>();
        }
    }
}