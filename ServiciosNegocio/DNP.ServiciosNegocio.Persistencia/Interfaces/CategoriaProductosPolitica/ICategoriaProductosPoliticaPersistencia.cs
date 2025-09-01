using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.IndicadoresPolitica
{
    public interface ICategoriaProductosPoliticaPersistencia
    {
        /// <summary>
        /// Categoria Productos Politica
        /// </summary>
        /// <param name="Bpin"></param>
        /// <returns></returns>
        string ObtenerDatosCategoriaProductosPolitica(string Bpin, int fuenteId, int politicaId);

        /// <summary>
        /// Guardar Datos SolicitudRecursos
        /// </summary>
        /// <param name="categoriaProductoPoliticaDto"></param>
        /// <returns></returns>
        string GuardarDatosSolicitudRecursos(ParametrosGuardarDto<CategoriaProductoPoliticaDto> categoriaProductoPoliticaDto, string usuario);
    }
}
