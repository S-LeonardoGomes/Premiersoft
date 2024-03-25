using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Interface
{
    public interface IIdempotenciaRepository
    {
        IdempotenciaResponse ObterIdempotenciaPorIdRequisicao(string idRequisicao);
        bool SalvarIdempotencia(string idRequisicao, string request, string response);
    }
}
