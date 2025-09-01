using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class SectorNegocioDto
    {
        public Guid Id { get; set; }

        public int SectorNegocioId { get; set; }
        public string Nombre { get; set; }
    }
}
