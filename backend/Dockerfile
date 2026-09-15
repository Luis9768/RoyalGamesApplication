# Imagem base para compilação do .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj e restaurar pacotes
COPY ["RoyalGamess.csproj", "./"]
RUN dotnet restore "RoyalGamess.csproj"

# Copiar o restante do código e compilar
COPY . .
RUN dotnet build "RoyalGamess.csproj" -c Release -o /app/build

# Publicação da aplicação
FROM build AS publish
RUN dotnet publish "RoyalGamess.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagem de execução final (ASP.NET 8 Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Porta padrão do .NET 8
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RoyalGamess.dll"]
