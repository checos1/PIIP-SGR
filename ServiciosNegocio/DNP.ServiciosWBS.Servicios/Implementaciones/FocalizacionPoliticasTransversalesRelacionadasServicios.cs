using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionada;
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
    public class FocalizacionPoliticasTransversalesRelacionadasServicios : ServicioBase<PoliticaTransversalRelacionadaDto>, IFocalizacionPoliticasTransversalesRelacionadasServicios
    {
        private readonly IFocalizacionPoliticasTransversalesRelacionadasPersistencia _focalizacionPoliticasTransversalesRelacionadasPersistencia;

        public FocalizacionPoliticasTransversalesRelacionadasServicios(IFocalizacionPoliticasTransversalesRelacionadasPersistencia focalizacionPoliticasTransversalesRelacionadasPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _focalizacionPoliticasTransversalesRelacionadasPersistencia = focalizacionPoliticasTransversalesRelacionadasPersistencia;
        }

        public PoliticaTransversalRelacionadaDto ObtenerFocalizacionPoliticasTransversalesRelacionadasServicios(ParametrosConsultaDto parametrosConsulta)
        {
            _focalizacionPoliticasTransversalesRelacionadasPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public PoliticaTransversalRelacionadaDto ObtenerFocalizacionPoliticasTransversalesRelacionadasServiciosPreview()
        {
            return _focalizacionPoliticasTransversalesRelacionadasPersistencia.ObtenerFocalizacionPoliticasTransversalesRelacionadasPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<PoliticaTransversalRelacionadaDto> parametrosGuardar, string usuario)
        {
            _focalizacionPoliticasTransversalesRelacionadasPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override PoliticaTransversalRelacionadaDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _focalizacionPoliticasTransversalesRelacionadasPersistencia.ObtenerFocalizacionPoliticasTransversalesRelacionadas(parametrosConsultaDto.Bpin);
        }
    }
}
