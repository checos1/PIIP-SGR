using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTUS;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Viabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    internal class CTUSServicioMock : ICTUSServicio
    {
        public string Usuario { get; set; }
        public string Ip { get; set; }
        public string ValidarInstanciaCTUSNoFinalizada(int idProyecto)
        {
            return "{\"InstanciaCTUSNoFinalizada\":true, \"MensajeRespuesta\":\"Mensaje\"}";
        }       

        public ResultadoProcedimientoDto SGR_CTUS_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioCTUSDto json, string usuario)
        {
            throw new NotImplementedException();
        }

        public ResultadoProcedimientoDto SGR_CTUS_GuardarProyectoCtusConcepto(ConceptoCTUSDto json, string usuario)
        {
            throw new NotImplementedException();
        }

        public ResultadoProcedimientoDto SGR_CTUS_GuardarResultadoConceptoCtus(ResultadoConceptoCTUSDto json, string usuario)
        {
            throw new NotImplementedException();
        }

        public ConceptoCTUSDto SGR_CTUS_LeerProyectoCtusConcepto(int proyectoCtusId)
        {
            throw new NotImplementedException();
        }

        public UsuarioEncargadoCTUSDto SGR_CTUS_LeerProyectoCtusUsuarioEncargado(int proyectoCtusId, Guid instanciaId)
        {
            throw new NotImplementedException();
        }

        public RolApruebaCTUSDto SGR_CTUS_LeerRolDirectorProyectoCtus(int proyectoId, Guid instanciaId)
        {
            throw new NotImplementedException();
        }

        public bool SGR_Proyectos_ActualizarEntidadAdscritaCTUS(int proyectoId, int entityId, string tipo, string user)
        {
            throw new NotImplementedException();
        }
    }
}
