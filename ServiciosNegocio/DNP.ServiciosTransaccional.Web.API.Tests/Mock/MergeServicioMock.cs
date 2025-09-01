namespace DNP.ServiciosTransaccional.Web.API.Test.Mock
{
    using Servicios.Interfaces.Proyectos;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public class MergeServicioMock : IMergeServicio
    {
        public object AplicarMerge(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            return int.Parse(parametrosGuardar.Contenido.ObjetoNegocioId) % 2 == 0;
        }
    }
}
