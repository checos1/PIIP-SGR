using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.DiligenciarFuentes;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.DiligenciarFuentes
{
    public interface IDiligenciarFuentesServicios
    {
        DiligenciarFuentesDto ObtenerDiligenciarFuentesProyecto(ParametrosConsultaDto parametrosConsulta);
    }
}
