using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Dominio.Dto.Formulario;
    public interface IRegionalizacionIndicadoresPersistencia
    {        
        RegionalizacionIndicadorDto ObtenerRegionalizacionIndicadores(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        RegionalizacionIndicadorDto ObtenerRegionalizacionIndicadoresPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<RegionalizacionIndicadorDto> parametrosGuardar, string usuario);


    }
}
