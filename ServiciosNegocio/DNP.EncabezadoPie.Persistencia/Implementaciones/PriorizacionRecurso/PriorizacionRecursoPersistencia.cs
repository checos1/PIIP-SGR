namespace DNP.EncabezadoPie.Persistencia.Implementaciones.PriorizacionRecurso
{
    using System.Collections.Generic;
    using DNP.EncabezadoPie.Dominio.Dto.PriorizacionRecurso;
    using DNP.EncabezadoPie.Persistencia.Interfaces.PriorizacionRecurso;
    using Interfaces;
    using System.Linq;
    using DNP.ServiciosNegocio.Comunes.Utilidades;
    using System;
    using DNP.ServiciosNegocio.Comunes;
    using Newtonsoft.Json;


    public class PriorizacionRecursoPersistencia : Persistencia, IPriorizacionRecursoPersistencia
    {
        public PriorizacionRecursoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public PriorizacionRecursoDto ObtenerPriorizacionRecurso(string bpin)
        {
            var listadoFuentesFinanciacion = Contexto.GetEncForm_PrioRec_JSON(bpin).SingleOrDefault();
            return JsonConvert.DeserializeObject<PriorizacionRecursoDto>(listadoFuentesFinanciacion);
        }

        public PriorizacionRecursoDto ObtenerPriorizacionRecursoPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<PriorizacionRecursoDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                       @RutasPreviewRecursos.RutaPreviewPriorizacionRecurso);
        }
    }
}
