using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesAsociacionIndicadores;
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
    public class FocalizacionPoliticasTransversalesAsociacionIndicadoresServicios : ServicioBase<PoliticaTIndicadoresDto>, IFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios
    {
        private readonly IFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia _focalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia;

        public FocalizacionPoliticasTransversalesAsociacionIndicadoresServicios(IFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia focalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
                _focalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia = focalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia;
        }

        public PoliticaTIndicadoresDto ObtenerFocalizacionPoliticasTransversalesAsociacionIndicadores(ParametrosConsultaDto parametrosConsulta)
        {
            _focalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public PoliticaTIndicadoresDto ObtenerFocalizacionPoliticasTransversalesAsociacionIndicadoresPreview()
        {
            return _focalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia.ObtenerFocalizacionPoliticasTransversalesAsociacionIndicadoresPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<PoliticaTIndicadoresDto> parametrosGuardar, string usuario)
        {
            _focalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override PoliticaTIndicadoresDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _focalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia.ObtenerFocalizacionPoliticasTransversalesAsociacionIndicadores(parametrosConsultaDto.Bpin);
        }
    }
}
