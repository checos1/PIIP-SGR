using System;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class AlcanceTramiteDto
    {
        public int TramiteId { get; set; }
        public int NuevoTramiteId { get; set; }
        public Guid InstanciaId { get; set; }
        public Guid NuevaInstanciaId { get; set; }
        public Guid? FlujoId { get; set; }
        public string Usuario { get; set; }
    }

}
