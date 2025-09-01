namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Transversales
{
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using Interfaces;
    using System;
    using System.Data.SqlClient;
    using System.Linq;

    public class DocumentoSoportePersistencia : Persistencia, IDocumentoSoportePersistencia
    {
        #region Constructor

        public DocumentoSoportePersistencia(IContextoFactory contextoFactory) : base(contextoFactory) { 
        
        }

        #endregion

        /// <summary>
        /// Obtiene lista de documento soporte por rol
        /// </summary>     
        /// <param name="tipoTramiteId"></param>   
        /// <param name="roles"></param>   
        /// <param name="tramiteId"></param>   
        /// <param name="nivelId"></param>  
        /// <param name="instanciaId"></param> 
        /// <param name="accionId"></param> 
        /// <returns>string</returns> 
        public string ObtenerListaTipoDocumentosSoportePorRolTrv(int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId)
        {
            var jsonLista = Contexto.Database.SqlQuery<string>("Transversal.uspGet_ListaTipoDocumentosSoportePorRolTrv @tipoTramiteId, @rol, @nivelId, @instanciaId, @accionId, @tramiteId",
                                               new SqlParameter("tipoTramiteId", tipoTramiteId),
                                               new SqlParameter("rol", roles),
                                               new SqlParameter("nivelId", nivelId),
                                               new SqlParameter("instanciaId", instanciaId),
                                               new SqlParameter("accionId", accionId),
                                               new SqlParameter("tramiteId", tramiteId)
                                                ).FirstOrDefault();
            return jsonLista;
        }
    }
}
