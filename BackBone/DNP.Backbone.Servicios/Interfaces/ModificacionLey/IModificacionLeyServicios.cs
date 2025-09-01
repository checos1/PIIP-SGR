using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.ModificacionLey;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.ModificacionLey
{
    public interface IModificacionLeyServicios
    {
        Task<string> ObtenerInformacionPresupuestalMLEncabezado(int EntidadDestinoId, int tramiteid, string origen, string usuarioDnp);
        Task<string> ObtenerInformacionPresupuestalMLDetalle(int tramiteidProyectoId, string origen, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarInformacionPresupuestalML(InformacionPresupuestalMLDto InformacionPresupuestal, string usuarioDnp);
    }
}
