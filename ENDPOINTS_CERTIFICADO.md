# Endpoints de Conclusão de Matrícula e Emissão de Certificado

## Resumo da Implementação

Implementados dois novos endpoints no contexto de Alunos para gerenciar o fluxo completo de conclusão de curso:

### 1. Concluir Matrícula
**Endpoint:** `POST /api/Alunos/matriculas/{matriculaId}/concluir`

**Funcionalidade:** Altera o status da matrícula de "Ativa" para "Concluída"

**Regras de Negócio:**
- ✅ Somente matrículas com status `Ativa` podem ser concluídas
- ✅ Matrícula deve existir no sistema
- ✅ Validação no domain layer (método `Matricula.Concluir()`)
- ✅ Dispara evento `CursoConcluidoEvent`

**Request:**
```http
POST http://localhost:5137/api/Alunos/matriculas/{matriculaId}/concluir
Authorization: Bearer {token}
```

**Response Success (200):**
```json
{
  "message": "Matrícula concluída com sucesso.",
  "matriculaId": "guid-da-matricula"
}
```

**Response Error (400):**
```json
{
  "message": "Somente matrículas ativas podem ser concluídas."
}
```

---

### 2. Emitir Certificado
**Endpoint:** `POST /api/Alunos/matriculas/{matriculaId}/certificado`

**Funcionalidade:** Emite certificado digital para matrícula concluída

**Regras de Negócio:**
- ✅ Somente matrículas com status `Concluída` podem gerar certificado
- ✅ Não permite emissão duplicada (verifica se já existe certificado para a matrícula)
- ✅ Gera código único de verificação no formato: `{yyyyMMddHHmmss}-{8digitos}`
- ✅ Retorna mensagem formatada do certificado

**Request:**
```http
POST http://localhost:5137/api/Alunos/matriculas/{matriculaId}/certificado
Authorization: Bearer {token}
```

**Response Success (200):**
```json
{
  "certificadoId": "guid-do-certificado",
  "codigo": "20250126142530-A1B2C3D4",
  "dataEmissao": "2025-01-26T14:25:30.123Z",
  "mensagemCertificado": "\n    ╔═══════════════════════════════════════════════╗\n    ║         CERTIFICADO DE CONCLUSÃO              ║\n    ╚═══════════════════════════════════════════════╝\n    \n    Certificamos que\n        JOÃO DA SILVA\n    concluiu com êxito o curso\n        ASP.NET Core Avançado\n    com carga horária de 40 horas\n    \n    Data de Conclusão: 26/01/2025\n    Código de Verificação: 20250126142530-A1B2C3D4\n    ",
  "aluno": {
    "id": "guid-do-aluno",
    "nome": "João da Silva",
    "email": "joao@email.com"
  },
  "curso": {
    "id": "guid-do-curso",
    "titulo": "ASP.NET Core Avançado",
    "cargaHoraria": "1.16:40:00",
    "valor": 500.00
  }
}
```

**Response Error (400) - Status Inválido:**
```json
{
  "message": "A matrícula deve estar concluída para emitir o certificado."
}
```

**Response Error (400) - Duplicado:**
```json
{
  "message": "Certificado já foi emitido para esta matrícula."
}
```

---

## Fluxo Completo de Uso

### Passo 1: Criar Matrícula
```http
POST http://localhost:5137/api/Alunos/{alunoId}/matriculas
Content-Type: application/json
Authorization: Bearer {token}

{
  "cursoId": "guid-do-curso"
}
```
**Status da Matrícula:** `Pendente (0)`

### Passo 2: Realizar Pagamento
```http
POST http://localhost:5137/api/Pagamentos/{matriculaId}
Content-Type: application/json
Authorization: Bearer {token}

{
  "dadosCartao": {
    "numero": "4111111111111111",
    "nomeTitular": "João Silva",
    "validade": "12/26",
    "cvv": "123"
  },
  "valor": 500.00
}
```
**Status da Matrícula:** `Ativa (1)` após pagamento aprovado

