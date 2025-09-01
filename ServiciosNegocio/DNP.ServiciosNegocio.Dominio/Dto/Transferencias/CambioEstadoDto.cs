namespace DNP.ServiciosNegocio.Dominio.Dto.Transferencias
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CambioEstadoDto
    {
        /// <summary>
        /// Obtiene o establece el ProjectId
        /// </summary>  
        public string ProjectId { get; set; }

        /// <summary>
        /// Obtiene o establece la ProjectStatus
        /// </summary>
        public string ProjectStatus { get; set; }

        /// <summary>
        /// Obtiene o establece el valor Comment
        /// </summary> 
        public string Comment { get; set; }

        /// <summary>
        /// Obtiene o establece el valor CreatedBy
        /// </summary>  
        public string CreatedBy { get; set; }

        /// <summary>
        /// Obtiene o establece el valor CreatedBy
        /// </summary>
        public string CreatedByRole { get; set; }

        /// <summary>
        /// Obtiene o establece el valor BPIN
        /// </summary>  
        public string BPIN { get; set; }
    }
}
