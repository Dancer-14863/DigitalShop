using AutoMapper;
using Microsoft.EntityFrameworkCore;
using User.API.Models;

namespace User.API.Dto
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, GetAccountDto>();
            CreateMap<Account, UpdateAccountDto>();
            CreateMap<UpdateAccountDto, Account>();
        }
    }
}