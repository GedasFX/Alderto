export interface IGuild {
  icon: string;
  id: string;
  name: string;
  owner: boolean;
  permissions: number;

  isAdmin: boolean;

  channels: IGuildChannel[];
  roles: IGuildRole[];
}

export interface IGuildChannel {
  id: string;
  name: string;
}

export interface IGuildRole {
  id: string;
  name: string;
}
