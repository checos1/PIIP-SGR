using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.Procesos
{
    public class AprobacionProyectoDto
    {
        public int Orden {  get; set; }
        public int EntidadId {  get; set; }
        public string NombreEntidad { get; set; }
        public Nullable<bool> Aprobado { get; set; }
    }
}
