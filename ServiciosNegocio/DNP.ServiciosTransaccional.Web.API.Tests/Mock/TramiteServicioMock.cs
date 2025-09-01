using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosTransaccional.Servicios.Dto;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Tramites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Web.API.Test.Mock
{

    public class TramiteServicioMock : ITramiteServicio
    {
        public Task<ResponseDto<bool>> CargarDocumentoElectronicoORFEO(DatosDocumentoElectronicoDSDto datosDocumentoElectronicoDSDto, string usuarioDNP, string usuarioRadica = "")
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<bool>> ConsultarRadicado(string radicadoSalida, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResponseDto<dynamic>> GenerarRadicadoEntrada(string numeroTramite, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResponseDto<dynamic>> GenerarRadicadoSalida(string tramiteId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<bool>> ReasignarRadicadoORFEO(ReasignacionRadicadoDto parametros, string usuario)
        {
            throw new NotImplementedException();
        }

        Task<CommonResponseDto<CreacionRadicadoEntradaDto>> ITramiteServicio.GenerarRadicadoSalida(string numeroTramite, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        async Task<CommonResponseDto<bool>> ITramiteServicio.CerrarInstancias(string objetoNegocio, string usuarioDnp)
        {
            objetoNegocio = "00000000";
            usuarioDnp = "CC505050";
            CommonResponseDto<bool> respuesta = new CommonResponseDto<bool>();
            await Task.Delay(500);
            respuesta.Estado = true;
            respuesta.Mensaje = "Se cerrararon ls instancias";
            return respuesta;

        }

        public async Task<Carta> ConsultarCarta(int tramiteId, string usuarioDNP)
        {
            Carta carta = new Carta();
            await Task.Delay(500);
            carta.TramiteId = 500;
            carta.RadicadoEntrada = "202225252521";
            carta.RadicadoSalida = "202225252526";
            return carta;
        }

        public async Task<DetalleTramiteDto> ObtenerDetalleTramite(string numeroTramite, string usuarioDNP)
        {
            DetalleTramiteDto det = new DetalleTramiteDto();
            await Task.Delay(500);
            det.TramiteId = 500;
            det.TipoTramiteId = 4;
            det.NombreEntidad = "Presidencia De La República";
            det.EntidadId = 2;
            return det;
        }

        public async Task<TramitesResultado> FirmarCarta(int tramiteId, string radicadoSalida, string usuarioDNP)
        {
            TramitesResultado det = new TramitesResultado();
            await Task.Delay(500);
            det.Exito = true;
            det.Mensaje = "Exitoso";
            return det;
        }

        public async Task<string> ObtenerPDF(int tramiteId,int TipoTramiteId, string usuarioDNP)
        {
            await Task.Delay(500);
            return "Exito";
        }

        public Task<ResponseDto<bool>> CerrarRadicadosTramite(string numeroTramite, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResponseDto<bool>> GenerarDocumentoFirmado(string numeroTramite, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<bool>> CerrarRadicado(CerrarRadicadoDto parametros, string usuarioDNP, bool hasUserSelected = false)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> NotificarUsuarios(Guid idInstancia, string usuarioDNP)
        {
            bool respuesta = true;
            await Task.Delay(500);
            return respuesta;
        }

        public Task<ResponseDto<bool>> CerrarRadicadosTramiteDummy(string numeroTramite, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResponseDto<dynamic>> GenerarRadicadoEntradaDummy(string numeroTramite, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResponseDto<CreacionRadicadoEntradaDto>> GenerarRadicadoSalidaDummy(string numeroTramite, string usuarioDnp)
        {
            throw new NotImplementedException();
        }
        public Task<string> EliminarMarcaPrevioProyectoVigencia(string bpin, string vigencia, List<CodigoPresupuestal_Proyecto> lista, string usuarioDNP)
        {
            try
            {
                var rta   =  Task.FromResult<string>("Exitoso"); 
                //if (string.IsNullOrEmpty(rta) || !rta.Equals("Exitoso"))
                //{
                //    //await ResetAccionesFlujoPorTramite(numeroTramite, usuarioDNP);
                //}
                return rta;
            }
            catch (Exception e)
            {
                // await ResetAccionesFlujoPorTramite(numeroTramite, usuarioDnp);
                //response.Mensaje = e.Message;
                throw new Exception(e.Message);
            }
        }

        public ResponseDto<bool> ActualizarCargueMasivo(string numeroProceso, string usuario)
        {
            var det = new ResponseDto<bool>();
            det.Estado = true;
            det.Mensaje = "Exitoso";
            return det;
        }

        public string ConsultarCargueExcel(string numeroProceso)
        {

            var rta =  "[{'codigoProceso':'EJ - TP - CAL - 000000 - 0001','totalCargue':null,'DatosCargueEntidad':null}]";

            return rta;
        }

        public List<CodigoPresupuestal_Proyecto> ObtenerDatosMarcaPrevioVigencia_Proyectos(string Bpin)
        {
            List<CodigoPresupuestal_Proyecto> lista = new List<CodigoPresupuestal_Proyecto>();
            CodigoPresupuestal_Proyecto d = new CodigoPresupuestal_Proyecto();
            d.NombreEntidad = "MINISTERIO DE HACIENDA";
            d.Programa = "1206  - Sistema penitenciario y carcelario en el marco de los derechos humanos";
            d.Subprograma = "0800 INTERSUBSECTORIAL JUSTICIA";
            lista.Add(d);
            return lista;
        }

        public Task<string> EnviarCorreoMarcaPrevio(string bpin, string vigencia, List<CodigoPresupuestal_Proyecto> lista, string usuarioDNP)
        {
            try
            {
                var rta = Task.FromResult<string>("Exitoso");
                
                return rta;
            }
            catch (Exception e)
            {
                
                throw new Exception(e.Message);
            }
        }
        public Task<string> NotificarMarcaPrevio(string bpin, string vigencia, List<CodigoPresupuestal_Proyecto> lista, string usuarioDNP)
        {
            try
            {
                var rta = Task.FromResult<string>("Exitoso");

                return rta;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

    }
}
