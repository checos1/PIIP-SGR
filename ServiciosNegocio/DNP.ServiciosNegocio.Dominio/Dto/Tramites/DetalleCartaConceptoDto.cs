namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class DetalleCartaConceptoDto
    {
        public int TramiteId { get; set; }

        public int FaseId { get; set; }

        public string RadicadoEntrada { get; set; }

        public string RadicadoSalida { get; set; }

        public string ExpedienteId { get; set; }
    }
}
