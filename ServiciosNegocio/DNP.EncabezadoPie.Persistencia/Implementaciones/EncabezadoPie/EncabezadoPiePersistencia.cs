
namespace DNP.EncabezadoPie.Persistencia.Implementaciones.EncabezadoPie
{
    using System.Collections.Generic;
    using DNP.EncabezadoPie.Dominio.Dto;
    using Interfaces;
    using Interfaces.EncabezadoPie;
    using System.Linq;
    using DNP.ServiciosNegocio.Comunes.Utilidades;
    using System;
    using DNP.ServiciosNegocio.Comunes;
    using Newtonsoft.Json;

    public class EncabezadoPiePersistencia : Persistencia, IEncabezadoPiePersistencia
    {
        public EncabezadoPiePersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public EncabezadoPieBasicoDto ConsultarEncabezadoPieBasico(ParametrosEncabezadoPieDto parametros)
        {
            var listaEncabezadoPieBasico = new EncabezadoPieBasicoDto();
            var listaEntidades = Contexto.Proyecto
                                         .Include("Sector")
                                         .Include("EntityTypeCatalogOption")
                                         .Where(c => parametros.Id == null || c.Id == parametros.Id )
                                         .Where(c => parametros.Bpin.ToString() == "" || c.BPIN == parametros.Bpin.ToString())
                                         .Select( c => new EncabezadoPieBasicoDto
                                         {
                                             Id = c.Id,
                                             Bpin = c.BPIN,
                                             Sector = c.Sector.Description,
                                             Entidad = c.Name,
                                             AnioInicio = (int)c.PeriodZero
                                         })
                                         .FirstOrDefault();

            return (listaEntidades == null ? listaEncabezadoPieBasico : listaEntidades);
        }

        public EncabezadoPieBasicoDto ConsultarEncabezadoPieBasicoPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<EncabezadoPieBasicoDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaEncabezadoPieBasico);
        }

        public EncabezadoGeneralDto ObtenerEncabezadoGeneral(ParametrosEncabezadoGeneral parametros)
        {
             //bool EsBpin = false;
            //if (!string.IsNullOrEmpty(parametros.idProyectoStr))
            //{
            //    if (parametros.idProyectoStr.Length > 8)
            //        EsBpin = true;
            //    else
            //        parametros.idProyecto = Convert.ToInt32(parametros.idProyectoStr);
            //}
            //else
            //    parametros.idProyecto = 0;

            ////try { parametros.idProyecto = parametros.idProyectoStr != "" ? Convert.ToInt32(parametros.idProyectoStr) : 0; } catch { EsBpin = true; }
            //if (EsBpin)
            //    parametros.idProyecto = ConsultarIdProyecto(parametros.idProyectoStr);
            var proyecto = parametros.idProyecto == 0 ? Int32.Parse(parametros.idProyectoStr) : parametros.idProyecto;
            var r = Contexto.UspGetEncabezado(parametros.idInstancia, parametros.idFlujo, parametros.idNivel, proyecto, parametros.tramite).FirstOrDefault();

            if (r == null)
                return new EncabezadoGeneralDto();

            var EncabezadoGeneral = new EncabezadoGeneralDto()
            {
                CodigoProceso = r.Proceso,
                Fecha = r.Fecha,
                Tipo = r.Tipo,
                ProyectoId = r.ProyectoId,
                NombreProyecto = r.NombreProyecto,
                CodBPIN = r.BPIN,
                vigenciaInicial = r.VigenciaInicial,
                vigenciaFinal = r.VigenciaFinal,
                entidad = r.Entidad,
                Estado = r.Estado,
                Horizonte = r.Horizonte,
                sector = r.Sector,
                valorTotal = r.Valor,
                apropiacionInicial = r.ApropiacionInicial,
                apropiacionVigente = r.ApropiacionVigente,
                ValorTotalConTramiteActual = r.ValorTotalConTramiteActual,
                ApropiacionVigenteConTramiteActual = r.ApropiacionVigenteConTramiteActual,
                ContieneTramite = r.ContieneTramite,
                TramiteId = r.TramiteId,
                Alcanceproyecto = r.AlcanceProyecto,
                CostoTotalProyecto = r.CostoTotalProyecto,
                Ejecutor = r.Ejecutor,
                FechaRealInicio = r.FechaRealInicio,
                AplicaEjecucionPlaneacion = r.AplicaEjecucionPlaneacion,
                TipoId = r.TipoId,
                IdTipoTramitePresupuestal = r.IdTipoTramitePresupuestal,
                PeriodoAbierto = r.PeriodoAbierto,
                FechaInicioReporte= r.FechaInicioReporte,
                FechaLimiteReporte = r.FechaLimiteReporte
            };


            return EncabezadoGeneral;
        }

        public int ConsultarIdProyecto(string BPIN)
        {
            int idProyecto = 0;
            var proyecto = Contexto.Proyecto.FirstOrDefault(x => x.BPIN == BPIN);
            if (proyecto != null)
                idProyecto = proyecto.Id;


            return idProyecto;
        }

    }
}
