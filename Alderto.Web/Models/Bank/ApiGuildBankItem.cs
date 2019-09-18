using Alderto.Data.Models.GuildBank;
using Newtonsoft.Json;

namespace Alderto.Web.Models.Bank
{
    public sealed class ApiGuildBankItem : GuildBankItem
    {
        [JsonIgnore]
        public override GuildBank GuildBank {
            get => base.GuildBank;
            set => base.GuildBank = value;
        }

        public ApiGuildBankItem(GuildBankItem guildBankItem)
        {
            GuildBank = guildBankItem.GuildBank;
            Description = guildBankItem.Description;
            GuildBankId = guildBankItem.GuildBankId;
            ImageUrl = guildBankItem.ImageUrl;
            Id = guildBankItem.Id;
            Name = guildBankItem.Name;
            Quantity = guildBankItem.Quantity;
            Value = guildBankItem.Quantity;
        }
    }
}
