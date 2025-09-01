using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class ProyectoNuevaDto
    {
        public DateTime FechaInicioDesarrolloProyecto { get; set; }
        public string Alcance { get; set; }
    }
}
