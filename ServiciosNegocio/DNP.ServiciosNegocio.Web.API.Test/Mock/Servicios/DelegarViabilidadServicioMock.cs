using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.DelegarViabilidad;
using System;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public class DelegarViabilidadServicioMock : IDelegarViabilidadServicio
    {
        public string SGR_DelegarViabilidad_ObtenerEntidades(string bpin)
        {
            return string.Empty;
        }

        public string SGR_DelegarViabilidad_ObtenerProyecto(string bpin, Nullable<Guid> instanciaId)
        {
            return string.Empty;
        }

        public Task<ReponseHttp> SGR_DelegarViabilidad_Registrar(DelegarViabilidadDto json, string usuario)
        {
            throw new NotImplementedException();
        }
    }
}