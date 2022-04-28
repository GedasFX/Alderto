# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

RUN apt update && \
    curl -fsSL https://deb.nodesource.com/setup_12.x | bash - && \
    apt install -y nodejs

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Alderto.Web/*.csproj ./Alderto.Web/
COPY Alderto.Bot/*.csproj ./Alderto.Bot/
COPY Alderto.Data/*.csproj ./Alderto.Data/
COPY Alderto.Tests/*.csproj ./Alderto.Tests/
COPY Alderto.Bot.Lua/*.csproj ./Alderto.Bot.Lua/
COPY Alderto.Services/*.csproj ./Alderto.Services/
COPY AspNet.Security.OAuth.Discord/*.csproj ./AspNet.Security.OAuth.Discord/
RUN dotnet restore

# copy everything else and build app
COPY . .
WORKDIR /source/Alderto.Web 
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Alderto.Web.dll"]