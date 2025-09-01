namespace DNP.ServiciosNegocio.Dominio.Dto.CostosEntregables
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class CostosEntregablesDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<Vigencia> vigencias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Vigencia
    {
        public int vigencia { get; set; }
        public double ValorFuentesPeriodo { get; set; }
        public double ValorActividadesPeriodo { get; set; }
        public List<ObjetivoEspecifico> ObjetivosEspecificos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ObjetivoEspecifico
    {
        public int ObjetivoEspecificoId { get; set; }
        public string objetivoEspecifico { get; set; }
        public List<Producto> Productos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Producto
    {
        public int ProductoId { get; set; }
        public int ProductoCId { get; set; }
        public string producto { get; set; }
        public int UnidaddeMedidaId { get; set; }
        public string UnidaddeMedida { get; set; }
        public double Cantidad { get; set; }
        public double ValorTotalProductoAjustado { get; set; }
        public double ValorTotalProductoFirme { get; set; }
        public double? ValorTotalSolicitadosGR { get; set; }
        public double? ValorTotalVigenteAjuste { get; set; }
        public double? ValorTotalSolictadosAjuste { get; set; }
        public List<Entregable> Entregables { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Entregable
    {
        public int? EntregableId { get; set; }
        public int? EntregableCId { get; set; }
        public string entregable { get; set; }
        public int? EntregableInsumoId { get; set; }
        public double? RecursosSolicitadosMGA { get; set; }
        public double? RecursosVigentesFirme { get; set; }
        public double? RecursosVigentesAjuste { get; set; }      
              
    }
}