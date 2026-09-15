# RoyalGamesApplication

Plataforma de e-commerce e catálogo de jogos eletrônicos construída com arquitetura fullstack moderna, backend em **C# .NET 8 Web API**, banco de dados relacional **PostgreSQL**, e frontend em **React / Next.js (TypeScript)**, totalmente containerizada para deploy no **Easypanel** ou qualquer ambiente Docker.

---

## 🚀 Tecnologias

- **Backend**: C# .NET 8, ASP.NET Core Web API, Entity Framework Core 8, Npgsql (PostgreSQL), JWT Authentication, Swagger/OpenAPI.
- **Frontend**: Next.js 16 (Pages Router), React 19, TypeScript, Axios, React-Toastify, Material UI (Paginação), CSS Modules (estilização Cyberpunk).
- **Banco de Dados**: PostgreSQL 16 (com triggers de exclusão lógica e log de alterações, e auto-seed na inicialização).
- **DevOps**: Docker, Docker Compose, pronto para Easypanel / Railway / Render.

---

## 📁 Estrutura do Projeto

```text
RoyalGames/
├── backend/                       # API C# .NET 8
│   ├── Aplications/               # DTOs, Services, Regras de Negócio e DbInitializer
│   ├── Contexts/                  # DbContext do Entity Framework Core
│   ├── Controllers/               # Endpoints REST (Jogos, Usuários, Gêneros, etc.)
│   ├── Domains/                   # Modelos e Entidades de Domínio
│   ├── Dockerfile                 # Multi-stage build .NET 8
│   └── Program.cs                 # Configuração de serviços, CORS, JWT e Swagger
├── frontend/                      # Aplicação Web Next.js / React
│   ├── public/                    # Assets públicos (imagens dos jogos, logos, banners)
│   ├── src/
│   │   ├── components/            # Header, Footer, Card, Lista de Jogos, Paginação
│   │   ├── pages/                 # Rotas: /home, /catalogo, /detalhe-produto/[id], /cadastro-jogo, /login
│   │   ├── services/              # Integração Axios tipada (API, Auth, Jogos, Plataformas)
│   │   └── utils/                 # Utilitários de autenticação, formatação e notificações
│   └── Dockerfile                 # Multi-stage build Next.js (Node 20 Alpine)
├── SQL/                           # Scripts DDL/DML para PostgreSQL e SQL Server
│   └── RoyalGamesPostgres.sql     # Script completo para PostgreSQL
├── docker-compose.yml             # Orquestração local e para Easypanel
└── README.md
```

---

## ⚡ Como Executar Localmente com Docker Compose

Com o [Docker Desktop](https://www.docker.com/) instalado:

1. Na raiz da pasta `RoyalGames`, execute:
```bash
docker compose up -d --build
```

2. Acesse os serviços nos links:
- **Frontend**: [http://localhost:3000](http://localhost:3000)
- **Backend (Swagger)**: [http://localhost:5000/swagger](http://localhost:5000/swagger)
- **Health Check da API**: [http://localhost:5000/health](http://localhost:5000/health)
- **PostgreSQL**: Porta `5432` (Usuário: `postgres`, Senha: `postgres`, Banco: `royal_games`)

---

## 🔑 Credenciais Padrão (Seed Inicial)

O banco de dados semeia automaticamente na primeira inicialização:
- **Email do Administrador**: `carlos@vhburguer.com`
- **Senha**: `admin@123`

---

## ☁️ Deploy no Easypanel

### Método 1: Via Docker Compose no Easypanel (Recomendado)
1. No seu Easypanel, clique em **+ Project** e crie um projeto (ex: `royal-games`).
2. Adicione um novo serviço do tipo **App** -> selecione **Docker Compose**.
3. Conecte ao repositório GitHub `https://github.com/Luis9768/RoyalGamesApplication.git`.
4. O Easypanel detectará o `docker-compose.yml` e subirá o banco PostgreSQL, a API backend e o frontend automaticamente.
5. Nas configurações de domínio do Easypanel:
   - Aponte seu domínio principal para o serviço `frontend` (porta `3000`).
   - (Opcional) Aponte um subdomínio (ex: `api.seudominio.com`) para o serviço `backend` (porta `8080`).

### Método 2: Como Serviços Separados no Easypanel
1. **Banco de Dados**: Crie um serviço **Postgres** no Easypanel (gerará a variável `DATABASE_URL`).
2. **Backend**:
   - Tipo de Serviço: **App** -> Conectar ao Git `https://github.com/Luis9768/RoyalGamesApplication.git`.
   - Build Path / Context: `./backend`.
   - Port: `8080`.
   - Variáveis de Ambiente:
     - `DATABASE_URL`: `${postgres.DATABASE_URL}` (ou a connection string do PostgreSQL criado).
     - `ASPNETCORE_ENVIRONMENT`: `Production`.
     - `Jwt__Key`: `SENHA_SUPER_SECRETA_CRIACAO_DA_CHAVE_JWT_32`.
     - `Jwt__Issuer`: `RoyalGamessApi`.
     - `Jwt__Audience`: `RoyalGamessFront`.
     - `Jwt__ExpireEmMinutos`: `120`.
3. **Frontend**:
   - Tipo de Serviço: **App** -> Conectar ao Git `https://github.com/Luis9768/RoyalGamesApplication.git`.
   - Build Path / Context: `./frontend`.
   - Port: `3000`.
   - Build Args / Variáveis de Ambiente:
     - `NEXT_PUBLIC_API_URL`: URL pública da sua API (ex: `https://api.seudominio.com/api` ou `http://backend:8080/api`).

---

## 🛠️ Execução em Desenvolvimento (Sem Docker)

### Backend:
```bash
cd backend
dotnet run
```
*A API subirá com Swagger configurado e criará o schema no PostgreSQL definido no `appsettings.json`.*

### Frontend:
```bash
cd frontend
npm install
npm run dev
```
*O frontend subirá em [http://localhost:3000](http://localhost:3000).*
