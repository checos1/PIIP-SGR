using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Test.Mock
{
    using System;
    using Persistencia.Interfaces.Entidades;
    using Dominio.Dto.Entidades;
    using Castle.Core.Internal;

    public class EntidadAccionesPersistenciaMock : IEntidadAccionesPersistencia
    {
        public List<EntidadDestinoDto> ObtenerEntidadesDestino(string bPin, List<RolDto> listaRoles, Guid? instanciaId)
        {
            if (bPin.IsNullOrEmpty())return new List<EntidadDestinoDto>();
                var listado = new List<EntidadDestinoDto>()
                          {
                              new EntidadDestinoDto()
                              {
                                  IdRol = Guid.Parse("6F7C8930-6962-4E6A-9FB4-E4F7CA0DDAC3"),
                                  IdEntidadMGA = 40,
                                  NombreEntidadMGA = "AGENCIA NACIONAL DE INFRAESTRUCTURA"
                              },
                              new EntidadDestinoDto()
                              {
                                  IdRol = Guid.Parse("56828712-69C6-4D7C-9169-8D7BD18854CC"),
                                  IdEntidadMGA = 41,
                                  NombreEntidadMGA = "AGENCIA NACIONAL DE MINERÍA - ANM"
                              }
                          };
            return listado;
        }
    }
}
