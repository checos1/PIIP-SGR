using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.IndicadoresPolitica
{
    public interface ICategoriaProductosPoliticaServicio
    {
        #region Obtener Datos Categoria Productos Politica

        /// <summary>
        /// Obtener Datos Categoria Productos Politica
        /// </summary>
        /// <param name="Bpin"></param>
        /// <returns></returns>
        string ObtenerDatosCategoriaProductosPolitica(string Bpin, int fuenteId, int politicaId);

        #endregion

        #region Guardar Datos Solicitud Recursos

        /// <summary>
        /// Guardar Datos Solicitud Recursos
        /// </summary>
        /// <param name="Bpin"></param>
        /// <returns></returns>
        string GuardarDatosSolicitudRecursos(ParametrosGuardarDto<CategoriaProductoPoliticaDto> categoriaProductoPoliticaDto, string usuario);

        #endregion
    }
}
