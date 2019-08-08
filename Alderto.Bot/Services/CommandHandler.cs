using System;
using System.Reflection;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Bot.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly IGuildPreferencesProvider _guildPreferences;

        public CommandHandler(
            DiscordSocketClient client,
            CommandService commands,
            IServiceProvider services,
            IGuildPreferencesProvider guildPreferences)
        {
            _client = client;
            _commands = commands;
            _services = services;
            _guildPreferences = guildPreferences;
        }

        public async Task StartAsync()
        {
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;
            _client.UserUpdated += (before, after) =>
            {
                if (before.Username != after.Username)
                {
                    Console.WriteLine($"{before.Username} + {after.Username}");
                }
                return Task.CompletedTask;
            };

            // Here we discover all of the command modules in the entry 
            // assembly and load them. Starting from Discord.NET 2.0, a
            // service provider is required to be passed into the
            // module registration method to inject the 
            // required dependencies.
            //
            // If you do not use Dependency Injection, pass null.
            // See Dependency Injection guide for more information.
            using (var scope = _services.CreateScope())
            {
                await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), scope.ServiceProvider);
            }
            
        }
        
        public async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            if (!(messageParam is SocketUserMessage message))
                return;

            // Create a number to track where the prefix ends and the command begins
            var argPos = 0;

            // Check if message was sent in a guild context. If not - ignore.
            // TODO: Make a redirect messages channel and maybe add command handler to DM
            if (!(message.Author is SocketGuildUser guildUser))
                return;

            // Get the prefix preference of a guild (if applicable)
            var prefix = (await _guildPreferences.GetPreferencesAsync(guildUser.Guild.Id)).Prefix;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasStringPrefix(prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)) || message.Author.IsBot)
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.

            // Create a scope to prevent leaks.
            using (var scope = _services.CreateScope())
            {
                // TODO: Upgrade this to RunMode.Async

                // Keep in mind that result does not indicate a return value
                // rather an object stating if the command executed successfully.
                var result = await _commands.ExecuteAsync(context, argPos, scope.ServiceProvider);

                // Delete successful triggers.
                if (result.IsSuccess)
                {
                    try
                    {
                        await message.DeleteAsync();
                    }
                    catch (Discord.Net.HttpException)
                    {
                        // Delete most likely failed due to no ManageMessages permission. Ignore regardless.
                    }
                }

                // Optionally, we may inform the user if the command fails
                // to be executed; however, this may not always be desired,
                // as it may clog up the request queue should a user spam a
                // command.

                else if (result.Error != CommandError.UnknownCommand)
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
        }
    }
}