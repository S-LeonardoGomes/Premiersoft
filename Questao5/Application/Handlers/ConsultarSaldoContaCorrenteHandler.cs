using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.Interface;

namespace Questao5.Application.Handlers
{
    public class ConsultarSaldoContaCorrenteHandler : IRequestHandler<ConsultarSaldoContaCorrenteRequest, ResponseDefault<ConsultarSaldoContaCorrenteResponse>>
    {
        private readonly IContaCorrenteRepository _contaRepository;
        private readonly IMovimentoRepository _movimentoRepository;

        public ConsultarSaldoContaCorrenteHandler(IContaCorrenteRepository contaRepository, IMovimentoRepository movimentoRepository)
        {
            _contaRepository = contaRepository;
            _movimentoRepository = movimentoRepository;
        }

        public Task<ResponseDefault<ConsultarSaldoContaCorrenteResponse>> Handle(ConsultarSaldoContaCorrenteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                ContaCorrente conta = ObterContaCorrente(request.IdContaCorrente.ToString());

                #region Validacoes
                if (conta == null)
                {
                    return Task.FromResult(new ResponseDefault<ConsultarSaldoContaCorrenteResponse>()
                    {
                        DescricaoErro = "Apenas contas correntes cadastradas podem consultar o saldo",
                        StatusEnum = StatusEnum.INVALID_ACCOUNT,
                        Dado = null
                    });
                }

                if (conta.Ativo == 0)
                {
                    return Task.FromResult(new ResponseDefault<ConsultarSaldoContaCorrenteResponse>()
                    {
                        DescricaoErro = "Apenas contas correntes ativas podem consultar o saldo",
                        StatusEnum = StatusEnum.INACTIVE_ACCOUNT,
                        Dado = null
                    });
                }
                #endregion

                List<Movimento> movimentosConta = ObterMovimentosContaCorrente(request.IdContaCorrente.ToString());

                if (!movimentosConta.Any())
                {
                    return Task.FromResult(new ResponseDefault<ConsultarSaldoContaCorrenteResponse>()
                    {
                        StatusEnum = StatusEnum.SUCCESS,
                        Dado = new ConsultarSaldoContaCorrenteResponse() 
                        { 
                            Numero = conta.Numero,
                            NomeTitular = conta.Nome,
                            DataHoraConsulta = DateTime.Now,
                            Saldo = 0 
                        }
                    });
                }

                decimal saldoMovimentosEntrada = movimentosConta.Where(x => x.TipoMovimento == "C").Select(x => x.Valor).Sum();
                decimal saldoMovimentosSaida = movimentosConta.Where(x => x.TipoMovimento == "D").Select(x => x.Valor).Sum();

                return Task.FromResult(new ResponseDefault<ConsultarSaldoContaCorrenteResponse>()
                {
                    StatusEnum = StatusEnum.SUCCESS,
                    Dado = new ConsultarSaldoContaCorrenteResponse() 
                    {
                        Numero = conta.Numero,
                        NomeTitular = conta.Nome,
                        DataHoraConsulta = DateTime.Now,
                        Saldo = saldoMovimentosEntrada - saldoMovimentosSaida 
                    }
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseDefault<ConsultarSaldoContaCorrenteResponse>()
                {
                    DescricaoErro = ex.Message,
                    StatusEnum = StatusEnum.INTERNAL_SERVER_ERROR,
                    Dado = null
                });
            }
        }

        private ContaCorrente ObterContaCorrente(string idContaCorrente)
        {
            try
            {
                return _contaRepository.ObterContaCorrente(idContaCorrente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<Movimento> ObterMovimentosContaCorrente(string idContaCorrente)
        {
            try
            {
                return _movimentoRepository.ObterMovimentosContaCorrente(idContaCorrente);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
