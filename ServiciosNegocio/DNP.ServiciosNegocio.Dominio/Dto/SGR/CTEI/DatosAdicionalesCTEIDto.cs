using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.CTEI
{
    public class DatosAdicionalesCTEIDto
    {
        public int Id;
        public int ProyectoId;
        public Guid InstanciaId;
        public int ProgramaEstrategia;
        public string PAED;

        public int AtencionDeDesastres;
        public int EnfoqueDiferencial;
        public int Minorias;
        public int ConcordanteConAcuerdos;
        public int SolicitaVigenciasFuturas;
        public int RequiereCertificado;

        public decimal ValorInterventoria;
        public decimal ValorSupervision;
    }
}
