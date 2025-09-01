namespace DNP.Backbone.Comunes.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ParametrosObjetosNegocioDto
    {
        public Guid IdTipoObjetoNegocio { get; set; }
        public string IdUsuarioDNP { get; set; }
        public List<Guid> IdsRoles { get; set; }
        public List<int> EntidadesVisualizador { get; set; }
        public string TokenAutorizacion { get; set; }
        public Guid? InstanciaId { get; set; }
        public int? EntidadId { get; set; }
        public string UsuarioDNP { get; set; }

        public ProyectoFiltroDto ProyectoFiltro { get; set; }
    }
    public class ParametrosValidarFlujoDto
    {
        public string IdUsuarioDNP { get; set; }
        public List<int> ListaEntidades { get; set; }
        public Guid? Flujo { get; set; }
    }

    public class RespuestaParametrosValidarFlujoDto
    {
        public bool EstaActivo { get; set; }
        public string Mensaje { get; set; }
    }
}
