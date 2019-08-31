export interface IGuildBank {
  id: number;
  guildId: string;
  name: string;
  currencyCount: number;
  logChannelId: string;

  contents: IGuildBankItem[];
}

export interface IGuildBankItem {
  id: number;
  guildId: string;
  name: string;
  description: string;
  imageUrl: string;
  value: number;
  quantity: number;
}
