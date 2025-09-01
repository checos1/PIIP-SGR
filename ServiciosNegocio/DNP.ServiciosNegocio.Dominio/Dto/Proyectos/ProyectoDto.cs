using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    using System.Collections.Generic;
    using Formulario;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ProyectoDto
    {
        public string Id { get; set; }
        public string Bpin { get; set; }
        public string Proyecto { get; set; }
        public string Entidad { get; set; }
        public int VigenciaInicial { get; set; }
        public int VigenciaFinal { get; set; }
        public string Horizonte { get; set; }
        public decimal ValorTotalProyecto { get; set; }
        public string EstadoBanco { get; set; }
        public string Estado { get; set; }

        public List<VigenciaDto> Vigencia{ get; set; }
      
    }
}
