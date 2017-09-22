using Discord;
using System.Net.Http;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace androgee_csharp
{
    class Program
    {
        private DiscordSocketClient _client;
        private string _httpResponse;
		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
            _client = new DiscordSocketClient();

            _client.MessageReceived += MessageReceived;
            _client.UserJoined += UserJoined;
            _client.UserLeft += UserLeft;
            _client.Ready += Test;

            string token = System.Environment.GetEnvironmentVariable("NUEVO");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await _client.SetGameAsync("WarThunder");

            // Block this task until the program is closed.
            await Task.Delay(-1);
		}
        private async Task Test()
        {
        }

        private async Task UserJoined(SocketGuildUser user) 
        {
            var egeeio = _client.GetGuild(183740337976508416);
            await egeeio.DefaultChannel.SendMessageAsync("**" + user.Username + "**" + " joined the server!");
        }

        private async Task UserLeft(SocketGuildUser user) 
        {
            var egeeio = _client.GetGuild(183740337976508416);
            var debugChannel = egeeio.GetTextChannel(268141230091796481);
            await debugChannel.SendMessageAsync("**" + user.Username + "**" + " just left the server.");
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content.Contains("~help"))
            {
                await message.Channel.SendMessageAsync("Not Implement Yet.");
            }
            else if (message.Content.Contains("~fortune"))
            {
                await message.Channel.SendMessageAsync("Eat Shit.");
            }
            else if (message.Content.Contains("~catpic"))
            {
                await GetCatPic();
                await message.Channel.SendMessageAsync(_httpResponse);
                
            }
            else if (message.Content.Contains("~catgif"))
            {
                await GetCatGif();
                await message.Channel.SendMessageAsync(_httpResponse);
            }
            else if (message.Content.Contains("~chucknorris"))
            {
                await GetChuckNorrisQuote();
                await message.Channel.SendMessageAsync(_httpResponse);
            }
        }
        private async Task GetChuckNorrisQuote()
        {
            var client = new HttpClient();

            var serializer = new DataContractJsonSerializer(typeof(List<repo>));
            var stringTask = client.GetStringAsync("http://api.icndb.com/jokes/random?exclude=[explicit]");

            var jsonString = await stringTask;

            // var o = JObject.Parse(jsonString);

            dynamic jsonJson = JObject.Parse(jsonString);
            _httpResponse =jsonJson.value.joke;
        }
        private async Task GetCatPic()
        {
            var client = new HttpClient();
            var stringTask = client.GetAsync("http://thecatapi.com/api/images/get?format=src&type=jpg");

            var msg = await stringTask;
            _httpResponse = msg.RequestMessage.RequestUri.AbsoluteUri;
        }
        private async Task GetCatGif()
        {
            var client = new HttpClient();
            var stringTask = client.GetAsync("http://thecatapi.com/api/images/get?format=src&type=gif");

            var msg = await stringTask;
            _httpResponse = msg.RequestMessage.RequestUri.AbsoluteUri;
        }
    }
}
