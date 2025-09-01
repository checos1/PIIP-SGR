namespace DNP.Autorizacion.Dominio.Dto
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class PermisoDto
    {
        public Guid IdPermiso { get; set; }
        public Guid IdOpcion { get; set; }
        public Guid IdRol { get; set; }
        public bool Adicionar  { get; set; }
        public bool Eliminar { get; set; }
    }
}
