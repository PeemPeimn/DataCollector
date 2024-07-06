using Microsoft.Extensions.Configuration;
using Reddit.Formatters;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Reddit;

[ExcludeFromCodeCoverage]
class Program
{
    static async Task Main(string[] args)
    {
        IConfigurationRoot configs = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        RedditDbContext dbContext = new RedditDbContext(configs["PostgresConnectionString"]);
        RedditRepository repository = new RedditRepository(dbContext, TimeProvider.System);

        Console.Write("Enter the Reddit post's URL: ");
        string URL = Console.ReadLine()!;
        URL += ".json";

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", configs["UserAgent"]);
        HttpResponseMessage response = await client.GetAsync(URL);
        string postJson = await response.Content.ReadAsStringAsync();

        List<Thing> listOfThings = JsonSerializer.Deserialize<List<Thing>>(postJson)!;

        List<SFTTrainerData> dataList;
        DataFormatter.Format(listOfThings, out dataList);

        repository.InsertData(postJson, dataList);
    }

}