### Passo 3: Concluir Matrícula ✨ NOVO
```http
POST http://localhost:5137/api/Alunos/matriculas/{matriculaId}/concluir
Authorization: Bearer {token}
```
**Status da Matrícula:** `Concluída (2)`

### Passo 4: Emitir Certificado ✨ NOVO
```http
POST http://localhost:5137/api/Alunos/matriculas/{matriculaId}/certificado
Authorization: Bearer {token}
```
**Recebe:** Certificado com mensagem formatada

---

## Arquitetura da Solução

### Commands Implementados

#### ConcluirMatriculaCommand
```csharp
// Localização: EducationHub.Alunos.Application/Commands/ConcluirMatriculaCommand.cs
public class ConcluirMatriculaCommand : Command
{
    public Guid MatriculaId { get; set; }
}
```

#### ConcluirMatriculaCommandHandler
```csharp
// Validações:
// 1. Matrícula deve existir
// 2. Status deve ser Ativa (validado no domain)
// 3. Atualiza status para Concluída
// 4. Persiste alteração
```

#### EmitirCertificadoCommand
```csharp
// Localização: EducationHub.Alunos.Application/Commands/EmitirCertificadoCommand.cs
public class EmitirCertificadoCommand : Command, IRequest<CertificadoEmitidoViewModel>
{
    public Guid MatriculaId { get; set; }
}
```

#### EmitirCertificadoCommandHandler
```csharp
// Validações:
// 1. Matrícula deve existir
// 2. Status deve ser Concluída
// 3. Certificado não pode estar duplicado
// 4. Busca dados do aluno e curso via MediatR
// 5. Cria entidade Certificado com código único
// 6. Gera mensagem formatada (ASCII art)
// 7. Retorna ViewModel completo
```

### ViewModels

#### CertificadoEmitidoViewModel
```csharp
// Localização: EducationHub.Alunos.Application/ViewModels/CertificadoEmitidoViewModel.cs
public class CertificadoEmitidoViewModel
{
    public Guid CertificadoId { get; set; }
    public string Codigo { get; set; }
    public DateTime DataEmissao { get; set; }
    public string MensagemCertificado { get; set; }  // ✨ Mensagem formatada
    public AlunoMatriculaViewModel Aluno { get; set; }
    public CursoMatriculaViewModel Curso { get; set; }
}
```

### Domain

#### Entidade Certificado
```csharp
// Localização: EducationHub.Alunos.Domain/Entidades/Certificado.cs
public class Certificado : Entity, IAggregateRoot
{
    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public string TituloCurso { get; private set; }
    public DateTime DataEmissao { get; private set; }
    public string Codigo { get; private set; }  // Formato: yyyyMMddHHmmss-{8digitos}
}
```

#### Método Matricula.Concluir()
```csharp
// Validação no domain layer
public void Concluir()
{
    if (Status != StatusMatriculaEnum.Ativa)
        throw new DomainException("Somente matrículas ativas podem ser concluídas.");
    
    Status = StatusMatriculaEnum.Concluida;
    AdicionarEvento(new CursoConcluidoEvent(Id, AlunoId, CursoId));
}
```

### Repository

#### ICertificadoRepository
```csharp
// Localização: EducationHub.Alunos.Domain/Interfaces/ICertificadoRepository.cs
public interface ICertificadoRepository : IRepository<Certificado>
{
    Task AdicionarAsync(Certificado certificado);
    Task<Certificado> ObterPorId(Guid id);
    Task<Certificado> ObterPorMatriculaId(Guid matriculaId);  // Previne duplicação
    Task<IEnumerable<Certificado>> ObterPorAlunoId(Guid alunoId);
}
```

