// using EducationHub.Faturamento.Application.Commands;
// using MediatR;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace EducationHub.API.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class PagamentosController : ControllerBase
//     {
//         private readonly IMediator _mediator;

//         public PagamentosController(IMediator mediator)
//         {
//             _mediator = mediator;
//         }

//         /// <summary>
//         /// Realiza um pagamento para ativar uma matrícula.
//         /// </summary>
//         [HttpPost]
//         [Authorize]
//         public async Task<IActionResult> RealizarPagamento([FromBody] RealizarPagamentoCommand command)
//         {
//             if (!ModelState.IsValid)
//                 return BadRequest(ModelState);

//             var resultado = await _mediator.Send(command);
            
//             if (!resultado)
//                 return BadRequest("Não foi possível processar o pagamento.");

//             return Ok(new 
//             { 
//                 Message = "Pagamento realizado com sucesso.", 
//                 PagamentoId = command.Id 
//             });
//         }
//     }
// }
