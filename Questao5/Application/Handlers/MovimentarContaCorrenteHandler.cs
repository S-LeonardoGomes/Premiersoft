using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.Interface;

namespace Questao5.Application.Handlers
{
    public class MovimentarContaCorrenteHandler : IRequestHandler<MovimentarContaCorrenteRequest, ResponseDefault<MovimentarContaCorrenteResponse>>
    {
        private readonly IContaCorrenteRepository _contaRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        private readonly IMovimentoRepository _movimentoRepository;

        public MovimentarContaCorrenteHandler(IContaCorrenteRepository contaRepository, IIdempotenciaRepository idempotenciaRepository, IMovimentoRepository movimentoRepository)
        {
            _contaRepository = contaRepository;
            _idempotenciaRepository = idempotenciaRepository;
            _movimentoRepository = movimentoRepository;
        }

        public Task<ResponseDefault<MovimentarContaCorrenteResponse>> Handle(MovimentarContaCorrenteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                #region Validacoes
                IdempotenciaResponse requisicaoIdempotente = ObtemResultadoIdempotente(request.RequisicaoId.ToString());
                if (requisicaoIdempotente != null)
                {
                    return Task.FromResult(new ResponseDefault<MovimentarContaCorrenteResponse>()
                    {
                        StatusEnum = StatusEnum.SUCCESS,
                        Dado = JsonConvert.DeserializeObject<MovimentarContaCorrenteResponse>(requisicaoIdempotente.Resultado)
                    });
                }

                if (!IsContaCadastrada(request.IdContaCorrente.ToString()))
                {
                    return Task.FromResult(new ResponseDefault<MovimentarContaCorrenteResponse>()
                    {
                        DescricaoErro = "Apenas contas correntes cadastradas podem receber movimentação",
                        StatusEnum = StatusEnum.INVALID_ACCOUNT,
                        Dado = null
                    });
                }

                if (!IsContaAtiva(request.IdContaCorrente.ToString()))
                {
                    return Task.FromResult(new ResponseDefault<MovimentarContaCorrenteResponse>()
                    {
                        DescricaoErro = "Apenas contas correntes ativas podem receber movimentação",
                        StatusEnum = StatusEnum.INACTIVE_ACCOUNT,
                        Dado = null
                    });
                }

                if (!IsValorRecebidoValido(request.Valor))
                {
                    return Task.FromResult(new ResponseDefault<MovimentarContaCorrenteResponse>()
                    {
                        DescricaoErro = "Apenas valores positivos podem ser recebidos",
                        StatusEnum = StatusEnum.INVALID_VALUE,
                        Dado = null
                    });
                }

                if (!IsTipoMovimentacaoValido(request.TipoMovimento))
                {
                    return Task.FromResult(new ResponseDefault<MovimentarContaCorrenteResponse>()
                    {
                        DescricaoErro = "Apenas os tipos 'débito' ou 'crédito' podem ser aceitos",
                        StatusEnum = StatusEnum.INVALID_TYPE,
                        Dado = null
                    });
                }
                #endregion

                Movimento movimento = SalvarMovimento(request);
                SalvarIdempotencia(request, movimento);

                return Task.FromResult(new ResponseDefault<MovimentarContaCorrenteResponse>()
                {
                    StatusEnum = StatusEnum.SUCCESS,
                    Dado = new MovimentarContaCorrenteResponse() { IdMovimento = movimento.IdMovimento },
                });

            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseDefault<MovimentarContaCorrenteResponse>()
                {
                    DescricaoErro = ex.Message,
                    StatusEnum = StatusEnum.INTERNAL_SERVER_ERROR,
                    Dado = null
                });
            }
        }

        public Movimento SalvarMovimento(MovimentarContaCorrenteRequest request)
        {
            Movimento movimentoRequest = new()
            {
                IdMovimento = Guid.NewGuid(),
                DataMovimento = DateTime.Today.ToString("yyyy-MM-dd"),
                IdContaCorrente = request.IdContaCorrente.ToString(),
                TipoMovimento = request.TipoMovimento,
                Valor = request.Valor
            };

            try
            {
                if (_movimentoRepository.SalvarMovimento(movimentoRequest))
                    return movimentoRequest;
                else
                    throw new Exception($"Não foi possível salvar o movimento.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SalvarIdempotencia(MovimentarContaCorrenteRequest request, Movimento movimento)
        {
            try
            {
                string idRequisicao = request.RequisicaoId.ToString();
                string req = JsonConvert.SerializeObject(request);
                string resp = JsonConvert.SerializeObject(movimento);

                if (!_idempotenciaRepository.SalvarIdempotencia(idRequisicao, req, resp))
                    throw new Exception("Não foi possível salvar a idempotencia.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Validacoes
        private IdempotenciaResponse ObtemResultadoIdempotente(string idRequisicao)
        {
            try
            {
                return _idempotenciaRepository.ObterIdempotenciaPorIdRequisicao(idRequisicao);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsContaCadastrada(string idContaCorrente)
        {
            try
            {
                return _contaRepository.VerificarContaCorrenteCadastradaPorId(idContaCorrente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsContaAtiva(string idContaCorrente)
        {
            try
            {
                return _contaRepository.VerificarContaCorrenteAtivaPorId(idContaCorrente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsValorRecebidoValido(decimal valor) => valor > 0;

        private bool IsTipoMovimentacaoValido(string tipoMovimento) => tipoMovimento.ToUpper() == "C" || tipoMovimento.ToUpper() == "D";
        #endregion
    }
}
