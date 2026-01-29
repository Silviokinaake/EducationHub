using EducationHub.Core.DomainObjects;
using EducationHub.Faturamento.Application.Services;
using EducationHub.Faturamento.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EducationHub.API.Controllers
{ 
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FaturamentosController : ControllerBase
    {
        private readonly IPagamentoAppService _pagamentoAppService;

        public FaturamentosController(IPagamentoAppService pagamentoAppService)
        {
            _pagamentoAppService = pagamentoAppService;
        }

        /// <summary>
        /// Realiza o pagamento de uma pré-matrícula.
        /// </summary>
        /// <param name="request">Dados do pagamento e do cartão.</param>
        /// <returns>201 Created com o Pagamento ou 400/500 conforme o caso.</returns>
        [HttpPost("pagar")]
        public async Task<IActionResult> Pagar([FromBody] RealizarPagamentoRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var pagamentoVm = await _pagamentoAppService.RealizarPagamentoAsync(
                    request.AlunoId,
                    request.PreMatriculaId,
                    request.Valor,
                    request.DadosCartao);

                // Se o serviço retornar nulo por algum motivo, devolve 500
                if (pagamentoVm is null) return StatusCode(500, "Erro ao processar pagamento.");

                return CreatedAtRoute("GetPagamentoById", new { id = pagamentoVm.Id }, pagamentoVm);
            }
            catch (DomainException ex)
            {
                // Retorna 400 Bad Request para erros de validação de domínio
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um pagamento pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do pagamento (GUID).</param>
        [HttpGet("pagamento/{id:guid}", Name = "GetPagamentoById")]
        public async Task<IActionResult> GetPagamento(Guid id)
        {
            var pagamento = await _pagamentoAppService.ObterPorIdAsync(id);
            if (pagamento is null) return NotFound();
            return Ok(pagamento);
        }

        /// <summary>
        /// Lista pagamentos de um aluno.
        /// </summary>
        /// <param name="alunoId">Identificador do aluno (GUID).</param>
        [HttpGet("aluno/{alunoId:guid}")]
        public async Task<IActionResult> GetPorAluno(Guid alunoId)
        {
            var pagamentos = await _pagamentoAppService.ObterPorAlunoAsync(alunoId);
            return Ok(pagamentos);
        }

        /// <summary>
        /// Requisição para realizar pagamento (payload).
        /// </summary>
        public class RealizarPagamentoRequest
        {
            [Required] public Guid AlunoId { get; set; }
            
            [Required] 
            [System.Text.Json.Serialization.JsonPropertyName("matriculaId")]
            public Guid PreMatriculaId { get; set; }
            
            [Required] [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que 0")] public decimal Valor { get; set; }
            [Required] public DadosCartaoViewModel DadosCartao { get; set; } = new DadosCartaoViewModel();
        }
    }
}
