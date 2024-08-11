using Microsoft.Extensions.Configuration;
using Reddit.Converters;
using Reddit.Models;
using Reddit.Repositories;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Reddit;

[ExcludeFromCodeCoverage]
static class Program
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
        ObjectConverter.Convert(listOfThings[0]);
        ObjectConverter.Convert(listOfThings[1]);

        var data = Formatters.SftTrainerDataFormatter.Format(listOfThings);

        repository.InsertData(postJson, data);
    }

}
