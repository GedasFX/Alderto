# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Alderto.Web/*.csproj ./Alderto.Web/
COPY Alderto.Bot/*.csproj ./Alderto.Bot/
COPY Alderto.Application/*.csproj ./Alderto.Application/
COPY Alderto.Domain/*.csproj ./Alderto.Domain/
COPY Alderto.Data/*.csproj ./Alderto.Data/
RUN dotnet restore

# copy everything else and build app
COPY . .
WORKDIR /source/Alderto.Web 
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Alderto.Web.dll"]