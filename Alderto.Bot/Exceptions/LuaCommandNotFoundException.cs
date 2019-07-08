using System;

namespace Alderto.Bot.Exceptions
{
    public class LuaCommandNotFoundException : Exception
    {
        public LuaCommandNotFoundException(string message) : base(message) { }
    }
}
