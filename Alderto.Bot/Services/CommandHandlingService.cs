﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Alderto.Data;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Bot.Services
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly IAldertoDbContext _context;

        private readonly Dictionary<ulong, char> _guildPrefixes = new Dictionary<ulong, char>();

        public CommandHandlingService(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
            _context = _services.GetService<IAldertoDbContext>();
        }

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;

            // Here we discover all of the command modules in the entry 
            // assembly and load them. Starting from Discord.NET 2.0, a
            // service provider is required to be passed into the
            // module registration method to inject the 
            // required dependencies.
            //
            // If you do not use Dependency Injection, pass null.
            // See Dependency Injection guide for more information.
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            if (!(messageParam is SocketUserMessage message)) return;

            // Create a number to track where the prefix ends and the command begins
            var argPos = 0;

            // Get the prefix preference of a guild (if applicable)
            var prefix = '.';
            if (message.Author is SocketGuildUser guildUser)
            {
                var guildId = guildUser.Guild.Id;
                if (!_guildPrefixes.TryGetValue(guildId, out prefix))
                {
                    var guild = await _context.Guilds.FindAsync(guildId);

                    // If guild is null or its prefix is null do prefix of '.', otherwise use whatever the guild has set.
                    _guildPrefixes[guildId] = guild?.Prefix ?? '.';
                }

            }

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix(prefix, ref argPos) ||
                  message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.

            // Keep in mind that result does not indicate a return value
            // rather an object stating if the command executed successfully.
            var result = await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);

            // Optionally, we may inform the user if the command fails
            // to be executed; however, this may not always be desired,
            // as it may clog up the request queue should a user spam a
            // command.
#if DEBUG
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
#endif
        }
    }
}
