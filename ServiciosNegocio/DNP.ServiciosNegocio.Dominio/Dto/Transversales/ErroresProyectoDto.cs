namespace DNP.ServiciosNegocio.Dominio.Dto.Transversales
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ErroresProyectoDto
    {
        public string Seccion { get; set; }
        public string Capitulo { get; set; }
        public string Errores { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class ErroresJson
    {
        public string Error { get; set; }
        public string Descripcion { get; set; }
        public string Data { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ErroresJsonCostos
    {
        public List<ErroresJson> recursoscostosdelasacti { get; set; }

        public ErroresJsonCostos()
        {
            recursoscostosdelasacti = new List<ErroresJson>();
        }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciasFuentesCostosDtoJson
    {
        public string BPIN { get; set; }
        public List<VigenciasEtapas> Etapas { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciasEtapas
    {
        public string Etapa { get; set; }
        public List<VigenciasFuentesCostosDto> Valores { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciasFuentesCostosDto
    {
        public int Vigencia { get; set; }
        public double ValorFuentes { get; set; }
        public double ValorCosto { get; set; }
    }


    [ExcludeFromCodeCoverage]
    public class DataProyectoFaseDto
    {
        public int? FaseId { get; set; }
        public string NivelPaso { get; set; }
        public int? PasoFaseId { get; set; }
        public int? FaseMacroproceso { get; set; }
        public int? FaseProceso { get; set; }
        public int? FasePaso { get; set; }
        public int? ProyectoId { get; set; }
        public string GuidInstancia { get; set; }

        public DataProyectoFaseDto()
        {


        }
    }
}
