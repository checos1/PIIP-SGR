using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesAsociacionIndicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface IFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia
    {
        PoliticaTIndicadoresDto ObtenerFocalizacionPoliticasTransversalesAsociacionIndicadores(string bpin);
        PoliticaTIndicadoresDto ObtenerFocalizacionPoliticasTransversalesAsociacionIndicadoresPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTIndicadoresDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
