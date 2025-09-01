using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using System;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.DelegarViabilidad
{
    public interface IDelegarViabilidadPersistencia
    {
        string SGR_DelegarViabilidad_ObtenerProyecto(string bpin, Nullable<Guid> instanciaId);
        string SGR_DelegarViabilidad_ObtenerEntidades(string bpin);
        void SGR_DelegarViabilidad_Registrar(DelegarViabilidadDto json, string usuario);
    }
}
