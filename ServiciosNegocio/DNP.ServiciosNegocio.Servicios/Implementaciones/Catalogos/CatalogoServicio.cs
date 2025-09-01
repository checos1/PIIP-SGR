namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Catalogos
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using Comunes;
    using Comunes.Enum;
    using Comunes.Excepciones;
    using Dominio.Dto.Catalogos;
    using Interfaces.Catalogos;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Catalogos;

    public class CatalogoServicio : ICatalogoServicio
    {
        private readonly ICatalogoPersistencia _catalogoPersistencia;
        private readonly ICacheServicio _cacheServicio;

        public CatalogoServicio(ICatalogoPersistencia catalogoPersistencia, ICacheServicio cacheServicio)
        {
            _catalogoPersistencia = catalogoPersistencia;
            _cacheServicio = cacheServicio;
        }

        public async Task<List<CatalogoDto>> ObtenerCatalogo(string nombreCatalogo, string tokenAutorizacion)
        {
            if (!EsNombreCatalogoValido(nombreCatalogo)) return null;

            var listaCatalogo = await _cacheServicio.ObtenerCatalogo(nombreCatalogo, tokenAutorizacion);
            if(listaCatalogo != null)
                return listaCatalogo;

            listaCatalogo = ObtenerListaCatalogo(nombreCatalogo);

            var ttl = AsignarTiempoVidaEnCache(nombreCatalogo);

            var guardado = await _cacheServicio.GuardarListaCatalogo(nombreCatalogo, listaCatalogo, ttl, tokenAutorizacion);
            if(guardado)
               return listaCatalogo;

            throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorGuardarCache);
        }

        public async Task<CatalogoDto> ObtenerCatalogosPorReferencia(string nombreCatalogo, int idCatalogo, string nombreCatalogoReferencia, string tokenAutorizacion)
        {
            if (!SonNombresReferenciasCatalogoValidos(nombreCatalogo, nombreCatalogoReferencia)) return null;

            var catalogo = await _cacheServicio.ConsultarPorReferencia(nombreCatalogo, idCatalogo, nombreCatalogoReferencia, tokenAutorizacion);

            if (catalogo != null)
                return catalogo;

            catalogo = ObtenerCatalogoReferencia(nombreCatalogo, idCatalogo, nombreCatalogoReferencia);

            var ttl = AsignarTiempoVidaEnCache(nombreCatalogoReferencia);

            var guardado = await _cacheServicio.GuardarReferencia(nombreCatalogo, idCatalogo, nombreCatalogoReferencia, catalogo, ttl, tokenAutorizacion);
            if (guardado)
                return catalogo;

            throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorGuardarCache);
        }

        public async Task<CatalogoDto> ObtenerCatalogoPorId(string nombreCatalogo, int idCatalogo, string tokenAutorizacion)
        {
            if (!EsNombreCatalogoValido(nombreCatalogo)) return null;
            var catalogo = await _cacheServicio.ObtenerCatalogoPorId(nombreCatalogo, idCatalogo, tokenAutorizacion);

            if (catalogo == null)
            {
                var listaCatalogo = await ObtenerCatalogo(nombreCatalogo, tokenAutorizacion);
                if (listaCatalogo != null && listaCatalogo.Count > 0)
                {
                    catalogo = listaCatalogo?.FirstOrDefault(x => x.Id == idCatalogo);
                }
            }
            return catalogo;
        }

        public string ObtenerTablasBasicas(string jsonCondicion, string Tabla)
        {
            return _catalogoPersistencia.ObtenerTablasBasicas(jsonCondicion, Tabla);
        }

        #region Métodos Privados 

        public List<CatalogoDto> ObtenerListaCatalogo(string nombreCatalogo)
        {
            if (nombreCatalogo == CatalogoEnum.Entidades.ToString()) return _catalogoPersistencia.ObtenerEntidades();

            if (nombreCatalogo == CatalogoEnum.DireccionTecnica.ToString()) return _catalogoPersistencia.ObtenerDireccionesTecnicas();

            if (nombreCatalogo == CatalogoEnum.SubDireccionTecnica.ToString()) return _catalogoPersistencia.ObtenerSubDireccionesTecnicas();

            if (nombreCatalogo == CatalogoEnum.AnalistasSubDireccionTecnica.ToString()) return _catalogoPersistencia.ObtenerAnalistasSubDireccionesTecnicas();

            if (nombreCatalogo == CatalogoEnum.TiposEntidades.ToString()) return _catalogoPersistencia.ObtenerTiposEntidades();

            if (nombreCatalogo == CatalogoEnum.TodosTiposEntidades.ToString()) return _catalogoPersistencia.ObtenerTodosTiposEntidades();

            if (nombreCatalogo == CatalogoEnum.Sectores.ToString()) return _catalogoPersistencia.ConsultarSectores();

            if (nombreCatalogo == CatalogoEnum.SectoresEntity.ToString()) return _catalogoPersistencia.ConsultarSectoresEntity();

            if (nombreCatalogo == CatalogoEnum.Regiones.ToString()) return _catalogoPersistencia.ConsultarRegiones();

            if (nombreCatalogo == CatalogoEnum.Departamentos.ToString()) return _catalogoPersistencia.ConsultarDepartamentos();

            if (nombreCatalogo == CatalogoEnum.Municipios.ToString()) return _catalogoPersistencia.ConsultarMunicipios();

            if (nombreCatalogo == CatalogoEnum.Resguardos.ToString()) return _catalogoPersistencia.ConsultarResguardos();

            if (nombreCatalogo == CatalogoEnum.Programas.ToString()) return _catalogoPersistencia.ConsultarProgramas();

            if (nombreCatalogo == CatalogoEnum.Productos.ToString()) return _catalogoPersistencia.ConsultarProductos();

            if (nombreCatalogo == CatalogoEnum.Alternativas.ToString()) return _catalogoPersistencia.ConsultarAlternativas();

            if (nombreCatalogo == CatalogoEnum.TiposRecursos.ToString()) return _catalogoPersistencia.ConsultarTiposRecursos();

            if (nombreCatalogo == CatalogoEnum.ClasificacionesRecursos.ToString()) return _catalogoPersistencia.ConsultarClasificacionesRecursos();

            if (nombreCatalogo == CatalogoEnum.GruposRecursos.ToString()) return _catalogoPersistencia.ConsultarGruposRecursos();

            if (nombreCatalogo == CatalogoEnum.TiposAgrupaciones.ToString()) return _catalogoPersistencia.ConsultarTiposAgrupaciones();

            if (nombreCatalogo == CatalogoEnum.Politicas.ToString()) return _catalogoPersistencia.ConsultarPoliticas((int)TipoPoliticaEnum.Politica);

            if (nombreCatalogo == CatalogoEnum.PoliticasNivel1.ToString()) return _catalogoPersistencia.ConsultarPoliticas((int)TipoPoliticaEnum.Dimension);

            if (nombreCatalogo == CatalogoEnum.PoliticasNivel2.ToString()) return _catalogoPersistencia.ConsultarPoliticas((int)TipoPoliticaEnum.Nivel2);

            if (nombreCatalogo == CatalogoEnum.PoliticasNivel3.ToString()) return _catalogoPersistencia.ConsultarPoliticas((int)TipoPoliticaEnum.Nivel3); 

            if (nombreCatalogo == CatalogoEnum.IndicadoresPoliticas.ToString()) return _catalogoPersistencia.ConsultarIndicadoresPoliticas();

            if (nombreCatalogo == CatalogoEnum.TiposCofinanciaciones.ToString()) return _catalogoPersistencia.ConsultarTiposCofinanciaciones();

            if (nombreCatalogo == CatalogoEnum.Entregables.ToString()) return _catalogoPersistencia.ConsultarEntregables();

            if (nombreCatalogo == CatalogoEnum.Agrupaciones.ToString()) return _catalogoPersistencia.ConsultarAgrupaciones();

            if (nombreCatalogo == CatalogoEnum.Rubros.ToString()) return _catalogoPersistencia.ConsultarRubros();
            
            if (nombreCatalogo == CatalogoEnum.Fondos.ToString()) return _catalogoPersistencia.ConsultarFondos();

            if (nombreCatalogo == CatalogoEnum.TipoCofinanciador.ToString()) return _catalogoPersistencia.ConsultarTipoCofinanciador();

            return nombreCatalogo == CatalogoEnum.Etapas.ToString() ? _catalogoPersistencia.ConsultarEtapas() : null;
        }

        public CatalogoDto ObtenerCatalogoReferencia(string nombreCatalogo, int idCatalogo,
                                                           string nombreCatalogoReferencia)
        {
            if (nombreCatalogo == CatalogoEnum.Regiones.ToString()&& nombreCatalogoReferencia == CatalogoEnum.Departamentos.ToString()) return _catalogoPersistencia.ConsultarDepartamentosPorIdRegion(idCatalogo);

            if (nombreCatalogo == CatalogoEnum.Departamentos.ToString()&& nombreCatalogoReferencia == CatalogoEnum.Municipios.ToString()) return _catalogoPersistencia.ConsultarMunicipioPorIdDepartamento(idCatalogo);

            if (nombreCatalogo == CatalogoEnum.Municipios.ToString()&& nombreCatalogoReferencia == CatalogoEnum.Resguardos.ToString()) return _catalogoPersistencia.ConsultarResguardosPorIdMunicipio(idCatalogo);

            if (nombreCatalogo == CatalogoEnum.TiposEntidades.ToString() && nombreCatalogoReferencia == CatalogoEnum.TiposRecursos.ToString()) return _catalogoPersistencia.ConsultarTiposRecursosPorTipoEntidadId(idCatalogo);

            if (nombreCatalogo == CatalogoEnum.Municipios.ToString() && nombreCatalogoReferencia == CatalogoEnum.Agrupaciones.ToString()) return _catalogoPersistencia.ConsultarAgrupacionesPorIdMunicipio(idCatalogo);

            if (nombreCatalogo == CatalogoEnum.Municipios.ToString() && nombreCatalogoReferencia == CatalogoEnum.Agrupaciones.ToString()) return _catalogoPersistencia.ConsultarAgrupacionesPorIdMunicipio(idCatalogo);

            return null;
        }

        public List<DepartamentoCatalogoDto> ConsultarDepartamentosRegion()
        {
            return _catalogoPersistencia.ConsultarDepartamentosRegion();
        }

        public List<CatalogoDto> ConsultarTiposRecursosEntidad(int entityTypeCatalogId)
        {
            return _catalogoPersistencia.ConsultarTiposRecursosEntidad(entityTypeCatalogId);
        }

        public List<CatalogoDto> ConsultarEjecutorPorTipoEntidadId(int idTipoEntidad)
        {
            return _catalogoPersistencia.ConsultarEjecutorPorTipoEntidadId(idTipoEntidad);
        }

        public List<CatalogoDto> ConsultarCategoriaByPadre(int idPadre)
        {
            return _catalogoPersistencia.ConsultarCategoriaByPadre(idPadre);
        }

        public List<AgrupacionCodeDto> ConsultarAgrupacionesCompleta()
        {
            return _catalogoPersistencia.ConsultarAgrupacionesCompleta();
        }
        public List<CatalogoDto> ConsultarTiposRecursosEntidadPorGrupoRecursos(int entityTypeCatalogId, int resourceGroupId, bool incluir)
        {
            return _catalogoPersistencia.ConsultarTiposRecursosEntidadPorGrupoRecursos(entityTypeCatalogId, resourceGroupId, incluir);
        }
        private static long AsignarTiempoVidaEnCache(string nombreCatalogo)
        {
            string tiempo;
            var tiempoExpiracion = new TimeSpan();

            if (nombreCatalogo == CatalogoEnum.Entidades.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Entidades"];
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.TiposEntidades.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_TiposEntidades"];
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.Sectores.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Sectores"];
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.Regiones.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Regiones"];
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.Resguardos.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Resguardos"];
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.Programas.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Programas"];
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.Productos.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Productos"];
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.Alternativas.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Alternativas"];
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.TiposRecursos.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_TiposRecursos"];
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.ClasificacionesRecursos.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_ClasificacionesRecursos"];
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.Etapas.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Etapas"];
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.Departamentos.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Departamentos"];
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.GruposRecursos.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Etapas"];//Se reutiliza esta key
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.TiposAgrupaciones.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Etapas"];//Se reutiliza esta key
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.Agrupaciones.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Etapas"];//Se reutiliza esta key
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.Politicas.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Etapas"];//Se reutiliza esta key
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.TiposCofinanciaciones.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Etapas"];//Se reutiliza esta key
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.Entregables.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Etapas"];//Se reutiliza esta key
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.PoliticasNivel1.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Etapas"];//Se reutiliza esta key
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.PoliticasNivel2.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Etapas"];//Se reutiliza esta key
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.PoliticasNivel3.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Etapas"];//Se reutiliza esta key
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0, 0);
            }

            if (nombreCatalogo == CatalogoEnum.IndicadoresPoliticas.ToString())
            {
                tiempo = ConfigurationManager.AppSettings["TTL_Etapas"];//Se reutiliza esta key
                tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0, 0);
            }

            if (nombreCatalogo != CatalogoEnum.Municipios.ToString()) return tiempoExpiracion.Ticks;

            tiempo = ConfigurationManager.AppSettings["TTL_Municipios"];
            tiempoExpiracion = new TimeSpan(Convert.ToInt32(tiempo), 0, 0, 0);

            return tiempoExpiracion.Ticks;
        }

        private static bool EsNombreCatalogoValido(string nombreCatalogo)
        {
            return nombreCatalogo == CatalogoEnum.Entidades.ToString() ||
                   nombreCatalogo == CatalogoEnum.TiposEntidades.ToString() ||
                   nombreCatalogo == CatalogoEnum.Sectores.ToString() ||
                   nombreCatalogo == CatalogoEnum.Regiones.ToString() ||
                   nombreCatalogo == CatalogoEnum.Departamentos.ToString() ||
                   nombreCatalogo == CatalogoEnum.Municipios.ToString() ||
                   nombreCatalogo == CatalogoEnum.Resguardos.ToString() ||
                   nombreCatalogo == CatalogoEnum.Programas.ToString() ||
                   nombreCatalogo == CatalogoEnum.Productos.ToString() ||
                   nombreCatalogo == CatalogoEnum.Alternativas.ToString() ||
                   nombreCatalogo == CatalogoEnum.TiposRecursos.ToString() ||
                   nombreCatalogo == CatalogoEnum.ClasificacionesRecursos.ToString() ||
                   nombreCatalogo == CatalogoEnum.Etapas.ToString() ||
                   nombreCatalogo == CatalogoEnum.TiposAgrupaciones.ToString() ||
                   nombreCatalogo == CatalogoEnum.Agrupaciones.ToString() ||
                   nombreCatalogo == CatalogoEnum.Politicas.ToString() ||
                   nombreCatalogo == CatalogoEnum.PoliticasNivel1.ToString() ||
                   nombreCatalogo == CatalogoEnum.PoliticasNivel2.ToString() ||
                   nombreCatalogo == CatalogoEnum.PoliticasNivel3.ToString() ||
                   nombreCatalogo == CatalogoEnum.IndicadoresPoliticas.ToString() ||
                   nombreCatalogo == CatalogoEnum.TiposCofinanciaciones.ToString() ||
                   nombreCatalogo == CatalogoEnum.Entregables.ToString();

        }

        private static bool SonNombresReferenciasCatalogoValidos(string nombreCatalogo, string nombreCatalogoReferencia)
        {
            return nombreCatalogo == CatalogoEnum.Regiones.ToString() &&
                   nombreCatalogoReferencia == CatalogoEnum.Departamentos.ToString() ||
                   nombreCatalogo == CatalogoEnum.Departamentos.ToString() &&
                   nombreCatalogoReferencia == CatalogoEnum.Municipios.ToString() ||
                   nombreCatalogo == CatalogoEnum.Municipios.ToString() &&
                   nombreCatalogoReferencia == CatalogoEnum.Resguardos.ToString()||
                   nombreCatalogo == CatalogoEnum.Municipios.ToString() &&
                   nombreCatalogoReferencia == CatalogoEnum.Agrupaciones.ToString()||
                   nombreCatalogo == CatalogoEnum.TiposEntidades.ToString() &&
                   nombreCatalogoReferencia == CatalogoEnum.Entidades.ToString()||
                   nombreCatalogo == CatalogoEnum.TiposEntidades.ToString() &&
                   nombreCatalogoReferencia == CatalogoEnum.TiposRecursos.ToString();
        }
        #endregion
    }
}
