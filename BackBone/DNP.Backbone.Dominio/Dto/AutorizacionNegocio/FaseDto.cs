namespace DNP.Autorizacion.Dominio.Dto
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    [ExcludeFromCodeCoverage]
    public class FaseDto
    {
        public string NombreFase { get; set; }
        public Nullable<System.Guid> FaseGUID { get; set; }
        public int Id { get; set; }
    }
}
