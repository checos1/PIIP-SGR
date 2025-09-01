namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Transversales
{
    using DNP.ServiciosNegocio.Comunes.Utilidades;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using Interfaces;
    using Interfaces.Proyectos;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class EstadoPersistencia : Persistencia, IEstadoPersistencia
    {
        #region Constructor

        public EstadoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        #endregion


        public List<EstadoDto> ObtenerListaEstado()
        {
            var resultSp = Contexto.uspGetEstados().ToList();

            var estados = new List<EstadoDto>();
            
            estados = resultSp.Select(est => new EstadoDto()
            {
                Id = est.Id,
                Estado = est.Estado,
                Codigo = est.Codigo
            }).ToList();

            return estados;
        }

       

    }
}
