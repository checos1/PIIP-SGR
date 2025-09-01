using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.GestionRecursos.Aprobacion
{
    public class ProyectoResumenEstadoAprobacionCreditoDto
    {
        // Clave de la entidad que aprueba
        public int EntidadApruebaId { get; set; }

        // Cabecera del proyecto
        public int ProyectoId { get; set; }
        public string EstadoInstancia { get; set; }
        public string Entidad { get; set; }
        public string EntidadTipo { get; set; }
        public DateTime FechaAprobacion { get; set; }

        // Detalle anidado
        public List<DetalleAprobacionDto> Detalles { get; set; }
    }

    public class DetalleAprobacionDto
    {
        public string Etapa { get; set; }
        public string TipoRecurso { get; set; }
        public string EntidadSolicita { get; set; }
        public string TipoEntidadSolicita { get; set; }
        public string BienioSolicitado { get; set; }
        public decimal ValorCreditoAprobado { get; set; }
        public decimal CostoFinanciero { get; set; }
        public decimal PatrimonioAutonomo { get; set; }
        public decimal ValorSolicitado { get; set; }
        public decimal ValorCreditoOPC { get; set; }
        public decimal ValorFinancieroOPC { get; set; }
        public decimal PatrimonioAutonomoOPC { get; set; }

        public decimal ServicioDeuda { get; set; }
        public string Bienio { get; set; }
        public int Orden { get; set; }
        public bool EstadoAprobacion { get; set; }
    }


}
