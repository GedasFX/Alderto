export interface IGuild {
  icon: string;
  id: string;
  name: string;
  owner: boolean;
  permissions: number;

  channels: IGuildChannel[];
}

export interface IGuildChannel {
  id: string;
  name: string;
}
