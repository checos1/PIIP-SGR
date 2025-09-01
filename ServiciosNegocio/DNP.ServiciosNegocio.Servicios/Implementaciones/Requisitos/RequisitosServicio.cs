namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Requisitos
{
    using Comunes;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Comunes.Enum;
    using Dominio.Dto.Requisitos;
    using Interfaces.Requisitos;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Requisitos;
    using System;
    using System.Linq;
    using System.Net.Http;

    public class RequisitosServicio : ServicioBase<ServicioAgregarRequisitosDto>, IRequisitosServicio
    {
        private readonly IRequisitosPersistencia _requisitosPersistencia;

        public RequisitosServicio(IRequisitosPersistencia requisitosPersistencia, IAuditoriaServicios auditoriaServicios) : base(null, auditoriaServicios)
        {
            _requisitosPersistencia = requisitosPersistencia;
        }
        
        public override ServicioAgregarRequisitosDto Obtener(ParametrosConsultaDto parametrosConsultaDto)
        {
            return ObtenerDefinitivo(parametrosConsultaDto);
        }

        protected override ServicioAgregarRequisitosDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            ServicioAgregarRequisitosDto requisitos = new ServicioAgregarRequisitosDto();
            var nivelId = parametrosConsultaDto.IdNivel;
            var bPin = parametrosConsultaDto.Bpin;
            var instanciaId = parametrosConsultaDto.InstanciaId;
            var formularioId = parametrosConsultaDto.FormularioId;

            requisitos.ListadoAtributos = _requisitosPersistencia.ObtenerRequisitos(bPin, nivelId, instanciaId, formularioId);
            requisitos.IdNivel = nivelId;
            requisitos.Bpin = bPin;

            return requisitos;
        }
        public override void Guardar(ParametrosGuardarDto<ServicioAgregarRequisitosDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria,
                                     bool guardarTemporalmente)
        {
            GuardadoDefinitivo(parametrosGuardar, parametrosAuditoria.Usuario);
            var  mensajeAccion = string.Format(ServiciosNegocioRecursos.GuardadoDefinitivo, parametrosGuardar.Contenido);
            
            GenerarAuditoria(parametrosGuardar,
                             parametrosAuditoria,
                             parametrosAuditoria.Ip,
                             parametrosAuditoria.Usuario,
                             TipoMensajeEnum.Creacion,
                             mensajeAccion);
        }
        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ServicioAgregarRequisitosDto> parametrosGuardar,string usuario)
        {
            _requisitosPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        public ParametrosGuardarDto<ServicioAgregarRequisitosDto> ConstruirParametrosGuardar(HttpRequestMessage request)
        {
            var parametrosGuardar = new ParametrosGuardarDto<ServicioAgregarRequisitosDto>();

            if (request.Headers.Contains("piip-idInstanciaFlujo"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idInstanciaFlujo").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.InstanciaId = valor;

            if (request.Headers.Contains("piip-idAccion"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idAccion").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.AccionId = valor;

            return parametrosGuardar;
        }
    }
}
