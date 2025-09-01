using System;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class EnvioSubDireccionDto
    {
        public int Id { get; set; }
        public int TramiteId { get; set; }
        public string IdUsuarioDNP { get; set; }
        public int EntityTypeCatalogOptionId { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
        public bool Enviado { get; set; }
        public int  ParentId { get; set; }
        public string NombreUsuarioDNP { get; set; }
        public string NombreEntidad { get; set; }
        public string Correo { get; set; }
        public string IdUsuarioDNPQueEnvia { get; set; }
        public string NombreUsuarioQueEnvia { get; set; }
        public DateTime FechaEntrega { get; set; }

    }
}
