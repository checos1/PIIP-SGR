using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
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
    public class GestionSeguimientoPersistencia : Persistencia, IGestionSeguimientoPersistencia
    {
        #region Constructor

        /// <summary>
        /// Constructor de DesagregarEdtPersistencia
        /// </summary>
        /// <param name=\\\\"contextoFactory\\\\"></param>
        public GestionSeguimientoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
           
        }

        #endregion

        #region Get
        public List<TransversalSeguimientoDto> ObtenerListadoUnidadesMedida()
        {
            var unidades = new List<TransversalSeguimientoDto>();
            var unidadesDb = Contexto.uspGetUnidadesMedidas();
            unidades = unidadesDb.Select(est => new TransversalSeguimientoDto()
            {
                Id = est.ID,
                Text = est.Description
            }).ToList();
            return unidades;
        }

        #endregion

        #region Post
        public List<ErroresProyectoDto> ObtenerErroresProyecto(GestionSeguimientoDto proyectoParams)
        {
            var eroresProyecto = new List<ErroresProyectoDto>();

            var datosFase = new DataProyectoFaseDto();
            var datosFaseBD = Contexto.UspGetDatosFaseMacroproceso(proyectoParams.GuidMacroproceso,
                                         proyectoParams.GuidInstancia, 
                                         proyectoParams.IdProyecto).FirstOrDefault();

            datosFase.FaseId =datosFaseBD.FaseId;
            datosFase.NivelPaso = Convert.ToString(datosFaseBD.NivelPaso);
            datosFase.PasoFaseId = datosFaseBD.PasoFaseId;
            datosFase.FaseMacroproceso = datosFaseBD.FaseMacroproceso;
            datosFase.FaseProceso = datosFaseBD.FaseProceso;
            datosFase.GuidInstancia = proyectoParams.GuidInstancia;
            datosFase.ProyectoId = proyectoParams.IdProyecto;

            /* -- Sección Desagregar -- */
            var seccionDesagregarErrores = GetErroresDesagregarEdt(datosFase);
            if (seccionDesagregarErrores.Count > 0) eroresProyecto.AddRange(seccionDesagregarErrores);

            return eroresProyecto;
        }
        #endregion

        #region Delete
        public void EliminarActividad(string usuario, List<RegistroEntregable> nivelesNuevos)
        {
            var errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            try
            {
                using (var tx = Contexto.Database.BeginTransaction())
                {
                    var jsonModel = JsonUtilidades.ACadenaJson(nivelesNuevos);

                    var result = Contexto.UspPostEliminarNivelesProductos(usuario, jsonModel, errorValidacionNegocio);
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
                throw new ServiciosNegocioException(e.Message);
            }

        }
        #endregion

        #region ObtenerErroresPorSeccion
        private List<ErroresProyectoDto> GetErroresDesagregarEdt(DataProyectoFaseDto datosFase) {
            var errores = new List<ErroresProyectoDto>();
            try
            {
                var erroresBD = Contexto.UspGetErroresProyectoSeccionDesagregarEdt(
                    Guid.Parse(datosFase.GuidInstancia),
                    datosFase.ProyectoId,
                    datosFase.FaseId,
                    datosFase.PasoFaseId);

                if (erroresBD != null)
                {
                    errores = erroresBD.Select(est => new ErroresProyectoDto()
                    {
                        Seccion = est.Seccion,
                        Capitulo = est.Capitulo,
                        Errores = est.Errores
                    }).ToList();
                }


                /* ----------- Agregar Validaciones complejas -------------*/         
                var indxDesagregarCap = errores.FindIndex(p => p.Capitulo == "desagregarcap");
                if(indxDesagregarCap != -1)
                    errores[indxDesagregarCap].Errores = GetErroresDesagregarEdt(datosFase, indxDesagregarCap);

            }
            catch (Exception e) {
                return new List<ErroresProyectoDto>();
            }
            errores = errores.Where(p => p.Errores != null).ToList();
            return errores;

        }
        #endregion

        #region ObtenerErroresPorCapitulo
        private string GetErroresDesagregarEdt(DataProyectoFaseDto datosFase, int indexCap)
        {
            var errores = string.Empty;
            try
            {
                var capituloDesagregarErrores = Contexto.UspValidacionDescargregarEdt(datosFase.ProyectoId).ToList();
                if (capituloDesagregarErrores != null)
                {
                    if (capituloDesagregarErrores.Count <= 0) return null;

                    var desagregarObj = new
                    {
                        desagregaredtdesagregarcap = capituloDesagregarErrores
                    };
                    return JsonConvert.SerializeObject(desagregarObj);
                }
            }
            catch (Exception e)
            {
                return string.Empty;
            }

            return errores;
        }
        #endregion
    }
}

