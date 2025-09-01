using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public interface IRegionalizacionIndicadorAgregarPersistencia
    {
        RegionalizacionIndicadorAgregarDto ObtenerRegionalizacionIndicadorAgregar(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        RegionalizacionIndicadorAgregarDto ObtenerRegionalizacionIndicadorAgregarPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<RegionalizacionIndicadorAgregarDto> parametrosGuardar, string usuario);

    }
}
