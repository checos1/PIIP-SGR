using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Programacion;

namespace DNP.ServiciosTransaccional.Web.API.Test.Mock
{
    public class ProgramacionServicioMock: IProgramacionServicio
    {
        public TramitesResultado GuardarDatosProgramacionDistribucion(string NumeroTramite, string usuario)
        {
            var resultado = new TramitesResultado();

            if (NumeroTramite != "")
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Numero de tramite viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public TramitesResultado InclusionFuentesProgramacion(string NumeroTramite, string usuario)
        {
            var resultado = new TramitesResultado();

            if (NumeroTramite != "")
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Numero de tramite viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

    }
}
