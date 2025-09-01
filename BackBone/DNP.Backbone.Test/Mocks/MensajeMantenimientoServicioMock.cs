using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto.MensajeMantenimiento;
using DNP.Backbone.Servicios.Interfaces.MensajesMantenimiento;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Test.Mocks
{
    public class MensajeMantenimientoServicioMock : IMensajeMantenimientoServicio
    {
        public Task<MensajeMantenimientoDto> CrearActualizarMensaje(ParametrosMensajeMantenimiento parametros)
            => !string.IsNullOrEmpty(parametros.ParametrosDto?.IdUsuarioDNP) && !string.IsNullOrEmpty(parametros.MensajeMantenimientoDto?.NombreMensaje) 
                ? Task.FromResult(new MensajeMantenimientoDto()) 
                : Task.FromResult<MensajeMantenimientoDto>(null);

        public Task EliminarMensaje(ParametrosMensajeMantenimiento parametros)
            => parametros.FiltroDto?.Ids.Length > 0 ? Task.CompletedTask
                : Task.FromException(new BackboneException());

        public Task<IEnumerable<MensajeMantenimientoDto>> ObtenerListaMensajes(ParametrosMensajeMantenimiento parametros)
        => !string.IsNullOrEmpty(parametros.ParametrosDto?.IdUsuarioDNP) 
                ? Task.FromResult((IEnumerable<MensajeMantenimientoDto>) new List<MensajeMantenimientoDto>())
                : Task.FromResult<IEnumerable<MensajeMantenimientoDto>>(null);
    }
}
