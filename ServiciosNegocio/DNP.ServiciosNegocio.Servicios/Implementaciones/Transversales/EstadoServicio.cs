namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Transversales
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using Comunes.Dto.ObjetosNegocio;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using Dominio.Dto.Proyectos;
    using Interfaces.Proyectos;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Proyectos;

    /// <summary>
    /// Clase responsable de la gestión o estado del proyectos
    /// </summary>
    public class EstadoServicio : IEstadoServicio
    {
        private readonly IEstadoPersistencia _estadoPersistencia;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="estadoPersistencia">Instancia de persistencia de estados</param>        
        public EstadoServicio(IEstadoPersistencia estadoPersistencia)
        {
            _estadoPersistencia = estadoPersistencia;
        }

        /// <summary>
        /// Lista de estados
        /// </summary>
        /// <returns>Lista de estados del proyectos.</returns>
        public Task<List<EstadoDto>> ConsultarEstados()
        {
            var estados = new List<EstadoDto>();
            
            estados = _estadoPersistencia.ObtenerListaEstado();
            
            if (estados.Count == 0) return Task.FromResult<List<EstadoDto>>(null);
            return Task.FromResult(estados);
        }

       

    }
}
