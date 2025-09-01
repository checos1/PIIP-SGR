namespace DNP.Autorizacion.Dominio.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class OpcionDto
    {
        public Guid IdOpcion { get; set; }
        public string Nombre { get; set; }
        public Guid IdAplicacion { get; set; }
        public string NombreTipoOpcion { get; set; }
        public bool Agregar { get; set; }
    }
}
