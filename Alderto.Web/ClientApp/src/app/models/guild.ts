export interface IGuild {
  icon: string;
  id: string;
  name: string;
  owner: boolean;
  permissions: number;

  isAdmin: boolean;

  channels: IGuildChannel[];
}

export interface IGuildChannel {
  id: string;
  name: string;
}
