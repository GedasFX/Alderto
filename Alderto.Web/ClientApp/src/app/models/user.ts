export class User {
  public id: number;
  public token: string;
  public discordToken: string;

  public username: string;
  public role: string;

  constructor(id?: number, token?: string, discordToken?: string, username?: string, role?: string) {
    this.id = id;
    this.token = token;
    this.discordToken = discordToken;
    this.username = username;
    this.role = role;
  }
}
