using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Discord;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Services
{
    public class MemberManager : IMemberManager
    {
        private readonly IAldertoDbContext _context;

        public MemberManager(IAldertoDbContext context)
        {
            _context = context;
        }

        
    }
}