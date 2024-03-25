﻿using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Database.Interface
{
    public interface IContaCorrenteRepository
    {
        bool VerificarContaCorrenteCadastradaPorId(string idContaCorrente);
        bool VerificarContaCorrenteAtivaPorId(string idContaCorrente);
    }
}
