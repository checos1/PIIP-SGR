using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.DelegarViabilidad;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGR.DelegarViabilidad
{
    public class DelegarViabilidadPersistencia : Persistencia, IDelegarViabilidadPersistencia
    {
        #region Constructor

        public DelegarViabilidadPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        #endregion

        #region Metodos

        public string SGR_DelegarViabilidad_ObtenerProyecto(string bpin, Nullable<Guid> instanciaId)
        {
            var listaFuentes = Contexto.uspGetProyectoDelegarViabilidad_JSON(bpin, instanciaId).FirstOrDefault();
            return listaFuentes;
        }

        public string SGR_DelegarViabilidad_ObtenerEntidades(string bpin)
        {
            var listaFuentes = Contexto.uspGetProyectoEntidadesDelegar_JSON(bpin).FirstOrDefault();
            return listaFuentes;
        }

        public void SGR_DelegarViabilidad_Registrar(DelegarViabilidadDto json, string usuario)
        {
            var errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            try
            {
                using (var tx = Contexto.Database.BeginTransaction())
                {
                    var jsonModel = JsonUtilidades.ACadenaJson(json);

                    var result = Contexto.uspPostDelegarViabilidadAgregar(jsonModel, usuario, errorValidacionNegocio);
                    if (!string.IsNullOrEmpty(Convert.ToString(errorValidacionNegocio.Value)))
                    {
                        tx.Rollback();
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }

                    tx.Commit();
                }
            }
            catch (Exception e)
            {
                string erorr = e.InnerException == null ? e.Message : e.InnerException.Message;
                throw new ServiciosNegocioException(erorr);
            }
        }

        #endregion
    }
}
