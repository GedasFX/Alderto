using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Alderto.Bot.Modules;
using Alderto.Data;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Alderto.Tests.ModulesTests
{
    public class CurrencyModulesTests
    {
        [Fact]
        public async Task Give() => await new CommandService().ExecuteAsync(Dummies.SocketCommandContext, "give 20 ", Dummies.ServiceProvider);
    }
}
