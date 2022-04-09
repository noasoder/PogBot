using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace PogBot
{
	public class Program
	{
		private DiscordSocketClient client;
		private CommandService commands;
		private CommandHandler commandHandler;

		public static Task Main(string[] args) => new Program().MainAsync();

		public async Task MainAsync()
		{
			Global.Setup();

			client = new DiscordSocketClient();
			client.Log += Log;

			await client.LoginAsync(TokenType.Bot, Global.Instance.token);
			await client.StartAsync();

			commands = new CommandService();
			commandHandler = new CommandHandler(client, commands);

			await commandHandler.InstallCommandsAsync();

			while (Console.ReadKey().Key != ConsoleKey.Escape)
			{

			};

			Global.Shutdown();
			await client.StopAsync();
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
