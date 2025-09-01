using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosTransaccional.Servicios.Dto
{
    public class CrearRadicadoRequestDto
    {
        public int IdEmpresa { get; set; }

        public string RadicadoPadreId { get; set; }

        public string UsuarioDnpDestino { get; set; }

        public string NumeroTramite { get; set; }

        public string ExpedienteId { get; set; }

        public bool EsVigenciaFutura { get; set; }

        public string CodigoTipoTramite { get; set; }

        public int? CodigoDocumental { get; set; }
        public int? CodigoDependencia { get; set; }

        public List<TramiteProyectoDto> ProyectosTramite { get; set; }

        public CrearRadicadoRequestEntidadDto DetalleEntidad { get; set; }

        public CrearRadicadoDetalleDto DatosRadicado { get; set; }

        public CrearRadicadoDatosExpedienteDto DatosExpediente { get; set; }

        public CrearRadicadoRequestDto() { }

        public static CrearRadicadoRequestDto CrearRadicadoEntrada(string numeroTramite, DetalleTramiteDto detalleTramiteDto, List<TramiteProyectoDto> proyectosTramite, string usuarioDnp)
        {
            var asuntoTramite = string.Format("Solicitud del trámite: {0}", detalleTramiteDto.TipoTramite);
            var nombreExpediente = string.Format(detalleTramiteDto.DescripcionInstancia);

            return new CrearRadicadoRequestDto()
            {
                IdEmpresa = detalleTramiteDto.EntidadId,
                UsuarioDnpDestino = usuarioDnp,
                RadicadoPadreId = "0",
                NumeroTramite = numeroTramite,
                ProyectosTramite = proyectosTramite,
                CodigoTipoTramite = detalleTramiteDto.CodigoTipoTramite,
                EsVigenciaFutura = (detalleTramiteDto.CodigoTipoTramite == "VF" || detalleTramiteDto.CodigoTipoTramite == "VFO" || detalleTramiteDto.CodigoTipoTramite == "VFE"),
                CodigoDocumental = detalleTramiteDto.CodigoDocumental,
                DetalleEntidad = new CrearRadicadoRequestEntidadDto
                {
                    NombreEntidad = detalleTramiteDto.NombreEntidad,
                    NombreTramite = string.Format("{0} - {1}", detalleTramiteDto.NombreSector, detalleTramiteDto.TipoTramite)
                },
                DatosRadicado = new CrearRadicadoDetalleDto
                {
                    Asunto = asuntoTramite,
                    TipoRadicado = 2,
                    FechaOficio = DateTime.Today
                },
                DatosExpediente = new CrearRadicadoDatosExpedienteDto
                {
                    Nombre = nombreExpediente,
                    Sector = detalleTramiteDto.SectorId,
                    FechaInicio = DateTime.Today,
                    NivelPrivacidad = 0,
                    Ano = DateTime.Today.Year
                }
            };
        }

        public static CrearRadicadoRequestDto CrearRadicadoEntrada(string numeroTramite, DetalleTramiteDto detalleTramiteDto, List<TramiteProyectoDto> proyectosTramite, string usuarioDnp, int? CodigoDependencia)
        {
            var asuntoTramite = string.Format("Solicitud del trámite: {0}", detalleTramiteDto.TipoTramite);
            var nombreExpediente = string.Format(detalleTramiteDto.DescripcionInstancia);

            return new CrearRadicadoRequestDto() {
                IdEmpresa = detalleTramiteDto.EntidadId,
                UsuarioDnpDestino = usuarioDnp,
                RadicadoPadreId = "0",
                NumeroTramite = numeroTramite,
                ProyectosTramite = proyectosTramite,
                CodigoTipoTramite = detalleTramiteDto.CodigoTipoTramite,
                EsVigenciaFutura = (detalleTramiteDto.CodigoTipoTramite == "VF" || detalleTramiteDto.CodigoTipoTramite == "VFO" || detalleTramiteDto.CodigoTipoTramite == "VFE"),
                CodigoDocumental = detalleTramiteDto.CodigoDocumental,
                CodigoDependencia = CodigoDependencia,
                DetalleEntidad = new CrearRadicadoRequestEntidadDto {
                    NombreEntidad = detalleTramiteDto.NombreEntidad,
                    NombreTramite = string.Format("{0} - {1}", detalleTramiteDto.NombreSector, detalleTramiteDto.TipoTramite)
                },                
                DatosRadicado = new CrearRadicadoDetalleDto {
                    Asunto = asuntoTramite,
                    TipoRadicado = 2,
                    FechaOficio = DateTime.Today
                },
                DatosExpediente = new CrearRadicadoDatosExpedienteDto {
                    Nombre = nombreExpediente,
                    Sector = detalleTramiteDto.SectorId,
                    FechaInicio = DateTime.Today,
                    NivelPrivacidad = 0,
                    Ano = DateTime.Today.Year
                }
            };
        }

        public static CrearRadicadoRequestDto CrearRadicadoSalida(
            string numeroTramite, 
            DetalleTramiteDto detalleTramiteDto, 
            string radicadoEntrada, 
            string analistaDestino,
            string expedienteId
        ) {
            var asuntoTramite = string.Format("Trámite para la solicitud No. {0}", numeroTramite);
            var nombreExpediente = string.Format(detalleTramiteDto.DescripcionInstancia);

            return new CrearRadicadoRequestDto()
            {
                IdEmpresa = detalleTramiteDto.EntidadId,
                RadicadoPadreId = radicadoEntrada,
                ExpedienteId = expedienteId,
                UsuarioDnpDestino = analistaDestino,
                CodigoTipoTramite = detalleTramiteDto.CodigoTipoTramite,
                CodigoDocumental = detalleTramiteDto.CodigoDocumental,
                EsVigenciaFutura = (detalleTramiteDto.CodigoTipoTramite == "VF" || detalleTramiteDto.CodigoTipoTramite == "VFO" || detalleTramiteDto.CodigoTipoTramite == "VFE"),
                DatosRadicado = new CrearRadicadoDetalleDto
                {
                    Asunto = asuntoTramite,
                    TipoRadicado = 6,
                    FechaOficio = DateTime.Today
                },
                DatosExpediente = new CrearRadicadoDatosExpedienteDto
                {
                    Nombre = nombreExpediente,
                    Sector = detalleTramiteDto.SectorId,
                    FechaInicio = DateTime.Today,
                    NivelPrivacidad = 0,
                    Ano = DateTime.Today.Year
                }
            };
        }
    }

    public class CrearRadicadoRequestEntidadDto
    {
        public string NombreEntidad { get; set; }

        public string NombreTramite { get; set; }
    }

    public class CrearRadicadoDetalleDto
    {
        public string Asunto { get; set; }

        public int TipoRadicado { get; set; }

        public DateTime FechaOficio { get; set; }
    }

    public class CrearRadicadoDatosExpedienteDto
    {
        public string Nombre { get; set; }

        public int Sector { get; set; }

        public DateTime FechaInicio { get; set; }

        public int NivelPrivacidad { get; set; }

        public int Ano { get; set; }
    }
}
