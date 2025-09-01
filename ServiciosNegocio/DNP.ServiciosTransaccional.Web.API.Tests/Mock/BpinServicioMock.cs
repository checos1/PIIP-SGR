namespace DNP.ServiciosTransaccional.Web.API.Test.Mock
{
    using Servicios.Interfaces.Proyectos;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public class BpinServicioMock : IBpinServicio
    {
        public object GenerarBPIN(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            return int.Parse(parametrosActualizar.Contenido.ObjetoNegocioId) % 2 == 0;
        }

        public object GenerarBPINSgr(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            return new
            {
                Exitoso = int.Parse(parametrosActualizar.Contenido.ObjetoNegocioId) % 2 == 0,
                Mensaje = "Prueba OK"
            };
        }
    }
}
