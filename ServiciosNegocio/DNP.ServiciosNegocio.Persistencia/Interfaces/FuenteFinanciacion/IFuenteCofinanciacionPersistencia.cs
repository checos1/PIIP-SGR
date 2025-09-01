using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion
{
    using System.Collections.Generic;
    using Comunes.Dto.Formulario;
    public interface IFuenteCofinanciacionPersistencia
    {
        FuenteCofinanciacionProyectoDto ObtenerFuenteCofinanciacionProyecto(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        FuenteCofinanciacionProyectoDto ObtenerFuenteCofinanciacionProyectoPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<FuenteCofinanciacionProyectoDto> parametrosGuardar, string usuario);
    }
}
