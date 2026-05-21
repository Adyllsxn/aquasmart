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

### 2. Configurar a connection string

Edite o arquivo src/apps/Aquasmart.Server/appsettings.Development.json:
```bash
{
  "ConnectionStrings": {
    "postgres": "Host=localhost;Port=5432;Database=db_aquasmart;Username=seu_usuario;Password=sua_senha"
  }
}
```
> ⚠️ Importante: Substitua seu_usuario e sua_senha pelas suas credenciais do PostgreSQL.

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
```bash
cd src/apps/Aquasmart.Web
dotnet run
```

### 7. Acessar a aplicação
> PARA API: Abra o navegador em: https://localhost:5205

> PARA WEB: Abra o navegador em: https://localhost:5063