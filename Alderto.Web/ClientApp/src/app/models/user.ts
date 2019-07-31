export class User {
  public id: number;
  public token?: string;

  public username: string;
  public role: string;

  constructor(id: number = 0, token: string = null, username: string = null, role: string = null) {
    this.id = id;
    this.token = token;
    this.username = username;
    this.role = role;
  }
}
