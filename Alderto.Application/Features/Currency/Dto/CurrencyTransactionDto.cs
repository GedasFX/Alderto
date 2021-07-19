using System;
using System.Collections.Generic;

namespace Alderto.Application.Features.Currency.Dto
{
    public class CurrencyTransactionDto
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public IList<TransactionEntry> Transactions { get; set; }

        public CurrencyTransactionDto(string name, string symbol, IList<TransactionEntry>? transactions = null)
        {
            Name = name;
            Symbol = symbol;
            Transactions = transactions ?? new List<TransactionEntry>();
        }

        public class TransactionEntry
        {
            public DateTimeOffset Date { get; set; }
            public ulong SenderId { get; set; }
            public ulong RecipientId { get; set; }
            public int Amount { get; set; }
            public bool IsAward { get; set; }
        }
    }
}
