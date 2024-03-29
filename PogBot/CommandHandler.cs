﻿using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using Discord;
using Discord.Net;
using Newtonsoft.Json;
using System.Diagnostics;
using Discord.Audio;

namespace PogBot
{
    public class CommandHandler
    {
        private DiscordSocketClient client;
        private CommandService commands;

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            this.commands = commands;
            this.client = client;
        }

        public async Task InstallCommandsAsync()
        {
            client.SlashCommandExecuted += SlashCommandHandler;

            await commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: null);
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case Global.discordCommand:
                    await command.RespondAsync(embed: await HandlePog(command.Data.Options.Count > 0 ? (string)command.Data.Options.First().Value : ""));
                    break;
                case Global.nasaCommand:
                    await command.RespondAsync(embed: await HandleNasa());
                    break;
                case Global.musicCommand:
                    HandleMusic(command);
                    break;
                default:
                    break;
            }

            return;
        }

        [Command("join", RunMode = RunMode.Async)]
        private async Task HandleMusic(SocketSlashCommand command)
        {
            await command.DeferAsync();
            var voiceChannel = (command.User as IGuildUser)?.VoiceChannel;
            if (voiceChannel is null)
            {
                await command.ModifyOriginalResponseAsync(content => content.Embed = new EmbedBuilder().WithTitle("Join a voice channel to play songs").Build());
                return;
            }

            var search = (string)command.Data.Options.First().Value;

            var builder = new EmbedBuilder();
            await command.ModifyOriginalResponseAsync(content => content.Embed = builder.AddField("Search:", search).Build());

            var audioClient = await voiceChannel.ConnectAsync();
            await command.ModifyOriginalResponseAsync(content => content.Embed = builder.AddField("Voice", "CONNECTED").Build());
            //var spotify = new Spotify();
            await Spotify.StartPlayback(audioClient);

            //await voiceChannel.DisconnectAsync();
            return;
        }

        private static async Task<Embed> HandleNasa()
        {
            var result = await NASA.GetAPOD();
            if (result.Item1.Equals("") || result.Item2.Equals("") || result.Item3.Equals(""))
            {
                return new EmbedBuilder()
                    .WithTitle(Global.noImageMessage)
                    .Build();
            }

            return new EmbedBuilder()
                .WithTitle(result.Item1)
                .WithDescription(result.Item2)
                .WithImageUrl(result.Item3)
                .Build();
        }

        private static async Task<Embed> HandlePog(string message)
        {
            var customSearch = message;

            if (customSearch.Equals(Global.cleanSaveCommand))
            {
                File.Delete(Global.saveFile);
                return new EmbedBuilder().WithTitle(Global.deletedSavesMessage).Build();
            }

            var imageSearchResult = await Search.GetImage(customSearch);
            var searchQuery = imageSearchResult.Item1;
            var imageURL = imageSearchResult.Item2;

            if (imageURL.Equals(""))
                imageURL = Global.noImageMessage;
            if (imageURL.Equals(Global.noImageMessage))
                Console.WriteLine(Global.noImageMessage);
            else
                await Search.SaveImageRef(imageURL);

            return new EmbedBuilder()
                .WithTitle(searchQuery)
                .WithImageUrl(imageURL)
                .Build();
        }

        //Should only run when a command has been updated/added
        public async Task BuildCommands()
        {
            Console.WriteLine("Building global slash commands STARTED");

            var commands = new List<ApplicationCommandProperties>();

            var command = new SlashCommandBuilder()
                .WithName(Global.musicCommand)
                .WithDescription("Searches for and plays the requested song")
                .AddOption("search", ApplicationCommandOptionType.String, "Keyword or Spotify link", isRequired: true);
            commands.Add(command.Build());

            var pogCommand = new SlashCommandBuilder()
                .WithName(Global.discordCommand)
                .WithDescription("Replies with a poggers image")
                .AddOption("search", ApplicationCommandOptionType.String, "Keyword", isRequired: false);
            commands.Add(pogCommand.Build());
            
            var nasaCommand = new SlashCommandBuilder()
                .WithName(Global.nasaCommand)
                .WithDescription("Daily APOD from NASA");
            commands.Add(nasaCommand.Build());

            var tasks = new List<Task>();
            foreach (var c in commands)
            {
                tasks.Add(client.CreateGlobalApplicationCommandAsync(c));
            }

            await Task.WhenAll(tasks.ToArray());
            Console.WriteLine("Building global slash commands COMPLETE");
            return;
        }
    }
}
