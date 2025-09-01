namespace DNP.ServiciosTransaccional.Web.API.Test.Mock
{
    using Servicios.Interfaces.Proyectos;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;
    using System.Threading.Tasks;

    public class ProyectoServicioMock : IProyectoServicio
    {
        public string Usuario { get; set; }
        public string Ip { get; set; }
        public object ActualizarEstado(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            if (parametrosActualizar.Contenido.ObjetoNegocioId.Equals("BPIN"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return true;
        }

        public object ActualizarEstadoSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            if (parametrosActualizar.Contenido.ObjetoNegocioId.Equals("BPIN"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return true;
        }

        public object IniciarFlujoSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            if (parametrosActualizar.Contenido.ObjetoNegocioId.Equals("BPIN"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return true;
        }

        public object ActualizarNombre(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            if (parametrosActualizar.Contenido.ObjetoNegocioId.Equals("200000000000000"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.PostNoExitoso);
            return new object();

        }

        Task<object> IProyectoServicio.GenerarFichaViabilidadSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            return Task.FromResult<object>(null);
        }

        public Task<string> SGR_Proyectos_GenerarMensajeEstadoProyecto(Guid instanciaId, string usuarioDnp)
        {
            if (instanciaId != default)
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return Task.FromResult("Proyecto viable");
        }

        public Task<string> SGR_CTUS_CrearInstanciaCtusSGR(ObjetoNegocio objetoNegocio, string usuarioDnp)
        {
            if (objetoNegocio.ObjetoNegocioId.Equals("BPIN"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            if (objetoNegocio.FlujoId.Equals("FlujoId"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            if (objetoNegocio.InstanciaId.Equals("InstanciaId"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            if (objetoNegocio.IdAccion.Equals("IdAccion"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            if (objetoNegocio.IdRol.Equals("IdRol"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return Task.FromResult("Creación de instancia exitosa.");
        }

        public Task<object> GenerarFichaCTUSSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            return Task.FromResult<object>(null);
        }

        public Task<string> GenerarAdjuntarFichaManualSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            ObjetoNegocio objetoNegocio = parametrosActualizar.Contenido;

            if (objetoNegocio.ObjetoNegocioId.Equals("BPIN"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            if (objetoNegocio.FlujoId.Equals("FlujoId"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            if (objetoNegocio.InstanciaId.Equals("InstanciaId"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            if (objetoNegocio.IdAccion.Equals("IdAccion"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            if (objetoNegocio.IdRol.Equals("IdRol"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return Task.FromResult("Creación de instancia exitosa.");           
        }

        public Task<bool> SGR_Proyectos_NotificarUsuariosViabilidad(Guid instanciaId, string proyectoId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<string> SGR_CTUS_CrearInstanciaCtusAutomaticaSGR(ObjetoNegocio objetoNegocio, string usuarioDnp)
        {
            if (objetoNegocio.ObjetoNegocioId.Equals("BPIN"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            if (objetoNegocio.FlujoId.Equals("FlujoId"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            if (objetoNegocio.InstanciaId.Equals("InstanciaId"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            if (objetoNegocio.IdAccion.Equals("IdAccion"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            if (objetoNegocio.IdRol.Equals("IdRol"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return Task.FromResult("Creación de instancia exitosa.");
        }

        public object IniciarFlujoSGP(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            if (parametrosActualizar.Contenido.ObjetoNegocioId.Equals("BPIN"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return true;
        }

        Task<bool> IProyectoServicio.IniciarFlujoSGP(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            throw new NotImplementedException();
        }

        public Task<object> GenerarFichaViabilidadSGP(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            return Task.FromResult<object>(true);
        }

        public Task<object> GenerarFichaGenerico(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria, int tipoFicha)
        {
            return Task.FromResult<object>(true);
        }

        Task<string> IProyectoServicio.SGR_Proyectos_PostAplicarFlujoSGR(string FlujoId, string ObjetoNegocioId, Guid instanciaId, string usuarioDnp)
        {
            return Task.FromResult("Aplicación exitosa");
        }

        Task<string> IProyectoServicio.SGR_Proyectos_PostDevolverFlujoSGR(string FlujoId, string ObjetoNegocioId, Guid instanciaId, string usuarioDnp)
        {
            return Task.FromResult("Aplicación exitosa");
        }
    }
}