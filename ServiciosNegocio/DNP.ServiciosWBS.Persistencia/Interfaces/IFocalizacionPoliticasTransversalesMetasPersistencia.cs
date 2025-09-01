using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTranversalesMetas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface IFocalizacionPoliticasTransversalesMetasPersistencia
    {
        PoliticaTMetasDto ObtenerFocalizacionPoliticasTransversales(string bpin);
        PoliticaTMetasDto ObtenerFocalizacionPoliticasTransversalesPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTMetasDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
