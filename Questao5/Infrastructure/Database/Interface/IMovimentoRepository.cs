using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Interface
{
    public interface IMovimentoRepository
    {
        bool SalvarMovimento(Movimento movimento);
    }
}
