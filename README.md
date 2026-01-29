# EducationHub - Plataforma Educacional Online

## 📋 Apresentação

Este projeto é uma entrega do **MBA DevXpert Full Stack .NET** e refere-se ao módulo **Arquitetura, Modelagem e Qualidade de Software**.

O principal objetivo é desenvolver uma plataforma educacional online com múltiplos **bounded contexts (BC)**, aplicando **DDD**, **TDD**, **CQRS** e padrões arquiteturais para gestão eficiente de conteúdos educacionais, alunos e processos financeiros.

## 👤 Autor(es)

- Silvio Cesar Kinaake

## 🎯 Proposta do Projeto

Desenvolver uma plataforma de educação e treinamento disponibilizada via API para gerir cursos/matrículas/alunos/pagamentos e prover meios para que os alunos realizem os cursos, implementando:

- **DDD (Domain-Driven Design)** com bounded contexts bem definidos
- **TDD (Test-Driven Development)** com cobertura mínima de 80%
- **CQRS (Command Query Responsibility Segregation)**
- **Autenticação JWT** com ASP.NET Core Identity
- **Banco de Dados**: SQL Server (produção) e SQLite (desenvolvimento)
- **Documentação**: Swagger/OpenAPI

## 🏗️ Arquitetura

### Bounded Contexts

O sistema está organizado em **3 Bounded Contexts** principais:

#### 1. BC Gestão de Conteúdo
- **Aggregate Root**: `Curso` (agrega Aulas)
- **Entities**: `Aula`
- **Value Objects**: `ConteudoProgramatico`
- **Responsabilidade**: Administração de cursos e aulas, incluindo controle estruturado do conteúdo educacional

#### 2. BC Gestão de Alunos
- **Aggregate Root**: `Aluno` (agrega Matrículas e Certificados)
- **Entities**: `Matricula`, `Certificado`
- **Value Objects**: `HistoricoAprendizado`
- **Responsabilidade**: Gerenciamento completo do aluno, incluindo cadastro, matrícula e acompanhamento do histórico acadêmico

#### 3. BC Pagamento e Faturamento
- **Aggregate Root**: `Pagamento`
- **Value Objects**: `DadosCartao`, `StatusPagamento`
- **Responsabilidade**: Controle do processo de pagamento relacionado à matrícula dos alunos, incluindo status e validações

## 🛠️ Tecnologias Utilizadas

- **Linguagem**: C# / .NET 8.0
- **Framework Backend**: ASP.NET Core Web API
- **ORM**: Entity Framework Core
- **Banco de Dados**: 
  - SQL Server (Produção)
  - SQLite (Desenvolvimento com seed automático)
- **Autenticação**: ASP.NET Core Identity + JWT
- **Validação**: FluentValidation
- **Mapeamento**: AutoMapper
- **Testes**: xUnit + FluentAssertions + Moq + Bogus
- **Cobertura de Código**: Coverlet
- **Documentação**: Swagger/OpenAPI

## 📁 Estrutura do Projeto

