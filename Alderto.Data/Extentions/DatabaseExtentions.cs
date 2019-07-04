using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alderto.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Data.Extentions
{
    public static class DatabaseExtentions
    {
        /// <summary>
        /// Adds a member to <see cref="IAldertoDbContext.Members"/> and, if needed, creates a guild in <see cref="IAldertoDbContext.Guilds"/>
        /// </summary>
        /// <param name="context">CbContext containing Members and Guilds</param>
        /// <param name="member">New member to add</param>
        /// <returns></returns>
        public static async Task<Member> AddMemberAsync(this IAldertoDbContext context, Member member)
        {
            var guild = await context.Guilds.FindAsync(member.GuildId);
            
            // Check if guild exists, and if it doesn't - add one.
            // Ohterwise bot will spew relation problems.
            if (guild == null)
            {
                guild = new Guild(member.GuildId);
                await context.Guilds.AddAsync(guild);
            }

            return (await context.Members.AddAsync(member)).Entity;
            

        }
    }
}
