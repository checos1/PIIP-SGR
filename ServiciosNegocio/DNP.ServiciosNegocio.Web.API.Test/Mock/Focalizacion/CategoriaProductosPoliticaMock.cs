namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Servicios.Interfaces.IndicadoresPolitica;
    public class CategoriaProductosPoliticaMock : ICategoriaProductosPoliticaServicio
    {
        public string ObtenerDatosCategoriaProductosPolitica(string Bpin, int fuenteId, int politicaId)
        {
            return string.Empty;
        }

        public string GuardarDatosSolicitudRecursos(ParametrosGuardarDto<CategoriaProductoPoliticaDto> categoriaProductoPoliticaDto, string usuario)
        {
            return string.Empty;
        }
    }
}

