using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace androgee_csharp
{
    class Program
    {
        private DiscordSocketClient _client;
		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
            _client = new DiscordSocketClient();

            _client.MessageReceived += MessageReceived;
            _client.UserJoined += UserJoined;
            _client.UserLeft += UserLeft;

            string token = System.Environment.GetEnvironmentVariable("NUEVO");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await _client.SetGameAsync("hl2_linux");

            // Block this task until the program is closed.
            await Task.Delay(-1);
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
            if (message.Content == "arroz is the bestest")
            {
                await message.Channel.SendMessageAsync("Oui, he is!!");
            }
        }
    }
}
