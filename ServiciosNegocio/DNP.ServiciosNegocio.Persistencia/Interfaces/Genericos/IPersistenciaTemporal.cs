using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Persistencia.Modelo;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Genericos
{


    public interface IPersistenciaTemporal
    {
        void GuardarTemporalmente<T>(ParametrosGuardarDto<T> parametrosGuardar);
        AlmacenamientoTemporal ObtenerTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
