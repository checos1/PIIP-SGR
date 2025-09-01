using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ProyectoPriorizarDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public string NombreProyecto { get; set; }
        public int EntidadId { get; set; }
        public string NombreEntidad { get; set; }
        public bool Priorizado { get; set; }
        public int Orden { get; set; }
        public bool PermitePriorizar { get; set; }
        public Guid FlujoId { get; set; }

    }
}
