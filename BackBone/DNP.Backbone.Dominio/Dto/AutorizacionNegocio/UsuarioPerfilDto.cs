namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    using DNP.Autorizacion.Dominio.Dto;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class UsuarioPerfilDto
    {
        public Guid IdUsuarioPerfil { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid IdPerfil { get; set; }
        public Guid IdEntidad { get; set; }

        public PerfilDto Perfil { get; set; }
        public UsuarioDto Usuario { get; set; }

        public IEnumerable<UsuarioPerfilSectorDto> Sectores { get; set; }
        
        public IEnumerable<UsuarioPerfilProyectoDto> Proyectos { get; set; }
    }
}
