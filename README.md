# Alderto
A discord bot with a Web user interface.

## Disclaimer
This bot is in it's early Alpha stages the entire structure of the bot can change at any minor release.

## Instalation
Requirements:
1) [.Net Core 2.2 Runtime](https://dotnet.microsoft.com/download/dotnet-core/2.2)
2) [Optional] [Node.js version 10.9.0 or later](https://nodejs.org/en/) (for WebUI, currently WIP)

Process:
1) [Download latest release](/GedasFX/Alderto/releases/latest/download/alderto.zip)
2) Extract the folder
3) Find `settings.json` file and fill it out.
   * If you have trouble filling out connection string, this should satisfy most, if not all, of the cases:
   ```
   "DbConnectionString": "Data Source=[Database URL];Database=[Database Name];User ID=[Username];Password=[Password];Connect Timeout=30"
   ```
   * `"DiscordApp:ClientId"` and `"DiscordApp:ClientSecret"` can be found at https://discordapp.com/developers/applications/[Your_Bot_Application_ID]/information
   * `"DiscordApp:BotToken"` can be found at https://discordapp.com/developers/applications/[Your_Bot_Application_ID]/bots
4) Launch the bot using command `dotnet Alderto.Bot.dll`
   
## Development
Requirements: [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) with [.Net Core 2.2 Runtime](https://dotnet.microsoft.com/download/dotnet-core/2.2)

Process:
1) Clone the repository 
2) Launch project in Visual Studio, make sure depenencies are resolved
3) Open up a command terminal and in Alderto.Data folder type `dotnet ef database update`
4) Set up `settings.json` simmilarly like in Installation
