using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentarContaCorrenteRequest : IRequest<ResponseDefault<MovimentarContaCorrenteResponse>>
    {
        public Guid RequisicaoId { get; set; }
        public Guid IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string? TipoMovimento { get; set; }
    }
}
