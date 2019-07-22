using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class CustomCommand
    {
        /// <summary>
        /// Trigger keyword. First word to be triggered on ".cc trigger args..." command.
        /// </summary>
        [Key]
        [MaxLength(20)]
        public string TriggerKeyword { get; set; }

        /// <summary>
        /// Guild Id to which this command is registered to.
        /// </summary>
        [Key]
        public ulong GuildId { get; set; }

        /// <summary>
        /// Command's code. Does not include the function header and end. Starts in body's first line.
        /// </summary>
        /// <example>
        /// if (args[0] == '_<see cref="GuildId"/>_<see cref="TriggerKeyword"/>') then
        ///     arg1 = tonumber(args[1])
        ///     if (arg1 &gt; tonumber(args[2])) then
        ///         return arg1
        ///     else
        ///         return args[2]
        ///     end
        /// else 
        ///     return 3
        /// end
        /// </example>
        [MaxLength(2000)]
        public string LuaCode { get; set; }

        /// <summary>
        /// <see cref="Guild"/>, which owns and can run this command.
        /// </summary>
        [ForeignKey(nameof(GuildId))]
        public virtual Guild Guild { get; set; }

        /// <summary>
        /// Initializes a new empty instance of <see cref="CustomCommand"/>.
        /// </summary>
        public CustomCommand()
        { 
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CustomCommand"/>, with primary key (<see cref="GuildId"/> and <see cref="TriggerKeyword"/>) set.
        /// </summary>
        /// <param name="guildId"><see cref="GuildId"/> property.</param>
        /// <param name="triggerKeyword"><see cref="TriggerKeyword"/> property.</param>
        public CustomCommand(ulong guildId, string triggerKeyword)
        {
            GuildId = guildId;
            TriggerKeyword = triggerKeyword;
        }
    }
}
