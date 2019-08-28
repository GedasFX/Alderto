export interface IGuild {
  icon: string;
  id: string;
  name: string;
  owner: boolean;
  permissions: number;

  mutual: boolean;

  channels: IGuildChannel[];
}

export interface IGuildChannel {
  id: string;
  name: string;
}
