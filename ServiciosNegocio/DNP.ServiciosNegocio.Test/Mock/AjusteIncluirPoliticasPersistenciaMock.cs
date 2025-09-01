using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos;

namespace DNP.ServiciosNegocio.Test.Mock
{
    using System;
    using Comunes.Dto.Formulario;
    public class AjusteIncluirPoliticasPersistenciaMock : IAjusteIncluirPoliticasPersistencia
    {

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {

        }

        public IncluirPoliticasDto ObtenerAjusteIncluirPoliticas(string bpin)
        {
            throw new NotImplementedException();
        }

        public IncluirPoliticasDto ObtenerAjusteIncluirPoliticasPreview()
        {
            return new IncluirPoliticasDto
            {
                ProyectoId = 72210,
                BPIN = "202000000000005",
                Politicas = new List<Politicas>()

            };
        }
    }
}