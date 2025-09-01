using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos;


namespace DNP.ServiciosNegocio.Test.Mock
{

    using System;
    using Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    public class DevolverProyectoPersistenciaMock : IDevolverProyectoPersistencia
    {

        public void GuardarDefinitivamente(ParametrosGuardarDto<DevolverProyectoDto> parametrosGuardar, string usuario)
        {

        }

        public DevolverProyectoDto ObtenerDevolverProyecto(string bpin)
        {
            if (bpin.Equals("2017005500250"))
            {
                return new DevolverProyectoDto()
                {
                    Bpin = "2017005500250",
                    ProyectoId = 42073,
                    Observacion = "Prueba devolver",
                    DevolverId = false,
                    EstadoDevolver = 7 
                };
            }
            else
            {
                return null;
            }
        }

    }
}
