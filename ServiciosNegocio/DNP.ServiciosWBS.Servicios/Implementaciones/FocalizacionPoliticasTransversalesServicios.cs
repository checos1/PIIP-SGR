using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;
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
    public class FocalizacionPoliticasTransversalesServicios : ServicioBase<PoliticaTRelacionadasDto>, IFocalizacionPoliticasTransversalesServicios
    {
        private readonly IFocalizacionPoliticasTransversalesPersistencia _focalizacionPoliticasTransversalesPersistencia;

        public FocalizacionPoliticasTransversalesServicios(IFocalizacionPoliticasTransversalesPersistencia focalizacionPoliticasTransversalesPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _focalizacionPoliticasTransversalesPersistencia = focalizacionPoliticasTransversalesPersistencia;
        }
        public PoliticaTRelacionadasDto ObtenerFocalizacionPoliticasTransversales(ParametrosConsultaDto parametrosConsulta)
        {
            _focalizacionPoliticasTransversalesPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public PoliticaTRelacionadasDto ObtenerFocalizacionPoliticasTransversalesPreview()
        {
            return _focalizacionPoliticasTransversalesPersistencia.ObtenerFocalizacionPoliticasTransversalesPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<PoliticaTRelacionadasDto> parametrosGuardar, string usuario)
        {
            _focalizacionPoliticasTransversalesPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override PoliticaTRelacionadasDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return  _focalizacionPoliticasTransversalesPersistencia.ObtenerFocalizacionPoliticasTransversales(parametrosConsultaDto.Bpin);            
        }
    }
}
