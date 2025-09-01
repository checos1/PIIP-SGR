namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class FuenteFinanciacionAgregarDto
    {
        public int? FuenteId { get; set; }
        public int? IdGrupoRecurso { get; set; }
        public string CodigoGrupoRecurso { get; set; }
        public string NombreGrupoRecurso { get; set; }
        public int? IdTipoEntidad { get; set; }
        public string CodigoTipoEntidad { get; set; }
        public string NombreTipoEntidad { get; set; }
        public int? IdEntidad { get; set; }
        public string CodigoEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public int? IdTipoRecurso { get; set; }
        public string CodigoTipoRecurso { get; set; }
        public string NombreTipoRecurso { get; set; }
        public int? IdEtapa{ get; set; }
        public string NombreEtapa { get; set; }

    }
}
