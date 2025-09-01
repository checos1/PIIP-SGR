namespace DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionadaAjustes
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class PoliticaTransversalRelacionadaAjustesDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<VigenciasPoliticasCuantificaBeneficiario> Vigencias_Politicas_Cuantifica_Beneficiarios { get; set; }
        public List<VigenciasPoliticasNoCuantificaBeneficiario> Vigencias_Politicas_No_Cuantifica_Beneficiarios { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Localizacion
    {
        public int? DepartmentId { get; set; }
        public string Departamento { get; set; }
        public int? BeneficiariosPoliticaPrincipalVigente_Firme { get; set; }
        public int? BeneficiariosPoliticaPrincipalVigente_Ajuste { get; set; }
        public double? RecursosPoliticaPrincipalVigente_Firme { get; set; }
        public double? RecursosPoliticaPrincipalVigente_Ajuste { get; set; }
        public int? BeneficiariosPoliticaRelacionadaVigente_Firme { get; set; }
        public int? BeneficiariosPoliticaRelacionadaVigente_Ajuste { get; set; }
        public double? RecursosPoliticaRelacionada_Firme { get; set; }
        public double? RecursosPoliticaRelacionada_Ajuste { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PoliticasRelacionada
    {
        public int? FocalizacionPoliticaId { get; set; }
        public int? PoliticaRelacionadaId { get; set; }
        public string PoliticaRelacionada { get; set; }
        public List<Localizacion> Localizacion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PoliticasPrincipales
    {
        public int? CuantificaBeneficiariosPPrincipal { get; set; }
        public int? PoliticaPrincipalId { get; set; }
        public string PoliticaPrincipal { get; set; }
        public int? PeriodoProyectoId { get; set; }
        public List<PoliticasRelacionada> PoliticasRelacionadas { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciasPoliticasCuantificaBeneficiario
    {
        public int? Año { get; set; }
        
        public List<PoliticasPrincipales> PoliticasPrincipales { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciasPoliticasNoCuantificaBeneficiario
    {
        public int? Año { get; set; }
        public List<PoliticasPrincipales> PoliticasPrincipales { get; set; }
    }

}
