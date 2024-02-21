namespace Models;

public record Transacao(
    int Valor,

    char Tipo,

    string Descricao,

    DateTime Realizada_em
);