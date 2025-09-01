namespace DNP.Autorizacion.Dominio.Dto
{
    using System.Diagnostics.CodeAnalysis;
    using System;
    using System.Collections.Generic;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class RolDto
    {
        public Guid IdRol { get; set; }
        public string Nombre { get; set; }        
        public string OpcionesConcat { get; set; }
        public List<OpcionDto> Opciones { get; set; }
        public Guid IdAplicacion { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public bool Agregar { get; set; }
        public bool Seleccionado { get; set; }
        public bool Vigente { get; set; }
        public bool ForzarDesvinculacionDePerfil { get; set; }
        public List<PermisoDto> Permisos { get; set; }
        public string UsuarioDnp { get; set; }
    }

    public class ParametrosUsuariosConfiguracionDto
    {
        public string Identificacion { get; set; }

        public string Nombre { get; set; }
        public List<Guid> Entidades { get; set; }
    }

    public class RespuestaUsuariosConfiguracionDto
    {
        public Guid IdUsuario { get; set; }

        public string IdUsuarioDNP { get; set; }
        public string NombreUsuario { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
    }
    public class ParametrosSectoresUsuarioConfiguracionDto
    {
        public Guid IdUsuario { get; set; }
        public List<Guid> Entidades { get; set; }
    }

    public class RespuestaSectoresUsuarioConfiguracionDto
    {
        public Guid IdUsuarioPerfil { get; set; }
        public Guid IdEntidad { get; set; }

        public string NombreEntidad { get; set; }
        public Guid IdPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public Guid IdRol { get; set; }
        public string NombreRol { get; set; }
        public List<SectoresUsuarioDto> UsuarioSectores { get; set; }

    }
    public class SectoresUsuarioDto
    {
        public Guid? IdUsuarioPerfilSector1 { get; set; }
        public Guid? IdSector1 { get; set; }
        public string NombreSector1 { get; set; }
        public bool EstadoSector1 { get; set; }
        //
        public Guid? IdUsuarioPerfilSector2 { get; set; }
        public Guid? IdSector2 { get; set; }
        public string NombreSector2 { get; set; }
        public bool EstadoSector2 { get; set; }
        //
        public Guid? IdUsuarioPerfilSector3 { get; set; }
        public Guid? IdSector3 { get; set; }
        public string NombreSector3 { get; set; }
        public bool EstadoSector3 { get; set; }
    }
}
