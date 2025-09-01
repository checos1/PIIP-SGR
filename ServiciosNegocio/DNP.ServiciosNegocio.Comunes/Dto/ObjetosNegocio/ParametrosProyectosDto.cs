namespace DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ParametrosProyectosDto
    {
        public List<int> IdsEntidades { get; set; }
        public List<string> NombresEstadosProyectos { get; set; }
        public string TokenAutorizacion { get; set; }
        public int? IdTramite { get; set; }
        public string IdUsuarioDNP { get; set; }
        //Estas propiedades se adicionan para realizar la busqueda de los proyectos
        public Guid flujoid { get; set; }
        public int tipoTramiteId { get; set; }
        public string tipoEntidad { get; set; }
    }
}