#### CertificadoRepository
```csharp
// Localização: EducationHub.Alunos.Data/Repository/CertificadoRepository.cs
// Implementação usando AlunoDbContext
// Método ObterPorMatriculaId busca por AlunoId + CursoId da matrícula
```

---

## Exemplo de Mensagem de Certificado

```
╔═══════════════════════════════════════════════╗
║         CERTIFICADO DE CONCLUSÃO              ║
╚═══════════════════════════════════════════════╝

Certificamos que
    MARIA OLIVEIRA SANTOS
concluiu com êxito o curso
    Domain-Driven Design na Prática
com carga horária de 60 horas

Data de Conclusão: 26/01/2025
Código de Verificação: 20250126153045-F7E8D9C0
```

---

## StatusMatriculaEnum

```csharp
public enum StatusMatriculaEnum
{
    Pendente = 0,    // Após criar matrícula
    Ativa = 1,       // Após pagamento aprovado
    Concluida = 2,   // Após concluir curso ✨ NOVO ESTADO
    Cancelada = 4    // Se pagamento rejeitado
}
```

---

## Transições de Estado Válidas

```
┌─────────────┐
│  Pendente   │  (Matrícula criada)
└──────┬──────┘
       │
       │ Pagamento aprovado
       ▼
┌─────────────┐
│    Ativa    │  (Pode assistir aulas)
└──────┬──────┘
       │
       │ Concluir matrícula ✨ NOVO
       ▼
┌─────────────┐
│  Concluída  │  (Pode emitir certificado)
└──────┬──────┘
       │
       │ Emitir certificado ✨ NOVO
       ▼
   Certificado
     Gerado
```

---

## Dependências Injetadas

**Program.cs:**
```csharp
builder.Services.AddScoped<EducationHub.Alunos.Domain.Interfaces.ICertificadoRepository, 
                           EducationHub.Alunos.Data.Repository.CertificadoRepository>();
```

---

## Segurança

- ✅ Todos os endpoints requerem autenticação (`[Authorize]`)
- ✅ Token JWT necessário no header `Authorization: Bearer {token}`
- ✅ Validações no domain layer impedem estados inválidos
- ✅ Impossível emitir certificado duplicado

---

## Testes de Cenários

### ✅ Cenário 1: Fluxo Completo de Sucesso
1. Criar matrícula (Status: Pendente)
2. Pagar matrícula (Status: Ativa)
3. Concluir matrícula (Status: Concluída) ✅
4. Emitir certificado ✅
5. Resultado: Certificado com mensagem formatada

### ❌ Cenário 2: Tentar Concluir Matrícula Pendente
1. Criar matrícula (Status: Pendente)
2. Tentar concluir sem pagar
3. Resultado: `400 Bad Request - "Somente matrículas ativas podem ser concluídas"`

### ❌ Cenário 3: Tentar Emitir Certificado de Matrícula Ativa
1. Criar matrícula (Status: Pendente)
2. Pagar matrícula (Status: Ativa)
3. Tentar emitir certificado sem concluir
4. Resultado: `400 Bad Request - "A matrícula deve estar concluída"`

### ❌ Cenário 4: Tentar Emitir Certificado Duplicado
1. Criar matrícula → Pagar → Concluir → Emitir certificado (Sucesso)
2. Tentar emitir certificado novamente
3. Resultado: `400 Bad Request - "Certificado já foi emitido para esta matrícula"`

---

## API Rodando

**URL Base:** `http://localhost:5137`

**Swagger UI:** `http://localhost:5137/swagger`

---

## Próximos Passos Sugeridos

1. ✅ **Implementado:** Endpoints de conclusão e certificado
2. 📝 **Sugestão:** Adicionar endpoint GET para listar certificados do aluno
3. 📝 **Sugestão:** Endpoint para download de certificado em PDF
4. 📝 **Sugestão:** Endpoint de validação de certificado por código
5. 📝 **Sugestão:** Notificação por email quando certificado for emitido
