using System;
using System.Globalization;

namespace Questao1
{
    class Program
    {
        static void Main(string[] args)
        {
            /****
             * Para manter a fluidez do programa, optei por repetir as operações mal-sucedidas até que a mesma fosse realizada
             * com sucesso. Isso elimina a necessidade de reiniciar o programa a cada teste e ter que reinserir todos os dados 
             * novamente.
             ****/

            const double TAXA_SAQUE = 3.5;
            string opcaoDigitada;

            do
            {
                Console.Clear();
                ContaBancaria conta;

                do
                {
                    conta = CriarContaBancaria();
                } while (conta == null);

                ExibirDadosContaBancaria(conta, false);

                Console.WriteLine();
                RealizarDepositoContaBancaria(conta);
                ExibirDadosContaBancaria(conta, true);

                Console.WriteLine();
                RealizarSaqueContaBancaria(conta, TAXA_SAQUE);
                ExibirDadosContaBancaria(conta, true);

                Console.WriteLine();
                Console.Write("Entre 's' caso deseje inserir mais um exemplo: ");
                opcaoDigitada = Console.ReadLine().ToLower();
            } while (opcaoDigitada == "s");

            Console.WriteLine("Questão 1 encerrada.");

            /* Output expected:
            Exemplo 1:

            Entre o número da conta: 5447
            Entre o titular da conta: Milton Gonçalves
            Haverá depósito inicial(s / n) ? s
            Entre o valor de depósito inicial: 350.00

            Dados da conta:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00

            Entre um valor para depósito: 200
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 550.00

            Entre um valor para saque: 199
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50

            Exemplo 2:
            Entre o número da conta: 5139
            Entre o titular da conta: Elza Soares
            Haverá depósito inicial(s / n) ? n

            Dados da conta:
            Conta 5139, Titular: Elza Soares, Saldo: $ 0.00

            Entre um valor para depósito: 300.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ 300.00

            Entre um valor para saque: 298.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ -1.50
            */
        }

        private static ContaBancaria CriarContaBancaria()
        {
            try
            {
                Console.Write("Entre o número da conta: ");
                int numero = int.Parse(Console.ReadLine());

                Console.Write("Entre o titular da conta: ");
                string titular = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(titular))
                    throw new ArgumentException("Um nome de titular deve ser informado. Tente novamente, por favor.");

                Console.Write("Haverá depósito inicial (s/n)? ");
                char resp = char.Parse(Console.ReadLine().ToLower());

                double depositoInicial = 0;

                if (resp == 's')
                {
                    Console.Write("Entre o valor de depósito inicial: ");
                    depositoInicial = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

                    if (depositoInicial < 0)
                        throw new ArgumentException("O valor do depósito inicial não pode ser negativo. Tente novamente, por favor.");
                }
                else if (resp != 'n')
                {
                    throw new ArgumentException("A opção digitada para depósito inicial é inválida. Tente novamente, por favor.");
                }

                return new ContaBancaria(numero, titular, depositoInicial);
            }
            catch (FormatException)
            {
                Console.WriteLine();
                Console.WriteLine("-----------------------------------------------------------------------------------");
                Console.WriteLine("Erro: Um dos campos foi digitado no formato incorreto. Tente novamente, por favor.");
                Console.WriteLine("-----------------------------------------------------------------------------------");
                Console.WriteLine();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine();
                Console.WriteLine("------------------------------------------------------------------------------------");
                Console.WriteLine($"Erro: {ex.Message}");
                Console.WriteLine("------------------------------------------------------------------------------------");
                Console.WriteLine();
            }
            
            return null;
        }

        private static void ExibirDadosContaBancaria(ContaBancaria contaBancaria, bool isAtualizacao)
        {
            string fraseExibicao = isAtualizacao ? "Dados da conta atualizados:" : "Dados da conta:";

            if (!isAtualizacao) Console.WriteLine();
            Console.WriteLine(fraseExibicao);
            Console.WriteLine(contaBancaria);
        }

        private static void RealizarSaqueContaBancaria(ContaBancaria contaBancaria, double taxaSaque)
        {
            Console.Write("Entre um valor para saque: ");

            try
            {
                double quantia = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                contaBancaria.Saque(Math.Abs(quantia), taxaSaque);
            }
            catch (FormatException)
            {
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.WriteLine("Erro: O valor do saque foi digitado no formato incorreto. Tente novamente, por favor.");
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.WriteLine();

                RealizarSaqueContaBancaria(contaBancaria, taxaSaque);
            }
        }

        private static void RealizarDepositoContaBancaria(ContaBancaria contaBancaria)
        {
            Console.Write("Entre um valor para depósito: ");

            try
            {
                double quantia = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                contaBancaria.Deposito(Math.Abs(quantia));
            }
            catch (FormatException)
            {
                Console.WriteLine();
                Console.WriteLine("-----------------------------------------------------------------------------------------");
                Console.WriteLine("Erro: O valor do depósito foi digitado no formato incorreto. Tente novamente, por favor.");
                Console.WriteLine("-----------------------------------------------------------------------------------------");
                Console.WriteLine();

                RealizarDepositoContaBancaria(contaBancaria);
            }
        }
    }
}
