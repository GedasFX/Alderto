export interface IGuildBank {
  id: number;
  guildId: string;
  name: string;
  logChannelId: string;
  moderatorRoleId: string;

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