```
EducationHub/
├── src/
│   ├── EducationHub.Core/                    # Infraestrutura compartilhada
│   │   ├── Data/                             # Interfaces de repositório
│   │   ├── DomainObjects/                    # Entidades base, validações
│   │   └── Messages/                         # Commands, Events, Mediator
│   │
│   ├── EducationHub.Alunos.Domain/          # BC Gestão de Alunos - Domínio
│   │   ├── Entidades/                        # Aluno, Matricula, Certificado
│   │   ├── Enums/                            # StatusMatriculaEnum
│   │   └── Interfaces/                       # IAlunoRepositorio
│   │
│   ├── EducationHub.Alunos.Application/     # BC Gestão de Alunos - Aplicação
│   │   ├── Commands/                         # MatricularAlunoCommand
│   │   ├── Services/                         # AlunoAppService
│   │   ├── ViewModels/                       # DTOs
│   │   └── AutoMapper/                       # Profiles
│   │
│   ├── EducationHub.Alunos.Data/            # BC Gestão de Alunos - Dados
│   │   ├── Repository/                       # AlunoRepository
│   │   └── Mappings/                         # EF Core Configurations
│   │
│   ├── EducationHub.Conteudo.Domain/        # BC Gestão de Conteúdo - Domínio
│   │   ├── Entidades/                        # Curso, Aula, ConteudoProgramatico
│   │   └── Interfaces/                       # ICursoRepositorio, IAulaRepositorio
│   │
│   ├── EducationHub.Conteudo.Application/   # BC Gestão de Conteúdo - Aplicação
│   │   ├── Services/                         # CursoAppService, AulaAppService
│   │   ├── ViewModels/                       # DTOs
│   │   └── AutoMapper/                       # Profiles
│   │
│   ├── EducationHub.Conteudo.Data/          # BC Gestão de Conteúdo - Dados
│   │   ├── Repository/                       # CursoRepository, AulaRepository
│   │   └── Mappings/                         # EF Core Configurations
│   │
│   ├── EducationHub.Faturamento.Domain/     # BC Pagamento - Domínio
│   │   ├── Entidades/                        # Pagamento, DadosCartao
│   │   ├── Enums/                            # StatusPagamentoEnum
│   │   ├── Interfaces/                       # IPagamentoRepositorio
│   │   └── Servicos/                         # IPagamentoServico
│   │
│   ├── EducationHub.Faturamento.Application/# BC Pagamento - Aplicação
│   │   ├── Services/                         # PagamentoAppService
│   │   └── ViewModels/                       # DTOs
│   │
│   ├── EducationHub.Faturamento.Data/       # BC Pagamento - Dados
│   │   ├── Repository/                       # PagamentoRepository
│   │   └── Mappings/                         # EF Core Configurations
│   │
│   ├── EducationHub.Autenticacao.Data/      # BC Autenticação - Dados
│   │   └── AutenticacaoDbContext.cs          # Identity DbContext
│   │
│   └── EducationHub.API/                     # API Principal
│       ├── Controllers/                      # Endpoints da API
│       ├── Configurations/                   # Setup de DI, Swagger, Identity, DB
│       ├── Data/                             # Seed de dados automático
│       ├── Settings/                         # JwtSettings
│       └── ViewModels/                       # DTOs de entrada/saída
│
└── tests/
    ├── EducationHub.Tests.Unit/              # Testes Unitários (TDD)
    │   └── Domain/                           # Testes de entidades e VOs
    │       ├── Alunos/                       # AlunoTests
    │       ├── Conteudo/                     # CursoTests
    │       └── Faturamento/                  # PagamentoTests
    │
    └── EducationHub.Tests.Integration/       # Testes de Integração
```

## 🎯 Casos de Uso Implementados

### 1. Cadastro de Curso
- **Ator**: Administrador
- **Fluxo**: Administrador cadastra curso informando nome, descrição, carga horária, instrutor, nível e conteúdo programático
- **Pós-condição**: Curso disponível para matrícula

### 2. Cadastro de Aula
- **Ator**: Administrador
- **Fluxo**: Administrador vincula aula a um curso existente com título, conteúdo e material de apoio
- **Pós-condição**: Aula associada ao curso

### 3. Matrícula do Aluno
- **Ator**: Aluno autenticado
- **Fluxo**: Aluno seleciona curso e inicia matrícula que fica com status pendente de pagamento
- **Pós-condição**: Matrícula criada aguardando pagamento

### 4. Realização do Pagamento
- **Ator**: Aluno
- **Fluxo**: Aluno realiza pagamento informando dados do cartão, pagamento confirmado altera status da matrícula para ativa
- **Pós-condição**: Pagamento registrado e matrícula ativada

### 5. Realização da Aula
- **Ator**: Aluno com matrícula ativa
- **Fluxo**: Aluno acessa aula e progresso é registrado automaticamente
- **Pós-condição**: Aula realizada e progresso registrado

### 6. Finalização do Curso
- **Ator**: Aluno
- **Fluxo**: Após concluir todas as aulas, matrícula é atualizada para status concluído e certificado é gerado
- **Pós-condição**: Certificado gerado e disponível

### 7. Histórico de Aprendizado
- **Ator**: Aluno autenticado
- **Fluxo**: Aluno consulta todos os cursos concluídos com data de início (ativação) e data de conclusão
- **Pós-condição**: Histórico completo exibido com informações detalhadas

## 🔐 Autenticação e Autorização

