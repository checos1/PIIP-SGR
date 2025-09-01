using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.CostosEntregables;
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
    public class CostosEntregablesServicios : ServicioBase<CostosEntregablesDto>, ICostosEntregablesServicios
    {
        private readonly ICostosEntregablesPersistencia _costosEntregablesPersistencia;

        public CostosEntregablesServicios(ICostosEntregablesPersistencia costosEntregablesPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _costosEntregablesPersistencia = costosEntregablesPersistencia;
        }

        public CostosEntregablesDto ObtenerCostosEntregables(ParametrosConsultaDto parametrosConsulta)
        {
           // _costosEntregablesPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta); 
        }

        public CostosEntregablesDto ObtenerCostosEntregablesPreview()
        {
            return _costosEntregablesPersistencia.ObtenerCostosEntregablesPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<CostosEntregablesDto> parametrosGuardar, string usuario)
        {
            _costosEntregablesPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override CostosEntregablesDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _costosEntregablesPersistencia.ObtenerCostosEntregables(parametrosConsultaDto.Bpin);
        }
    }
}
