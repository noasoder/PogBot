﻿using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;

public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;

    // Retrieve client and CommandService instance via ctor
    public CommandHandler(DiscordSocketClient client, CommandService commands)
    {
        _commands = commands;
        _client = client;
    }

    public async Task InstallCommandsAsync()
    {
        // Hook the MessageReceived event into our command handler
        _client.MessageReceived += HandleCommandAsync;

        await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                        services: null);
    }

    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        // Don't process the command if it was a system message
        var message = messageParam as SocketUserMessage;
        if (message == null) return;

        // Create a number to track where the prefix ends and the command begins
        int argPos = 0;
        
        // Determine if the message is a command based on the prefix and make sure no bots trigger commands
        if (!(message.HasStringPrefix(Global.discordCommand, ref argPos) ||
            message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            message.Author.IsBot)
            return;

        // Create a WebSocket-based command context based on the message
        var context = new SocketCommandContext(_client, message);

        // Execute the command with the command context we just
        // created, along with the service provider for precondition checks.
        await _commands.ExecuteAsync(
            context: context,
            argPos: argPos,
            services: null);

        var customSearch = message.Content;
        customSearch = customSearch.Remove(0, Global.discordCommand.Length);
        var imageURL = await Global.Instance.GetImage(customSearch);
        await SaveImageRef(imageURL);
        await message.Channel.SendMessageAsync(imageURL);
    }

    public async Task<bool> SaveImageRef(string imageURL)
    {
        if (!File.Exists("src/saved_images.txt"))
            File.Create("src/saved_images.txt");

        var found = await Global.IsInSaved(imageURL);

        if(!found)
        {
            File.AppendAllText("src/saved_images.txt", imageURL + "\n");
        }
        
        return found;
    }
}