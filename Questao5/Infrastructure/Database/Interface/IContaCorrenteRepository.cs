using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Interface
{
    public interface IContaCorrenteRepository
    {
        bool VerificarContaCorrenteCadastradaPorId(string idContaCorrente);
        bool VerificarContaCorrenteAtivaPorId(string idContaCorrente);
        ContaCorrente ObterContaCorrente(string idContaCorrente);
    }
}
