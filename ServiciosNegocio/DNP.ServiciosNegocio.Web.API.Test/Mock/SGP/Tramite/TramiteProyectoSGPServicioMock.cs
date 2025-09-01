using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Tramite;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.SGP.Tramite
{
    public class TramiteProyectoSGPServicioMock : ITramiteProyectoSGPServicio
    {
        public ParametrosGuardarDto<DatosTramiteProyectosDto> ConstruirParametrosGuardadoVentanas(DatosTramiteProyectosDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<DatosTramiteProyectosDto>();

            if (contenido != null)
                parametrosGuardar.Contenido = contenido;
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "contenido"));

            return parametrosGuardar;
        }

        public string ObtenerProyectosTramiteNegocio(int TramiteId)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario)
        {
            var resultado = new TramitesResultado();

            if (datosTramiteProyectosDto.TramiteId != null)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public string ValidacionProyectosTramiteNegocio(int TramiteId)
        {
            throw new System.NotImplementedException();
        }
    }
}
