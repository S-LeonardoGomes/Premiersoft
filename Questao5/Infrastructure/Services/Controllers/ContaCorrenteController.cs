using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Consulta o saldo de uma conta corrente
        /// </summary>
        [HttpPost("consultar-saldo")]
        [ProducesResponseType(typeof(ResponseDefault<ConsultarSaldoContaCorrenteResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDefault<ConsultarSaldoContaCorrenteResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ContarSaldoContaCorrente(ConsultarSaldoContaCorrenteRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusEnum == StatusEnum.SUCCESS ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest, response);
        }

        /// <summary>
        /// Realiza operações de Crédito ou Débito
        /// </summary>
        [HttpPut("movimentar")]
        [ProducesResponseType(typeof(ResponseDefault<MovimentarContaCorrenteResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDefault<MovimentarContaCorrenteResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MovimentarContaCorrente(MovimentarContaCorrenteRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusEnum == StatusEnum.SUCCESS ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest, response);
        }
    }
}
