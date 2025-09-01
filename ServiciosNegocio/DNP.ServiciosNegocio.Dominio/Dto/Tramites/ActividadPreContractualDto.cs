namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ActividadPreContractualDto
    {       
        public int? ProyectoId { get; set; }
        public int? TramiteId { get; set; }

        public List<ProyectoActividadCronogramaDto> ActividadesPreContractuales { get; set; }
        public List<ProyectoActividadCronogramaDto> ActividadesContractuales { get; set; }
        public List<ProyectoActividadCronogramaDto> EliminarActividadesContractuales { get; set; }
    }       
}