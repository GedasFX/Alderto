using System;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Bot.Services;
using Discord;
using Discord.Commands;
using NLua;

namespace Alderto.Bot.Modules
{
    [Group("Cc")]
    public class CustomCommandsModule : ModuleBase<SocketCommandContext>
    {
        private readonly CustomCommandsProviderService _cmdProvider;

        public CustomCommandsModule(CustomCommandsProviderService cmdProvider)
        {
            _cmdProvider = cmdProvider;
        }

        [Command]
        public void ExecuteAsync([Remainder] string args)
        {

            
                c.CancelAfter(500);
                await Task.Run(, c.Token);

                var luaFunction = _luaCode["ScriptFunc"] as LuaFunction;
                var r = luaFunction?.Call(5, 2);

                Console.WriteLine(r);
            }
        }
    }
}
