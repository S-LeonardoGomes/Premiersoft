using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Interface;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repository
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public MovimentoRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public bool SalvarMovimento(Movimento movimento)
        {
            try
            {
                using var connection = new SqliteConnection(databaseConfig.Name);

                return connection.Execute(@"insert into movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                                            values (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);",
                    new 
                    { 
                        IdMovimento = movimento.IdMovimento.ToString().ToUpper(), 
                        IdContaCorrente = movimento.IdContaCorrente.ToUpper(),
                        DataMovimento = movimento.DataMovimento,
                        TipoMovimento = movimento.TipoMovimento,
                        Valor = movimento.Valor
                    }, commandTimeout: 60) == 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Movimento> ObterMovimentosContaCorrente(string idContaCorrente)
        {
            try
            {
                using var connection = new SqliteConnection(databaseConfig.Name);

                return connection.Query<Movimento>(@"select * from movimento where idcontacorrente = @IdContaCorrente;", new { IdContaCorrente = idContaCorrente.ToUpper() }, commandTimeout: 60).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
