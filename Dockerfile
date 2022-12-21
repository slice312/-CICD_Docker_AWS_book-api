#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["book-app-api.csproj", "book-app-api/"]
RUN dotnet restore "book-app-api/book-app-api.csproj" -r linux-x64

WORKDIR "/src/book-app-api"
COPY . .
RUN dotnet build "book-app-api.csproj" -c Release -o /app/build -r linux-x64

FROM build AS publish
RUN dotnet publish "book-app-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "book-app-api.dll"]