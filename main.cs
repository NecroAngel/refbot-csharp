using Discord;
using System.IO;
using System.Net.Http;
using Discord.WebSocket;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System;

namespace androgee
{
    internal class Program
    {
        private DiscordSocketClient _client;
        private string _httpResponse;
		public static void Main()
			=> new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
		{
            Random rnd = new Random();
            var json = JObject.Parse(File.ReadAllText(@"blob.json"));
            var games = json.SelectToken("games");
            var length = games.Children().Count() - 1;

            _client = new DiscordSocketClient();

            _client.MessageReceived += MessageReceived;
            _client.UserJoined += UserJoined;
            _client.UserLeft += UserLeft;
            _client.Ready += Test;

            string token = System.Environment.GetEnvironmentVariable("NUEVO");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await _client.SetGameAsync(games[rnd.Next(0, length)].ToString());

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
            string output;

            switch (message.Content)
            {
                case ("~help"):
                    await message.Channel.SendMessageAsync("Get fucked");
                    break;
                case "~fortune":
                    output = "fortune -s | cowsay".ToBash();
                    await message.Channel.SendMessageAsync("``" + output + "``");
                    break;
                case "~catpic":
                await GetCatPic();
                await message.Channel.SendMessageAsync(_httpResponse);
                    break;
                case "~catgif":
                await GetCatGif();
                await message.Channel.SendMessageAsync(_httpResponse);
                    break;
                case "~chucknorris":
                await GetChuckNorrisQuote();
                await message.Channel.SendMessageAsync(_httpResponse);
                    break;
                case "~commands":
                    await message.Channel.SendMessageAsync("Get rekt");
                    break;
                case "~ping":
                    await message.Channel.SendMessageAsync("C'mon, I'm not a baby");
                    break;
                case "~ghostbusters":
                    output = "cowsay -f ghostbusters Who you Gonna Call".ToBash();
                    await message.Channel.SendMessageAsync("``" + output + "``");
                    break;
                case "~moo":
                    output = "apt-get moo".ToBash();
                    await message.Channel.SendMessageAsync("``" + output + "``");
                    break;
                case "~dragon":
                break;
                case "~tux":
                break;
                case "~penguin":
                break;
                default:
                    if (message.Content.Contains("~test"))
                    {
                        var distro = message.Content.Remove(0, 6);
                        var guild = _client.GetGuild(183740337976508416);
                        var user = guild.GetUser(message.Author.Id);
                        var roles = guild.Roles;

                        JObject json = JObject.Parse(File.ReadAllText(@"blob.json"));
                        var protectedRoles = json.SelectToken("protectedRoles");
                        var test = protectedRoles.Children().Contains(distro);

                        if (test == false)
                        {
                            var myRole = roles.Single(r => r.Name == distro);
                            await user.AddRoleAsync(myRole);
                        }
                    }
                break;
            }
        }
        private async Task GetChuckNorrisQuote()
        {
            var client = new HttpClient();
            var stringTask = client.GetStringAsync("http://api.icndb.com/jokes/random?exclude=[explicit]");
            var jsonString = await stringTask;

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
