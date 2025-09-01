using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesDistribucionesAnteriores;
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
    public class TramitesDistribucionesAnterioresServicios : ServicioBase<TramitesDistribucionesAnterioresDto>, ITramitesDistribucionesAnterioresServicios
    {
        private readonly ITramitesDistribucionesAnterioresPersistencia _tramitesDistribucionesAnterioresPersistencia;

        public TramitesDistribucionesAnterioresServicios(ITramitesDistribucionesAnterioresPersistencia tramitesDistribucionesAnterioresServicios,
                    IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
   base(persistenciaTemporal, auditoriaServicios)
        {
            _tramitesDistribucionesAnterioresPersistencia = tramitesDistribucionesAnterioresServicios;
        }

        public TramitesDistribucionesAnterioresDto ObtenerTramitesDistribucionAnterior(ParametrosConsultaDto parametrosConsultaDto)
        {
            return ObtenerDefinitivo(parametrosConsultaDto);
        }

        protected override TramitesDistribucionesAnterioresDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            TramitesDistribucionesAnterioresDto infoPersistencia = _tramitesDistribucionesAnterioresPersistencia.ObtenerTramitesDistribucionAnterior(parametrosConsultaDto.InstanciaId);
            return infoPersistencia;
        }

        public TramitesDistribucionesAnterioresDto ObtenertramitesDistribucionAnterioresPreview()
        {
            return _tramitesDistribucionesAnterioresPersistencia.ObtenertramitesDistribucionAnterioresPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<TramitesDistribucionesAnterioresDto> parametrosGuardar, string usuario)
        {
            _tramitesDistribucionesAnterioresPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }

}
