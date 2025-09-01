using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.DiligenciarFuentes;
using DNP.ServiciosWBS.Persistencia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface IDiligenciarFuentesPersistencia
    {
        DiligenciarFuentesProyectoDto ObtenerDiligenciarFuentes(string bpin);        
        DiligenciarFuentesProyectoDto ObtenerDiligenciarFuentesPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<DiligenciarFuentesProyectoDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
