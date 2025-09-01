using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.Usuario
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UsuarioPerfilProyectoDto
    {
        public int ProyectoId { get; set; }
        public string CodigoBpin { get; set; }
        public string ProyectoNombre { get; set; }

        public int IdProyecto { get; set; }
        public Guid IdUsuarioPerfilProyecto { get; set; }
        public Guid IdUsuarioPerfil { get; set; }
    }
}
