using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.ModificacionLey
{
    public class InformacionPresupuestalMLDto
    {
        public int TramiteProyectoId { get; set; }
        public string NivelId { get; set; }
        public int SeccionCapitulo { get; set; }
        public string Origen { get; set; }
        public List<ValoresFuenteML> ValoresFuente { get; set; }
    }

    public class ValoresFuenteML
    {
        public int FuenteId { get; set; }
        public decimal? NacionCSF { get; set; }
        public decimal? NacionSSF { get; set; }
        public decimal? Propios { get; set; }
    }
}
