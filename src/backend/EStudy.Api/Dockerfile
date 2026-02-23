# Estágio de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
# A porta padrão do .NET 10 é 8080 para usuários não-root
EXPOSE 8080
EXPOSE 8081
USER app

# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia apenas o .csproj primeiro para aproveitar o cache do Docker (Restore)
COPY ["src/backend/EStudy.Api/EStudy.Api.csproj", "backend/EStudy.Api/"]
RUN dotnet restore "backend/EStudy.Api/EStudy.Api.csproj"

# Copia o restante dos arquivos
COPY . .

# Ajuste do caminho: Entrando na pasta onde o projeto foi copiado
WORKDIR "/src/src/backend/EStudy.Api"

RUN dotnet build "EStudy.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Estágio de Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EStudy.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Estágio Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EStudy.Api.dll"]