using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
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
    public class FocalizacionPoliticasTransversalesFuentesServicios : ServicioBase<FocalizacionPoliticaTFuentesDto>, IFocalizacionPoliticasTransversalesFuentesServicios
    {
        private readonly IFocalizacionPoliticasTransversalesFuentesPersistencia _focalizacionPoliticasTransversalesFuentesPersistencia;

        public FocalizacionPoliticasTransversalesFuentesServicios(IFocalizacionPoliticasTransversalesFuentesPersistencia focalizacionPoliticasTransversalesFuentesServicios,
                        IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
       base(persistenciaTemporal, auditoriaServicios)
        {
            _focalizacionPoliticasTransversalesFuentesPersistencia = focalizacionPoliticasTransversalesFuentesServicios;
        }

        public FocalizacionPoliticaTFuentesDto ObtenerFocalizacionPoliticasTransversalesFuentes(ParametrosConsultaDto parametrosConsultaDto)
        {
            return ObtenerDefinitivo(parametrosConsultaDto);
        }

        protected override FocalizacionPoliticaTFuentesDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            FocalizacionPoliticaTFuentesDto infoPersistencia = _focalizacionPoliticasTransversalesFuentesPersistencia.ObtenerFocalizacionPoliticasTransversalesFuentes(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public FocalizacionPoliticaTFuentesDto ObtenerFocalizacionPoliticasTransversalesFuentesPreview()
        {
            return _focalizacionPoliticasTransversalesFuentesPersistencia.ObtenerFocalizacionPoliticasTransversalesFuentesPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<FocalizacionPoliticaTFuentesDto> parametrosGuardar, string usuario)
        {
            _focalizacionPoliticasTransversalesFuentesPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
      
    }
}

