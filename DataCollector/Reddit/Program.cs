using DataCollector.Reddit.DataFormatter;
using System.Diagnostics;
using System.Text.Json;

namespace Reddit;

class Program
{
    static async Task Main(string[] args)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "intamoon.peem");
        HttpResponseMessage response = await client.GetAsync("https://www.reddit.com/r/socialskills/comments/1dgvkf9/a_60m_at_the_gym_keeps_talking_to_me_18f_after_i.json");

        List<Thing> listOfThings = JsonSerializer.Deserialize<List<Thing>>(response.Content.ReadAsStream())!;

        List<SFTTrainerData> dataList;
        DataFormatter.Format(listOfThings, out dataList);
        Debug.WriteLine("Finished");
    }

}
