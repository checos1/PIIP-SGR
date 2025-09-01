using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Servicios.Interfaces.Consola;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DNP.Backbone.Test.Mocks
{
    public class ConsolaProyectoServiciosMock : IConsolaProyectosServicio
    {
        public Task<object> InsertarAuditoria(AuditoriaEntidadDto auditoriaEntidad, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<object> ObtenerAuditoriaEntidadProyecto(int proyectoId, string idUsuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<ProyectoDto> ObtenerInstanciasProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            throw new NotImplementedException();
        }

        public Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token)
        {
            return Task.FromResult(new ProyectoDto());
        }

        public Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            return Task.FromResult(new ProyectoDto { GruposEntidades = new List<GrupoEntidadProyectoDto> { new GrupoEntidadProyectoDto() } });
        }

        Task<object> IConsolaProyectosServicio.InsertarAuditoria(AuditoriaEntidadDto auditoriaEntidad, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        Task<object> IConsolaProyectosServicio.ObtenerAuditoriaEntidadProyecto(int proyectoId, string idUsuarioDNP)
        {
            throw new NotImplementedException();
        }

        Task<ProyectoDto> IConsolaProyectosServicio.ObtenerInstanciasProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            throw new NotImplementedException();
        }

        Task<ProyectoDto> IConsolaProyectosServicio.ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token)
        {
            throw new NotImplementedException();
        }

        Task<ProyectoDto> IConsolaProyectosServicio.ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            throw new NotImplementedException();
        }

        List<ProjectDocumentSimpleDto> IConsolaProyectosServicio.ObtenerDocumentosAdjuntos(string idProyecto)
        {
            return new List<ProjectDocumentSimpleDto>();
        }

        HttpContent IConsolaProyectosServicio.ObtenerDocumentoAdjunto(string idProyecto, string nombreArchivo)
        {
            return null;
        }

        public Task<Guid?> ObtenerIdAplicacionPorBpin(string idObjetoNegocio, string usuarioDnp)
        {
            return Task.FromResult((Guid?)Guid.NewGuid());
        }
    }
}
