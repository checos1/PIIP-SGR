using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosTransaccional.Servicios.Interfaces;

namespace DNP.ServiciosTransaccional.Web.API.Test.Mock
{
    public class ModificacionLeyServicioMock : IModificacionLeyServicio
    {
        public object ActualizarValoresPoliticasML(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            if (parametrosActualizar.Contenido.ObjetoNegocioId.Equals("BPIN"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return true;
        }
    }
}
