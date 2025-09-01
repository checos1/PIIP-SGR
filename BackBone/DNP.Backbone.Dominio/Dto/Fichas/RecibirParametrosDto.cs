namespace DNP.Backbone.Dominio.Dto.Fichas
{
    public class RecibirParametrosDto
    {
        public string IdReporte { get; set; }
        public string NombreReporte { get; set; }
        public bool PARAM_BORRADOR { get; set; }
        public string PARAM_BPIN { get; set; }
        public string InstanciaId { get; set; }
        public string NivelId { get; set; }
        public string TramiteId { get; set; }
    }
}