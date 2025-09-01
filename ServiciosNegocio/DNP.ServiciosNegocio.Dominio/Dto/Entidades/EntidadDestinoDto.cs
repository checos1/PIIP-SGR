using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Entidades
{
    using System.Collections.Generic;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class EntidadDestinoDto
    {
        public Guid IdRol { get; set; }
        public int IdEntidadMGA { get; set; }
        public string NombreEntidadMGA { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class RolDto
    {
        public Guid IdRol { get; set; }
        public string NombreRol { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class EntidadAcciones
    {
        public List<EntidadDestinoDto> ListadoEntidadDestino { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class EntidadAccionesEntrada
    {
        public string Bpin { get; set; }
        public List<RolDto> ListadoRoles { get; set; }
        public Guid? InstanciaId { get; set; }
    }
}
