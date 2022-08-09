using AutoMapper;
using TweetWebAPI.Dto;
using TweetWebAPI.Models;

namespace TweetWebAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Tweet, TweetDto>();
            CreateMap<TweetDto, Tweet>();

            CreateMap<User, TweetUserDto>();
            CreateMap<TweetUserDto, User>();

            CreateMap<Tweet, GetTweetDto>();
            CreateMap<GetTweetDto, Tweet>();
        }
    }
}
