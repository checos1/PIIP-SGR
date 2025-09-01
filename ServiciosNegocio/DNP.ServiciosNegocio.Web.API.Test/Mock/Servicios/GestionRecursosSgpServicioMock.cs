using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.GestionRecursos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public  class GestionRecursosSgpServicioMock : IGestionRecursosSgpServicio
    {
        public string Usuario { get; set; }
        public string Ip { get; set; }
        public string ObtenerLocalizacionProyectosSgp(string bpin)
        {
            return string.Empty;
        }
        public string ObtenerFocalizacionPoliticasTransversalesFuentesSgp(string bpin)
        {
            return string.Empty;
        }
        public string ObtenerPoliticasTransversalesProyectoSgp(string bpin)
        {
            return string.Empty;
        }
        public TramitesResultado EliminarPoliticasProyectoSgp(int tramiteidProyectoId, int politicaId)
        {
            var resultado = new TramitesResultado();

            if (tramiteidProyectoId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public TramitesResultado AgregarPoliticasTransversalesSgp(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {
            var resultado = new TramitesResultado();

            if (parametrosGuardar.Contenido.ProyectoId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }
        public string ConsultarPoliticasCategoriasIndicadoresSgp(Guid instanciaId)
        {
            return string.Empty;
        }
        public TramitesResultado ModificarPoliticasCategoriasIndicadoresSgp(CategoriasIndicadoresDto parametrosGuardar, string usuario)
        {
            var resultado = new TramitesResultado();

            if (parametrosGuardar.ProyectoId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }
        public string ObtenerPoliticasTransversalesCategoriasSgp(string instanciaId)
        {
            return string.Empty;
        }
        public TramitesResultado EliminarCategoriasPoliticasProyectoSgp(int proyectoId, int politicaId, int categoriaId)
        {
            var resultado = new TramitesResultado();

            if (proyectoId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }
        public TramitesResultado GuardarFocalizacionCategoriasAjustesSgp(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario)
        {
            var resultado = new TramitesResultado();

            if (focalizacionCategoriasAjuste.Count > 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public string GetCategoriasSubcategoriasSgp(int padreId, int? entidadId, int esCategoria, int esGruposEtnicos)
        {
            return string.Empty;
        }

        public TramitesResultado GuardarCategoriasPoliticaTransversalesAjustesSgp(ParametrosGuardarDto<FocalizacionCategoriasAjusteDto> parametrosGuardar, string usuario)
        {
            var resultado = new TramitesResultado();

            if (parametrosGuardar.Contenido.ProyectoId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public string ObtenerCrucePoliticasAjustesSgp(Guid instanciaId)
        {
            return string.Empty;
        }
        public string ObtenerPoliticasTransversalesResumenSgp(Guid instanciaId)
        {
            return string.Empty;
        }

        public TramitesResultado GuardarCrucePoliticasAjustesSgp(ParametrosGuardarDto<List<CrucePoliticasAjustesDto>> parametrosGuardar, string usuario)
        {
            var resultado = new TramitesResultado();

            if (parametrosGuardar.Contenido.Count > 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public string ObtenerDesagregarRegionalizacionSgp(string bpin)
        {
            return string.Empty;
        }
        public string ObtenerFuenteFinanciacionVigenciaSgp(string bpin)
        {
            return string.Empty;
        }
        public string ObtenerFuentesProgramarSolicitadoSgp(string bpin)
        {
            return string.Empty;
        }

        public TramitesResultado EliminarFuentesFinanciacionProyectoSgp(int fuentesFinanciacionId)
        {
            var resultado = new TramitesResultado();

            if (fuentesFinanciacionId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        
        }

        public TramitesResultado GuardarFuentesProgramarSolicitadoSgp(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuario)
        {
            var resultado = new TramitesResultado();

            if (objProgramacionValorFuenteDto.fuenteId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public string ObtenerDatosAdicionalesFuenteFinanciacionSgp(int fuenteId)
        {
            return string.Empty;
        }

        public TramitesResultado EliminarDatosAdicionalesSgp(int coFinanciacionId)
        {
            var resultado = new TramitesResultado();

            if (coFinanciacionId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;

        }

        public TramitesResultado GuardarDatosAdicionalesSgp(ParametrosGuardarDto<DatosAdicionalesDto> parametrosGuardar, string usuario)
        {
            var resultado = new TramitesResultado();

            if (parametrosGuardar.Contenido.fuenteId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public TramitesResultado GuardarFuenteFinanciacionSgp(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar, string usuario)
        {
            var resultado = new TramitesResultado();

            if (parametrosGuardar.Contenido.ProyectoId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public string ObtenerDatosIndicadoresPoliticaSgp(string Bpin)
        {
            return string.Empty;
        }

        public string ObtenerDatosCategoriaProductosPoliticaSgp(string Bpin, int fuenteId, int politicaId)
        {
            return string.Empty;
        }

        public string GuardarDatosSolicitudRecursosSgp(ParametrosGuardarDto<CategoriaProductoPoliticaDto> categoriaProductoPoliticaDto, string usuario)
        {
            return string.Empty;
        }
    }
}
