namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class OrigenRecursosDto
    {       
        public int TramiteId { get; set; }
        public int TipoOrigenId { get; set; }
        public string Rubro { get; set; }
    }       
}
