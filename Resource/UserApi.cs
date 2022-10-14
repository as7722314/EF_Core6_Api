using AutoMapper;
using CoreApiTest.Models;

namespace CoreApiTest.Resource
{
    public class UserApi : Profile
    {
        public UserApi()
        {
            CreateMap<User, UserApiResource>();
        }
    }
}