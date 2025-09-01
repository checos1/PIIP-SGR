namespace DNP.ServiciosWBS.Persistencia.Implementaciones.Transversales
{
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosWBS.Persistencia.Interfaces.Transversales;
    using Interfaces;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SeccionCapituloPersistencia : Persistencia, ISeccionCapituloPersistencia
    {
        private readonly IFasePersistencia _fasePersistencia;

        #region Constructor
        /// <summary>
        /// Constructor de SeccionCapituloPersistencia
        /// </summary>
        /// <param name="contextoFactory"></param>
        public SeccionCapituloPersistencia(IContextoFactory contextoFactory, IFasePersistencia fasePersistencia) : base(contextoFactory)
        {            
            _fasePersistencia = fasePersistencia;
        }

        #endregion

        public CapituloModificado ObtenerSeccionCapitulo(string GuiMacroproceso, string nombreCapitulo, string nombreSeccion)
        {
            int capitulo = 2;
            int seccion = 1;
            var capituloEntidad = Contexto.Capitulo.FirstOrDefault(x => x.Nombre == nombreCapitulo);
            if (capituloEntidad != null)
                capitulo = capituloEntidad.Id;

            var seccionEntidad = Contexto.Seccion.FirstOrDefault(x => x.Nombre == nombreSeccion);
            if (seccionEntidad != null)
                seccion = seccionEntidad.Id;

            var faseId = _fasePersistencia.ObtenerFaseByGuid(GuiMacroproceso);
            var macroprocesoSeccion = Contexto.MacroprocesoSeccion.FirstOrDefault(p => p.SeccionId == seccion && p.FaseId == faseId.Id);
            var seccionCapitulo = Contexto.SeccionCapitulos.FirstOrDefault(p => p.CapituloId == capitulo && p.MacroprocesoSeccionId == macroprocesoSeccion.Id);

            var capitulosModificados = new CapituloModificado();
            capitulosModificados.SeccionCapituloId = seccionCapitulo.Id;
            capitulosModificados.CapituloId = capitulo;
            capitulosModificados.SeccionId = seccion;

            return capitulosModificados;
        }

        public bool GuardarJustificacionCambios(CapituloModificado capituloModificados)
        {
            var idActualizacion = Contexto.uspPostActualizaCapituloModificado(
                capituloModificados.ProyectoId,
                capituloModificados.Usuario,
                capituloModificados.Justificacion,
                capituloModificados.InstanciaId,
                capituloModificados.SeccionCapituloId,
                capituloModificados.AplicaJustificacion,
                capituloModificados.Cuenta,
                capituloModificados.Modificado
            );
            return idActualizacion > 0;
        }
    }
}
