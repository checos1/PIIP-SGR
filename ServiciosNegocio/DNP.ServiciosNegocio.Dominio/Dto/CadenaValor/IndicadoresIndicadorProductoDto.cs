namespace DNP.ServiciosNegocio.Dominio.Dto.CadenaValor
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class IndicadoresIndicadorProductoDto
    {
        public int? IndicadorId { get; set; }
        public string NombreIndicador { get; set; }
        public string CodigoIndicador { get; set; }
        public string IndicadorTipo { get; set; }
        public bool? IndicadorAcumula { get; set; }
        public int? IndicadorUnidadMedidaId { get; set; }
        public string NombreUnidadMedida { get; set; }
        public int? Firme { get; set; }
        public double? MetaTotalIndicadorMga { get; set; }
        public double? MetaTotalActual { get; set; }
        public double? MetaTotalFirme { get; set; }
        public double? MetaTotalFirmeAjustado { get; set; }
        public string IndicadorAcumulaAjustado { get; set; }
        public string LabelBotonIndicador { get; set; }
        public bool HabilitaEditarIndicador { get; set; } = false;
        public int EsCreadoPIIP { get; set; }
        public bool? IndicadorAcumulaOriginal { get; set; }
        public string IndicadorAcumulaAjustadoOriginal { get; set; }
        public double? MetaTotalFirmeAjustadoOriginal { get; set; }
        public List<VigenciaIndicadorProductoDto> Vigencias { get; set; }
    }
}
