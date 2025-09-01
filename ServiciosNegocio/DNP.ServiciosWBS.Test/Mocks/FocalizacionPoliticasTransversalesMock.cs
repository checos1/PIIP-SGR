using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;

namespace DNP.ServiciosWBS.Test.Mocks
{
    public class FocalizacionPoliticasTransversalesMock
    {
        public PoliticaTRelacionadasDto ObtenerFocalizacionPoliticasTransversalesDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return new PoliticaTRelacionadasDto()
            {
                BPIN = "202000000000005"
            };

        }
        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }
        public void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTRelacionadasDto> parametrosGuardar, string usuario)
        {
        }
        public PoliticaTRelacionadasDto DiligenciarFocalizacionPoliticasTransversalesPreview()
        {
            return new PoliticaTRelacionadasDto()
            {
                BPIN = "202000000000005"
            };
        }
    }
}
