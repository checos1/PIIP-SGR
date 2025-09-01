using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionadaAjustes;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasIndicadoresCategorias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface IFocalizacionPoliticasTransversalesRelacionadasAjustesPersistencia
    {
        PoliticaTransversalRelacionadaAjustesDto ObtenerFocalizacionPoliticasTransversalesRelacionadasAjustes(string bpin);
        PoliticaTransversalRelacionadaAjustesDto ObtenerFocalizacionPoliticasTransversalesRelacionadasAjustesPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTransversalRelacionadaAjustesDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
