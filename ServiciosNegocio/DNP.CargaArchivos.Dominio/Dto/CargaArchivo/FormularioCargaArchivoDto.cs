

namespace DNP.CargaArchivos.Dominio.Dto.CargaArchivo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class FormularioCargaArchivoDto
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Version { get; set; }

        public DateTime FechaCreacion { get; set; }

        public bool Activo { get; set; }

        public List<TipoOperacionesDto> TipoOperaciones { get; set; }
    }
}
