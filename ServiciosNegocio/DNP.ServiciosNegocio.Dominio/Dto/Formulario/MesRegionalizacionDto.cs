
namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using Proyectos;

    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class MesRegionalizacionDto
    {
        public int? Mes { get; set; }
        public string NombreMes { get; set; }
        public List<ObjetivoEspecificoDto> Objetivos { get; set; }
    }
}
