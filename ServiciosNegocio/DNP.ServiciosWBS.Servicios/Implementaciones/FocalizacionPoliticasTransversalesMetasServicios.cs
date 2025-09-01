using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTranversalesMetas;
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
    public class FocalizacionPoliticasTransversalesMetasServicios : ServicioBase<PoliticaTMetasDto>, IFocalizacionPoliticasTransversalesMetasServicios
    {
        private readonly IFocalizacionPoliticasTransversalesMetasPersistencia _FocalizacionPoliticasTransversalesMetasPersistencia;
        public FocalizacionPoliticasTransversalesMetasServicios(IFocalizacionPoliticasTransversalesMetasPersistencia focalizacionPoliticasTransversalesMetasPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _FocalizacionPoliticasTransversalesMetasPersistencia = focalizacionPoliticasTransversalesMetasPersistencia;
        }
        public PoliticaTMetasDto ObtenerFocalizacionPoliticasTransversales(ParametrosConsultaDto parametrosConsulta)
        {
            _FocalizacionPoliticasTransversalesMetasPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public PoliticaTMetasDto ObtenerFocalizacionPoliticasTransversalesPreview()
        {
            return _FocalizacionPoliticasTransversalesMetasPersistencia.ObtenerFocalizacionPoliticasTransversalesPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<PoliticaTMetasDto> parametrosGuardar, string usuario)
        {
            _FocalizacionPoliticasTransversalesMetasPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override PoliticaTMetasDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _FocalizacionPoliticasTransversalesMetasPersistencia.ObtenerFocalizacionPoliticasTransversales(parametrosConsultaDto.Bpin);
        }
    }
}
