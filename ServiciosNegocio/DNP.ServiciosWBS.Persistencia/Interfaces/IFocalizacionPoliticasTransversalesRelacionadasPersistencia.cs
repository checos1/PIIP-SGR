using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface IFocalizacionPoliticasTransversalesRelacionadasPersistencia
    {
        PoliticaTransversalRelacionadaDto ObtenerFocalizacionPoliticasTransversalesRelacionadas(string bpin);
        PoliticaTransversalRelacionadaDto ObtenerFocalizacionPoliticasTransversalesRelacionadasPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTransversalRelacionadaDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
