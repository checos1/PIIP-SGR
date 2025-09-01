using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.ValidarViabilidadCompletarInfo
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class MensajeErrorDto
    {
        public string MensajeError { get; set; }
       

    }
   

}
