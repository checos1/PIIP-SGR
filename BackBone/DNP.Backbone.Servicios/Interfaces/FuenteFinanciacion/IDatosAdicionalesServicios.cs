
namespace DNP.Backbone.Servicios.Interfaces.DatosAdicionales
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.ServiciosNegocio.Dominio.Dto.DatosAdicionales;
    using Dominio.Dto.Proyecto;

    public interface IDatosAdicionalesServicios
    {
        /// <summary>
        /// llamado al servicio para consultar fuente de financiacion
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        Task<string> ObtenerDatosAdicionales(int fuenteId, string usuarioDNP, string tokenAutorizacion);

        /// <summary>
        /// llamado al servicio para agregar fuente de financiacion
        /// </summary>
        /// <param name="proyectoDatosAdicionalesAgregarDto"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        Task<RespuestaGeneralDto> AgregarDatosAdicionales(DatosAdicionalesDto objDatosAdicionalesDto, string usuarioDNP, string tokenAutorizacion);
                
        Task<string> EliminarDatosAdicionales(int cofinanciadorId, string usuarioDNP, string tokenAutorizacion);
    
    }
}
