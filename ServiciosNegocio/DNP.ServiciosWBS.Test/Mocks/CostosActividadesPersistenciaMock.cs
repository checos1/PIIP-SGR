namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using DNP.ServiciosNegocio.Dominio.Dto.CostosActividades;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using ServiciosNegocio.Dominio.Dto.Indicadores;
    public class CostosActividadesPersistenciaMock : ICostosActividadesPersistencia
    {       
        public CostosActividadesDto ObtenerCostosActividades(string bpin)

        {
            if (bpin.Equals("202000000000005"))
            {
                return  new CostosActividadesDto()
                {
                    ProyectoId = 72210,
                    BPIN = "202000000000005",

                    vigencias = new List<Vigencia> {
                        new Vigencia()
                        {
                          vigencia = 2020
                        }
                    },
                };
            }
            else
            {
                return null;
            }
        }

        public CostosActividadesDto ObtenerCostosActividadesPreview()
        {
            return new CostosActividadesDto()
            {
                ProyectoId = 72210,
                BPIN = "202000000000005",

                vigencias = new List<Vigencia> {
                        new Vigencia()
                        {
                          vigencia = 2020
                        }
                    },
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<CostosActividadesDto> parametrosGuardar, string usuario)
        {

        }

    }
}
