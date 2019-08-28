export interface IGuildBank {
  id: number;
  guildId: number;
  name: string;
  currencyCount: number;

  contents: IGuildBankContents[];
}

export interface IGuildBankContents {

}
