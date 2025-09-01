using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SeguimientoControl
{
    public class ReporteAvanceRegionalizacionPersistencia : Persistencia, IReporteAvanceRegionalizacionPersistencia
    {

        #region Constructor

        /// <summary>
        /// Constructor de DesagregarEdtPersistencia
        /// </summary>
        /// <param name=\\\\"contextoFactory\\\\"></param>
        public ReporteAvanceRegionalizacionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        #endregion

        #region Get
        public string ConsultarAvanceRegionalizacion(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad)
        {
            try
            {
                var resultSp = ContextoOnlySP.uspGetRegionalizacionProgramacionSeguimiento_JSON(instanciaId, proyectoId, codigoBpin, vigencia, periodoPeriodicidad).FirstOrDefault();
                return resultSp;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion


        #region Post

        //public void ActualizarAvanceMetaProducto(AvanceMetaProductoDto IndicadorDto, string usuario)
        //{
        //    var errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

        //    try
        //    {
        //        using (var tx = ContextoOnlySP.Database.BeginTransaction())
        //        {
        //            var jsonModel = JsonUtilidades.ACadenaJson(IndicadorDto);

        //            var result = ContextoOnlySP.uspPostActualizarAvanceMetaProducto(jsonModel, usuario, errorValidacionNegocio);
        //            if (!string.IsNullOrEmpty(Convert.ToString(errorValidacionNegocio.Value)))
        //            {
        //                tx.Rollback();
        //                var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
        //                throw new ServiciosNegocioException(mensajeError);
        //            }
        //            tx.Commit();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        string erorr = e.InnerException == null ? e.Message : e.InnerException.Message;
        //        throw new ServiciosNegocioException(erorr);
        //    }
        //}
        #endregion





    }
}
