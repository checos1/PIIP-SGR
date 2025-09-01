namespace DNP.ServiciosNegocio.Dominio.Dto.Transversales
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ErroresPreguntasDto
    {
        public string Seccion { get; set; }
        public string Capitulo { get; set; }
        public string Errores { get; set; }

    }

}
