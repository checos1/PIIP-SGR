using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.CostosActividades;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursos;
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
    public class CostosActividadesServicios : ServicioBase<CostosActividadesDto>, ICostosActividadesServicios
    {
        private readonly ICostosActividadesPersistencia _costosActividadesPersistencia;

        public CostosActividadesServicios(ICostosActividadesPersistencia costosActividadesPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _costosActividadesPersistencia = costosActividadesPersistencia;
        }

        public CostosActividadesDto ObtenerCostosActividades(ParametrosConsultaDto parametrosConsulta)
        {
           // _costosActividadesPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta); 
        }

        public CostosActividadesDto ObtenerCostosActividadesPreview()
        {
            return _costosActividadesPersistencia.ObtenerCostosActividadesPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<CostosActividadesDto> parametrosGuardar, string usuario)
        {
            _costosActividadesPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override CostosActividadesDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _costosActividadesPersistencia.ObtenerCostosActividades(parametrosConsultaDto.Bpin);
        }
    }
}
