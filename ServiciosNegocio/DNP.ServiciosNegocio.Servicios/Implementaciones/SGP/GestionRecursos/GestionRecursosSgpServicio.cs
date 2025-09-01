using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.GestionRecursos;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.GestionRecursos;
using DNP.ServiciosNegocio.Dominio.Dto.Productos;
using System;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.SGR;
using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGP.GestionRecursos
{
    public class GestionRecursosSgpServicio : ServicioBase<ProyectoDto>, IGestionRecursosSgpServicio
    {
        private readonly IGestionRecursosSgpPersistencia _gestionRecursosSgpPersistencia;
        public string Usuario { get; set; }
        public string Ip { get; set; }

        public GestionRecursosSgpServicio(IGestionRecursosSgpPersistencia gestionRecursosSgpPersistencia, IAuditoriaServicios auditoriaServicios) : base(null, auditoriaServicios)
        {
            _gestionRecursosSgpPersistencia = gestionRecursosSgpPersistencia;
        }
       
        public string ObtenerLocalizacionProyectosSgp(string bpin)
        {
            var infoPersistencia = _gestionRecursosSgpPersistencia.ObtenerlocalizacionSgp(bpin);
            return infoPersistencia;
        }
        public string ObtenerFocalizacionPoliticasTransversalesFuentesSgp(string bpin)
        {
            var infoPersistencia = _gestionRecursosSgpPersistencia.ObtenerFocalizacionPoliticasTransversalesFuentesSgp(bpin);
            return infoPersistencia;
        }
        public string ObtenerPoliticasTransversalesProyectoSgp(string Bpin)
        {
            return _gestionRecursosSgpPersistencia.ObtenerPoliticasTransversalesProyectoSgp(Bpin);
        }

        public TramitesResultado EliminarPoliticasProyectoSgp(int tramiteidProyectoId, int politicaId)
        {
            var result = _gestionRecursosSgpPersistencia.EliminarPoliticasProyectoSgp(tramiteidProyectoId, politicaId);

            var parametrosGuardar = new ParametrosGuardarDto<object>
            {
                Contenido = new { tramiteidProyectoId, politicaId }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Eliminacion, "EliminarPoliticasProyectoSgp");

            return result;
        }
        public TramitesResultado AgregarPoliticasTransversalesSgp(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {
            var result = _gestionRecursosSgpPersistencia.AgregarPoliticasTransversalesSgp(parametrosGuardar, usuario);

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Modificacion, "AgregarPoliticasTransversalesSgp");

            return result;
        }
        public string ConsultarPoliticasCategoriasIndicadoresSgp(Guid instanciaId)
        {
            return _gestionRecursosSgpPersistencia.ConsultarPoliticasCategoriasIndicadoresSgp(instanciaId);
        }

        public TramitesResultado ModificarPoliticasCategoriasIndicadoresSgp(CategoriasIndicadoresDto parametrosGuardar, string usuario)
        {
            var result = _gestionRecursosSgpPersistencia.ModificarPoliticasCategoriasIndicadoresSgp(parametrosGuardar, usuario);

            var parametrosGuardarCISgp = new ParametrosGuardarDto<CategoriasIndicadoresDto>
            {
                Contenido = parametrosGuardar
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardarCISgp, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Modificacion, "ModificarPoliticasCategoriasIndicadoresSgp");

            return result;
        }

        public string ObtenerPoliticasTransversalesCategoriasSgp(string instanciaId)
        {
            return _gestionRecursosSgpPersistencia.ObtenerPoliticasTransversalesCategoriasSgp(instanciaId);
        }

        public TramitesResultado EliminarCategoriasPoliticasProyectoSgp(int proyectoId, int politicaId, int categoriaId)
        {
            var result = _gestionRecursosSgpPersistencia.EliminarCategoriaPoliticasProyectoSgp(proyectoId, politicaId, categoriaId);

            var parametrosGuardar = new ParametrosGuardarDto<object>
            {
                Contenido = new { proyectoId, politicaId, categoriaId }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Eliminacion, "EliminarCategoriasPoliticasProyectoSgp");

            return result;
        }

        public TramitesResultado GuardarFocalizacionCategoriasAjustesSgp(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario)
        {
            var result = _gestionRecursosSgpPersistencia.GuardarFocalizacionCategoriasAjustesSgp(focalizacionCategoriasAjuste, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<List<FocalizacionCategoriasAjusteDto>>
            {
                Contenido = focalizacionCategoriasAjuste
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "GuardarFocalizacionCategoriasAjustesSgp");


            return result;
        }
        public string GetCategoriasSubcategoriasSgp(int padreId, int? entidadId, int esCategoria, int esGruposEtnicos)
        {
            return _gestionRecursosSgpPersistencia.GetCategoriasSubcategoriasSgp(padreId, entidadId, esCategoria, esGruposEtnicos);
        }

        public TramitesResultado GuardarCategoriasPoliticaTransversalesAjustesSgp(ParametrosGuardarDto<FocalizacionCategoriasAjusteDto> parametrosGuardar, string usuario)
        {
            var result = _gestionRecursosSgpPersistencia.GuardarCategoriasPoliticaTransversalesAjustesSgp(parametrosGuardar, usuario);

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "GuardarCategoriasPoliticaTransversalesAjustesSgp");

            return result;
        }
        public string ObtenerCrucePoliticasAjustesSgp(Guid instanciaId)
        {
            return _gestionRecursosSgpPersistencia.ObtenerCrucePoliticasAjustesSgp(instanciaId);
        }
        public string ObtenerPoliticasTransversalesResumenSgp(Guid instanciaId)
        {
            return _gestionRecursosSgpPersistencia.ObtenerPoliticasTransversalesResumenSgp(instanciaId);
        }
        public TramitesResultado GuardarCrucePoliticasAjustesSgp(ParametrosGuardarDto<List<CrucePoliticasAjustesDto>> parametrosGuardar, string usuario)
        {
            var result = _gestionRecursosSgpPersistencia.GuardarCrucePoliticasAjustesSgp(parametrosGuardar, usuario);

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "GuardarCrucePoliticasAjustesSgp");

            return result;
        }

        public string ObtenerDesagregarRegionalizacionSgp(string bpin)
        {
            return _gestionRecursosSgpPersistencia.ObtenerDesagregarRegionalizacionSgp(bpin);
        }

        public string ObtenerFuenteFinanciacionVigenciaSgp(string bpin)
        {
            return _gestionRecursosSgpPersistencia.ObtenerFuenteFinanciacionVigenciaSgp(bpin);
        }

        public string ObtenerFuentesProgramarSolicitadoSgp(string bpin)
        {
            return _gestionRecursosSgpPersistencia.ObtenerFuentesProgramarSolicitadoSgp(bpin);
        }

        public TramitesResultado EliminarFuentesFinanciacionProyectoSgp(int fuentesFinanciacionId)
        {
            var result = _gestionRecursosSgpPersistencia.EliminarFuentesFinanciacionProyectoSgp(fuentesFinanciacionId);

            var parametrosGuardar = new ParametrosGuardarDto<int>
            {
                Contenido = fuentesFinanciacionId
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Eliminacion, "EliminarFuentesFinanciacionProyectoSgp");

            return result;
        }

        public TramitesResultado GuardarFuentesProgramarSolicitadoSgp(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuario)
        {
            var result = _gestionRecursosSgpPersistencia.GuardarFuentesProgramarSolicitadoSgp(objProgramacionValorFuenteDto, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<ProgramacionValorFuenteDto>
            {
                Contenido = objProgramacionValorFuenteDto
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "GuardarFuentesProgramarSolicitadoSgp");

            return result;
        }

        public string ObtenerDatosAdicionalesFuenteFinanciacionSgp(int fuenteId)
        {
            return _gestionRecursosSgpPersistencia.ObtenerDatosAdicionalesFuenteFinanciacionSgp(fuenteId);
        }
       public TramitesResultado GuardarDatosAdicionalesSgp(ParametrosGuardarDto<DatosAdicionalesDto> parametrosGuardar, string usuario)
        {
            var result = _gestionRecursosSgpPersistencia.GuardarDatosAdicionalesSgp(parametrosGuardar, usuario);

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "GuardarDatosAdicionalesSgp");

            return result;
        }


        public TramitesResultado EliminarDatosAdicionalesSgp(int coFinanciacionId)
        {
            var result = _gestionRecursosSgpPersistencia.EliminarDatosAdicionalesSgp(coFinanciacionId);

            var parametrosGuardar = new ParametrosGuardarDto<int>
            {
                Contenido = coFinanciacionId
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Eliminacion, "EliminarDatosAdicionalesSgp");

            return result;
        }
        public TramitesResultado GuardarFuenteFinanciacionSgp(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar, string usuario)
        {
            var result = _gestionRecursosSgpPersistencia.GuardarFuenteFinanciacionSgp(parametrosGuardar, usuario);

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "GuardarFuenteFinanciacionSgp");

            return result;
        }
        public string ObtenerDatosIndicadoresPoliticaSgp(string Bpin)
        {
            return _gestionRecursosSgpPersistencia.ObtenerDatosIndicadoresPoliticaSgp(Bpin);
        }

        public string ObtenerDatosCategoriaProductosPoliticaSgp(string Bpin, int fuenteId, int politicaId)
        {
            return _gestionRecursosSgpPersistencia.ObtenerDatosCategoriaProductosPoliticaSgp(Bpin, fuenteId, politicaId);
        }
        public string GuardarDatosSolicitudRecursosSgp(ParametrosGuardarDto<CategoriaProductoPoliticaDto> categoriaProductoPoliticaDto, string usuario)
        {
            var result = _gestionRecursosSgpPersistencia.GuardarDatosSolicitudRecursosSgp(categoriaProductoPoliticaDto, usuario);

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(categoriaProductoPoliticaDto, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "GuardarDatosSolicitudRecursosSgp");

            return result;
        }

        protected override ProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new NotImplementedException();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ProyectoDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }
    }
}
