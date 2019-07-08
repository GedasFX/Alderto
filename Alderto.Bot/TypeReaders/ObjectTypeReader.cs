using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace Alderto.Bot.TypeReaders
{
    public class ObjectTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services) =>
            Task.FromResult(TypeReaderResult.FromSuccess(input));
    }
}
