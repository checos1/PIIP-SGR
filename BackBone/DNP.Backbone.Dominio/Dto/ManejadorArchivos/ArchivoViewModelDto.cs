using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.ManejadorArchivos
{
    public class ArchivoViewModelDto : ArchivoInfoCargueDto
    {
        public ArchivoViewModelDto()
        {
            this.Archivos = new List<ArchivoEntidadDto>();
        }

        public List<Dictionary<string, object>> FormFile { get; set; }
    }
}
