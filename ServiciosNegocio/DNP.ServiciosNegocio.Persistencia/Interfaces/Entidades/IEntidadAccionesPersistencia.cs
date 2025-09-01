namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Entidades
{
    using System;
    using System.Collections.Generic;
    using Dominio.Dto.Entidades;

    public interface IEntidadAccionesPersistencia
    {
        List<EntidadDestinoDto> ObtenerEntidadesDestino (string bPin, List<RolDto>listaRoles, Guid? instanciaId);
    }
}
