using System.Diagnostics;

namespace Reddit;

class Program
{
    static async Task Main(string[] args)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "intamoon.peem");
        HttpResponseMessage response = await client.GetAsync("https://www.reddit.com/r/PedroPeepos/comments/1dfjhww/kellin_has_spent_more_time_in_dk_than_nuguri_and.json");
        String data = await response.Content.ReadAsStringAsync();
        Debug.WriteLine(data);
    }
}
