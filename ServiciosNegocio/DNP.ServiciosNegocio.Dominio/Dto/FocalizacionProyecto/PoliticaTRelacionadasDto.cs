namespace DNP.ServiciosNegocio.Dominio.Dto.FocalizacionProyecto
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class PoliticaTRelacionadasDto
    {

        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public int CR { get; set; }
        public double ValortotalProyecto { get; set; }
        public int TotalBeneficiariosProyecto { get; set; }
        public List<Vigencia> Vigencias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Localizaciones
    {
        public int LocalizacionId { get; set; }
        public string Localizacion { get; set; }
        public int BeneficiariosPoliticaPrincipal { get; set; }
        public int BeneficiariosPoliticaRelacionada { get; set; }
        public double RecursosPoliticaPrincipal { get; set; }
        public int RecursosPoliticaRelacionada { get; set; }
        public int PeriodoProyectoId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PoliticasRelacionada
    {
        public int PoliticaId { get; set; }
        public string PoliticaRelacionada { get; set; }
        public int TotalBeneficiarios { get; set; }
        public double RecursosFocalizados { get; set; }
        public List<Localizaciones> Localizacion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PoliticasPrincipale
    {
        public int PoliticaId { get; set; }
        public string PoliticaPrincipal { get; set; }
        public int TotalBeneficiarios { get; set; }
        public double RecursosFocalizados { get; set; }
        public List<PoliticasRelacionada> PoliticasRelacionadas { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Vigencia
    {
        public int Año { get; set; }
        public List<PoliticasPrincipale> PoliticasPrincipales { get; set; }
    }

}
