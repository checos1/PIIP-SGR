namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using DNP.ServiciosNegocio.Dominio.Dto.CostosEntregables;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public class CostosEntregablesPersistenciaMock : ICostosEntregablesPersistencia
    {       
        public CostosEntregablesDto ObtenerCostosEntregables(string bpin)

        {
            if (bpin.Equals("202000000000005"))
            {
                return  new CostosEntregablesDto()
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

        public CostosEntregablesDto ObtenerCostosEntregablesPreview()
        {
            return new CostosEntregablesDto()
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

        public void GuardarDefinitivamente(ParametrosGuardarDto<CostosEntregablesDto> parametrosGuardar, string usuario)
        {

        }

    }
}
