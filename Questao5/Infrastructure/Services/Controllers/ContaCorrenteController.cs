using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
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

        [HttpGet("consultar-saldo")]
        [ProducesResponseType(typeof(ResponseDefault<MovimentarContaCorrenteResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDefault<MovimentarContaCorrenteResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ContarSaldoContaCorrente()
        {
            return Ok();
        }

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
