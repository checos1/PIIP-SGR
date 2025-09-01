using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos;

namespace DNP.ServiciosNegocio.Test.Mock
{
    using System;
    using Comunes.Dto.Formulario;
    public class DefinirAlcancePersistenciaMock : IDefinirAlcancePersistencia
    {
        public AlcanceDto ObtenerDefinirAlcance(string bpin)
        {
            var localizacionDto = new AlcanceDto();

            if (bpin.Equals("202000000000005"))
            {
                return new AlcanceDto()
                {
                    ProyectoId = 72210,
                    BPIN = "202000000000005",
                    FechaInicioEtapaInversion = DateTime.Now
                };
            }
            else
            {
                return new AlcanceDto();
            }
        }

        public AlcanceDto ObtenerDefinirAlcancePreview()
        {
            return new AlcanceDto()
            {
                ProyectoId = 72210,
                BPIN = "202000000000005",
                FechaInicioEtapaInversion = DateTime.Now
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<AlcanceDto> parametrosGuardar, string usuario)
        {

        }
    }
}
