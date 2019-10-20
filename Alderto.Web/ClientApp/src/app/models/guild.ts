export interface IGuild {
  icon: string;
  id: string;
  name: string;
  owner: boolean;
  permissions: number;
}

export interface IGuildChannel {
  id: string;
  name: string;
}

export interface IGuildRole {
  id: string;
  name: string;
}
