namespace DNP.ServiciosNegocio.Dominio.Dto.DiligenciarFuentes
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class DiligenciarFuentesProyectoDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public int CR { get; set; }
        public double ValorEtapaPreInversion { get; set; }
        public double ValorEtapaInversion { get; set; }
        public double ValorEtapaOperacion { get; set; }
        public double ValorTotalProyecto { get; set; }
        public List<Etapa> Etapas { get; set; }
        public List<Fuentes> Fuentes { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Etapa
    {
        public int Vigencia { get; set; }
        public double Preinversion { get; set; }
        public double Inversion { get; set; }
        public double Operacion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Vigencias
    {
        public int PeriodoProyectoId { get; set; }
        public int Vigencia { get; set; }
        public double Preinversion { get; set; }
        public double Inversion { get; set; }
        public double Operacion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Fuentes
    {
        public int FuenteId { get; set; }
        public string Fuente { get; set; }        
        public string Cofinanciador { get; set; }
        public int TipoCofinanciador { get; set; }
        public int? FuenteCofinanciadorId { get; set; }
        public List<Vigencias> Vigencias { get; set; }
    }
}
