namespace DNP.ServiciosNegocio.Dominio.Dto.Programacion
{
    public class CargueCuotaDto
    {
        public int EntityTypeCatalogOptionId { get; set; }
        public string Codigo { get; set; }
        public string Entidad { get; set; }
        public string Sector { get; set; }
        public decimal Propios { get; set; }
        public decimal NacionSSF { get; set; }
        public decimal NacionCSF { get; set; }
        public int CuotaEntidadProgramacionId { get; set; }
        public int CuotaEntidadId { get; set; }
        public int Vigencia { get; set; }
    }
}
