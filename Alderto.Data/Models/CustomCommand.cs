using System;
using System.ComponentModel.DataAnnotations;

namespace Alderto.Data.Models
{
    public class CustomCommand
    {
        /// <summary>
        /// Unique command identifier.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Guild Id to which this command is registered to.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// Trigger keyword. First word to be triggered on ".cc trigger args..." command.
        /// </summary>
        public string TriggerKeyword { get; set; }

        /// <summary>
        /// Command's code. Does not include the function header. Starts in body's first line.
        /// </summary>
        public string LuaCode { get; set; }

        /// <summary>
        /// Guild which owns and can run this command.
        /// </summary>
        public virtual Guild Guild { get; set; }

    }
}
