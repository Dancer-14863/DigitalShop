using Auth.API.Models;
using AutoMapper;

namespace Auth.API.Dto
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, SignInDto>();
            CreateMap<Account, GetAccountDto>();
            CreateMap<RegisterAccountDto, Account>();
        }
    }
}