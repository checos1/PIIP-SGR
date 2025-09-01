namespace DNP.ServiciosTransaccional.Servicios.Implementaciones
{
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Enum;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Auditoria;
    using Interfaces.Transversales;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public abstract class ServicioBase<T> where T : class
    {
        private readonly IAuditoriaServicios _auditoriaServicios;

        protected ServicioBase(IAuditoriaServicios auditoriaServicios)
        {
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

            if (contenido != null)
                parametrosGuardar.Contenido = contenido;
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "contenido"));

            return parametrosGuardar;
        }


        public virtual object Guardar(ParametrosGuardarDto<T> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            //TODO: Se debe incluir un scope transaccional
            object resultado;
            string mensajeAccion;

            parametrosGuardar.Usuario = parametrosAuditoria.Usuario;
            resultado = GuardadoDefinitivo(parametrosGuardar, parametrosAuditoria.Usuario);

            mensajeAccion = string.Format(ServiciosNegocioRecursos.GuardadoDefinitivo, parametrosGuardar.Contenido);

            GenerarAuditoria(parametrosGuardar,
                             parametrosAuditoria,
                             parametrosAuditoria.Ip,
                             parametrosAuditoria.Usuario,
                             TipoMensajeEnum.Creacion,
                             mensajeAccion);

            return resultado;
        }

        protected abstract object GuardadoDefinitivo(ParametrosGuardarDto<T> parametrosGuardar, string usuario);

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

    }
}