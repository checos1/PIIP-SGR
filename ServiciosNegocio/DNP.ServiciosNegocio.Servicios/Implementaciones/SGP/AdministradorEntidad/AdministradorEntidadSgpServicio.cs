using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.AdministradorEntidad;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Persistencia.Interfaces.AdministradorEntidad;
using DNP.ServiciosNegocio.Servicios.Interfaces.AdministradorEntidad;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.AdministradorEntidad
{
    public class AdministradorEntidadSgpServicio : ServicioBase<ListMatrizEntidadDestinoDto>, IAdministradorEntidadSgpServicio
    {
        private readonly ICacheServicio _cacheServicio;
        private readonly IAdministradorEntidadSgpPersistencia _AdministradorEntidadSgpPersistencia;
        public string Usuario { get; set; }
        public string Ip { get; set; }

        public AdministradorEntidadSgpServicio(ICacheServicio cacheServicio, IAdministradorEntidadSgpPersistencia AdministradorEntidadSgpPersistencia, IAuditoriaServicios auditoriaServicios) : base(null, auditoriaServicios)
        {
            _cacheServicio = cacheServicio;
            _AdministradorEntidadSgpPersistencia = AdministradorEntidadSgpPersistencia;
        }

        public string ObtenerSectores()
        {
            return _AdministradorEntidadSgpPersistencia.ObtenerSectores();
        }

        public string ObtenerFlowCatalog()
        {
            return _AdministradorEntidadSgpPersistencia.ObtenerFlowCatalog();
        }

        public List<ConfiguracionMatrizEntidadDestinoSGRDto> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto dto, string usuario)
        {
            return _AdministradorEntidadSgpPersistencia.ObtenerMatrizEntidadDestino(dto, usuario);
        }

        public Task<RespuestaGeneralDto> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto dto, string usuario)
        {
            var result = Task.FromResult(_AdministradorEntidadSgpPersistencia.ActualizarMatrizEntidadDestino(dto, usuario));

            var parametrosGuardar = new ParametrosGuardarDto<ListaMatrizEntidadUnidadDto>
            {
                Contenido = dto
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Modificacion, "ActualizarMatrizEntidadDestino");

            return result;
        }

        protected override ListMatrizEntidadDestinoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new System.NotImplementedException();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ListMatrizEntidadDestinoDto> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }
    }
}
