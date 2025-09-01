namespace DNP.Backbone.Web.API.Test.Mocks
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Servicios.Interfaces.Consola;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    public class ConsolaProyectoServiciosMock: IConsolaProyectosServicio
    {
        public Task<object> InsertarAuditoria(AuditoriaEntidadDto auditoriaEntidad, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        public Task<object> ObtenerAuditoriaEntidadProyecto(int proyectoId, string idUsuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        public HttpContent ObtenerDocumentoAdjunto(string idProyecto, string nombreArchivo)
        {
            throw new System.NotImplementedException();
        }

        public List<ProjectDocumentSimpleDto> ObtenerDocumentosAdjuntos(string idProyecto)
        {
            throw new System.NotImplementedException();
        }

        public Task<Guid?> ObtenerIdAplicacionPorBpin(string idObjetoNegocio, string usuarioDnp)
        {
            return Task.FromResult((Guid?)Guid.NewGuid());
        }

        public Task<ProyectoDto> ObtenerInstanciasProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            return Task.FromResult(new ProyectoDto());
        }

        public Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token)
        {
            return Task.FromResult(new ProyectoDto());
        }

        public Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            return Task.FromResult(new ProyectoDto { GruposEntidades = new List<GrupoEntidadProyectoDto> { new GrupoEntidadProyectoDto() } });
        }

      
    }
}
