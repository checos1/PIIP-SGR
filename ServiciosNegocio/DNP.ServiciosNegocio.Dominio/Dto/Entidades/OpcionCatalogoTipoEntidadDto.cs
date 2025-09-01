using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Entidades
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.

    public class OpcionCatalogoTipoEntidadDto : EntidadBase
    {
        public string Nombre { get; set; }
        public int? IdTipo { get; set; }
        public int? IdPadre { get; set; }
        public string CodigoEntidad { get; set; }
        public bool? EsActiva { get; set; }
        public AtributosEntidadDto Atributos { get; set; }
    }
}
