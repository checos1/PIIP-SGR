namespace DNP.Backbone.Comunes.Dto
{
    using System;
    using System.Collections.Generic;    
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class EntidadesPorCodigoParametrosDto
    {
        public string Aplicacion { get; set; }
        public string IdUsuario { get; set; }
        public Guid IdObjeto { get; set; }
        public Guid? InstanciaId { get; set; }
        public string IdFiltro { get; set; }
        public int IdDepartamento { get; set; }
    }
}
