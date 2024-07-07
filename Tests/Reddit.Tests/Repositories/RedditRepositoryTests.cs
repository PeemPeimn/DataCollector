using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Reddit.Models;
using System.Text.Json;

namespace Reddit.Tests.Repositories.Tests
{
    public class RedditRepositoryTest
    {

        public RedditDbContext CreateContext(string dbName)
        {
            dbName = dbName.ToLower();
            IConfigurationRoot configs = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.test.json").Build();
            string connectionString = configs["PostgresConnectionString"]!;

            RedditDbContext dbContext = new RedditDbContext(connectionString);

            string createTestDb = $"DROP DATABASE IF EXISTS {dbName}; CREATE DATABASE {dbName} WITH TEMPLATE data_template";

            dbContext.Database.ExecuteSqlRaw(createTestDb);
            dbContext.SaveChanges();

            dbContext.Database.SetConnectionString(connectionString.Replace("postgres", dbName));
            return dbContext;
        }

        [Fact]
        public void InsertDataEmptyDatabase()
        {
            string dbName = "InsertData";
            var dbContext = CreateContext(dbName);
            var mockTimeProvider = new Mock<TimeProvider>();
            DateTimeOffset mockNow = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            mockTimeProvider.Setup(timeProvider => timeProvider.GetUtcNow()).Returns(mockNow);
            var repository = new RedditRepository(dbContext, mockTimeProvider.Object);

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

            var expectedSavedPost = new RedditPost
            {
                Id = postId,
                CreatedAt = mockNow,
                Post = post
            };
            var expectedData = new RedditData
            {
                Id = 1,
                PostId = postId,
                CreatedAt = mockNow,
                Messages = JsonSerializer.Serialize(dataList[0].Messages)
            };

            repository.InsertData(post, dataList);

            var savedPost = dbContext.redditPosts.Where(p => p.Id == postId).FirstOrDefault()!;

            var savedData = dbContext.redditData.Where(d => d.PostId == postId).FirstOrDefault()!;

            Assert.Equivalent(expectedSavedPost, savedPost);
            Assert.Equivalent(expectedData, savedData);
        }

        [Fact]
        public void InsertDataReplace()
        {
            string dbName = "InsertDataReplace";
            var dbContext = CreateContext(dbName);
            var mockTimeProvider = new Mock<TimeProvider>();
            DateTimeOffset mockNow = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            mockTimeProvider.Setup(timeProvider => timeProvider.GetUtcNow()).Returns(mockNow);
            var repository = new RedditRepository(dbContext, mockTimeProvider.Object);

            string insertMockData = File.ReadAllText(
                "../../../Repositories/SqlScripts/insert-data-replace.sql"
            ).Replace("{", "{{").Replace("}", "}}");

            dbContext.Database.ExecuteSqlRaw(insertMockData);

            string post = """{"post": "This is a post"}""";
            string postId = "test_post_id";
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

            var expectedSavedPost = new RedditPost
            {
                Id = postId,
                CreatedAt = mockNow,
                Post = post
            };
            var expectedData = new RedditData
            {
                Id = 4,
                PostId = postId,
                CreatedAt = mockNow,
                Messages = JsonSerializer.Serialize(dataList[0].Messages)
            };

            repository.InsertData(post, dataList);

            var savedPost = dbContext.redditPosts.Where(p => p.Id == postId).FirstOrDefault()!;

            var savedData = dbContext.redditData.Where(d => d.PostId == postId).FirstOrDefault()!;

            Assert.Equivalent(expectedSavedPost, savedPost);
            Assert.Equivalent(expectedData, savedData);
        }
    }
}
