using System;

namespace Alderto.Bot.Lua.Exceptions
{
    public class LuaCommandNotFoundException : Exception
    {
        public LuaCommandNotFoundException(string message) : base(message) { }
    }
}
