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
                    movimento, commandTimeout: 60) == 1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
