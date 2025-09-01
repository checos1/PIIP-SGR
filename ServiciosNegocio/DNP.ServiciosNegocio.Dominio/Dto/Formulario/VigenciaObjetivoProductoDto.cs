namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using DNP.ServiciosNegocio.Dominio.Dto.Indicadores;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class VigenciaObjetivoProductoDto
    {
        public int? Vigencia { get; set; }
        public int? ObjetivoEspecificoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public int? ProductoId { get; set; }
        public string Producto { get; set; }
        public decimal? Meta { get; set; }
        public int? UnidadMedidaId { get; set; }
        public string NombreUnidadMedida { get; set; }
        public List<IndicadorDto> Indicadores { get; set; }
    }
}
