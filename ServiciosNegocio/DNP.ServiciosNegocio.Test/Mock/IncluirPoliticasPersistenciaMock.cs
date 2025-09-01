using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos;

namespace DNP.ServiciosNegocio.Test.Mock
{
    using System;
    using Comunes.Dto.Formulario;
    public class IncluirPoliticasPersistenciaMock : IIncluirPoliticasPersistencia
    {
        public IncluirPoliticasDto ObtenerIncluirPoliticas(string bpin)
        {
            var IncluirPoliticasDto = new IncluirPoliticasDto();

            if (bpin.Equals("202000000000005"))
            {
                return new IncluirPoliticasDto()
                {
                    ProyectoId = 72210,
                    BPIN = "202000000000005",
                };
            }
            else
            {
                return new IncluirPoliticasDto();
            }
        }

        public IncluirPoliticasDto ObtenerIncluirPoliticasPreview()
        {
            return new IncluirPoliticasDto()
            {
                ProyectoId = 72210,
                BPIN = "202000000000005",
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {

        }
    }
}
