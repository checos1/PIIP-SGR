
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using System;
using DNP.ServiciosTransaccional.Persistencia.Interfaces.Tramites;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Comunes.Dto.Tramites;

namespace DNP.ServiciosTransaccional.Test.Mock
{
    public class TramitePersistenciaMock : ITramitePersistencia
    {
        public TramitesResultado ActualizarCargueMasivo(string numeroProceso, string usuario)
        {

            throw new NotImplementedException();
        }

        public string ConsultarCargueExcel(string numeroProceso)
        {

            throw new NotImplementedException();
        }

        ResponseDto<bool> ITramitePersistencia.ActualizarCargueMasivo(string numeroProceso, string usuario)
        {
            return new ResponseDto<bool>
            {
                Estado = true
            };
        }

        string ITramitePersistencia.ConsultarCargueExcel(string numeroProceso)
        {
            throw new NotImplementedException();
        }

        string ITramitePersistencia.EliminarMarcaPrevioProyectoVigencia(string bpin, string vigencia)
        {
            throw new NotImplementedException();
        }

        DetalleCartaConceptoDto ITramitePersistencia.GetRadicadoEntradaORFEO(int? tramiteId)
        {
            throw new NotImplementedException();
        }

        List<ServiciosNegocio.Comunes.Dto.Tramites.TramiteProyectoDto> ITramitePersistencia.GetTramiteProyectos(int tramiteId)
        {
            throw new NotImplementedException();
        }

        string ITramitePersistencia.GetUsuarioDestinoORFEO(int? tramiteId, string idUsuarioDNP)
        {
            throw new NotImplementedException();
        }

        List<AnalistaResponsableDto> ITramitePersistencia.ObtenerAnalistaResponsablePorSector(int sectorId)
        {
            throw new NotImplementedException();
        }

        int ITramitePersistencia.PostActualizarCartaRadicado(int tramiteId, string usuarioDnp, string radicadoEntrada, string radicadoSalida, string expedienteId)
        {
            throw new NotImplementedException();
        }

        public List<DatosUsuarioDto> ObtenerUsuariosPorInstanciaPadre(Guid InstanciaId)
        {
            List<DatosUsuarioDto> lista = new List<DatosUsuarioDto>();
            if (InstanciaId == new Guid())
                return null;
            else
            {
                DatosUsuarioDto d = new DatosUsuarioDto();
                d.UsuarioDnp = "CC202002";
                d.NombreUsuario = "PUEBA";
                d.Cuenta = "prueba@yopmail.com";
                lista.Add(d);
            }
            return lista;
        }

        public List<CodigoPresupuestal_Proyecto> ObtenerDatosMarcaPrevioVigencia_Proyectos(string Bpin)
        {
            List<CodigoPresupuestal_Proyecto> lista = new List<CodigoPresupuestal_Proyecto>();
            if (string.IsNullOrEmpty(Bpin))
                return null;
            else
            {
                CodigoPresupuestal_Proyecto d = new CodigoPresupuestal_Proyecto();
                d.NombreEntidad = "MINISTERIO DE HACIENDA";
                d.Programa = "1206  - Sistema penitenciario y carcelario en el marco de los derechos humanos";
                d.Subprograma = "0800 INTERSUBSECTORIAL JUSTICIA";
                lista.Add(d);
            }
            return lista;
        }

        ResponseDto<bool> ITramitePersistencia.ActualizaCampoRemitenteConcepto(int tramiteId, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public DetalleTramiteDto ObtenerDetalleTramiteRadicado(string numeroTramite)
        {
            DetalleTramiteDto d = new DetalleTramiteDto();
            if (string.IsNullOrEmpty(numeroTramite))
                return null;
            else
            {
                d.TramiteId = 10;
                d.NombreEntidad = "MINISTERIO DE HACIENDA";
                d.CodigoDocumental = 48671;
                d.TipoTramiteId = 6;

            }


            return d;

        }
        public int ObtenerDependenciaByEntidadOrfeoId(int EntidadOrfeoId)
        {
            return 234;
        }
    }
}
