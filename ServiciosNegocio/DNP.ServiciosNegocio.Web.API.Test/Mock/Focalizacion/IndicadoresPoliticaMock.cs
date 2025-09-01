namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Servicios.Interfaces.IndicadoresPolitica;
    public class IndicadoresPoliticaMock : IIndicadoresPoliticaServicio
    {
        public string ObtenerDatosIndicadoresPolitica(string Bpin)
        {
            return string.Empty;
        }
    }
}

