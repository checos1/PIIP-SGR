using DNP.ServiciosNegocio.Dominio.Dto.ModificacionLey;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.ModificacionLey
{
    public interface IModificacionLeyServicio
    {
        string ObtenerInformacionPresupuestalMLEncabezado(int EntidadDestinoId, int tramiteid, string origen);
        string ObtenerInformacionPresupuestalMLDetalle(int tramiteidProyectoId, string origen);
        TramitesResultado GuardarInformacionPresupuestalML(InformacionPresupuestalMLDto InformacionPresupuestal, string usuario,string origen);
    }
}
