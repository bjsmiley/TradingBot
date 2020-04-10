FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /app

EXPOSE 80
EXPOSE 443

# Copy csproj and restore as distinct layers
#COPY Wallet.API/*.csproj ./Wallet.API/
#WORKDIR /app/Wallet.API
COPY Services/Users/Users.API/Users.API.csproj .
RUN dotnet restore
#WORKDIR /app

# Copy everything else and build
#COPY Wallet.API ./Wallet.API/
COPY Services/Users/Users.API/ .
#WORKDIR /app/Wallet.API
RUN dotnet publish -c release -o /out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /out
COPY --from=build /out ./
ENTRYPOINT ["dotnet", "Users.API.dll"]

