using Models;

namespace Entity;

public class Extract
{
    public Balance Saldo { get; set; }

    public List<Transacao> Ultimas_transacoes { get; set; }

    public Extract(Balance saldo, List<Transacao> ultimas_transacoes)
    {
        Saldo = saldo;
        Ultimas_transacoes = ultimas_transacoes;
    }
}