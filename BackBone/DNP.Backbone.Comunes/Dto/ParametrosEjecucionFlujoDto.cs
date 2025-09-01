using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ParametrosEjecucionFlujoDto
    {
        public Guid IdInstanciaFlujo { get; set; }
        public Guid IdAccion { get; set; }
        public ObjetoContextoDto ObjetoContexto { get; set; }
        public ObjetoDatosDto ObjetoDatos { get; set; }
    }
}
