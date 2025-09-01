using DNP.Autorizacion.Dominio.Dto;
using System;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public sealed class EntidadDto
    {

        public EntidadDto()
        {
            ListadoProyectos = new HashSet<ProyectoDto>();
            ListadoSolicitudes = new HashSet<SolicitudDto>();
            ListadoRoles = new HashSet<RolDto>();
        }


        public Guid IdEntidad { get; set; }

        public string NombreEntidad { get; set; }

        private String nombreCompletoEntidad = String.Empty;
        public String NombreCompletoEntidad
        {
            get { return this.nombreCompletoEntidad; }
            set
            {
                if (!String.IsNullOrEmpty(value) && this.nombreCompletoEntidad.Equals(value)) return;
                this.nombreCompletoEntidad = value;
            }
        }

        public ICollection<ProyectoDto> ListadoProyectos { get; set; }

        public ICollection<SolicitudDto> ListadoSolicitudes { get; set; }

        public ICollection<RolDto> ListadoRoles { get; set; }

        public int SectorId { get; set; }
        public string TipoEntidad { get; set; }
    }
}
