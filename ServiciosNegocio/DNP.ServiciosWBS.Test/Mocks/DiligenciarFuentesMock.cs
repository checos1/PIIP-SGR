using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.DiligenciarFuentes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Test.Mocks
{
    public class DiligenciarFuentesMock
    {
        public DiligenciarFuentesProyectoDto ObtenerProductoDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return new DiligenciarFuentesProyectoDto()
            {
                BPIN = "202000000000005"
            };

        }
        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }
        public void GuardarDefinitivamente(ParametrosGuardarDto<DiligenciarFuentesProyectoDto> parametrosGuardar, string usuario)
        {
        }
        public DiligenciarFuentesProyectoDto DiligenciarFuentesPreview()
        {
            return new DiligenciarFuentesProyectoDto()
            {
                BPIN = "202000000000005"
            };
        }
    }
}
