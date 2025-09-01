namespace DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionada
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class PoliticaTransversalRelacionadaDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public int CR { get; set; }
        public List<VigenciasPoliticasCuantificaBeneficiario> Vigencias_Politicas_Cuantifica_Beneficiarios { get; set; }
        public List<VigenciasPoliticasNoCuantificaBeneficiario> Vigencias_Politicas_No_Cuantifica_Beneficiarios { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Localizacion
    {
        public int? DepartmentId { get; set; }
        public string Departamento { get; set; }
        public int? BeneficiariosPoliticaPrincipalDepto { get; set; }
        public double? RecursosFocalizadosPoliticaPrincipalDepto { get; set; }
        public int? BeneficiariosPoliticaRelacionadaDepto { get; set; }
        public double? RecursosFocalizadosPoliticaRelacionadaDepto { get; set; }
        public int? LCPRelacionadaValoresBeneficiariosId { get; set; }
        public int? LCPRelacionadaValoresRecursosId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PoliticasRelacionada
    {
        public int? FocalizacionPoliticaId { get; set; }
        public int? PoliticaRelacionadaId { get; set; }
        public string PoliticaRelacionada { get; set; }
        public int? TotalBeneficiariosPoliticaRelacionada { get; set; }
        public double? TotalRecursosFocalizadosPoliticaRelacionada{ get; set; }
        public List<Localizacion> Localizacion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PoliticasPrincipale
    {
        public int? CuantificaBeneficiariosPPrincipal { get; set; }
        public int? PoliticaPrincipalId { get; set; }
        public string PoliticaPrincipal { get; set; }
        public int? PeriodoProyectoId { get; set; }
        public int? TotalBeneficiariosPoliticaPrincipal { get; set; }
        public double? TotalRecursosFocalizadosPoliticaPrincipal { get; set; }
        public List<PoliticasRelacionada> PoliticasRelacionadas { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciasPoliticasCuantificaBeneficiario
    {
        public int? Año { get; set; }
        public double? ValorProyecto { get; set; }
        public int? TotalBeneficiariosProyecto { get; set; }
        public List<PoliticasPrincipale> PoliticasPrincipales { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciasPoliticasNoCuantificaBeneficiario
    {
        public int? Año { get; set; }
        public double? ValorProyecto { get; set; }
        public int? TotalBeneficiariosProyecto { get; set; }
        public List<PoliticasPrincipale> PoliticasPrincipales { get; set; }
    }

}
