
namespace DNP.EncabezadoPie.Dominio.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class EncabezadoPieBasicoDto
    {
        public int Id { get; set; }

        public string Bpin { get; set; }

        public string Sector { get; set; }

        public string Entidad { get; set; }

        public int AnioInicio { get; set; }

    }
}
