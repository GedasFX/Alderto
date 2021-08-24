using System.Collections.Generic;
using Alderto.Data.Models;

namespace Alderto.Domain.Models
{
    public class GuildSetup
    {
        public GuildConfiguration Configuration { get; }
        public Dictionary<string, string>? Aliases { get; }

        public GuildSetup(GuildConfiguration configuration, Dictionary<string, string>? aliases = null)
        {
            Configuration = configuration;
            Aliases = aliases;
        }
    }
}
