export interface IDiscordUser {
  id: number;
  username: string;
  discriminator: string;
  avatar: string;
  flags: number;
  premium_type: number;
}
