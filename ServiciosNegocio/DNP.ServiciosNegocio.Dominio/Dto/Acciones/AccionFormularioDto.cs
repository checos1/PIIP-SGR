using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Acciones
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class AccionFormularioDto
    {
        public Guid IdInstanciaAccion { get; set; }
        public Guid IdInstanciaFlujo { get; set; }
        public Guid IdAcccion { get; set; }
        public int IdEstadoEjecucionAccion { get; set; }
        public int TipoAccion { get; set; }
        public string DireccionIp { get; set; }
        public ObjetoContextoDto ObjetoContexto { get; set; }
        public ObjetoDatosDto ObjetoDatos { get; set; }
    }

    public class AccionFlujoDto
    {
        public Guid IdInstancia { get; set; }
        public Guid IdAcccion { get; set; }
        public string ObjetoNegocioId { get; set; }
        public string UsuarioDNP { get; set; }
        public string ObjetoJson { get; set; }
    }

}
