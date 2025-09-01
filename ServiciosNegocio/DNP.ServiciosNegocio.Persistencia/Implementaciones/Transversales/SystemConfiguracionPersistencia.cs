namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Transversales
{
    using Comunes.Utilidades;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using Interfaces;
    using Modelo;
    using Newtonsoft.Json;
    using System.Data.Entity.Migrations;
    using System.Linq;
    public class SystemConfiguracionPersistencia : Persistencia, ISystemConfiguracionPersistencia
    {
        #region Constructor
        public SystemConfiguracionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }
        #endregion
        public SystemConfigurationDto ObtenerSystemConfiguracion(string VariableKey, string Separador)
        {
            //var listadoConfiguracion = new SystemConfigurationDto();
            //var listadoDatos = ContextoOnlySP.uspGetSystemConfiguracion_JSON(VariableKey, Separador).FirstOrDefault();
            //return listadoDatos;
            var result = ContextoOnlySP.uspGetSystemConfiguracion_JSON(VariableKey, Separador).FirstOrDefault();
            if (!string.IsNullOrEmpty(result))
            {
                return JsonConvert.DeserializeObject<SystemConfigurationDto>(result);
            }
            else
            {
                return new SystemConfigurationDto();
            }
        }
    }
}

