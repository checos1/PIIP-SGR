using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using System;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SGR.DelegarViabilidad
{
    public interface IDelegarViabilidadServicio
    {
        string SGR_DelegarViabilidad_ObtenerProyecto(string bpin, Nullable<Guid> instanciaId);
        string SGR_DelegarViabilidad_ObtenerEntidades(string bpin);
        Task<ReponseHttp> SGR_DelegarViabilidad_Registrar(DelegarViabilidadDto json, string usuario);
    }
}
