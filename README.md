# Observabilidade Api (.NET 9 + Docker + OpenTelemetry + Elasticsearch)

API RESTful desenvolvida em .NET 9 com foco em rastreabilidade e monitoramento. Utiliza OpenTelemetry para coleta de métricas, logs e traces, enviando os dados para o Elasticsearch via OTEL Collector. Ideal para ambientes distribuídos que exigem visibilidade de ponta a ponta.

---

## 📚 Sumário

- [🛠 Tecnologias](#-tecnologias)
- [🧪 Testes](#-testes)
- [🌱 Convenção de Branches](#-convenção-de-branches)
- [🚀 Como Rodar Localmente](#-como-rodar-localmente)
- [📦 Arquitetura](#-arquitetura)
- [📄 Licença](#-licença)

---

## 🛠 Tecnologias

- [.NET 9](https://dotnet.microsoft.com/)
- [OpenTelemetry](https://opentelemetry.io/)
- [Elasticsearch](https://www.elastic.co/elasticsearch/)
- [Docker](https://www.docker.com/)
- [OTEL Collector](https://opentelemetry.io/docs/collector/)
- [xUnit](https://xunit.net/) *(em breve)*

---

## 🧪 Testes

Este projeto ainda ****não possui testes automatizados****.

A estrutura está preparada para receber testes com ****xUnit**** ou ****MSTest****, e a cobertura será adicionada nas próximas versões.

---

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

---

## 🚀 Como rodar localmente

```bash
# Clonar o repositório
git clone https://github.com/seu-usuario/Observabilidade.Api.git
cd Observabilidade.Api

# Criar um arquivo `.env` na raiz da solução com as variáveis que são utilizadas nos arquivos `.yml`

# Subir os containers
docker-compose up --build

# Acessar a API
http://localhost:8080
```

---

## 📦 Arquitetura
A arquitetura da aplicação foi pensada para garantir rastreabilidade completa dos eventos e requisições. Abaixo, um panorama simplificado:

```text
[Usuário] → [Observabilidade.Api] → [OpenTelemetry SDK] → [OTEL Collector] → [Elasticsearch]
```

### 🔍 Componentes

- ****Observabilidade.Api****: API principal desenvolvida em .NET 9, responsável por gerar logs e traces.
- ****OpenTelemetry SDK****: Instrumenta a aplicação para coletar dados de observabilidade.
- ****OTEL Collector****: Recebe os dados da aplicação e os exporta para destinos como Elasticsearch.
- ****Elasticsearch****: Armazena os logs e traces para análise posterior.
- ****(Opcional) Kibana****: Interface para visualização dos dados armazenados no Elasticsearch.

Essa arquitetura permite monitoramento distribuído, facilitando o diagnóstico de problemas e o entendimento do comportamento da aplicação em tempo real.

---

## 📄 Licença

Este projeto está licenciado sob os termos da ****Licença MIT****. Você pode usar, copiar, modificar, mesclar, publicar, distribuir, sublicenciar e/ou vender cópias do software, desde que preserve o aviso de copyright.

Para mais detalhes, consulte o arquivo [LICENSE](./LICENSE).
