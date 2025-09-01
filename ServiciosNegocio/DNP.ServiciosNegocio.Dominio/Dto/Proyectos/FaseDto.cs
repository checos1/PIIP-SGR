using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class FaseDto
    {
        public string NombreFase { get; set; }
        public Nullable<System.Guid> FaseGUID { get; set; }
        public int Id { get; set; }
    }
}
