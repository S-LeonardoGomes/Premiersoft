using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;

namespace Questao5.Application.Queries.Requests
{    
    public class ConsultarSaldoContaCorrenteRequest : IRequest<ResponseDefault<ConsultarSaldoContaCorrenteResponse>>
    {
        public Guid IdContaCorrente { get; set; }
    }
}
