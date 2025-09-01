namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using ServiciosNegocio.Dominio.Dto.Indicadores;
    public class AjustesRegionalizaFuentesPersistenciaMock : IAjustesRegionalizaFuentesPersistencia
    {
        public AjustesRegionalizaFuentesDto ObtenerAjustesRegionalizaFuentes(string bpin)

        {
            if (bpin.Equals("202000000000005"))
            {
                return new AjustesRegionalizaFuentesDto()
                {
                    ProyectoId = 72210,
                    BPIN = "202000000000005",
                    Regionalizacion = new List<AjustesRegionalizaFuentesRegionalizacionDto> {
                        new AjustesRegionalizaFuentesRegionalizacionDto()
                        {
                           Vigencia = 2019
                        }
                    },
                };
            }
            else
            {
                return null;
            }
        }

        public AjustesRegionalizaFuentesDto ObtenerAjustesRegionalizaFuentesPreview()
        {
            return new AjustesRegionalizaFuentesDto()
            {
                ProyectoId = 72210,
                BPIN = "202000000000005",
                Regionalizacion = new List<AjustesRegionalizaFuentesRegionalizacionDto> {
                        new AjustesRegionalizaFuentesRegionalizacionDto()
                        {
                           Vigencia = 2019
                        }
                    },
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<AjustesRegionalizaFuentesDto> parametrosGuardar, string usuario)
        {

        }
    }
}
