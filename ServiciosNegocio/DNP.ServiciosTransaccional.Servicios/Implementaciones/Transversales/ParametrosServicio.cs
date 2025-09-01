using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
using DNP.ServiciosTransaccional.Persistencia.Interfaces.Transversales;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Transversales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Implementaciones.Transversales
{
    public class ParametrosServicio : ServicioBase<ObjetoNegocio>, IParametrosServicio
    {
        private readonly IAuditoriaServicios _auditoriaServicios;
        private readonly IParametrosPersistencia _parametrosPersistencia;

        public ParametrosServicio(IParametrosPersistencia parametrosPersistencia, IAuditoriaServicios auditoriaServicios) : base(auditoriaServicios)
        {
            _auditoriaServicios = auditoriaServicios;
            _parametrosPersistencia = parametrosPersistencia;
        }

        public string ConsultarParametro(string llave)
        {
            var valor = _parametrosPersistencia.ConsultarParametro(llave);
            return valor;
        }

        protected override object GuardadoDefinitivo(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }
    }
}
