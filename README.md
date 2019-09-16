
# Alderto
A discord accountant bot with a Web user interface.

## Versions:
* Full version
  > Harder to set up, has Web user interface.

* Lite version
  > Faster, less setup time, cannot control it thru Web.

## Instalation
Requirements: [.Net Core 2.2 **Runtime**](https://dotnet.microsoft.com/download/dotnet-core/2.2), [A recent version of PostgreSQL](https://www.postgresql.org/download/) (v11.x works).

Process:
1) Download [latest release](/GedasFX/Alderto/releases/latest/);
2) Extract the folder;
3) Find `appsettings.json` file and fill it out (see Configuration file below);
4) Launch the bot using command `dotnet Alderto.Web.dll` (`dotnet Alderto.Bot.dll` for lite version).
> Based on the configuration of the machine and `appsettings.json`, you may have to run it in administrator mode for port allocation (not applicable to lite version). adding `sudo` in front of the command on Linux would do the trick.
   
### Configuration file
Bot's configuration can be found at `appsettings.json` file.
Tutorial how to set up the configuration:
* Set `JWTPrivateKey` to any base64 string with length of at least 32.
* Set `Database:Host`, `Database:Port`, `Database:Username`, `Database:Password` to your PostgreSQL db connection. `Database:Database` can be set to whatever you like, default is `"Alderto"`. 
* `"DiscordAPI:ClientId"` and `"DiscordAPI:ClientSecret"` can be found at https://discordapp.com/developers/applications/[Your_Bot_Application_ID]/information
* `"DiscordAPI:BotToken"` can be found at https://discordapp.com/developers/applications/[Your_Bot_Application_ID]/bots
* `Discord:ErrorLogChannelId` is the channel where bot will send its error messages. `Discord:NewsChannelId` is the channel where bot will get its news from (not available in lite version). 
 > To get id's for channels and other things, follow [this guide from Discord support](https://support.discordapp.com/hc/en-us/articles/206346498-Where-can-I-find-my-User-Server-Message-ID-).

### Https
>In order to use SSL, you must first have a certificate present on the host device. Big cloud hosting providers, such as AWS of Azure can issue a certificate, otherwise you will have to issue one yourself. [Let's encrypt](https://letsencrypt.org/) is a project that can help you achieve that.

To activate SSL in `appsettings.json` remove the two comments surrounding `Kestrel:Endpoints:Https`:
```
  "Kestrel": {
    "Endpoints": {
      /* If you wish to use SSL, uncomment the part "Https". Requires certificate present on the device. See Kestrel help for more info. */
      /*  <- REMOVE!!!
      "Https": {
        "Url": "https://*"
      },
      */  <- REMOVE!!!
      "Http": {
        "Url": "http://*"
      }
    }
  }
```
## Development
Requirements: [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) with [.Net Core 2.2 **SDK**](https://dotnet.microsoft.com/download/dotnet-core/2.2), [Node.js version 10.9.0 or later](https://nodejs.org/en/) (for WebUI only)

Process:
1) Clone the repository 
1) Launch project in Visual Studio.
  > There are 2 versions of the bot. Lite and Full. To use lite version, select `Alderto.Bot` as startup project. 
  > For full version select `Alderto.Web` as startup project.
3) Set up user secrets with the key `c53fe5d3-16e9-400d-a588-4859345371e5`. Follow [this guide](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2) on how to do this.
1) *[Optional: Full version only] Set up Angular app:
   * Go to folder `Alderto.Web/ClientApp`;
   * Open command line and type the following commands:
     * `npm install`, `npm start`. This will install all required libraries and start the application. Installation is only needed once per update.
       > Note: If `npm` does not work, make sure Node.js is installed properly and `npm` is bound to the PATH variable.
1) Build the project.

### Misc development quirks
Database migrations are applied on launch of application. To create a migration, use this CLI command in the root `Alderto` folder: 
`dotnet ef migrations add MIGRATION_NAME --project Alderto.Data --startup-project Alderto.Web`
