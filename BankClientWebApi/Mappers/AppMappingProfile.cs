using AutoMapper;
using BankClientWebApi.Models;
using BankClientWebApi.Protos;

namespace BankClientWebApi.Mappers
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<UserReply, UserResponse>().ReverseMap();
            CreateMap<ListReply, ListAccountsResponse>();
            CreateMap<AccountReply, AccountResponse>().ReverseMap();
        }   
    }
}
