using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class HorizonteProyectoDto
    {
        public int IdProyecto { get; set; }
        public int Mantiene { get; set; }
        public string VigenciaInicio { get; set; }
        public string VigenciaFinal { get; set; }
        public string Usuario { get; set; }
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public string GuiMacroproceso { get; set; }
        public Guid InstanciaId { get; set; }
        public int SeccionCapituloId { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class DocumentoCONPESDto
    {
        public int id { get; set; }
        public string ano { get; set; }
        public string titulo { get; set; }
        public string numeroCONPES { get; set; }
        public DateTime? fechaAprobacion { get; set; }
        public string tipoCONPES { get; set; }
        public string docNombre { get; set; }
        public string docUrl { get; set; }
        public int seleccionado { get; set; }
        public int proyectoId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CapituloConpes
    {
        public List<DocumentoCONPESDto> Conpes { get; set; }
        public int ProyectoId { get; set; }
        public string Justificacion { get; set; }
        public int SeccionCapituloId { get; set; }
        public string GuiMacroproceso { get; set; }
        public Guid InstanciaId { get; set; }
    }
}


