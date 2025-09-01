using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DNP.ServiciosNegocio.Servicios.Interfaces.Transversales
{
    public interface ISystemConfiguracionServicios
    {
        SystemConfigurationDto ObtenerSystemConfiguracion(string VariableKey, string Separador);
    }
}