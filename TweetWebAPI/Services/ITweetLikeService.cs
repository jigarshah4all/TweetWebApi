namespace TweetWebAPI.Services
{
    public interface ITweetLikeService
    {
        Task<ServiceResponse<int>> TweetLike(int tweetId, string userName);
    }
}
