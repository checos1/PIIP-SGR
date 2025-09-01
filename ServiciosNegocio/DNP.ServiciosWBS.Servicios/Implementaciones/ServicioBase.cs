namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces.Transversales;
    using Newtonsoft.Json;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Enum;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Auditoria;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public abstract class ServicioBase<T> where T : class
    {
        private readonly IPersistenciaTemporal _persistenciaTemporal;
        private readonly IAuditoriaServicios _auditoriaServicios;

        protected ServicioBase(IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios)
        {
            _persistenciaTemporal = persistenciaTemporal;
            _auditoriaServicios = auditoriaServicios;
        }
        public ParametrosGuardarDto<T> ConstruirParametrosGuardado(HttpRequestMessage request, T contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<T>();

            if (request.Headers.Contains("piip-idInstanciaFlujo"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idInstanciaFlujo").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.InstanciaId = valor;
                else
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos,
                                                              "piip-idInstanciaFlujo"));
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                          "piip-idInstanciaFlujo"));

            if (request.Headers.Contains("piip-idAccion"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idAccion").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.AccionId = valor;
                else
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos,
                                                              "piip-idAccion"));
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                          "piip-idAccion"));

            if (request.Headers.Contains("piip-idFormulario"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idFormulario").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.FormularioId = valor;
                else
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos,
                                                              "piip-idFormulario"));
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                          "piip-idFormulario"));

            if (contenido != null)
                parametrosGuardar.Contenido = contenido;
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "contenido"));

            return parametrosGuardar;
        }
        public T Obtener(ParametrosConsultaDto parametrosConsultaDto)
        {
            var temporal = _persistenciaTemporal.ObtenerTemporal(parametrosConsultaDto);

            return temporal != null ? JsonConvert.DeserializeObject<T>(temporal.Json) : ObtenerDefinitivo(parametrosConsultaDto);
        }
        protected abstract T ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto);
        public void Guardar(ParametrosGuardarDto<T> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria,bool guardarTemporalmente)
        {
            //TODO: Se debe incluir un scope transaccional
            string mensajeAccion;

            if (guardarTemporalmente)
            {
                GuardadoTemporal(parametrosGuardar);
                mensajeAccion = string.Format(ServiciosNegocioRecursos.GuardadoTemporal, parametrosGuardar.Contenido);
            }
            else
            {
                parametrosGuardar.Usuario = parametrosAuditoria.Usuario;
                GuardadoDefinitivo(parametrosGuardar, parametrosAuditoria.Usuario);
                mensajeAccion = string.Format(ServiciosNegocioRecursos.GuardadoDefinitivo, parametrosGuardar.Contenido);
            }

            GenerarAuditoria(parametrosGuardar,
                             parametrosAuditoria,
                             parametrosAuditoria.Ip,
                             parametrosAuditoria.Usuario,
                             TipoMensajeEnum.Creacion,
                             mensajeAccion);

        }
        private void GuardadoTemporal(ParametrosGuardarDto<T> parametrosGuardar)
        {
            _persistenciaTemporal.GuardarTemporalmente(parametrosGuardar);
        }
        protected abstract void GuardadoDefinitivo(ParametrosGuardarDto<T> parametrosGuardar, string usuario);
        private void GenerarAuditoria(ParametrosGuardarDto<T> parametrosGuardar,
                                      ParametrosAuditoriaDto parametrosAuditoria, string ip, string usuario,
                                      TipoMensajeEnum tipoMensaje, string accion)
        {
            var guardarAuditoria = new GuardarAuditoriaDto<T>
            {
                Mensaje = accion,
                InstanciaId = parametrosGuardar.InstanciaId,
                AccionId = parametrosGuardar.AccionId,
                Usuario = parametrosAuditoria.Usuario,
                FechaAccion = DateTime.Now,
                Contenido = parametrosGuardar.Contenido
            };

            var mensaje = JsonConvert.SerializeObject(guardarAuditoria);
            //se quitan las llaves adicionales que genero el newtonsoft
            var contenidoMensaje = mensaje.Substring(1, mensaje.Length - 2);

            Task.Run(() => _auditoriaServicios.RegistrarTrazabilidadAuditoriaServiciosNegocio(contenidoMensaje,
                                                                                              ip,
                                                                                              usuario,
                                                                                              tipoMensaje));
        }

    }
}
