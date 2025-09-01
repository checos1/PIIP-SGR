namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    [ExcludeFromCodeCoverage]
    public class EntidadesAsociarComunDto
    {
        public int Id { get; set; }
        public string NombreEntidad { get; set; }

        public bool CabezaSector { get; set; }
    }
}
