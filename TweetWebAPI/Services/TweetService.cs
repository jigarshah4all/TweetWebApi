using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TweetWebAPI.Data;
using TweetWebAPI.Dto;
using TweetWebAPI.Models;

namespace TweetWebAPI.Services
{
    public class TweetService : ITweetService
    {
        private readonly TweetContext dataContext;
        private readonly IMapper mapper;

        public TweetService(IMapper mapper, TweetContext dataContext)
        {
            this.mapper = mapper;
            this.dataContext = dataContext;
        }

        public async Task<ServiceResponse<List<GetTweetDto>>> GetAllTweets()
        {
            ServiceResponse<List<GetTweetDto>> response = new ServiceResponse<List<GetTweetDto>>();
            var tweets = dataContext.Tweets.Include(x=>x.User).ToList();
            response.Success = true;
            response.Data = this.mapper.Map<List<GetTweetDto>>(tweets);
            return response;
        }

        public async Task<ServiceResponse<List<GetTweetDto>>> GetTweets(string userName)
        {
            ServiceResponse<List<GetTweetDto>> response = new ServiceResponse<List<GetTweetDto>>();
            if (string.IsNullOrEmpty(userName))
            {
                response.Success = false;
                response.Message = "User name is required.";
            }
            else
            {
                var user = dataContext.Users.FirstOrDefault(x => x.LoginId.ToLower() == userName.ToLower());

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not exists.";
                }
                else
                {
                    var tweets = dataContext.Tweets.Where(x=>x.User.Id == user.Id).Include(x => x.User).ToList();
                    response.Success = true;
                    response.Data = this.mapper.Map<List<GetTweetDto>>(tweets);
                }
            }

            return response;
        }

        public async Task<ServiceResponse<int>> AddTweet(TweetDto tweetDto, string userName)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (string.IsNullOrEmpty(userName))
            {
                response.Success = false;
                response.Message = "User name is required.";
            }
            else
            {
                var user = dataContext.Users.FirstOrDefault(x => x.LoginId.ToLower() == userName.ToLower());

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not exists.";
                }
                else
                {
                    Tweet tweet = this.mapper.Map<Tweet>(tweetDto);
                    tweet.User = user;
                    tweet.ParentTweet = null;

                    dataContext.Tweets.Add(tweet);
                    await dataContext.SaveChangesAsync();

                    response.Data = tweet.Id;
                    response.Message = "Tweet added successfully.";
                }
            }

            return response;
        }

        public async Task<ServiceResponse<int>> ReplyTweet(TweetDto tweetDto, string userName, int tweetId)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (string.IsNullOrEmpty(userName))
            {
                response.Success = false;
                response.Message = "User name is required.";
            }
            else
            {
                var user = dataContext.Users.FirstOrDefault(x => x.LoginId.ToLower() == userName.ToLower());

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not exists.";
                }
                else
                {
                    Tweet tweetReply = dataContext.Tweets.FirstOrDefault(x => x.Id == tweetId);
                    Tweet tweet = this.mapper.Map<Tweet>(tweetDto);
                    if (tweetReply != null)
                    {
                        tweet.User = user;
                        tweet.ParentTweet = tweetReply;

                        dataContext.Tweets.Add(tweet);
                        await dataContext.SaveChangesAsync();

                        response.Data = tweet.Id;
                        response.Message = "Tweet replyed successfully.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Tweet not exists.";
                    }
                }
            }

            return response;
        }

        public async Task<ServiceResponse<int>> UpdateTweet(int tweetid, UpdateTweetDto tweetDto, string userName)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (string.IsNullOrEmpty(userName))
            {
                response.Success = false;
                response.Message = "User name is required.";
            }
            else
            {
                var user = dataContext.Users.FirstOrDefault(x => x.LoginId.ToLower() == userName.ToLower());

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not exists.";
                }
                else
                {
                    Tweet oldTweet = dataContext.Tweets.FirstOrDefault(x => x.Id == tweetid && x.User.Id == user.Id);
                    if (oldTweet == null)
                    {
                        response.Success = false;
                        response.Message = "Tweet not exists.";
                    }
                    else
                    {
                        oldTweet.Message = tweetDto.Message;
                        oldTweet.Tag = tweetDto.Tag;

                        await dataContext.SaveChangesAsync();

                        response.Data = oldTweet.Id;
                        response.Message = "Tweet updated successfully.";
                    }
                }
            }

            return response;
        }

        public async Task<ServiceResponse<int>> DeleteTweet(int tweetId, string userName)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (string.IsNullOrEmpty(userName))
            {
                response.Success = false;
                response.Message = "User name is required.";
            }
            else
            {
                var user = dataContext.Users.FirstOrDefault(x => x.LoginId.ToLower() == userName.ToLower());

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not exists.";
                }
                else
                {
                    Tweet oldTweet = dataContext.Tweets.FirstOrDefault(x => x.Id == tweetId && x.User.Id == user.Id);
                    if (oldTweet == null)
                    {
                        response.Success = false;
                        response.Message = "Tweet not exists.";
                    }
                    else
                    {
                        await Delete(tweetId);

                        response.Message = "Tweet deleted successfully.";
                    }
                }
            }

            return response;
        }

        private async Task Delete(int tweetId)
        {
            var tweets = dataContext.Tweets.Where(x => x.ParentTweet != null && x.ParentTweet.Id == tweetId).ToList();
            if (tweets.Any() && tweets.Count > 0)
            {
                foreach (var item in tweets)
                {
                    await Delete(item.Id);
                }
            }

            var tweet = dataContext.Tweets.FirstOrDefault(x => x.Id == tweetId);
            dataContext.Tweets.Remove(tweet);
            await dataContext.SaveChangesAsync();
        }
    }
}
