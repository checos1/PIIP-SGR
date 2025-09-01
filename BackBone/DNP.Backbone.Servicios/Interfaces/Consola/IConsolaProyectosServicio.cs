namespace DNP.Backbone.Servicios.Interfaces.Consola
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Comunes.Dto;
    using Dominio.Dto.Proyecto;
    public interface IConsolaProyectosServicio
    {
        Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token);
        Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto);
        Task<ProyectoDto> ObtenerInstanciasProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto);
        Task<object> InsertarAuditoria(Dominio.Dto.Proyecto.AuditoriaEntidadDto auditoriaEntidad, string usuarioDNP);
        Task<object> ObtenerAuditoriaEntidadProyecto(int proyectoId, string idUsuarioDNP);
        List<ProjectDocumentSimpleDto> ObtenerDocumentosAdjuntos(string idProyecto);
        HttpContent ObtenerDocumentoAdjunto(string idProyecto, string nombreArchivo);
        Task<Guid?> ObtenerIdAplicacionPorBpin(string idObjetoNegocio, string usuarioDnp);
    }
}
