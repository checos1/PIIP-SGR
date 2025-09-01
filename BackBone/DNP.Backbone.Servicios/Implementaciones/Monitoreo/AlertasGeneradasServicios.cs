using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Servicios.Interfaces.Monitoreo;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.Monitoreo
{
    public class AlertasGeneradasServicios : IAlertasGeneradasServicios
    {
        private readonly IFlujoServicios _flujoServicios;

        public AlertasGeneradasServicios(IFlujoServicios flujoServicios)
        {
            _flujoServicios = flujoServicios;
        }

        public Task<ICollection<AlertasGeneradasDto>> ObtenerAlertasGeneradas(AlertasGeneradasFiltroDto filtro)
        {
            return _flujoServicios.ObtenerAlertasGeneradas(filtro);
        }
    }
}
