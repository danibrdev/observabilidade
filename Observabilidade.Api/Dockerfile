# 🔹 Imagem base para execução
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# 🌐 Declarar portas padrão do ASP.NET
EXPOSE 8080
#EXPOSE 8081

# 🔹 Imagem de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# 📁 Diretório onde fica o código para ser usado de base no build e no publish
WORKDIR /Observabilidade.Api

# Copiar csproj e restaurar
COPY ["Observabilidade.Api.csproj", "."]

# O token não fica nas camadas da imagem
# 🔹 Restaurar pacotes privados usando BuildKit secret
# Restaurar com sources adicionadas on-the-fly, usando secret
RUN --mount=type=secret,id=github_token,target=/run/secrets/.github_token \
    DOTNET_CLI_HOME=/tmp \
    GITHUB_TOKEN=$(cat /run/secrets/.github_token) && \
    rm -f /tmp/NuGet.config && \
    dotnet nuget remove source github || true && \
    dotnet nuget remove source nuget.org || true && \
    dotnet nuget add source --name github "https://nuget.pkg.github.com/danibrdev/index.json" --username danibrdev --password "$GITHUB_TOKEN" --store-password-in-clear-text && \
    dotnet nuget add source --name nuget.org "https://api.nuget.org/v3/index.json" && \
    dotnet restore "Observabilidade.Api.csproj"

# ⚙️ Configuração de build
ARG BUILD_CONFIGURATION=Release

# 📦 Copiar restante do projeto para /src e compilar
COPY . .
RUN dotnet build "Observabilidade.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 🔹 Publicação
FROM build AS publish
RUN dotnet publish "Observabilidade.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 🔹 Imagem final
FROM base AS final

# 📁 Define o diretório de trabalho dentro do container para /app
WORKDIR /app

# 📦 Copia os arquivos publicados da etapa 'publish' para o diretório atual (/app)
COPY --from=publish /app/publish .

# 🌐 Definir variáveis de ambiente para escuta nas portas
ENV ASPNETCORE_HTTP_PORTS=8080

# 🚀 Comando de inicialização
# Define o comando que será executado ao iniciar o container:
# roda a aplicação .NET usando o runtime instalado
ENTRYPOINT ["dotnet", "Observabilidade.Api.dll"]