using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.DiligenciarFuentes;
using DNP.ServiciosNegocio.Persistencia.Interfaces.DiligenciarFuentes;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Genericos;
using DNP.ServiciosNegocio.Servicios.Interfaces.DiligenciarFuentes;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.DiligenciarFuentes
{
    public class DiligenciarFuentesServicio : ServicioBase<DiligenciarFuentesProyectoDto>, IDiligenciarFuentesServicios
    {
        private readonly IDiligenciarFuentesPersistencia _diligenciarFuentesPersistencia;

        public DiligenciarFuentesServicio(IDiligenciarFuentesPersistencia diligenciarFuentesPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _diligenciarFuentesPersistencia = diligenciarFuentesPersistencia;
        }

        public DiligenciarFuentesDto ObtenerDiligenciarFuentesProyecto(ParametrosConsultaDto parametrosConsulta)
        {
            throw new NotImplementedException();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<DiligenciarFuentesProyectoDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        protected override DiligenciarFuentesProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new NotImplementedException();
        }
    }
}
