using Alderto.Data.Models;
using Alderto.Services;
using Alderto.Web.Models.GuildPreferences;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Alderto.Web.Controllers.Guild
{
    [Route("guilds/{guildId}/preferences")]
    public class PreferencesController : ApiControllerBase
    {
        private readonly IGuildPreferencesProvider _guildPreferencesProvider;

        public PreferencesController(IGuildPreferencesProvider guildPreferencesProvider)
        {
            _guildPreferencesProvider = guildPreferencesProvider;
        }

        [HttpGet]
        public async Task<ActionResult<GuildConfiguration>> GetGuildPreferencesAsync(ulong guildId)
        {
            var preferences = await _guildPreferencesProvider.GetPreferencesAsync(guildId);
            return preferences;
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateGuildPreferencesAsync(ulong guildId, GuildPreferencesInputModel model)
        {
            await _guildPreferencesProvider.UpdatePreferencesAsync(guildId, preferences =>
            {
                if (model.Prefix != null)
                {
                    preferences.Prefix = model.Prefix;
                }

                if (model.TimelyRewardQuantity != null)
                {
                    preferences.TimelyRewardQuantity = (int)model.TimelyRewardQuantity;
                }

                if (model.TimelyCooldown != null)
                {
                    preferences.TimelyCooldown = (int)model.TimelyCooldown;
                }

                if (model.CurrencySymbol != null)
                {
                    preferences.CurrencySymbol = model.CurrencySymbol;
                }

                if (model.AcceptedMemberRoleId != null)
                {
                    preferences.AcceptedMemberRoleId = (ulong)model.AcceptedMemberRoleId;
                }
            });

            return Ok();
        }
    }
}
