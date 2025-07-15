using System;
using System.IO;
using System.Threading.Tasks;
using Umamusume;
using Newtonsoft.Json;

namespace UmamusumeFriendSearch
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: UmamusumeFriendSearch <viewer_id>");
                return;
            }

            if (!int.TryParse(args[0], out var viewerId))
            {
                Console.WriteLine("Invalid viewer_id. Please provide a valid number.");
                return;
            }

            var client = new UmamusumeClient(new SimpleLz4Frame(0));
            var account = JsonConvert.DeserializeObject<Umamusume.Model.Account>(File.ReadAllText("account.json"));
            client.ChangeAccount(account);


            var friendSearch = new FriendSearch(client);

            try
            {
                var response = await friendSearch.Search(viewerId);
                Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }
    }
}
