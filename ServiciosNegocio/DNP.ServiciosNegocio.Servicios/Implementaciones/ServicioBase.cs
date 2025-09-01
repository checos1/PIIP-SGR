namespace DNP.ServiciosNegocio.Servicios.Implementaciones
{
    using Comunes;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Comunes.Enum;
    using Comunes.Excepciones;
    using Dominio.Dto.Auditoria;
    using Interfaces.Transversales;
    using Newtonsoft.Json;
    using Persistencia.Interfaces.Genericos;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
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
        public ParametrosConsultaDto ConstruirParametrosConsulta(HttpRequestMessage request)
        {
            var parametrosConsulta = new ParametrosConsultaDto();

            if (request.Headers.Contains("piip-idInstanciaFlujo"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idInstanciaFlujo").First(), out var valor) && valor != Guid.Empty)
                    parametrosConsulta.InstanciaId = valor;

            if (request.Headers.Contains("piip-idAccion"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idAccion").First(), out var valor) && valor != Guid.Empty)
                    parametrosConsulta.AccionId = valor;

            if (request.Headers.Contains("piip-idFormulario"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idFormulario").First(), out var valor) && valor != Guid.Empty)
                    parametrosConsulta.FormularioId = valor;

            if (request.RequestUri.Query.Contains("bpin"))
                if (HttpUtility.ParseQueryString(request.RequestUri.Query).Get("bpin") != string.Empty)
                    parametrosConsulta.Bpin = HttpUtility.ParseQueryString(request.RequestUri.Query).Get("bpin");

            if (request.RequestUri.Query.Contains("IdNivel"))
                if (Guid.TryParse(HttpUtility.ParseQueryString(request.RequestUri.Query).Get("IdNivel"), out var valor) && valor != Guid.Empty)
                    parametrosConsulta.IdNivel = valor;

            return parametrosConsulta;
        }
        public virtual T Obtener(ParametrosConsultaDto parametrosConsultaDto)
        {
            var temporal = _persistenciaTemporal.ObtenerTemporal(parametrosConsultaDto);

            return temporal != null ? JsonConvert.DeserializeObject<T>(temporal.Json) : ObtenerDefinitivo(parametrosConsultaDto);
        }
        protected abstract T ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto);
        public virtual void Guardar(ParametrosGuardarDto<T> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente)
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
        protected void GenerarAuditoria(ParametrosGuardarDto<T> parametrosGuardar,
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

        /// <summary>
        /// Genera y registra una auditoría global mediante la utilización de los parámetros proporcionados.
        /// </summary>
        /// <param name="parametrosGuardar">Objeto que encapsula los parámetros necesarios para guardar la auditoría.</param>
        /// <param name="parametrosAuditoria">Objeto que contiene los parámetros específicos de la auditoría como usuario, IP, etc.</param>
        /// <param name="tipoMensaje">Enum que especifica el tipo de mensaje asociado a la auditoría.</param>
        /// <param name="accion">Texto que describe la acción realizada que será registrado en la auditoría.</param>
        protected void GenerarAuditoriaGlobal<V>(ParametrosGuardarDto<V> parametrosGuardar,
                                              ParametrosAuditoriaDto parametrosAuditoria,
                                              TipoMensajeEnum tipoMensaje, string accion)
        {
            // Crear un objeto para guardar la auditoría
            var guardarAuditoria = new GuardarAuditoriaDto<V>
            {
                Mensaje = accion,
                InstanciaId = parametrosGuardar.InstanciaId,
                AccionId = parametrosGuardar.AccionId,
                Usuario = parametrosAuditoria.Usuario,
                FechaAccion = DateTime.Now,
                Contenido = parametrosGuardar.Contenido
            };

            // Convertir el objeto a formato JSON
            var mensaje = JsonConvert.SerializeObject(guardarAuditoria);

            // Se quitan las llaves adicionales generadas por Newtonsoft.Json
            var contenidoMensaje = mensaje.Substring(1, mensaje.Length - 2);

            // Registrar la trazabilidad de la auditoría de forma asíncrona
            Task.Run(() => _auditoriaServicios.RegistrarTrazabilidadAuditoriaServiciosNegocio(contenidoMensaje,
                                                                                              parametrosAuditoria.Ip,
                                                                                              parametrosAuditoria.Usuario,
                                                                                              tipoMensaje));
        }



        public ParametrosGuardarDto<T> ConstruirParametrosGuardadoVentanas(T contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<T>();

            //parametrosGuardar.InstanciaId = null;
            //parametrosGuardar.AccionId = null;
            //parametrosGuardar.FormularioId = null;

            if (contenido != null)
                parametrosGuardar.Contenido = contenido;
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "contenido"));

            return parametrosGuardar;
        }

    }
}