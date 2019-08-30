export interface IGuildBank {
  id: number;
  guildId: string;
  name: string;
  currencyCount: number;
  logChannelId: string;

  contents: IGuildBankContents[];
}

export interface IGuildBankContents {

}

export interface IGuildBankItem {
  id: number;
  guildId: string;
  name: string;
  description: string;
  imageUrl: string;
  value: number;
}
