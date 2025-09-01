using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesAsociacionIndicadoresAjuste;
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
    public class AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios : ServicioBase<PoliticaTIndicadoresAjusteDto>, IAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios
    {
        private readonly IAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia _ajusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia;
        public AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios(IAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia ajusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _ajusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia = ajusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia;
        }

        public PoliticaTIndicadoresAjusteDto ObtenerAjusteFocalizacionPoliticasTransversalesAsociacionIndicadores(ParametrosConsultaDto parametrosConsulta)
        {
            _ajusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public PoliticaTIndicadoresAjusteDto ObtenerAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPreview()
        {
            return _ajusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia.ObtenerAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<PoliticaTIndicadoresAjusteDto> parametrosGuardar, string usuario)
        {
            _ajusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override PoliticaTIndicadoresAjusteDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _ajusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia.ObtenerAjusteFocalizacionPoliticasTransversalesAsociacionIndicadores(parametrosConsultaDto.Bpin);
        }
    }
}
