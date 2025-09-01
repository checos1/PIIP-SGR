using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using DNP.ServiciosNegocio.Persistencia.Modelo;
namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales
{
    public interface ISystemConfiguracionPersistencia
    {
        SystemConfigurationDto ObtenerSystemConfiguracion(string VariableKey, string Separador);
    }
}
