using System;

namespace Alderto.Web.Models
{
    public class ApiLeaderboardEntry
    {
        public ulong MemberId { get; set; }
        public string? Name { get; set; }
        public int Points { get; set; }
        public DateTimeOffset LastClaimed { get; set; }
    }
}
