using System.Collections.Generic;
using DNP.ServiciosNegocio.Dominio.Dto.Formulario;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Formulario
{
    using Comunes.Dto.Formulario;

    public interface IRegionalizacionProyectoPersistencia
    {
        List<RegionalizacionDto> ObtenerRegionalizacion(string bpin);
        List<RegionalizacionDto> ObtenerRegionalizacionPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<RegionalizacionProyectoDto> parametrosGuardar, string usuario);       
    }
}
