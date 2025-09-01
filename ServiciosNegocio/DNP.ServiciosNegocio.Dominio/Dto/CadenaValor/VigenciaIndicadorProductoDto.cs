namespace DNP.ServiciosNegocio.Dominio.Dto.CadenaValor
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class VigenciaIndicadorProductoDto
    {
        public int? Vigencia { get; set; }
        public double? MetaVigenciaIndicadorMga { get; set; }
        public double? MetaVigenciaIndicadorFirme { get; set; }
        public double? MetaVigencialIndicadorAjuste { get; set; }
        public double? MetaVigencialIndicadorAjusteOriginal { get; set; }
    }
}
