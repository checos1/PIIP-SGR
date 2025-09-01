using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Transversales;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public class TransversalSgpServicioMock : ITransversalServicioSGP
    {
        public EncabezadoSGPDto ObtenerEncabezadoSGP(ParametrosEncabezadoSGP parametros)
        {
            EncabezadoSGPDto objEncabezado = new EncabezadoSGPDto();
            return objEncabezado;
        }
    }
}
