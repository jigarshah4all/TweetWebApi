namespace TweetWebAPI.Dto
{
    public class GetTweetDto
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public string Tag { get; set; }

        public int Likes { get; set; }

        public GetTweetDto? ParentTweet { get; set; }

        public TweetUserDto User { get; set; }
    }
}
