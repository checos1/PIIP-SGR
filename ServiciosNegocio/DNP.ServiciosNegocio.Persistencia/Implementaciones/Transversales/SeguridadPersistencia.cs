using DNP.ServiciosNegocio.Dominio.Dto.Acciones;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Transversales
{
    public class SeguridadPersistencia : Persistencia, ISeguridadPersistencia
    {

        #region Constructor

        /// <summary>
        /// Constructor de DesagregarEdtPersistencia
        /// </summary>
        /// <param name=\\\\"contextoFactory\\\\"></param>
        public SeguridadPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        #endregion

        #region Get
        public string PermisosAccionPaso(AccionFlujoDto accionFlujoDto)
        {                                                                
            var resultSp = ContextoOnlySP.uspGetPermisosAccionPaso_JSON(accionFlujoDto.IdInstancia, accionFlujoDto.IdAcccion, accionFlujoDto.ObjetoNegocioId, accionFlujoDto.UsuarioDNP, accionFlujoDto.ObjetoJson).SingleOrDefault();
            return resultSp;
        }

        #endregion


    }
}
