using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Dto
{
    public class ArchivoInfoCargueDto
    {
        public IEnumerable<ArchivoEntidadDto> Archivos { get; set; }

        /// <summary>
        ///     Gets or sets the collection.
        /// </summary>
        public string Coleccion { get; set; }

        /// <summary>
        ///     Gets or sets the metadatos.
        /// </summary>
        public string Metadatos { get; set; }

        /// <summary>
        ///     Gets or sets the nombre.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        public string Status { get; set; }
    }
}
