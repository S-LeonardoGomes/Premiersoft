using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Requests;
using Questao5.Infrastructure.Database.Interface;
using Questao5.Infrastructure.Sqlite;
using System.Linq.Expressions;

namespace Questao5.Infrastructure.Database.Repository
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public ContaCorrenteRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public bool VerificarContaCorrenteAtivaPorId(string idContaCorrente)
        {
            try
            {
                using var connection = new SqliteConnection(databaseConfig.Name);

                return connection.QueryFirstOrDefault<bool>("select 1 from contacorrente where idcontacorrente = @IdContaCorrente and ativo = 1;", 
                    new { IdContaCorrente = idContaCorrente }, commandTimeout: 60);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool VerificarContaCorrenteCadastradaPorId(string idContaCorrente)
        {
            try
            {
                using var connection = new SqliteConnection(databaseConfig.Name);

                return connection.QueryFirstOrDefault<bool>("select 1 from contacorrente where idcontacorrente = @IdContaCorrente;",
                    new { IdContaCorrente = idContaCorrente }, commandTimeout: 60);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
