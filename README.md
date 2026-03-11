# EStudy - Ambiente local + Docker

Este projeto ja vem preparado para ler configuracoes via variaveis de ambiente no `EStudy.Api`.

## Variaveis usadas pela API

- `ConnectionStrings__DefaultConnection`
- `POSTGRES_DB`
- `POSTGRES_USER`
- `POSTGRES_PASSWORD`
- `BackendUrl`
- `FrontendUrl`
- `ASPNETCORE_ENVIRONMENT`
- `Settings__Jwt__SigningKey`
- `Settings__Jwt__Issuer`
- `Settings__Jwt__Audience`
- `Settings__Jwt__ExpirationTimeMinutes`
- `Settings__IdCryptographyAlphabet`

## Rodar local (IDE/Rider ou dotnet run)

Os perfis de `src/backend/EStudy.Api/Properties/launchSettings.json` incluem apenas variaveis nao sensiveis para desenvolvimento.

Para segredo local (connection string), use User Secrets:

```powershell
dotnet user-secrets init --project .\src\backend\EStudy.Api\EStudy.Api.csproj
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=SEU_DB;Username=SEU_USER;Password=SUA_SENHA" --project .\src\backend\EStudy.Api\EStudy.Api.csproj
dotnet user-secrets set "Settings:Jwt:SigningKey" "SUA_CHAVE_FORTE_MIN_32_CHARS" --project .\src\backend\EStudy.Api\EStudy.Api.csproj
dotnet user-secrets set "Settings:Jwt:Issuer" "EStudy.Api" --project .\src\backend\EStudy.Api\EStudy.Api.csproj
dotnet user-secrets set "Settings:Jwt:Audience" "EStudy.Client" --project .\src\backend\EStudy.Api\EStudy.Api.csproj
dotnet user-secrets set "Settings:Jwt:ExpirationTimeMinutes" "60" --project .\src\backend\EStudy.Api\EStudy.Api.csproj
dotnet user-secrets set "Settings:IdCryptographyAlphabet" "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" --project .\src\backend\EStudy.Api\EStudy.Api.csproj
```

Se quiser sobrescrever pela sessao do PowerShell:

```powershell
$env:ASPNETCORE_ENVIRONMENT="Development"
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=SEU_DB;Username=SEU_USER;Password=SUA_SENHA"
$env:BackendUrl="http://localhost:5016"
$env:FrontendUrl="http://localhost:5173"
$env:Settings__Jwt__SigningKey="SUA_CHAVE_FORTE_MIN_32_CHARS"
$env:Settings__Jwt__Issuer="EStudy.Api"
$env:Settings__Jwt__Audience="EStudy.Client"
$env:Settings__Jwt__ExpirationTimeMinutes="60"
$env:Settings__IdCryptographyAlphabet="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"

dotnet run --project .\src\backend\EStudy.Api\EStudy.Api.csproj
```

## Rodar com Docker Compose

1. Copie o arquivo de exemplo:

```powershell
Copy-Item .env.example .env
```

2. Ajuste os valores no `.env` se necessario (`ConnectionStrings__DefaultConnection`, `Settings__Jwt__SigningKey` e `Settings__IdCryptographyAlphabet`).

Para Docker Compose, use `ConnectionStrings__DefaultConnection` com host `estudy-db`.
Exemplo: `Host=estudy-db;Port=5432;Database=estudy;Username=estudy;Password=...`

Observacao: `.env` esta no `.gitignore`, entao os segredos nao sobem para o repositorio.

3. Suba os containers:

```powershell
docker compose up --build
```

A API ficara exposta em `http://localhost:5000` (porta mapeada para `8080` no container).

