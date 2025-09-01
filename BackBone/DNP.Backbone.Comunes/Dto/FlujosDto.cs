using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class FlujoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public float Version { get; set; }
        public string JsonFlujo { get; set; }
        public List<AccionesFlujosDto> Acciones { get; set; }
        public string DireccionIp { get; set; }
        public string Usuario { get; set; }
        public Guid IdAplicacion { get; set; }
        public Guid IdNivel { get; set; }
        public int Estado { get; set; }
        public InstanciaDto Instancia { get; set; }
        public TipoObjetoDto TipoObjeto { get; set; }
        public Guid? TipoObjetoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string NumeroTramite { get; set; }
    }
}
