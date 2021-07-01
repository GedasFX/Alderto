using System.Collections.Generic;

namespace Alderto.Application.Features.Bank.Dto
{
    public class BankDto : BankBriefDto
    {
        public ICollection<BankItemDto> Contents { get; set; }

        public BankDto(int id, string name, ICollection<BankItemDto> contents) : base(id, name)
        {
            Id = id;
            Name = name;
            Contents = contents;
        }
    }
}
