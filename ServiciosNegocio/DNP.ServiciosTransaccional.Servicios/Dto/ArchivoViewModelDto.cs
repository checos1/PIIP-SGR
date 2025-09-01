using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Dto
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
