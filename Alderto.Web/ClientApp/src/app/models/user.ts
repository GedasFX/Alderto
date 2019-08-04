export class User {
  public id: number;
  public token: string;
  public discordToken: string;

  public username: string;
  public role: string;

  constructor(id: number = 0, token: string = null, discordToken = null, username: string = null, role: string = null) {
    this.id = id;
    this.token = token;
    this.discordToken = discordToken;
    this.username = username;
    this.role = role;
  }
}
