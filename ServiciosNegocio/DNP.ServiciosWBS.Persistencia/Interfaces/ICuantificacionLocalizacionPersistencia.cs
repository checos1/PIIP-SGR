using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{

    using ServiciosNegocio.Dominio.Dto.Poblacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface ICuantificacionLocalizacionPersistencia 
    {
        PoblacionDto ObtenerCuantificacionLocalizacion(string bpin);        
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        PoblacionDto ObtenerCuantificacionLocalizacionPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<PoblacionDto> parametrosGuardar, string usuario);

    }
}
