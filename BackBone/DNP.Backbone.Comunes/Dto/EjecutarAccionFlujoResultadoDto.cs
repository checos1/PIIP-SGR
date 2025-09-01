using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class EjecutarAccionFlujoResultadoDto
    {
        public ObjetoContextoDto ObjectoContexto { get; set; }
        public string MensajeEjecucion { get; set; }
    }
}
