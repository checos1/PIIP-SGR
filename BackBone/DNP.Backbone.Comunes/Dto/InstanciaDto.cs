using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class InstanciaDto
    {
        public string IdUsuario { get; set; }
        public Guid IdObjeto { get; set; }
        public List<Guid> ListaIdsRoles { get; set; }
        public List<int> ListaIdsEntidades { get; set; }
        public string Aplicacion { get; set; }
        public Guid Id { get; set; }
        public Guid RolId { get; set; }
        public int? EstadoInstanciaId { get; set; }
        public int? EntidadDestino { get; set; }
        public string ObjetoNegocioId { get; set; }
        public Guid? TipoObjetoId { get; set; }
        public string TipoObjeto { get; set; }
        public Guid FlujoId { get; set; }
        public Guid? PadreId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
        public string Descripcion { get; set; }
    }
}
