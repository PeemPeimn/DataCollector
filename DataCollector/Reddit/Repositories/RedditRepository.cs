using Microsoft.EntityFrameworkCore;
using Reddit.Models;
using System.Text.Json;

namespace Reddit.Repositories;

public class RedditDbContext : DbContext
{

    private readonly string? _connectionString;

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

public class RedditRepository(RedditDbContext dbContext, TimeProvider timeProvider)
{
    private readonly RedditDbContext _dbContext = dbContext;

    private readonly TimeProvider _timeProvider = timeProvider;

    public void InsertData(string post, SftTrainerData data)
    {
        var transaction = _dbContext.Database.BeginTransaction();
        var postId = data.PostId!;
        DateTimeOffset now = _timeProvider.GetUtcNow();
        var newPost = new RedditPost { Id = postId, CreatedAt = now, Post = post };

        _dbContext.redditPosts.Where(p => p.Id == postId).ExecuteDelete();
        _dbContext.redditPosts.Add(newPost);
        _dbContext.SaveChanges();

        foreach (var conversation in data.ConversationList)
        {
            var newData = new RedditData
            {
                PostId = postId,
                CreatedAt = now,
                Conversation = JsonSerializer.Serialize(conversation)
            };
            _dbContext.redditData.Add(newData);
        }
        _dbContext.SaveChanges();
        transaction.Commit();
    }
}