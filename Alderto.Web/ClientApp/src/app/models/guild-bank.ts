export interface IGuildBank {
  id: number;
  guildId: number;
  name: string;
  currencyCount: number;

  contents: IGuildBankContents[];
  transactions: IGuildBankTransaction[];
}

export interface IGuildBankContents {
  
}

export interface IGuildBankTransaction {

}
