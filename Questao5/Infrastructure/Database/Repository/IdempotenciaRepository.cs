using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Interface;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repository
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public IdempotenciaRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public IdempotenciaResponse ObterIdempotenciaPorIdRequisicao(string idRequisicao)
        {
            try
            {
                using var connection = new SqliteConnection(databaseConfig.Name);

                return connection.QueryFirstOrDefault<IdempotenciaResponse>("select * from idempotencia where chave_idempotencia = @Chave;",
                    new { Chave = idRequisicao }, commandTimeout: 60);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SalvarIdempotencia(string idRequisicao, string request, string response)
        {
            try
            {
                using var connection = new SqliteConnection(databaseConfig.Name);

                return connection.Execute("insert into idempotencia (chave_idempotencia, requisicao, resultado) values (@Chave, @Request, @Response);",
                    new { Chave = idRequisicao, Request = request, Response = response }, commandTimeout: 60) == 1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
