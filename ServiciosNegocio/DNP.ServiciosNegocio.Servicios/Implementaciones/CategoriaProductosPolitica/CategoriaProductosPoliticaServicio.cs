using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.IndicadoresPolitica;
using DNP.ServiciosNegocio.Servicios.Interfaces.IndicadoresPolitica;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.IndicadoresPolitica
{
    public class CategoriaProductosPoliticaServicio : ICategoriaProductosPoliticaServicio
    {
        private readonly ICategoriaProductosPoliticaPersistencia _CategoriaProductosPoliticaPersistencia;

        public CategoriaProductosPoliticaServicio(ICategoriaProductosPoliticaPersistencia categoriaProductosPoliticaPersistencia) 
        {
            _CategoriaProductosPoliticaPersistencia = categoriaProductosPoliticaPersistencia;
        }

        public string ObtenerDatosCategoriaProductosPolitica(string Bpin, int fuenteId, int politicaId)
        {
            return _CategoriaProductosPoliticaPersistencia.ObtenerDatosCategoriaProductosPolitica(Bpin, fuenteId, politicaId);
        }

        public string GuardarDatosSolicitudRecursos(ParametrosGuardarDto<CategoriaProductoPoliticaDto> categoriaProductoPoliticaDto, string usuario)
        {
            return _CategoriaProductosPoliticaPersistencia.GuardarDatosSolicitudRecursos(categoriaProductoPoliticaDto, usuario);
        }
    }
}
