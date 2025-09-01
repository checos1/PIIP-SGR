namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DNP.Autorizacion.Dominio.Dto;

    [ExcludeFromCodeCoverage]
    public class UsuarioReportesDto
    {

        public string CabezaSector { get; set; }
        public string AgrupadorNombreEntidad  { get; set; }
             
        public string NombreEntidad { get; set; }

        public string NombreUsuario { get; set; }

        public string TipoIdentificacion { get; set; }

        public string Identificacion { get; set; }

        public string Correo { get; set; }

        public string Perfil { get; set; }

        public string Activo { get; set; }
        
        public string ActivoUsuarioPerfil { get; set; }


    }

}
