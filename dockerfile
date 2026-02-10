FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["src/AgroSolutions.Usuario.Api/AgroSolutions.Usuario.Api.csproj", "AgroSolutions.Usuario.Api/"]
COPY ["src/AgroSolutions.Usuario.Aplicacao/AgroSolutions.Usuario.Aplicacao.csproj", "AgroSolutions.Usuario.Aplicacao/"]
COPY ["src/AgroSolutions.Usuario.Dominio/AgroSolutions.Usuario.Dominio.csproj", "AgroSolutions.Usuario.Dominio/"]
COPY ["src/AgroSolutions.Usuario.Infra/AgroSolutions.Usuario.Infra.csproj", "AgroSolutions.Usuario.Infra/"]
RUN dotnet restore "AgroSolutions.Usuario.Api/AgroSolutions.Usuario.Api.csproj"
COPY src/ .
WORKDIR "/src/AgroSolutions.Usuario.Api"
RUN dotnet build "AgroSolutions.Usuario.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AgroSolutions.Usuario.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgroSolutions.Usuario.Api.dll"]
