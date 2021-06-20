using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Domain.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Bot.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _services;
        private readonly IGuildSetupService _guildSetupService;

        public CommandHandler(
            IDiscordClient client,
            CommandService commandService,
            IServiceProvider services,
            IGuildSetupService guildSetupService)
        {
            if (client is not DiscordSocketClient socketClient)
                throw new ArgumentException("Discord client must be of Socket type to use Command Handler.");

            _client = socketClient;
            _commandService = commandService;
            _services = services;
            _guildSetupService = guildSetupService;
        }

        public async Task StartAsync()
        {
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;

            using var scope = _services.CreateScope();
            await _commandService.AddModulesAsync(typeof(CommandHandler).Assembly, scope.ServiceProvider);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            if (messageParam is not SocketUserMessage message)
                return;

            // Check if message was sent in a guild context. If not - ignore.
            if (message.Author is not SocketGuildUser author || message.Author.IsBot)
                return;

            // Fetch the configuration for prefix / command aliases.
            var guildSetup = await _guildSetupService.GetGuildSetupAsync(author.Guild.Id);

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            var argPos = 0;
            if (!(message.HasStringPrefix(guildSetup.Configuration.Prefix, ref argPos) ||
                  message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
                return;

            // Extract the string command from message;
            var cmd = ParseCommand(message.Content, argPos, guildSetup.Aliases);

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command on the Discord API
            using var scope = _services.CreateScope();
            var result = await _commandService.ExecuteAsync(context, cmd, scope.ServiceProvider);

            // Optionally, we may inform the user if the command fails
            // to be executed; however, this may not always be desired,
            // as it may clog up the request queue should a user spam a
            // command.

            if (result.Error != CommandError.UnknownCommand)
            {
                try
                {
                    await context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithDefault(result.ErrorReason, EmbedColor.Error).Build());
                }
                catch (Discord.Net.HttpException e)
                {
                    // 50013 occurs when bot cannot send embedded messages. All error reports use embeds.
                    if (e.DiscordCode == 50013)
                        await context.Channel.SendMessageAsync(
                            "Bot requires guild permission EmbedLinks to function properly.");
                }
            }
        }

        /// <summary>
        /// Parses command, potentially transforming the message into an alias.
        /// </summary>
        private static string ParseCommand(string messageContent, int argPos,
            IReadOnlyDictionary<string, string>? aliasMap)
        {
            var originalCommand = messageContent[argPos..];

            if (aliasMap == null)
                return originalCommand;

            var firstTokenEndIdx = originalCommand.IndexOf(" ", StringComparison.Ordinal);
            var firstToken = firstTokenEndIdx > 0 ? originalCommand[..firstTokenEndIdx] : originalCommand;

            return aliasMap.TryGetValue(firstToken, out var storedCommand)
                ? storedCommand + (firstTokenEndIdx > 0 ? originalCommand[firstTokenEndIdx..] : "")
                : originalCommand;
        }
    }
}