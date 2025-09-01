namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class VigenciasFocalizacionCuantificacionDto
    {
        public int? Vigencia { get; set; }
        public List<LocalizacionFocalizacionCuantificacionDto> Localizacion { get; set; }
    }
}
