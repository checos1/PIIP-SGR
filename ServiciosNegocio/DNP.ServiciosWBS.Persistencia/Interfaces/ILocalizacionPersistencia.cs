using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface ILocalizacionPersistencia
    {
        LocalizacionProyectoDto Obtenerlocalizacion(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        LocalizacionProyectoDto ObtenerlocalizacionPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<LocalizacionProyectoDto> parametrosGuardar, string usuario);
        ResultadoProcedimientoDto GuardarLocalizacion(LocalizacionProyectoAjusteDto localizacionProyecto, string usuario);



    }
}
