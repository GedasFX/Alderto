﻿namespace Alderto.Web.Models.Discord
{
    public class DiscordGuild
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ulong Permissions { get; set; }
        public bool Owner { get; set; }
        public string Icon { get; set; }
    }
}