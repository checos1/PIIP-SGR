using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursos;
using DNP.ServiciosWBS.Persistencia.Interfaces;
using DNP.ServiciosWBS.Persistencia.Interfaces.Transversales;
using DNP.ServiciosWBS.Servicios.Interfaces;
using DNP.ServiciosWBS.Servicios.Interfaces.Transversales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    public class RegionalizacionMetasyRecursosServicios : ServicioBase<RegMetasRecursosDto>, IRegionalizacionMetasyRecursosServicios
    {
        private readonly IRegionalizacionMetasyRecursosPersistencia _regionalizacionMetasyRecursosPersistencia;

        public RegionalizacionMetasyRecursosServicios(IRegionalizacionMetasyRecursosPersistencia regionalizacionMetasyRecursosPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _regionalizacionMetasyRecursosPersistencia = regionalizacionMetasyRecursosPersistencia;
        }

        public RegMetasRecursosDto ObtenerRegionalizacionMetasyRecursosServicios(ParametrosConsultaDto parametrosConsulta)
        {
            _regionalizacionMetasyRecursosPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta); 
        }

        public RegMetasRecursosDto ObtenerRegionalizacionMetasyRecursosServiciosPreview()
        {
            return _regionalizacionMetasyRecursosPersistencia.ObtenerRegionalizacionMetasyRecursosPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<RegMetasRecursosDto> parametrosGuardar, string usuario)
        {
            _regionalizacionMetasyRecursosPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override RegMetasRecursosDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _regionalizacionMetasyRecursosPersistencia.ObtenerRegionalizacionMetasyRecursos(parametrosConsultaDto.Bpin);
        }
    }
}
