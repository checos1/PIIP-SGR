using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ProyectoDto
    {
        public int Id { get; set; }
     
        public string Proyecto { get; set; }

        public int InversionRequerida { get; set; }

        public string Estado { get; set; }

        public string Bpin { get; set; }

        public string FechaInicio { get; set; }

        public string FechaFin { get; set; }

        public string Prioridad { get; set; }

        public string UsuarioResponsable { get; set; }
        
        public int EntidadId { get; set; }
        public string Entidad { get; set; }
        public int VigenciaInicial { get; set; }
        public int VigenciaFinal { get; set; }
        public string Horizonte { get; set; }
        public decimal ValorTotalProyecto { get; set; }
        public string EstadoBanco { get; set; }

    }
}

