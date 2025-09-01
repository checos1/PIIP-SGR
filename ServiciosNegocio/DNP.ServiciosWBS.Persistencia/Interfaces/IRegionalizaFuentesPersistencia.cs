using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using System.Collections.Generic;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IRegionalizaFuentesPersistencia
    {
        FuenteFinanciacionRegionalizacionDto ObtenerFuenteFinanciacionRegionalizacion(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        FuenteFinanciacionRegionalizacionDto ObtenerFuenteFinanciacionRegionalizacionPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<FuenteFinanciacionRegionalizacionDto> parametrosGuardar, string usuario);
    }
}
