using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Transversales
{
    public class SystemConfiguracionServicios : ISystemConfiguracionServicios
    {
        private readonly ISystemConfiguracionPersistencia _systemConfiguracionPersistencia;
        public SystemConfiguracionServicios(ISystemConfiguracionPersistencia systemConfiguracionPersistencia)
        {
            _systemConfiguracionPersistencia = systemConfiguracionPersistencia;
        }
        public SystemConfigurationDto ObtenerSystemConfiguracion(string VariableKey, string Separador)
        {
            return _systemConfiguracionPersistencia.ObtenerSystemConfiguracion(VariableKey, Separador);
        }
    }
}
