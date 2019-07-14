# Alderto
A discord bot with a WebUI

This bot is really Alpha, if, for whatever reason you wish to have it installed, do the following:

Requirements: VisualStudio 2019 with netcore2.2 runtime.

Set-up:
1) Clone the repository 
2) Launch project in Visual Studio, make sure depenencies are resolved.
3) Select debug development mode.
4) Open up a command terminal and in Alderto.Data folder type `dotnet ef database update`
5) Set up secrets, see https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows.
  The seecrets you will need to set up are
    a) For the bot itself: "DiscordApp:BotToken", optionally "DbConnectionString"
    b) For webclient: "DiscordApp:ClientId", "DiscordApp:ClientSecret"
  Can do this step in config.json instead

For Webclient (for now launches as a seperate project):
6) Make sure to have Node.js installed for Angular 8
7) Launch the application in Alderto.Web/ClientApp with `npm run`
8) Launch the Web app
