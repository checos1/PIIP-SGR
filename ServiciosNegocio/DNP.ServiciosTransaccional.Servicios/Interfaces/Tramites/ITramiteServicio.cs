using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosTransaccional.Servicios.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Interfaces.Tramites
{
    public interface ITramiteServicio
    {
        Task<ResponseDto<bool>> ReasignarRadicadoORFEO(ReasignacionRadicadoDto parametros, string usuario);

        Task<ResponseDto<bool>> CargarDocumentoElectronicoORFEO(DatosDocumentoElectronicoDSDto datosDocumentoElectronicoDSDto, string usuarioDNP, string usuarioRadica = "");
        Task<ResponseDto<bool>> ConsultarRadicado(string radicadoSalida, string usuarioDNP);
        Task<ResponseDto<bool>> CerrarRadicado(CerrarRadicadoDto parametros, string usuarioDNP, bool hasUserSelected = false);

        Task<ResponseDto<bool>> CerrarRadicadosTramite(string numeroTramite, string usuarioDNP);

        Task<ResponseDto<bool>> CerrarRadicadosTramiteDummy(string numeroTramite, string usuarioDNP);

        Task<CommonResponseDto<dynamic>> GenerarRadicadoEntrada(string numeroTramite, string usuarioDnp);

        Task<CommonResponseDto<dynamic>> GenerarRadicadoEntradaDummy(string numeroTramite, string usuarioDnp);

        Task<CommonResponseDto<CreacionRadicadoEntradaDto>> GenerarRadicadoSalidaDummy(string numeroTramite, string usuarioDnp);

        Task<CommonResponseDto<bool>> GenerarDocumentoFirmado(string numeroTramite, string usuarioDnp);

        Task<CommonResponseDto<CreacionRadicadoEntradaDto>> GenerarRadicadoSalida(string numeroTramite, string usuarioDnp);

        Task<CommonResponseDto<bool>> CerrarInstancias(string objetoNegocio, string usuarioDnp);

        Task<DetalleTramiteDto> ObtenerDetalleTramite(string numeroTramite, string usuarioDNP);

        Task<Carta> ConsultarCarta(int tramiteId, string usuarioDNP);
        Task<TramitesResultado> FirmarCarta(int tramiteId, string radicadoSalida, string usuarioDNP);
        Task<string> ObtenerPDF(int tramiteId, int TipoTramiteId, string usuarioDNP);

        Task<bool> NotificarUsuarios(Guid idInstancia, string usuarioDNP);
        Task<string> EliminarMarcaPrevioProyectoVigencia(string bpin, string vigencia, List<CodigoPresupuestal_Proyecto> lista, string usuarioDNP);
        ResponseDto<bool> ActualizarCargueMasivo(string numeroProceso, string usuario);
        string ConsultarCargueExcel(string numeroProceso);
        List<CodigoPresupuestal_Proyecto> ObtenerDatosMarcaPrevioVigencia_Proyectos(string Bpin);
        Task<string> EnviarCorreoMarcaPrevio(string bpin, string vigencia, List<CodigoPresupuestal_Proyecto> lista, string usuarioDNP);
        Task<string> NotificarMarcaPrevio(string bpin, string vigencia, List<CodigoPresupuestal_Proyecto> lista, string usuarioDNP);
    }
}
