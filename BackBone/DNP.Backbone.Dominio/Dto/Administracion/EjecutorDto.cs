
namespace DNP.Backbone.Dominio.Dto.Administracion

{
    using System;
    using System.Diagnostics.CodeAnalysis;
    

    [ExcludeFromCodeCoverage]
    public class EjecutorDto 
    {
        public int Id { get; set; }
        public string Nit { get; set; }
        public string Digito { get; set; }
        public string NombreEjecutor { get; set; }
        public int EntityTypeId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
        public bool Activo { get; set; }
        public string NombreEntityType { get; set; }


    }
}
