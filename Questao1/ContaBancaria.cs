namespace Questao1
{
    internal class ContaBancaria
    {
        public int Conta { get; }
        public string Titular { get; private set; }
        public double Saldo { get; private set; }

        public ContaBancaria(int numeroConta, string nomeTitular, double saldoInicial = 0)
        {
            Conta = numeroConta;
            Titular = nomeTitular;
            Saldo = saldoInicial;
        }

        public ContaBancaria() { }

        public void Saque(double valorSacado, double taxaSaque)
        {
            Saldo -= valorSacado + taxaSaque;
        }

        public void Deposito(double valorDepositado)
        {
            Saldo += valorDepositado;
        }

        public void AtualizarTitular(string nomeTitular)
        {
            Titular = nomeTitular;
        }

        public override string ToString()
        {
            return $"Conta: {Conta}, Titular: {Titular}, Saldo: $ {Saldo:N2}";
        }
    }
}
