namespace DNP.ServiciosTransaccional.Servicios.Interfaces.Proyectos
{
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;
    using System.Threading.Tasks;

    public interface IProyectoServicio
    {
        string Usuario { get; set; }
        string Ip { get; set; }
        object ActualizarEstado(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria);
        object ActualizarEstadoSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria);
        object IniciarFlujoSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria);
        object ActualizarNombre(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria);
        Task<object> GenerarFichaViabilidadSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria);
        Task<string> SGR_Proyectos_GenerarMensajeEstadoProyecto(Guid instanciaId, string usuarioDnp);
        Task<string> SGR_Proyectos_PostAplicarFlujoSGR(string FlujoId, string ObjetoNegocioId, Guid instanciaId, string usuarioDnp);
        Task<string> SGR_Proyectos_PostDevolverFlujoSGR(string FlujoId, string ObjetoNegocioId, Guid instanciaId, string usuarioDnp);
        Task<string> SGR_CTUS_CrearInstanciaCtusSGR(ObjetoNegocio objetoNegocio, string usuarioDnp);
        Task<object> GenerarFichaCTUSSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria);

        Task<object> GenerarFichaGenerico(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria, int tipoFicha);

        Task<string> GenerarAdjuntarFichaManualSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria);
        Task<bool> SGR_Proyectos_NotificarUsuariosViabilidad(Guid instanciaId, string proyectoId, string usuarioDnp);
        Task<string> SGR_CTUS_CrearInstanciaCtusAutomaticaSGR(ObjetoNegocio objetoNegocio, string usuarioDnp);
        Task<bool> IniciarFlujoSGP(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria);
        Task<object> GenerarFichaViabilidadSGP(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria);
    }
}
