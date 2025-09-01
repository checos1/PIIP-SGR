using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Test.Mock
{
    using System;
    using Comunes.Dto.Formulario;
    public class CofinanciacionAgregarPersistenciaMock : ICofinanciacionAgregarPersistencia
    {
        public CofinanciacionProyectoDto ObtenerCofinanciacionAgregar(string bpin)
        {
            if (bpin.Equals("202000000000005"))
            {
                return new CofinanciacionProyectoDto()
                {
                    ProyectoId = 72210,
                    CodigoBPIN = "202000000000005",
                    CR = 2,
                    Cofinanciacion = new List<CofinanciacionDto> {
                        new CofinanciacionDto()
                        {
                           ProyectoCofinanciadorId = 1,
                           TipoCofinanciadorId = 2,
                           TipoCofinanciador = "Rubro",
                           CofinanciadorId = "1"
                        }
                    },
                };
            }
            else
            {
                return null;
            }
        }

        public CofinanciacionProyectoDto ObtenerCofinanciacionAgregarPreview()
        {
            return new CofinanciacionProyectoDto()
            {
                ProyectoId = 72210,
                CodigoBPIN = "202000000000005",
                CR = 2,
                Cofinanciacion = new List<CofinanciacionDto> {
                        new CofinanciacionDto()
                        {
                           ProyectoCofinanciadorId = null,
                           TipoCofinanciadorId = 1,
                           TipoCofinanciador = "Proyecto",
                           CofinanciadorId = "2017011000129"
                        }
                    },
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<CofinanciacionProyectoDto> parametrosGuardar, string usuario)
        {

        }
    }
}
