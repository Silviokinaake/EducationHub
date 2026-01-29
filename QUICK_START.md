# EducationHub - Guia Rápido de Execução

## 🚀 Como Executar o Projeto

### 1. Pré-requisitos

- .NET SDK 8.0 ou superior
- Visual Studio 2022, VS Code ou Rider
- Git

### 2. Clonar e Restaurar

```bash
# Clonar o repositório
git clone https://github.com/Silviokinaake/EducationHub
cd EducationHub

# Restaurar dependências
dotnet restore
```

### 3. Executar a API

```bash
# Navegar para o projeto da API
cd src/EducationHub.API

# Executar
dotnet run
```

A API estará disponível em:
- Swagger: `http://localhost:5137/swagger/index.html`

### 4. Banco de Dados

**Não é necessário configurar nada!**

O projeto está configurado para:
- ✅ Usar SQLite automaticamente em desenvolvimento
- ✅ Criar o banco de dados automaticamente
- ✅ Aplicar migrations automaticamente
- ✅ Inserir dados de exemplo automaticamente

Os dados serão criados em:
- Banco: `EducationHub.db` (na pasta da API)
- Scripts: `Data/SeedScripts/` (arquivos .sql gerados automaticamente)

### 5. Testar a API

#### Opção 1: Via Swagger (Recomendado)

1. Acesse: `http://localhost:5137/swagger/index.html`
2. Use o endpoint `/api/auth/register` para criar um usuário
3. Use o endpoint `/api/auth/login` para obter o token
4. Clique em "Authorize" e cole o token
5. Teste os outros endpoints

#### Opção 2: Via cURL

```bash
# Registrar usuário
curl -X POST "http://localhost:5137/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{"email":"teste@email.com","password":"Teste@123","confirmPassword":"Teste@123"}'

# Fazer login
curl -X POST "http://localhost:5137/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"teste@email.com","password":"Teste@123"}'

# Obter cursos (usar o token retornado no login)
curl -X GET "http://localhost:5137/api/cursos" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

### 6. Executar Testes

```bash
# Executar todos os testes
dotnet test

# Executar apenas testes unitários
dotnet test tests/EducationHub.Tests.Unit/EducationHub.Tests.Unit.csproj

# Executar com mais detalhes
dotnet test --verbosity detailed

# Executar testes com cobertura (requer coverlet)
dotnet test /p:CollectCoverage=true
```

**Ou use o script PowerShell:**

```powershell
.\run-tests.ps1
```

## 📊 Dados de Exemplo

O sistema cria automaticamente:

### Cursos
- "Introdução ao C#" - Curso básico completo

### Alunos
- Email: `aluno.teste@example.com`
- Dados completos de matrícula e certificado

### Pagamentos
- Pagamento de exemplo vinculado à matrícula

## 🔐 Autenticação

### Criar Usuário Administrador

```json
POST /api/auth/register
{
  "email": "admin@educationhub.com",
  "password": "Admin@123",
  "confirmPassword": "Admin@123"
}
```

**Nota**: Após criar, você precisa adicionar a role "Administrador" manualmente no banco:

```sql
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id
FROM AspNetUsers u, AspNetRoles r
WHERE u.Email = 'admin@educationhub.com'
AND r.Name = 'Administrador';
```

### Criar Usuário Aluno

```json
POST /api/auth/register
{
  "email": "aluno@educationhub.com",
  "password": "Aluno@123",
  "confirmPassword": "Aluno@123"
}
```

## 📝 Endpoints Principais

### Autenticação
- `POST /api/auth/register` - Registrar novo usuário
- `POST /api/auth/login` - Fazer login (retorna token JWT)

### Cursos (Admin)
- `GET /api/cursos` - Listar todos os cursos
- `GET /api/cursos/{id}` - Obter curso por ID
- `POST /api/cursos` - Criar novo curso (Admin)
- `PUT /api/cursos/{id}` - Atualizar curso (Admin)
- `DELETE /api/cursos/{id}` - Deletar curso (Admin)

### Aulas (Admin)
- `GET /api/aulas` - Listar todas as aulas
- `GET /api/aulas/{id}` - Obter aula por ID
- `POST /api/aulas` - Criar nova aula (Admin)
- `PUT /api/aulas/{id}` - Atualizar aula (Admin)
- `DELETE /api/aulas/{id}` - Deletar aula (Admin)

### Alunos
- `GET /api/alunos` - Listar alunos
- `GET /api/alunos/{id}` - Obter aluno por ID
- `POST /api/alunos` - Criar novo aluno
- `PUT /api/alunos/{id}` - Atualizar aluno

### Pagamentos
- `GET /api/faturamentos` - Listar pagamentos
- `GET /api/faturamentos/{id}` - Obter pagamento por ID
- `POST /api/faturamentos` - Criar novo pagamento

## 🛠️ Configurações

### Alterar para SQL Server

Edite `appsettings.json`:

```json
{
  "DatabaseSelector": {
    "Provider": "SqlServer"
  },
  "ConnectionStrings": {
    "SqlServerConnection": "Server=localhost;Database=EducationHub;Trusted_Connection=True;"
  }
}
```

### Configurar JWT

As configurações JWT estão em `appsettings.json`:

```json
{
  "AppSettings": {
    "Secret": "CHAVE_SECRETA_SUPER_SEGURA_DEVE_TER_PELO_MENOS_32_CARACTERES",
    "ExpiracaoHoras": 2,
    "Emissor": "EducationHub",
    "ValidoEm": "https://localhost"
  }
}
```

## 🐛 Solução de Problemas

### Erro: "Database not found"
- ✅ Reinicie a aplicação - o seed automático criará o banco

### Erro: "Unauthorized"
- ✅ Verifique se o token JWT está no header: `Authorization: Bearer TOKEN`
- ✅ Verifique se o token não expirou (2 horas de validade padrão)

### Erro ao criar curso/aula
- ✅ Certifique-se de estar autenticado como Administrador
- ✅ Verifique os dados obrigatórios no Swagger

### Testes não executam
- ✅ Execute `dotnet restore` na raiz do projeto
- ✅ Certifique-se de que todos os projetos compilam: `dotnet build`

## 📚 Documentação Completa

- [README.md](README.md) - Documentação completa do projeto
- [IMPLEMENTACAO.md](IMPLEMENTACAO.md) - Resumo da implementação
- [GUIA_CONTINUACAO.md](GUIA_CONTINUACAO.md) - Como continuar o desenvolvimento

## ✨ Features Implementadas

- ✅ 3 Bounded Contexts (DDD)
- ✅ 84 Testes Unitários
- ✅ Autenticação JWT
- ✅ Seed Automático
- ✅ Swagger/OpenAPI
- ✅ Repository Pattern
- ✅ Clean Architecture
- ✅ CQRS (parcial)
- ✅ FluentValidation
- ✅ AutoMapper

## 🎯 Status dos Testes

```
Total: 84 testes
✅ Sucesso: 84
❌ Falhas: 0
⚠️ Ignorados: 0
Taxa de Sucesso: 100%
```

---

**Desenvolvido com ❤️ para o MBA DevXpert Full Stack .NET**
