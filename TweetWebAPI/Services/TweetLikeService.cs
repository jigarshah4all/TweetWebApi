using AutoMapper;
using TweetWebAPI.Data;
using TweetWebAPI.Models;

namespace TweetWebAPI.Services
{
    public class TweetLikeService : ITweetLikeService
    {
        private readonly TweetContext dataContext;
        private readonly IMapper mapper;

        public TweetLikeService(IMapper mapper, TweetContext dataContext)
        {
            this.mapper = mapper;
            this.dataContext = dataContext;
        }

        public async Task<ServiceResponse<int>> TweetLike(int tweetId, string userName)
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
                    Tweet oldTweet = dataContext.Tweets.FirstOrDefault(x => x.Id == tweetId);
                    if (oldTweet == null)
                    {
                        response.Success = false;
                        response.Message = "Tweet not exists.";
                    }
                    else
                    {
                        TweetLike tweetLike = dataContext.TweetLikes.FirstOrDefault(x => x.Tweet.Id == tweetId && x.UserId == user.Id);
                        if(tweetLike != null)
                        {
                            dataContext.TweetLikes.Remove(tweetLike);
                            oldTweet.Likes--;
                            await dataContext.SaveChangesAsync();
                            response.Data = tweetLike.Id;
                            response.Message = "Tweet dislike successfully.";
                        }
                        else
                        {
                            tweetLike = new TweetLike();
                            tweetLike.Tweet = oldTweet;
                            tweetLike.UserId = user.Id;
                            dataContext.TweetLikes.Add(tweetLike);
                            oldTweet.Likes++;
                            await dataContext.SaveChangesAsync();
                            response.Data = tweetLike.Id;
                            response.Message = "Tweet like successfully.";
                        }
                    }
                }
            }

            return response;
        }
    }
}
