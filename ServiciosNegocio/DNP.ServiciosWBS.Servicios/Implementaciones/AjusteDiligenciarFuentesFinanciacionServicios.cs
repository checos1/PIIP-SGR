using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
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
    public class AjusteDiligenciarFuentesFinanciacionServicios : ServicioBase<FuentesFinanciacionAjusteDto>, IAjusteDiligenciarFuentesFinanciacionServicios
    {
        private readonly IAjusteDiligenciarFuentesFinanciacionPersistencia _fuentesFinanciacionAjustePersistencia;
        public AjusteDiligenciarFuentesFinanciacionServicios(IAjusteDiligenciarFuentesFinanciacionPersistencia fuentesFinanciacionAjustePersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _fuentesFinanciacionAjustePersistencia = fuentesFinanciacionAjustePersistencia;
        }

        public FuentesFinanciacionAjusteDto ObtenerFuenteFinanciacionAjusteServicios(ParametrosConsultaDto parametrosConsulta)
        {
            _fuentesFinanciacionAjustePersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public FuentesFinanciacionAjusteDto ObtenerFuenteFinanciacionAjusteServiciosPreview()
        {
            return _fuentesFinanciacionAjustePersistencia.ObtenerFuenteFinanciacionAjustePreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<FuentesFinanciacionAjusteDto> parametrosGuardar, string usuario)
        {
            _fuentesFinanciacionAjustePersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override FuentesFinanciacionAjusteDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _fuentesFinanciacionAjustePersistencia.ObtenerFuenteFinanciacionAjuste(parametrosConsultaDto.Bpin);
        }
    }
}
