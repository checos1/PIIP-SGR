using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.FuenteFinanciacion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Genericos;
    using DNP.ServiciosNegocio.Comunes.Dto;

    public class FuenteFinanciacionAgregarServicio : ServicioBase<ProyectoFuenteFinanciacionAgregarDto>, IFuenteFinanciacionAgregarServicio
    {
        private readonly IFuenteFinanciacionAgregarPersistencia _fuenteFinanciacionAgregarPersistencia;
        public string Usuario { get; set; }
        public string Ip { get; set; }

        public FuenteFinanciacionAgregarServicio(IFuenteFinanciacionAgregarPersistencia fuenteFinanciacionAgregarPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
              _fuenteFinanciacionAgregarPersistencia = fuenteFinanciacionAgregarPersistencia;
        }

        public ProyectoFuenteFinanciacionAgregarDto ObtenerFuenteFinanciacionAgregar(ParametrosConsultaDto parametrosConsulta)
        {
            return Obtener(parametrosConsulta);
        }

        public string ObtenerFuenteFinanciacionAgregarN(string bpin)
        {
            return _fuenteFinanciacionAgregarPersistencia.ObtenerFuenteFinanciacionAgregarN(bpin);
        }

        public ProyectoFuenteFinanciacionAgregarDto ObtenerFuenteFinanciacionAgregarPreview()
        {
            return _fuenteFinanciacionAgregarPersistencia.ObtenerFuenteFinanciacionAgregarPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar, string usuario)
        {
            _fuenteFinanciacionAgregarPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override ProyectoFuenteFinanciacionAgregarDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
           ProyectoFuenteFinanciacionAgregarDto infoPersistencia = _fuenteFinanciacionAgregarPersistencia.ObtenerFuenteFinanciacionAgregar(parametrosConsultaDto.Bpin);
            return infoPersistencia;
          
        }

        /// <summary>
        /// nuevo proceso apra obtener el Json con funte cofinanciador y vigencias por etapa.
        /// </summary>
        /// <param name="bpin"></param>
        /// <returns>Json</returns>
        public string ObtenerFuenteFinanciacionVigencia(string bpin)
        {
            return _fuenteFinanciacionAgregarPersistencia.ObtenerFuenteFinanciacionVigencia(bpin);
        }

        public void GuardarFuenteFinanciacion(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar, string usuario)
        {
             _fuenteFinanciacionAgregarPersistencia.GuardarFuenteFinanciacion(parametrosGuardar, usuario);
        }

        public FuenteFinanciacionResultado EliminarFuentesFinanciacionProyecto(int fuentesFinanciacionId)
        {
            return _fuenteFinanciacionAgregarPersistencia.EliminarFuentesFinanciacionProyecto(fuentesFinanciacionId);
        }

        public string ObtenerResumenCostosVsSolicitado(string bpin)
        {
            return _fuenteFinanciacionAgregarPersistencia.ObtenerResumenCostosVsSolicitado(bpin) ;
        }


        //public ResumenFuenteFinanciacionDTO ConsultarResumenFteFinanciacion(string bpin)
        //{
        //    return _fuenteFinanciacionAgregarPersistencia.ConsultarResumenFteFinanciacion(bpin);
        //}

        public string ConsultarResumenFteFinanciacion(string bpin)
        {
            return _fuenteFinanciacionAgregarPersistencia.ConsultarResumenFteFinanciacion(bpin);
        }

        public string ConsultarCostosPIIPvsFuentesPIIP(string bpin)
        {
            return _fuenteFinanciacionAgregarPersistencia.ConsultarCostosPIIPvsFuentesPIIP(bpin);
        }

        public string FuentesFinanciacionRecursosAjustesAgregar(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuario)
        {
            return _fuenteFinanciacionAgregarPersistencia.FuentesFinanciacionRecursosAjustesAgregar(objFuenteFinanciacionAgregarAjusteDto, usuario);
        }

        public string ObtenerDetalleAjustesFuenteFinanciacion(string bpin, string usuario)
        {
            return _fuenteFinanciacionAgregarPersistencia.ObtenerDetalleAjustesFuenteFinanciacion(bpin, usuario);
        }

        public string ObtenerDetalleAjustesJustificaionFacalizacionPT(string bpin, string usuario)
        {
            return _fuenteFinanciacionAgregarPersistencia.ObtenerDetalleAjustesJustificaionFacalizacionPT(bpin, usuario);
        }

        public OperacionCreditoDatosGeneralesDto ObtenerOperacionCreditoDatosGenerales(string bpin, Guid? instanciaId)
        {
            return _fuenteFinanciacionAgregarPersistencia.ObtenerOperacionCreditoDatosGenerales(bpin, instanciaId);
        }

        public FuenteFinanciacionResultado GuardarOperacionCreditoDatosGenerales(OperacionCreditoDatosGeneralesDto OperacionCreditoDatosGeneralesDto, string usuario)
        {
            var result = _fuenteFinanciacionAgregarPersistencia.GuardarOperacionCreditoDatosGenerales(OperacionCreditoDatosGeneralesDto, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<OperacionCreditoDatosGeneralesDto>
            {
                Contenido = OperacionCreditoDatosGeneralesDto
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "GuardarOperacionCreditoDatosGenerales");

            return result;
        }

        public OperacionCreditoDetallesDto ObtenerOperacionCreditoDetalles(string bpin, Guid? instanciaId)
        {
            return _fuenteFinanciacionAgregarPersistencia.ObtenerOperacionCreditoDetalles(bpin, instanciaId);
        }

        public FuenteFinanciacionResultado GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto OperacionCreditoDetallesDto, string usuario)
        {
            var result = _fuenteFinanciacionAgregarPersistencia.GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<OperacionCreditoDetallesDto>
            {
                Contenido = OperacionCreditoDetallesDto
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "GuardarOperacionCreditoDetalles");

            return result;
        }
    }
}
