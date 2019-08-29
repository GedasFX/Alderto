export interface IGuildBank {
  id: number;
  guildId: number;
  name: string;
  currencyCount: number;
  logChannelId: string;

  contents: IGuildBankContents[];
}

export interface IGuildBankContents {

}
