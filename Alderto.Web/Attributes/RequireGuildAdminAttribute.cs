using System;

namespace Alderto.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireGuildAdminAttribute : Attribute
    {
    }
}
