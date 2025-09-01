namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using ServiciosNegocio.Dominio.Dto.Indicadores;
    public class RegionalizaFuentesPersistenciaMock : IRegionalizaFuentesPersistencia
    {       
        public FuenteFinanciacionRegionalizacionDto ObtenerFuenteFinanciacionRegionalizacion(string bpin)

        {
            if (bpin.Equals("202000000000005"))
            {
                return new FuenteFinanciacionRegionalizacionDto()
                {
                    ProyectoId = 72210,
                    BPIN = "202000000000005",
                    CR = 2,
                    Regionalizacion = new List<RegionalizacionDto> {
                        new RegionalizacionDto()
                        {
                           FuenteFinanciacion = "Territorial - Boyacá - Propios",
                           Vigencia = 2019,
                           ValorSolicitado = 427352437
                        }
                    },
                };
            }
            else
            {
                return null;
            }
        }

        public FuenteFinanciacionRegionalizacionDto ObtenerFuenteFinanciacionRegionalizacionPreview()
        {
            return new FuenteFinanciacionRegionalizacionDto()
            {
                ProyectoId = 72210,
                BPIN = "202000000000005",
                CR = 2,
                Regionalizacion = new List<RegionalizacionDto> {
                        new RegionalizacionDto()
                        {
                           FuenteFinanciacion = "Territorial - Boyacá - Propios",
                           Vigencia = 2019,
                           ValorSolicitado = 427352437
                        }
                    },
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<FuenteFinanciacionRegionalizacionDto> parametrosGuardar, string usuario)
        {

        }

    }
}
