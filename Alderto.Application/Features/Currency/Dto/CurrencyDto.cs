using System;

namespace Alderto.Application.Features.Currency.Dto
{
    public class CurrencyDto
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? TimelyInterval { get; set; }
        public int TimelyAmount { get; set; }
        public bool IsLocked { get; set; }

        public CurrencyDto(Guid id, string symbol, string name, string? description, int? timelyInterval, int timelyAmount)
        {
            Id = id;
            Symbol = symbol;
            Name = name;
            Description = description;
            TimelyInterval = timelyInterval;
            TimelyAmount = timelyAmount;
        }
    }
}
