using DNP.Backbone.Dominio.Dto.MensajeMantenimiento;
using DNP.Backbone.Servicios.Interfaces.MensajesMantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.Test.Mocks
{
    public class MensajeMantenimientoServicioMock : IMensajeMantenimientoServicio
    {
        private readonly IEnumerable<MensajeMantenimientoDto> _coleccion;

        public MensajeMantenimientoServicioMock()
        {
            _coleccion = new[] {
                new MensajeMantenimientoDto
                {
                    Id = 1,
                    CreadoPor = "usuario.prueba",
                    NombreMensaje = "mensaje teste",
                    FechaCreacion = DateTime.Now,
                    Roles = new[]
                    {
                        new MensajeRolAutorizacionDto
                        {
                            Id = new Guid(),
                            IdMensaje = 1,
                            NombreRol = "Rol prueba"
                        }
                    }
                },
                new MensajeMantenimientoDto
                {
                    Id = 2,
                    CreadoPor = "usuario.prueba",
                    NombreMensaje = "mensaje teste 2",
                    FechaCreacion = DateTime.Now,
                    Roles = new[]
                    {
                        new MensajeRolAutorizacionDto
                        {
                            Id = new Guid(),
                            IdMensaje = 1,
                            NombreRol = "Rol prueba"
                        }
                    }
                },
                new MensajeMantenimientoDto
                {
                    Id = 3,
                    CreadoPor = "usuario.prueba",
                    NombreMensaje = "mensaje teste 3",
                    FechaCreacion = DateTime.Now,
                    Roles = new[]
                    {
                        new MensajeRolAutorizacionDto
                        {
                            Id = new Guid(),
                            IdMensaje = 1,
                            NombreRol = "Rol prueba"
                        }
                    }
                }
            };
        }

        public Task<MensajeMantenimientoDto> CrearActualizarMensaje(ParametrosMensajeMantenimiento parametros)
            => Task.FromResult(_coleccion.FirstOrDefault());

        public Task EliminarMensaje(ParametrosMensajeMantenimiento parametros) => Task.CompletedTask;

        public Task<IEnumerable<MensajeMantenimientoDto>> ObtenerListaMensajes(ParametrosMensajeMantenimiento parametros)
        {
            var query = _coleccion;
            if (parametros.FiltroDto.Ids != null)
                query = query.Where(x => parametros.FiltroDto.Ids.Contains(x.Id)).ToList();

            return Task.FromResult(query);
        }
    }
}
