namespace DNP.ServiciosNegocio.Dominio.Dto.SGR
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class FocalizacionPoliticaSgrDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<FuentesFinanciacionSgr> FuentesFinanciacionSgr { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class FuentesFinanciacionSgr
    {
        public int? FuenteId { get; set; }
        public int? EtapaId { get; set; }
        public string Etapa { get; set; }
        public int? FinaciadorId { get; set; }
        public string Finaciador { get; set; }
        public int? EntidadId { get; set; }
        public string Entidad { get; set; }
        public int? RecursoId { get; set; }
        public string Recurso { get; set; }
        public string Texto { get; set; }
        public List<PoliticasSgr> PoliticasSgr { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PoliticasSgr
    {
        public int? FocalizacionPoliticaId { get; set; }
        public int? PoliticaId { get; set; }
        public string Politica { get; set; }
        public int? Categoria { get; set; }
        public int? Indicador { get; set; }
        public int? CrucePolitica { get; set; }
    }
}
