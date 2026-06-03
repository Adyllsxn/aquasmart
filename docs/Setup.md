# 🔧 Setup - Aquasmart

[![GitHub](https://img.shields.io/badge/github-aquasmart-181717?style=flat&logo=github)](https://github.com/Adyllsxn/aquasmart)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?style=flat&logo=postgresql)](https://www.postgresql.org/)

---

## 📋 Pré-requisitos

- [.NET SDK 10.0](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/)
- [PostgreSQL](https://www.postgresql.org/download/) (16 ou superior)

---

## 🚀 Instalação

### 1. Clonar o repositório

```bash
git clone https://github.com/Adyllsxn/aquasmart.git
cd aquasmart
```
### 2. Configurar

#### 2.1 Configurar a connection string

Edite o arquivo src/apps/Aquasmart.Server/appsettings.Development.json:
```bash
{
  "ConnectionStrings": {
    "postgres": "Host=localhost;Port=5432;Database=db_aquasmart;Username=SEU_USER_AQUI;Password=SEU_PASSWORD_AQUI"
  }
}
```
> ⚠️ Importante: Substitua seu_usuario e sua_senha pelas suas credenciais do PostgreSQL.

#### 2.2 docker-compose
Edite o arquivo `docker-compose.yml` em `src/orchestration/`:

```yaml
services:
  postgres:
    image: postgres:16
    environment:
      POSTGRES_USER: SEU_USER_AQUI
      POSTGRES_PASSWORD: SEU_PASSWORD_AQUI
      POSTGRES_DB: db_aquasmart
```
> ⚠️ A senha do PostgreSQL no Docker deve ser a mesma do appsettings.Development.json

##### 2.2.1 Rodar:
```bash
cd src/orchestration
chmod +x docker-start.sh
./docker-start.sh
```

##### 2.2.2 Acessar 
- Web:	http://localhost:5048
- API:	http://localhost:5047
- Banco:	localhost:5432

### 3. Criar o banco de dados

Entre na pasta do Server:
```bash
cd src/apps/Aquasmart.Server
```

#### Opção 1 - Criar banco manualmente:
```sql
CREATE DATABASE db_aquasmart;
```

#### Opção 2 - Deixar o EF criar:
```bash
dotnet ef database update
```

### 4. Rodar as migrações
```bash
dotnet ef database update
```

### 5. Rodar o projeto API
```bash
cd src/apps/Aquasmart.Server
dotnet run
```


### 6. Rodar o projeto Web
### 6.1. Configurar o base href
> Antes de rodar, edite o arquivo src/apps/Aquasmart.Web/wwwroot/index.html:

#### Para desenvolvimento local:
```bash
<base href="/" />
<!-- <base href="/aquasmart/" /> -->
```

#### Para GitHub Pages:
```bash
<!-- <base href="/" /> -->
<base href="/aquasmart/" />
```
>     ⚠️ Importante: Mantenha base href="/" para rodar localmente.

### 6.2. Executar o projeto
```bash
cd src/apps/Aquasmart.Web
dotnet run
```

### 7. Acessar a aplicação
> PARA API: Abra o navegador em: http://localhost:5205 ou https://localhost:7202

> PARA API COM SCALAR: Abra o navegador em: http://localhost:5205/scalar/ ou https://localhost:7202/scalar/

> PARA WEB: Abra o navegador em: http://localhost:5063 ou https://localhost:7058