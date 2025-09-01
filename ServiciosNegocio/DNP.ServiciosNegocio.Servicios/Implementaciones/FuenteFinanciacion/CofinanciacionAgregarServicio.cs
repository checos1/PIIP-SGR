using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.FuenteFinanciacion
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Genericos;
    public class CofinanciacionAgregarServicio : ServicioBase<CofinanciacionProyectoDto>, ICofinanciacionAgregarServicio
    {
        private readonly ICofinanciacionAgregarPersistencia _cofinanciacionAgregarPersistencia;

        public CofinanciacionAgregarServicio(ICofinanciacionAgregarPersistencia cofinanciacionAgregarPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _cofinanciacionAgregarPersistencia = cofinanciacionAgregarPersistencia;
        }

        public CofinanciacionProyectoDto ObtenerCofinanciacionAgregar(ParametrosConsultaDto parametrosConsulta)
        {
            _cofinanciacionAgregarPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public CofinanciacionProyectoDto ObtenerCofinanciacionAgregarPreview()
        {
            return _cofinanciacionAgregarPersistencia.ObtenerCofinanciacionAgregarPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<CofinanciacionProyectoDto> parametrosGuardar, string usuario)
        {
            _cofinanciacionAgregarPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override CofinanciacionProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            CofinanciacionProyectoDto infoPersistencia = _cofinanciacionAgregarPersistencia.ObtenerCofinanciacionAgregar(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }
    }
}
