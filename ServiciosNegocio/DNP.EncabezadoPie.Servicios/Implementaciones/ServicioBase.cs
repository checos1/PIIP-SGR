using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.EncabezadoPie.Servicios.Implementaciones
{
    using Persistencia.Interfaces.Genericos;
    public abstract class ServicioBase<T> where T : class
    {
        private readonly IPersistenciaTemporal _persistenciaTemporal;

        protected ServicioBase(IPersistenciaTemporal persistenciaTemporal)
        {
            _persistenciaTemporal = persistenciaTemporal;
        }

        public virtual T Obtener(ParametrosConsultaDto parametrosConsultaDto)
        {
            var temporal = _persistenciaTemporal.ObtenerTemporal(parametrosConsultaDto);

            return temporal != null ? JsonConvert.DeserializeObject<T>(temporal.Json) : ObtenerDefinitivo(parametrosConsultaDto);
        }
        protected abstract T ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto);        
    }
}
