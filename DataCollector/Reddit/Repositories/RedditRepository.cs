using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class RedditDbContext : DbContext
{

    private string? _connectionString;

    public DbSet<RedditPost> redditPosts { get; set; }
    public DbSet<RedditData> redditData { get; set; }

    public RedditDbContext(string? connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}

public class RedditRepository(RedditDbContext dbContext)
{
    private RedditDbContext _dbContext = dbContext;

    public void InsertData(string post, List<SFTTrainerData> dataList)
    {
        var transaction = _dbContext.Database.BeginTransaction();
        var postId = dataList[0].PostId!;
        DateTime now = DateTime.UtcNow;
        var newPost = new RedditPost { Id = postId, CreatedAt = now, Post = post };

        _dbContext.redditPosts.Where(p => p.Id == postId).ExecuteDelete();
        _dbContext.redditPosts.Add(newPost);
        _dbContext.SaveChanges();

        foreach (var data in dataList)
        {
            var newData = new RedditData
            {
                PostId = postId,
                CreatedAt = now,
                Messages = JsonSerializer.Serialize(data.Messages)
            };
            _dbContext.redditData.Add(newData);
        }
        _dbContext.SaveChanges();
        transaction.Commit();
    }
}