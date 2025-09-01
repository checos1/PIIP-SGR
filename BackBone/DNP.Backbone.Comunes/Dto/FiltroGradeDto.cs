
namespace DNP.Backbone.Comunes.Dto
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class FiltroGradeDto
    {
        public string Campo { get; set; }
        public string Campo2 { get; set; }
        public string Valor { get; set; }
        public string Valor2 { get; set; }
        public FiltroTipo Tipo { get; set; }
        public FiltroTipo Tipo2 { get; set; }
        public TipoCombinacao Combinacao { get; set; }
    }

    public enum FiltroTipo
    {
        Igual = 0,
        Diferente = 1,
        Menor = 2,
        MenorIgual = 3,
        Maior = 4,
        MaiorIgual = 5,
        Contem = 6,
        Inicia = 7,
        Termina = 8
    }

    public enum TipoCombinacao
    {
        NULO = 0,
        E = 1,
        OU = 2
    }
}
