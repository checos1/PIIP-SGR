using DNP.Autorizacion.Dominio.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.Usuario
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UsuarioDto
    {
        public UsuarioDto()
        {
            //RolListado = new HashSet<RolDto>();
        }


        public string UsuarioId { get; set; }

        public string IdUsuario { get; set; }

        public string IdUsuarioDnp { get; set; }

        public string Nombre { get; set; }

        public string Correo { get; set; }

        public bool Estado { get; set; }

        //public ICollection<RolDto> RolListado { get; set; }

        //[NotMapped]
        //public string UsuarioDnp { get; set; }

        public Guid IdUsuarioPerfil { get; set; }
        public Guid IdEntidad { get; set; }

        public string Identificacion { get; set; }
    }
}