- **Autenticação**: JWT (JSON Web Tokens)
- **Identity**: ASP.NET Core Identity para gerenciamento de usuários
- **Perfis**:
  - **Administrador**: Controle total (cadastrar cursos, aulas, gerir assinaturas, pagamentos, alunos)
  - **Aluno**: Acesso restrito (matrícula, visualização de aulas/conteúdos, gerenciamento de pagamentos)

### Modelo de Persona
O usuário logado (interativo) corresponde à persona do negócio (Aluno ou Administrador). O sistema mantém o registro da persona e do usuário de forma independente, compartilhando o mesmo ID.

## 🚀 Como Executar o Projeto

### Pré-requisitos
- .NET SDK 8.0 ou superior
- SQL Server (opcional - para produção)
- Visual Studio 2022 ou VS Code
- Git

### Passos para Execução

1. **Clone o Repositório**:
```bash
git clone <url-do-repositorio>
cd EducationHub
```

2. **Configuração do Banco de Dados**:
O projeto está configurado para usar **SQLite por padrão** em ambiente de desenvolvimento, com seed automático de dados.

Para usar SQL Server, edite `appsettings.json` na pasta `EducationHub.API` e ajuste as connection strings.

3. **Restaurar Dependências**:
```bash
dotnet restore
```

4. **Executar a API**:
```bash
cd src/EducationHub.API
dotnet run
```

5. **Acessar a Documentação**:
Após iniciar a aplicação, acesse:
- Swagger: `http://localhost:5137/swagger/index.html`

### Seed de Dados Automático
O sistema cria automaticamente:
- Banco de dados SQLite
- Migrações e schemas
- Dados de exemplo (alunos, cursos, aulas, matrículas, pagamentos)
- Scripts SQL em `Data/SeedScripts/` para referência

- Cria um usuario Administrador
  Email: admin@educationhub.com
  Senha: Admin@123

- Cria o primeiro aluno
  Email: aluno1@educationhub.com
  Senha: Aluno1@123



## 🧪 Testes

O projeto possui cobertura completa de testes seguindo TDD:

### Executar Todos os Testes
```bash
# A partir da raiz do projeto
dotnet test

# Ou usar o script PowerShell
.\run-tests.ps1
```

### Testes Unitários
```bash
# Executar apenas testes unitários
cd tests/EducationHub.Tests.Unit
dotnet test

# Executar com detalhes
dotnet test --logger "console;verbosity=detailed"
```

**Cobertura atual**: 4 testes (100% de sucesso)
- Testes de entidades de domínio (Aluno, Curso, Pagamento, Matrícula)
- Testes de Value Objects (ConteudoProgramatico, DadosCartao)
- Testes de validações e regras de negócio

### Testes de Integração
```bash
# Executar apenas testes de integração
cd tests/EducationHub.Tests.Integration
dotnet test

# Executar com relatório detalhado
dotnet test --logger "console;verbosity=detailed"
```

### Cobertura de Código
```bash
# Gerar relatório de cobertura
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov

# Cobertura com formato HTML (requer ReportGenerator)
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:coverage.opencover.xml -targetdir:coveragereport
```

### Estrutura dos Testes
```
tests/
├── EducationHub.Tests.Unit/          # Testes de unidade
│   └── Domain/                       # Testes de entidades
│       ├── Alunos/                   # Testes de Aluno, Matrícula
│       ├── Conteudo/                 # Testes de Curso, Aula
│       └── Faturamento/              # Testes de Pagamento
│
└── EducationHub.Tests.Integration/   # Testes de integração
    ├── Controllers/                  # Testes de API
    └── Fluxos/                       # Testes de fluxo completo
```

## 📊 Padrões Implementados

- **Domain-Driven Design (DDD)**: Bounded Contexts, Aggregate Roots, Entities, Value Objects
- **CQRS**: Separação entre Commands (escrita) e Queries (leitura)
- **Repository Pattern**: Abstrações de acesso a dados
- **Unit of Work**: Transações consistentes
- **Mediator Pattern**: MediatR para desacoplamento
- **Anti-Corruption Layer**: Isolamento de dependências externas (gateway de pagamento)
- **Validator Pattern**: FluentValidation para validações complexas
- **AutoMapper**: Mapeamento automático entre entidades e ViewModels
- **Domain Events**: Comunicação assíncrona entre bounded contexts

