using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Test.Mock
{
    using System;
    using Comunes.Dto.Formulario;
    public class FuenteCofinanciacionPersistenciaMock : IFuenteCofinanciacionPersistencia
    {
        public FuenteCofinanciacionProyectoDto ObtenerFuenteCofinanciacionProyecto(string bpin)

        {
            if (bpin.Equals("202000000000005"))
            {
                return new FuenteCofinanciacionProyectoDto()
                {
                    ProyectoId = 72210,
                    CodigoBPIN = "202000000000005",
                    CR = 2,
                    Cofinanciacion = new List<FuenteCofinanciacionDto> {
                        new FuenteCofinanciacionDto()
                        {
                           CofinanciadorId = 10,
                           TipoCofinanciadorId = 2,
                           TipoCofinanciador = "Rubro",
                           Cofinanciador = "RF-2021-MA"
                        }
                    },
                };
            }
            else
            {
                return null;
            }
        }

        public FuenteCofinanciacionProyectoDto ObtenerFuenteCofinanciacionProyectoPreview()
        {
            return new FuenteCofinanciacionProyectoDto()
            {
                ProyectoId = 72210,
                CodigoBPIN = "202000000000005",
                CR = 2,
                Cofinanciacion = new List<FuenteCofinanciacionDto> {
                    new FuenteCofinanciacionDto()
                    {
                        CofinanciadorId = 11,
                        TipoCofinanciadorId = 1,
                        TipoCofinanciador = "Proyecto",
                        Cofinanciador = "2017011000129"
                    }
                },
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<FuenteCofinanciacionProyectoDto> parametrosGuardar, string usuario)
        {

        }
    }
}
