using System.Text.Json.Serialization;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Web.Models.Bank
{
    public sealed class ApiGuildBankItem : GuildBankItem
    {
        [JsonIgnore]
        public override GuildBank? GuildBank {
            get => base.GuildBank;
            set => base.GuildBank = value;
        }

        public ApiGuildBankItem(GuildBankItem item)
            : base(item.Id, item.GuildBankId, item.Name)
        {
            GuildBank = item.GuildBank;

            Quantity = item.Quantity;
            Value = item.Value;

            Description = item.Description;
            ImageUrl = item.ImageUrl;
        }
    }
}
