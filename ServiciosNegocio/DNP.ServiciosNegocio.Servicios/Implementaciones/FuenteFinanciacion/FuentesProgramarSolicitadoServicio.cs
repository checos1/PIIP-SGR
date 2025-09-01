using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.FuenteFinanciacion
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Genericos;

    public class FuentesProgramarSolicitadoServicio : ServicioBase<FuentesProgramarSolicitadoDto>, IFuentesProgramarSolicitadoServicio
    {
       
        private readonly IFuentesProgramarSolicitadoPersistencia _FuentesProgramarSolicitadoPersistencia;

        public FuentesProgramarSolicitadoServicio(IFuentesProgramarSolicitadoPersistencia fuentesProgramarSolicitadoPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _FuentesProgramarSolicitadoPersistencia = fuentesProgramarSolicitadoPersistencia;
        }

        public string ObtenerFuentesProgramarSolicitado(string bpin)
        {
            return _FuentesProgramarSolicitadoPersistencia.ObtenerFuentesProgramarSolicitado(bpin);
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<FuentesProgramarSolicitadoDto> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }

        protected override FuentesProgramarSolicitadoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new System.NotImplementedException();
        }

        public string GuardarFuentesProgramarSolicitado(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuario)
        {
            return _FuentesProgramarSolicitadoPersistencia.GuardarFuentesProgramarSolicitado(objProgramacionValorFuenteDto, usuario);
        }

    }
}
