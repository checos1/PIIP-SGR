using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.Procesos
{
    public class PriorizacionProyectoDto
    {
        public int Orden {  get; set; }
        public int EntidadId {  get; set; }
        public string NombreEntidad { get; set; }
        public Nullable<bool> Priorizado { get; set; }
        public Nullable<Guid> InstanciaId { get; set; }
    }
}
