using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.FuenteFinanciacion
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Genericos;

    public class DatosAdicionalesServicio : ServicioBase<DatosAdicionalesDto>, IDatosAdicionalesServicio
    {
        private readonly IDatosAdicionalesPersistencia _DatosAdicionalesPersistencia;

        public DatosAdicionalesServicio(IDatosAdicionalesPersistencia DatosAdicionalesPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
              _DatosAdicionalesPersistencia = DatosAdicionalesPersistencia;
        }

        public string ObtenerDatosAdicionalesFuenteFinanciacion(int fuenteId)
        {
            return _DatosAdicionalesPersistencia.ObtenerDatosAdicionalesFuenteFinanciacion(fuenteId);
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<DatosAdicionalesDto> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }

        protected override DatosAdicionalesDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new System.NotImplementedException();
        }

        //Task<RespuestaGeneralDto> GuardarDatosAdicionales(ParametrosGuardarDto<DatosAdicionalesDto> parametrosGuardar, string usuario)
        //{
        //    return Task.FromResult(_DatosAdicionalesPersistencia.GuardarDatosAdicionales(parametrosGuardar, usuario));
        //}

        public DatosAdicionalesResultado EliminarDatosAdicionales(int coFinanciacionId)
        {
            return _DatosAdicionalesPersistencia.EliminarDatosAdicionales(coFinanciacionId);
        }

        Task<RespuestaGeneralDto> IDatosAdicionalesServicio.GuardarDatosAdicionales(ParametrosGuardarDto<DatosAdicionalesDto> parametrosGuardar, string usuario)
        {
            return Task.FromResult(_DatosAdicionalesPersistencia.GuardarDatosAdicionales(parametrosGuardar, usuario));
        }
    }
}