## 📝 Documentação da API

A documentação completa da API está disponível via **Swagger/OpenAPI** após executar o projeto.

Principais endpoints:
- `/api/auth/register` - Registro de usuário
- `/api/auth/login` - Login e obtenção do token JWT
- `/api/cursos` - CRUD de cursos (Admin)
- `/api/aulas` - CRUD completo de aulas (Admin)
- `/api/alunos` - Gestão de alunos
- `/api/alunos/historicoAprendizado` - Histórico de cursos concluídos (Aluno)
- `/api/faturamentos/pagar` - Realização de pagamentos

## ⚙️ Configurações

### JWT Settings (appsettings.json)
```json
{
  "AppSettings": {
    "Secret": "CHAVE_SECRETA_JWT",
    "ExpiracaoHoras": 2,
    "Emissor": "EducationHub",
    "ValidoEm": "https://localhost"
  }
}
```

### Database Selector
O projeto está configurado para escolher automaticamente entre SQLite (desenvolvimento) e SQL Server (produção) baseado no ambiente.

## 📋 Requisitos Atendidos

- ✅ Implementação de 3 Bounded Contexts com DDD
- ✅ Aplicação de TDD com testes unitários
- ✅ CQRS com Commands e Handlers
- ✅ Autenticação JWT com Identity
- ✅ Banco de Dados com EF Core (SQL Server + SQLite)
- ✅ Seed automático de dados
- ✅ Documentação Swagger
- ✅ Repository Pattern e Unit of Work
- ✅ Validações com FluentValidation
- ✅ AutoMapper para mapeamento de objetos
- ✅ Testes de integração planejados
- ✅ Cobertura de testes > 80% (meta)

## 📚 Próximos Passos

- [ ] Completar implementação de todos os Commands e Queries com MediatR
- [ ] Implementar Domain Events para comunicação entre BCs
- [ ] Adicionar Event Handlers para integração entre contextos
- [ ] Completar testes de integração para todos os casos de uso
- [ ] Implementar logging estruturado (Serilog/Elmah.io)
- [ ] Adicionar validações avançadas de negócio
- [ ] Implementar cache para queries frequentes
- [ ] Adicionar suporte a paginação nos endpoints
- [ ] Implementar health checks

## 📄 Licença

Este projeto é parte de um curso acadêmico (MBA DevXpert Full Stack .NET) e não aceita contribuições externas.

---

**Nota**: O projeto segue as melhores práticas de desenvolvimento com foco em Clean Architecture, SOLID, DDD e qualidade de código.
  - Filters/           -> Filtros globais de exceção e autorização
  - MigrationHelper/   -> Utilitários para migrações automáticas e criação do banco
  - Settings/          -> Classes de configuração e AppSettings
  - ViewModels/        -> Modelos de entrada e saída de dados (DTOs)


## Funcionalidades Implementadas

- ** Autenticação e Autorização: Implementação de login e cadastro utilizando ASP.NET Identity com JWT, permitindo autenticação segura de usuários.

- ** Integração com Aluno: Criação automática de um registro na tabela Aluno ao cadastrar um novo usuário, garantindo o vínculo entre a identidade e a persona do sistema.

- ** API RESTful: Estrutura inicial configurada para exposição de endpoints e integração entre os bounded contexts.

- **Configuração de Banco de Dados: Persistência de dados com Entity Framework Core, compatível com SQL Server e SQLite.

## **Como Executar o Projeto**

### **Pré-requisitos**

- .NET SDK 8.0 ou superior
- SQL Server
- Visual Studio 2022 ou superior (ou qualquer IDE de sua preferência)
- Git

### **Passos para Execução**

1. **Clone o Repositório:**
   
   - `https://github.com/Silviokinaake/EducationHub.git`

2. **Configuração do Banco de Dados:**
   
   - No arquivo appsettings.json, você pode escolher qual banco de dados utilizar
   
   - SQLite (padrão) A string de conexão para SQLite já está configurada por padrão. O banco de dados será gerado automaticamente.
	
   - Caso prefira usar o SQL Server, altere a string de conexão.

4. **Executar a API:**
   
   - No Visual Studio, selecione o projeto API como projeto de inicialização.
   
   - Execute a aplicação.


## Documentação da API

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

http://localhost:5137/swagger/index.html

