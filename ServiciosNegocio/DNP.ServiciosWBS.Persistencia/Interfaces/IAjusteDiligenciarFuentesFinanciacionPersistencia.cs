using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface IAjusteDiligenciarFuentesFinanciacionPersistencia
    {
        FuentesFinanciacionAjusteDto ObtenerFuenteFinanciacionAjuste(string bpin);
        FuentesFinanciacionAjusteDto ObtenerFuenteFinanciacionAjustePreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<FuentesFinanciacionAjusteDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
