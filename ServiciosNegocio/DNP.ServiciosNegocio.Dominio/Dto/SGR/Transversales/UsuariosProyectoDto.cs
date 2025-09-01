namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class UsuariosProyectoDto
    {
        public string Usuario { get; set; }
        public string Notificacion { get; set; }
    }

}
