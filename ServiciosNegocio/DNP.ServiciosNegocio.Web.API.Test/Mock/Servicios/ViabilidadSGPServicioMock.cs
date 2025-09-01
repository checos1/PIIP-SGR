using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Viabilidad;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public class ViabilidadSGPServicioMock : IViabilidadSGPServicio
    {
        public string Usuario { get; set; }
        public string Ip { get; set; }
        public string SGPTransversalLeerParametro(string parametro)
        {
            return string.Empty;
        }

        public LeerInformacionGeneralViabilidadDto SGPViabilidadLeerInformacionGeneral(int proyectoId, System.Guid instanciaId, string tipoConceptoViabilidadCode)
        {
            throw new NotImplementedException();
        }

        public string SGPViabilidadLeerParametricas(int proyectoId, System.Guid nivelId)
        {
            return string.Empty;
        }
        public ResultadoProcedimientoDto SGPViabilidadGuardarInformacionBasica(string json, string usuario)
        {
            throw new NotImplementedException();
        }
        public ResultadoProcedimientoDto SGPViabilidadFirmarUsuario(string json, string usuario)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<ProyectoViabilidadInvolucradosDto> SGPProyectosLeerProyectoViabilidadInvolucrados(int proyectoId, Guid instanciaId, int tipoConceptoViabilidadId)
        {
            throw new NotImplementedException();
        }

        public ProyectoViabilidadInvolucradosResultado GuardarProyectoViabilidadInvolucradosSGP(ParametrosGuardarDto<ProyectoViabilidadInvolucradosDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        public ProyectoViabilidadInvolucradosResultado EliminarProyectoViabilidadInvolucradosSGP(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProyectoViabilidadInvolucradosFirmaDto> SGPProyectosLeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId)
        {
            throw new NotImplementedException();
        }

        public EntidadDestinoResponsableFlujoSgpDto SGPProyectosObtenerEntidadDestinoResponsableFlujo(Guid rolId, int crTypeId, int entidadResponsableId, int proyectoId)
        {
            throw new NotImplementedException();
        }

        public EntidadDestinoResponsableFlujoSgpDto SGPProyectosObtenerEntidadDestinoResponsableFlujoTramite(Guid rolId, int entidadResponsableId, int tramiteId)
        {
            throw new NotImplementedException();
        }
    }
}