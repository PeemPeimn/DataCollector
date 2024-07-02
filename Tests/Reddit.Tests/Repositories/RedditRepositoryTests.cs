using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Reddit.Tests.Repositories.Tests
{
    public class RedditRepositoryTest
    {

        public RedditDbContext CreateContext(string dbName)
        {
            dbName = dbName.ToLower();
            IConfigurationRoot configs = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
            string connectionString = configs["PostgresConnectionString"]!;

            RedditDbContext dbContext = new RedditDbContext(connectionString);

            string createTestDb = $"DROP DATABASE IF EXISTS {dbName}; CREATE DATABASE {dbName} WITH TEMPLATE data OWNER admin";

            dbContext.Database.ExecuteSqlRaw(createTestDb);
            dbContext.SaveChanges();

            dbContext.Database.SetConnectionString(connectionString.Replace("data", dbName));
            return dbContext;
        }

        [Fact]
        public void InsertData()
        {
            string dbName = "InsertData";
            var dbContext = CreateContext(dbName);
            var repository = new RedditRepository(dbContext);

            string post = """{"post": "This is a post"}""";
            string postId = "post_id";
            List<SFTTrainerData> dataList = new List<SFTTrainerData> {
                new SFTTrainerData {
                    PostId = postId,
                    Messages = new List<SFTTrainerData.Message> {
                        new SFTTrainerData.Message {
                            Role = "user_1",
                            Content = "hello"
                        }
                    }
                }
            };

            var expectedSavedPost = new RedditPost { Id = postId, CreatedAt = DateTime.Now, Post = post };
            var expectedData = new RedditData { Id = 0, PostId = postId, CreatedAt = DateTime.Now, Messages = JsonSerializer.Serialize(dataList[0].Messages) };

            repository.InsertData(post, dataList);

            var savedPost = dbContext.redditPosts.Where(p => p.Id == postId).FirstOrDefault()!;
            expectedSavedPost.CreatedAt = savedPost.CreatedAt;

            var savedData = dbContext.redditData.Where(d => d.PostId == postId).FirstOrDefault()!;
            expectedData.CreatedAt = savedData.CreatedAt;
            expectedData.Id = savedData.Id;

            Assert.Equivalent(expectedSavedPost, savedPost);
            Assert.Equivalent(expectedData, savedData);
        }
    }
}
