namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosWBS.Persistencia.Interfaces.Transversales;
    using System;
    using System.Linq;

    // ReSharper disable once ClassNeverInstantiated.Global
    public class FasePersistencia : Persistencia, IFasePersistencia
    {
        #region Constructor

        public FasePersistencia(Interfaces.IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        #endregion

        /// <summary>
        /// Método para obtener la información de una fase por su GUID
        /// </summary>
        /// <param name="guid">Identificador GUID de la tabla [Transversal].[fase]</param>
        /// <returns>Detalle de registro fase</returns>
        public FaseDto ObtenerFaseByGuid(string guid)
        {
            var faseGuid = Guid.Parse(guid);
            return Contexto.VGetFase
                    .Where(p => p.FaseGUID == faseGuid)
                        .Select(x => new FaseDto
                        {
                            Id = x.Id,
                            NombreFase = x.NombreFase,
                            FaseGUID = x.FaseGUID
                        }).FirstOrDefault();
        }
    }
}