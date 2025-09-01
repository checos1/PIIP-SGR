using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public sealed class SectorDto 
    {

        public SectorDto()
        {
            EntidadList = new HashSet<EntidadDto>();
        }

        public int id { get; set; }
        public string sector { get; set; }

        public ICollection<EntidadDto> EntidadList { get; set; }
        

    }
}
