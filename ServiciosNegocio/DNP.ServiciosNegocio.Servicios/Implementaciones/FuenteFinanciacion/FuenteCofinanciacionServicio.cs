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

    //[ExcludeFromCodeCoverage]
    public class FuenteCofinanciacionServicio : ServicioBase<FuenteCofinanciacionProyectoDto>, IFuenteCofinanciacionServicio
    {
        private readonly IFuenteCofinanciacionPersistencia _fuenteCofinanciacionPersistencia;

        public FuenteCofinanciacionServicio(IFuenteCofinanciacionPersistencia fuenteFinanciacionPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _fuenteCofinanciacionPersistencia = fuenteFinanciacionPersistencia;
        }

        public FuenteCofinanciacionProyectoDto ObtenerFuenteCofinanciacionProyecto(ParametrosConsultaDto parametrosConsulta)
        {
            _fuenteCofinanciacionPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public FuenteCofinanciacionProyectoDto ObtenerFuenteCofinanciacionProyectoPreview()
        {
            return _fuenteCofinanciacionPersistencia.ObtenerFuenteCofinanciacionProyectoPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<FuenteCofinanciacionProyectoDto> parametrosGuardar, string usuario)
        {
            _fuenteCofinanciacionPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override FuenteCofinanciacionProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            FuenteCofinanciacionProyectoDto infoPersistencia = _fuenteCofinanciacionPersistencia.ObtenerFuenteCofinanciacionProyecto(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }
    }
}
