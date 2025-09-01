using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.ProgramarProducto
{
    public class ProgramarProductoDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public string usuarioDNP { get; set; }
        public List<Objetivos> Objetivos { get; set; }
    }
    public class Objetivos
    {
        public int ObjetivoEspecificoId { get; set; }
        public Productos Productos { get; set; }
    }
    public class Productos
    {
        public int ProductoId { get; set; }
        public int IndicadorId { get; set; }
        public List<Vigencias> Vigencias { get; set; }
    }
    public class Vigencias
    {
        public int PeriodoProyectoId { get; set; }
        public List<Meses> Meses { get; set; }

    }
    public class Meses
    {
        public int PeriodoPeriodicidadId { get; set; }
        public decimal CantidadProgramada { get; set; }
    }
}
