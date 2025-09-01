using DNP.ServiciosNegocio.Dominio.Dto.ModificacionLey;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Persistencia.Interfaces.ModificacionLey;
using DNP.ServiciosNegocio.Servicios.Interfaces.ModificacionLey;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.ModificacionLey
{
    public class ModificacionLeyServicio : IModificacionLeyServicio
    {
        private readonly IModificacionLeyPersistencia _modificacionLeyPersistencia;

        public ModificacionLeyServicio(IModificacionLeyPersistencia modificacionLeyPersistencia)
        {
            _modificacionLeyPersistencia = modificacionLeyPersistencia;
        }

        public string ObtenerInformacionPresupuestalMLEncabezado(int EntidadDestinoId, int tramiteid, string origen)
        {
            return _modificacionLeyPersistencia.ObtenerInformacionPresupuestalMLEncabezado(EntidadDestinoId, tramiteid, origen);
        }

        public string ObtenerInformacionPresupuestalMLDetalle(int tramiteidProyectoId, string origen)
        {
            return _modificacionLeyPersistencia.ObtenerInformacionPresupuestalMLDetalle(tramiteidProyectoId, origen);
        }

        public TramitesResultado GuardarInformacionPresupuestalML(InformacionPresupuestalMLDto InformacionPresupuestal, string usuario,string origen)
        {
            return _modificacionLeyPersistencia.GuardarInformacionPresupuestalML(InformacionPresupuestal, usuario, origen);
        }
    }
}
