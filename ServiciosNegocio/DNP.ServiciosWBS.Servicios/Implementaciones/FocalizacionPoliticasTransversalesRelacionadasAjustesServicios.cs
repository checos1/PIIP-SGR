using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionadaAjustes;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasIndicadoresCategorias;
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
    public class FocalizacionPoliticasTransversalesRelacionadasAjutesServicios : ServicioBase<PoliticaTransversalRelacionadaAjustesDto>, IFocalizacionPoliticasTransversalesRelacionadasAjustesServicios
    {
        private readonly IFocalizacionPoliticasTransversalesRelacionadasAjustesPersistencia _focalizacionPoliticasTransversalesRelacionadasAjustesPersistencia;

        public FocalizacionPoliticasTransversalesRelacionadasAjutesServicios(IFocalizacionPoliticasTransversalesRelacionadasAjustesPersistencia focalizacionPoliticasTransversalesRelacionadasAjustesPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _focalizacionPoliticasTransversalesRelacionadasAjustesPersistencia = focalizacionPoliticasTransversalesRelacionadasAjustesPersistencia;
        }

        public PoliticaTransversalRelacionadaAjustesDto ObtenerFocalizacionPoliticasTransversalesRelacionadasAjustesServicios(ParametrosConsultaDto parametrosConsulta)
        {
            _focalizacionPoliticasTransversalesRelacionadasAjustesPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public PoliticaTransversalRelacionadaAjustesDto ObtenerFocalizacionPoliticasTransversalesRelacionadasAjustesServiciosPreview()
        {
            return _focalizacionPoliticasTransversalesRelacionadasAjustesPersistencia.ObtenerFocalizacionPoliticasTransversalesRelacionadasAjustesPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<PoliticaTransversalRelacionadaAjustesDto> parametrosGuardar, string usuario)
        {
            _focalizacionPoliticasTransversalesRelacionadasAjustesPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override PoliticaTransversalRelacionadaAjustesDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _focalizacionPoliticasTransversalesRelacionadasAjustesPersistencia.ObtenerFocalizacionPoliticasTransversalesRelacionadasAjustes(parametrosConsultaDto.Bpin);
        }

    }
}
