# Observabilide Api (.NET 9 + Docker + OpenTelemetry + Elasticsearch)

API RESTful desenvolvida em .NET 9, containerizada com Docker e integrada com OpenTelemetry Collector e Elasticsearch para coleta de logs.

## 🧱 Tecnologias

- ASP.NET Core 9
- OpenTelemetry Collector
- Elasticsearch 8.12
- GitHub Actions CI
- Git Flow 
- Docker + Docker Compose

## 🧪 Testes

Este projeto ainda não possui testes automatizados. A estrutura está preparada para receber testes com xUnit ou MSTest, e a cobertura será adicionada nas próximas versões.

## 🧭 Convenção de Branches

Este projeto segue Git Flow com os seguintes padrões de nomeação:

| Tipo        | Prefixo        | Exemplo                          |
|-------------|----------------|----------------------------------|
| Funcionalidade | `feat/`   | `feat/user-authentication`   |
| Funcionalidade | `feature/`   | `feature/user-authentication`   |
| Correção urgente | `hotfix/` | `hotfix/fix-login-crash`        |
| Correção comum | `bugfix/`    | `bugfix/null-reference-check`   |
| Preparação de release | `release/` | `release/v1.2.0`           |
| Infraestrutura | `chore/`     | `chore/docker-update`           |
| Documentação | `docs/`        | `docs/api-spec-update`          |
| Experimentos | `experiment/`  | `experiment/graphql-prototype`  |

> 🔒 Branches `main` e `develop` são protegidas. Todo código deve passar por PRs com revisão e CI.


## 🚀 Como rodar localmente

```bash
docker-compose up --build
