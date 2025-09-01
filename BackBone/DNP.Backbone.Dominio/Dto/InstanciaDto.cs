using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    public class InstanciaDto
    {
        public Guid Id { get; set; }
        public string UsuarioId { get; set; }
        public Guid RolId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? EstadoInstanciaId { get; set; }
        public int? EntidadDestinoId { get; set; }
        public string ObjetoNegocioId { get; set; }
        public Guid? TipoObjetoId { get; set; }
        public string TipoObjeto { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
        public Guid FlujoId { get; set; }
        public Guid? PadreId { get; set; }
        public string Descripcion { get; set; }

    }
}
