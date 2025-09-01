using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursosDtoAjuste;
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
    public class AjusteRegionalizacionMetasyRecursosServicios : ServicioBase<AjusteRegMetasRecursosDto>, IAjusteRegionalizacionMetasyRecursosServicios
    {
        private readonly IAjusteRegionalizacionMetasyRecursosPersistencia _ajusteRegionalizacionMetasyRecursosPersistencia;

        public AjusteRegionalizacionMetasyRecursosServicios(IAjusteRegionalizacionMetasyRecursosPersistencia ajusteRegionalizacionMetasyRecursosPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _ajusteRegionalizacionMetasyRecursosPersistencia = ajusteRegionalizacionMetasyRecursosPersistencia;
        }

        public AjusteRegMetasRecursosDto ObtenerRegionalizacionMetasyRecursosAjusteServicios(ParametrosConsultaDto parametrosConsulta)
        {
            _ajusteRegionalizacionMetasyRecursosPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public AjusteRegMetasRecursosDto ObtenerRegionalizacionMetasyRecursosServiciosAjustePreview()
        {
            return _ajusteRegionalizacionMetasyRecursosPersistencia.ObtenerRegionalizacionMetasyRecursosAjustePreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<AjusteRegMetasRecursosDto> parametrosGuardar, string usuario)
        {
            _ajusteRegionalizacionMetasyRecursosPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override AjusteRegMetasRecursosDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _ajusteRegionalizacionMetasyRecursosPersistencia.ObtenerRegionalizacionMetasyRecursosAjuste(parametrosConsultaDto.Bpin);
        }
    }
}
