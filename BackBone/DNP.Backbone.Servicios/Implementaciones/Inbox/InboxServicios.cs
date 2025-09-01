namespace DNP.Backbone.Servicios.Implementaciones.Inbox
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using Comunes.Dto;
    using Comunes.Properties;
    using Dominio.Dto.Inbox;
    using Interfaces.Autorizacion;
    using Interfaces.ServiciosNegocio;
    using Interfaces.Inbox;

    /// <summary>
    /// Clase responsable de la gestión de proyectos
    /// </summary>
    public class InboxServicios : IInboxServicios
    {
        private readonly IFlujoServicios _flujoServicios;
        private readonly IAutorizacionServicios _autorizacionNegocioServicios;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="flujoServicios">Instancia de servicios de flujos</param>
        /// <param name="autorizacionNegocioServicios">Instancia de servicios de autorizacion de negocio servicios</param>
        public InboxServicios(IAutorizacionServicios autorizacionNegocioServicios, IFlujoServicios flujoServicios)
        {
            _autorizacionNegocioServicios = autorizacionNegocioServicios;
            _flujoServicios = flujoServicios;
        }

        /// <summary>
        /// Obtención de datos inbox por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="datosConsulta">Datos de consulta</param>
        /// <param name="token">token</param>
        /// <param name="proyectoFiltroDto">filtro de lista</param>
        /// <returns>Consulta de datos inbox.</returns>
        public Task<InboxDto> ObtenerInbox(ParametrosInboxDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            ParametrosObjetosNegocioInboxDto parametros = new ParametrosObjetosNegocioInboxDto()
            {
                IdsRoles = datosConsulta.ListaIdsRoles,
                IdTipoObjetoNegocio = datosConsulta.IdObjeto,
                IdUsuarioDNP = datosConsulta.IdUsuario,
                TokenAutorizacion = token,
                ProyectoFiltro = proyectoFiltroDto
            };

            var inbox = new InboxDto() { GruposEntidades = new List<GrupoEntidadDto>() };

            return Task.FromResult(inbox);
        }

        /// <summary>
        /// Obtención de datos inbox por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="datosConsulta">Datos de consulta</param>
        /// <param name="token">token</param>        
        /// <returns>Consulta de datos inbox.</returns>
        public Task<InboxDto> ObtenerInbox(ParametrosInboxDto datosConsulta, string token)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Obtención de datos inbox para PDF.
        /// </summary>
        /// <param name="datosConsulta">Datos de consulta</param>
        /// <param name="token">token</param>        
        /// <returns>Consulta de datos inbox en PDF.</returns>
        public Task<InboxDto> ObtenerInfoPDF(InstanciaInboxDto datosConsulta, string token)
        {
            var inbox = ObtenerInbox(datosConsulta.ParametrosInboxDto, token, datosConsulta.ProyectoFiltroDto);

            //inbox = (from dados in datosConsulta)

            return inbox;
        }


        #region Métodos Privados
        /// <summary>
        /// Obtención de grupo para datos inbox.
        /// </summary>
        /// <param name="listaObjetosNegocio">Lista de objeto negocio</param>        
        /// <returns>Consulta de grupos de datos inbox.</returns>
        private List<GrupoEntidadDto> CrearGrupoEntidades(List<NegocioDto> listaObjetosNegocio)
        {
            var grupoPorEntidades = from ge in listaObjetosNegocio
                                    group ge by new { ge.IdEntidad, ge.NombreEntidad, ge.TipoEntidad }
                                    into g
                                    select new EntidadInboxDto()
                                    {
                                        IdEntidad = g.Key.IdEntidad,
                                        NombreEntidad = g.Key.NombreEntidad,
                                        TipoEntidad = g.Key.TipoEntidad,
                                        ObjetosNegocio = g.ToList()
                                    };

            var grupoPorTipoEntidad = (from gte in grupoPorEntidades
                                       group gte by new { gte.TipoEntidad }
                                       into g
                                       select new GrupoEntidadDto()
                                       {
                                           TipoEntidad = g.Key.TipoEntidad,
                                           ListaEntidades = g.ToList()
                                       }).ToList();

            return grupoPorTipoEntidad;
        }

        #endregion
    }
}
