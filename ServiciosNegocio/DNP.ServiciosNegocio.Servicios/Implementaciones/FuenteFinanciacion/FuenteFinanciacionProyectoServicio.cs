using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.FuenteFinanciacion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Genericos;

    public class FuenteFinanciacionProyectoServicio : ServicioBase<ProyectoFuenteFinanciacionDto>, IFuenteFinanciacionServicios
    {
        private readonly IFuenteFinanciacionPersistencia _fuenteFinanciacionPersistencia;

        public FuenteFinanciacionProyectoServicio(IFuenteFinanciacionPersistencia fuenteFinanciacionPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _fuenteFinanciacionPersistencia = fuenteFinanciacionPersistencia;
        }

        public ProyectoFuenteFinanciacionDto ObtenerFuenteFinanciacionProyecto(ParametrosConsultaDto parametrosConsulta)
        {
            return Obtener(parametrosConsulta);
        }

        public FuenteFinanciacionProyectoDto ObtenerFuenteFinanciacionProyectoDto(ParametrosConsultaDto parametrosConsulta)
        {
            return _fuenteFinanciacionPersistencia.ObtenerFuenteFinanciacionProyecto(parametrosConsulta.Bpin);
        }

        public ProyectoFuenteFinanciacionDto ObtenerFuenteFinanciacionProyectoPreview()
        {
            return _fuenteFinanciacionPersistencia.ObtenerFuenteFinanciacionProyectoPreview();
        }

        public string ObtenerPoliticasTransversalesAjustes(string Bpin)
        {
            return _fuenteFinanciacionPersistencia.ObtenerPoliticasTransversalesAjustes(Bpin);
        }

        public string GuardarPoliticasTransversalesAjustes(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {
            return _fuenteFinanciacionPersistencia.GuardarPoliticasTransversalesAjustes(parametrosGuardar, usuario);
        }

        //public FuenteFinanciacionResultado EliminarFuentesFinanciacionProyecto(int fuentesFinanciacionId)
        //{
        //    return _fuenteFinanciacionPersistencia.EliminarFuentesFinanciacionProyecto(fuentesFinanciacionId);
        //}

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ProyectoFuenteFinanciacionDto> parametrosGuardar, string usuario)
        {
            _fuenteFinanciacionPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override ProyectoFuenteFinanciacionDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            List<FuenteFinanciacionProyectoDto> infoPersistencia = _fuenteFinanciacionPersistencia.ObtenerFuentesFinanciacionProyecto(parametrosConsultaDto.Bpin);
            return MapearInformacion(infoPersistencia);
        }

        private ProyectoFuenteFinanciacionDto MapearInformacion(IEnumerable<FuenteFinanciacionProyectoDto> infoPersistencia)
        {
            var fuenteFinanciacionProyectoDtos = infoPersistencia.ToList();

            if (!fuenteFinanciacionProyectoDtos.ToList().ToList().Any()) return null;

            ProyectoFuenteFinanciacionDto proyectoFuenteFinanciacionDto = new ProyectoFuenteFinanciacionDto()
            {
                CR = fuenteFinanciacionProyectoDtos[0].CrProyecto,
                ValorTotalProyecto = fuenteFinanciacionProyectoDtos[0].ValorTotalProyecto,
                BPIN = fuenteFinanciacionProyectoDtos[0].CodigoBpin,
                FuentesFinanciacion = new List<FuenteFinanciacionDto>()
            };

            foreach (var fuenteEnBd in fuenteFinanciacionProyectoDtos)
            {
                proyectoFuenteFinanciacionDto.FuentesFinanciacion.Add(new FuenteFinanciacionDto()
                {
                    FuenteId = fuenteEnBd.FuenteId,
                    ProgramacionId = fuenteEnBd.ProgramacionId,
                    EjecucionId = fuenteEnBd.EjecucionId,
                    Compromiso = fuenteEnBd.Compromiso,
                    OtraEntidad = fuenteEnBd.OtraEntidad,
                    EntidadId = fuenteEnBd.EntidadId,
                    Entidad = fuenteEnBd.Entidad,
                    Vigencia = fuenteEnBd.Vigencia,
                    ApropiacionInicial = fuenteEnBd.ApropiacionInicial,
                    TipoRecursoId = fuenteEnBd.TipoRecursoId,
                    TipoRecurso = fuenteEnBd.TipoRecurso,
                    TipoEntidadId = fuenteEnBd.TipoEntidadId,
                    TipoEntidad =fuenteEnBd.TipoEntidad,
                    NombreCompleto = fuenteEnBd.NombreCompleto,
                    Mes = fuenteEnBd.Mes,
                    Obligacion = fuenteEnBd.Obligacion,
                    ApropiacionVigente = fuenteEnBd.ApropiacionVigente,
                    GrupoRecurso = fuenteEnBd.GrupoRecurso,
                    EtapaId = fuenteEnBd.EtapaId,
                    Pago = fuenteEnBd.Pago,
                    Solicitado = fuenteEnBd.Solicitado
                });
            }
            return proyectoFuenteFinanciacionDto;
        }

        public string ObtenerPoliticasTransversalesCategorias(string Bpin)
        {
            return _fuenteFinanciacionPersistencia.ObtenerPoliticasTransversalesCategorias(Bpin);
        }

        public RespuestaGeneralDto EliminarPoliticasProyecto(int proyectoId, int politicaId)
        {
            return _fuenteFinanciacionPersistencia.EliminarPoliticasProyecto(proyectoId, politicaId);
        }

        public string GuardarCategoriasPoliticaTransversalesAjustes(ParametrosGuardarDto<FocalizacionCategoriasAjusteDto> parametrosGuardar, string usuario)
        {
            return _fuenteFinanciacionPersistencia.GuardarCategoriasPoliticaTransversalesAjustes(parametrosGuardar, usuario);
        }

        public string ObtenerPoliticasTransversalesResumen(string Bpin)
        {
            return _fuenteFinanciacionPersistencia.ObtenerPoliticasTransversalesResumen(Bpin);
        }

        public string ObtenerPoliticasCategoriasIndicadores(string Bpin)
        {
            return _fuenteFinanciacionPersistencia.ObtenerPoliticasCategoriasIndicadores(Bpin);
        }

        public ResultadoProcedimientoDto ModificarCategoriasIndicadores(CategoriasIndicadoresDto parametrosGuardar, string usuario)
        {
            return _fuenteFinanciacionPersistencia.ModificarCategoriasIndicadores(parametrosGuardar, usuario);
        }

        public RespuestaGeneralDto EliminarCategoriaPoliticasProyecto(int proyectoId, int politicaId, int categoriaId)
        {
            return _fuenteFinanciacionPersistencia.EliminarCategoriaPoliticasProyecto(proyectoId, politicaId, categoriaId);
        }

        public string ObtenerCrucePoliticasAjustes(string Bpin)
        {
            return _fuenteFinanciacionPersistencia.ObtenerCrucePoliticasAjustes(Bpin);
        }

        public RespuestaGeneralDto GuardarCrucePoliticasAjustes(ParametrosGuardarDto<List<CrucePoliticasAjustesDto>> parametrosGuardar, string usuario)
        {
            return _fuenteFinanciacionPersistencia.GuardarCrucePoliticasAjustes(parametrosGuardar, usuario);
        }

        public string ObtenerPoliticasSolicitudConcepto(string Bpin)
        {
            return _fuenteFinanciacionPersistencia.ObtenerPoliticasSolicitudConcepto(Bpin);
        }

        public string FocalizacionSolicitarConceptoDT(ParametrosGuardarDto<List<FocalizacionSolicitarConceptoDto>> parametrosGuardar, string usuario)
        {
            return _fuenteFinanciacionPersistencia.FocalizacionSolicitarConceptoDT(parametrosGuardar, usuario);
        }
        public string ObtenerDireccionesTecnicasPoliticasFocalizacion()
        {
            return _fuenteFinanciacionPersistencia.ObtenerDireccionesTecnicasPoliticasFocalizacion();
        }

        public string ObtenerResumenSolicitudConcepto(string Bpin)
        {
            return _fuenteFinanciacionPersistencia.ObtenerResumenSolicitudConcepto(Bpin);
        }
        public string ObtenerPreguntasEnvioPoliticaSubDireccion(Guid instanciaid, int proyectoid, string usuarioDNP, Guid nivelid)
        {
            return _fuenteFinanciacionPersistencia.ObtenerPreguntasEnvioPoliticaSubDireccion(instanciaid, proyectoid, usuarioDNP, nivelid);
        }

        public string GuardarPreguntasEnvioPoliticaSubDireccionAjustes(ParametrosGuardarDto<PreguntasEnvioPoliticaSubDireccionAjustes> parametrosGuardar, string usuario)
        {
            return _fuenteFinanciacionPersistencia.GuardarPreguntasEnvioPoliticaSubDireccionAjustes(parametrosGuardar, usuario);
        }

        public string GuardarRespuestaEnvioPoliticaSubDireccionAjustes(ParametrosGuardarDto<RespuestaEnvioPoliticaSubDireccionAjustes> parametrosGuardar, string usuario)
        {
            return _fuenteFinanciacionPersistencia.GuardarRespuestaEnvioPoliticaSubDireccionAjustes(parametrosGuardar, usuario);
        }
    }
}
