using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.AgregarPoliticas;
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
    public class IncluirPoliticasServicios : ServicioBase<IncluirPoliticasTDto>, IIncluirPoliticasServicios
    {
        private readonly IIncluirPoliticasPersistencia _IncluirPoliticasPersistencia;

        public IncluirPoliticasServicios(IIncluirPoliticasPersistencia incluirPoliticasPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _IncluirPoliticasPersistencia = incluirPoliticasPersistencia;
        }

        public IncluirPoliticasTDto ObtenerIncluirPoliticas(ParametrosConsultaDto parametrosConsulta)
        {
            _IncluirPoliticasPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public IncluirPoliticasTDto ObtenerIncluirPoliticasPreview()
        {
            return _IncluirPoliticasPersistencia.ObtenerIncluirPoliticasPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<IncluirPoliticasTDto> parametrosGuardar, string usuario)
        {
            _IncluirPoliticasPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override IncluirPoliticasTDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _IncluirPoliticasPersistencia.ObtenerIncluirPoliticas(parametrosConsultaDto.Bpin);
        }
    }
}
