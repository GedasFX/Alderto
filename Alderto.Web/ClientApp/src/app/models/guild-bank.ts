export interface IGuildBank {
  id: number;
  guildId: string;
  name: string;
  currencyCount: number;
  logChannelId: string;
  moderatorRoleId: string;

  canModify: boolean;

  contents: IGuildBankItem[];
}

export interface IGuildBankItem {
  id: number;
  guildBankId: number;
  name: string;
  description: string;
  imageUrl: string;
  value: number;
  quantity: number;
}
