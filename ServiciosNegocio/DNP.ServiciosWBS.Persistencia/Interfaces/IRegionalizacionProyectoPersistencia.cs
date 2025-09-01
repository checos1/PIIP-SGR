namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using System.Collections.Generic;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Formulario;

    public interface IRegionalizacionProyectoPersistencia
    {
        RegionalizacionProyectoDto ObtenerRegionalizacion(string bpin);
        RegionalizacionProyectoDto ObtenerRegionalizacionPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<RegionalizacionProyectoDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
