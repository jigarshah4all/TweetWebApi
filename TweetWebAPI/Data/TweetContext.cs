using Microsoft.EntityFrameworkCore;
using TweetWebAPI.Models;

namespace TweetWebAPI.Data
{
    public class TweetContext : DbContext
    {
        public TweetContext(DbContextOptions<TweetContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Tweet> Tweets { get; set; }

        public DbSet<TweetLike> TweetLikes { get; set; }

    }
}
