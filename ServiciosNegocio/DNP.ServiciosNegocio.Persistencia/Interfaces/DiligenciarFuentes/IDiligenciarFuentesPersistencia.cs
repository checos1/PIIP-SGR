using DNP.ServiciosNegocio.Dominio.Dto.DiligenciarFuentes;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.DiligenciarFuentes
{
    public interface IDiligenciarFuentesPersistencia
    {
        DiligenciarFuentesProyectoDto ObtenerDiligenciarFuentesAgregar(string bpin);
    }
}
