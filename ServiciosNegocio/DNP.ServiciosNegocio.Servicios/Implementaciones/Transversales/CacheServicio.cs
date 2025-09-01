namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Transversales
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Comunes.Enum;
    using Dominio.Dto.Catalogos;
    using Dominio.Dto.Proyectos;
    using Interfaces.Transversales;
    using Newtonsoft.Json;

    public class CacheServicio : ICacheServicio
    {

        #region PROYECTOS

        public async Task<ProyectoDto> ObtenerProyecto(string bpin, string tokenAutorizacion)
        {
            var uriMetodo = string.Format(ConfigurationManager.AppSettings["uriObtenerProyecto"], bpin);

            var response = await ConsumirServicioGet(uriMetodo, tokenAutorizacion);

            if (string.IsNullOrEmpty(response)) return new ProyectoDto();

            return JsonConvert.DeserializeObject<ProyectoDto>(response);
        }

        public async Task<List<ProyectoEntidadDto>> ConsultarProyectosEntidad(int idEntidad, string tokenAutorizacion)
        {
            var uriMetodo = string.Format(ConfigurationManager.AppSettings["uriObtenerProyectosEntidad"], idEntidad);

            var response = await ConsumirServicioGet(uriMetodo, tokenAutorizacion);

            if (string.IsNullOrEmpty(response)) return null;

            return new List<ProyectoEntidadDto>(JsonConvert.DeserializeObject<List<ProyectoEntidadDto>>(response));
        }

        public Task<bool> GuardarProyectosEntidad(int idEntidad, List<ProyectoEntidadDto> proyectosEntidad,
                                                  string tokenAutorizacion, long ttl)
        {
            var uriMetodo = string.Format(ConfigurationManager.AppSettings["uriGuardarProyectosEntidad"], idEntidad, ttl);

            return ConsumirServicioPost(proyectosEntidad, tokenAutorizacion, uriMetodo);
        }
        #endregion

        #region CATALOGOS

        public async Task<List<CatalogoDto>> ObtenerCatalogo(string nombreCatalogo, string tokenAutorizacion)
        {
            var uriMetodo = string.Format(ConfigurationManager.AppSettings["uriObtenerCatalogo"], nombreCatalogo);
            var response = await ConsumirServicioGet(uriMetodo, tokenAutorizacion);

            if (string.IsNullOrEmpty(response)) return null;

            return ConvertirJsonAListaCatalogo(nombreCatalogo, response);
        }

        public async Task<CatalogoDto> ObtenerCatalogoPorId(string nombreCatalogo, int idCatalogo, string tokenAutorizacion)
        {
            var uriMetodo = string.Format(ConfigurationManager.AppSettings["uriObtenerCatalogoPorId"], nombreCatalogo, idCatalogo);
            var response = await ConsumirServicioGet(uriMetodo, tokenAutorizacion);

            if (string.IsNullOrEmpty(response)) return null;

            return ConvertirJsonACatalogo(nombreCatalogo, response);
        }

        public Task<bool> GuardarListaCatalogo(string nombreCatalogo, List<CatalogoDto> listaCatalogo, long ttl, string tokenAutorizacion)
        {
            var uriMetodo = string.Format(ConfigurationManager.AppSettings["uriGuardarCatalogo"], nombreCatalogo, ttl);

            return ConsumirServicioPost(listaCatalogo, tokenAutorizacion, uriMetodo);
        }

        public async Task<CatalogoDto> ConsultarPorReferencia(string nombreCatalogo, int idCatalogo, string nombreCatalogoReferencia, string tokenAutorizacion)
        {
            var uriMetodo = string.Format(ConfigurationManager.AppSettings["uriConsultarRelacionCatalogo"],
                                          nombreCatalogo,
                                          idCatalogo,
                                          nombreCatalogoReferencia);

            var response = await ConsumirServicioGet(uriMetodo, tokenAutorizacion);

            if (string.IsNullOrEmpty(response)) return null;

            return ConvertirJsonACatalogo(nombreCatalogo, response);
        }

        public Task<bool> GuardarReferencia(string nombreCatalogo, int idCatalogo, string nombreCatalogoReferencia,
                                            CatalogoDto catalogo, long ttl, string tokenAutorizacion)
        {
            if ((nombreCatalogo != CatalogoEnum.Regiones.ToString() ||
                 nombreCatalogoReferencia != CatalogoEnum.Departamentos.ToString()) &&
                (nombreCatalogo != CatalogoEnum.Departamentos.ToString() ||
                 nombreCatalogoReferencia != CatalogoEnum.Municipios.ToString()) &&
                (nombreCatalogo != CatalogoEnum.Municipios.ToString() ||
                 nombreCatalogoReferencia != CatalogoEnum.Resguardos.ToString()) &&
                (nombreCatalogo != CatalogoEnum.TiposEntidades.ToString() ||
                 nombreCatalogoReferencia != CatalogoEnum.TiposRecursos.ToString()) &&
                (nombreCatalogo != CatalogoEnum.Municipios.ToString() ||
                 nombreCatalogoReferencia != CatalogoEnum.Agrupaciones.ToString()) &&
                (nombreCatalogo != CatalogoEnum.TiposEntidades.ToString() ||
                 nombreCatalogoReferencia != CatalogoEnum.Entidades.ToString())) return null;

            var uriMetodo = string.Format(ConfigurationManager.AppSettings["uriGuardarCatalogoReferencia"], nombreCatalogo, idCatalogo, nombreCatalogoReferencia, ttl);

            return ConsumirServicioPost(catalogo, tokenAutorizacion, uriMetodo);
        }

        #endregion

        #region Métodos Privados

        private List<CatalogoDto> ConvertirJsonAListaCatalogo(string nombreCatalogo, string result)
        {
            if (nombreCatalogo == CatalogoEnum.Entidades.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<EntidadCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.TiposEntidades.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<TiposEntidadesCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.Sectores.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<SectorCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.Regiones.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<RegionCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.Resguardos.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<ResguardoCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.Programas.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<ProgramaCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.Productos.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<ProductoCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.Alternativas.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<AlternativaCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.TiposRecursos.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<TipoRecursoCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.ClasificacionesRecursos.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<ClasificacionRecursoCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.Etapas.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<EtapaCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.TiposAgrupaciones.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<TipoAgrupacionCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.Agrupaciones.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<AgrupacionCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.GruposRecursos.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<GrupoRecursoCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.Departamentos.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<DepartamentoCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.Politicas.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<PoliticaCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.PoliticasNivel1.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<PoliticaCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.PoliticasNivel2.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<PoliticaCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.PoliticasNivel3.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<PoliticaCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.IndicadoresPoliticas.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<IndicadorPoliticaCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.TiposCofinanciaciones.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<TipoCofinanciadorCatalogoDto>>(result));

            if (nombreCatalogo == CatalogoEnum.Entregables.ToString()) return new List<CatalogoDto>(JsonConvert.DeserializeObject<List<EntregableCatalogoDto>>(result));

            return nombreCatalogo == CatalogoEnum.Municipios.ToString() ? new List<CatalogoDto>(JsonConvert.DeserializeObject<List<MunicipioCatalogoDto>>(result)) : null;

        }

        private CatalogoDto ConvertirJsonACatalogo(string nombreCatalogo, string result)
        {
            if (nombreCatalogo == CatalogoEnum.Entidades.ToString()) return JsonConvert.DeserializeObject<EntidadCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.TiposEntidades.ToString()) return JsonConvert.DeserializeObject<TiposEntidadesCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.Sectores.ToString()) return JsonConvert.DeserializeObject<SectorCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.Regiones.ToString()) return JsonConvert.DeserializeObject<RegionCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.Resguardos.ToString()) return JsonConvert.DeserializeObject<ResguardoCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.Programas.ToString()) return JsonConvert.DeserializeObject<ProgramaCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.Productos.ToString()) return JsonConvert.DeserializeObject<ProductoCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.Alternativas.ToString()) return JsonConvert.DeserializeObject<AlternativaCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.TiposRecursos.ToString()) return JsonConvert.DeserializeObject<TipoRecursoCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.ClasificacionesRecursos.ToString()) return JsonConvert.DeserializeObject<ClasificacionRecursoCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.Etapas.ToString()) return JsonConvert.DeserializeObject<EtapaCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.Departamentos.ToString()) return JsonConvert.DeserializeObject<DepartamentoCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.TiposAgrupaciones.ToString()) return JsonConvert.DeserializeObject<TipoAgrupacionCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.Agrupaciones.ToString()) return JsonConvert.DeserializeObject<AgrupacionCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.GruposRecursos.ToString()) return JsonConvert.DeserializeObject<GrupoRecursoCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.Politicas.ToString()) return JsonConvert.DeserializeObject<PoliticaCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.PoliticasNivel1.ToString()) return JsonConvert.DeserializeObject<PoliticaCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.PoliticasNivel2.ToString()) return JsonConvert.DeserializeObject<PoliticaCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.PoliticasNivel3.ToString()) return JsonConvert.DeserializeObject<PoliticaCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.IndicadoresPoliticas.ToString()) return JsonConvert.DeserializeObject<IndicadorPoliticaCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.TiposCofinanciaciones.ToString()) return JsonConvert.DeserializeObject<TipoCofinanciadorCatalogoDto>(result);

            if (nombreCatalogo == CatalogoEnum.Entregables.ToString()) return JsonConvert.DeserializeObject<EntregableCatalogoDto>(result);

            return nombreCatalogo == CatalogoEnum.Municipios.ToString() ? JsonConvert.DeserializeObject<MunicipioCatalogoDto>(result) : null;
        }

        private Task<string> ConsumirServicioGet(string uriMetodo, string tokenAutorizacion)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiCache"];

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", tokenAutorizacion);

                var response = client.GetAsync(endPoint + uriMetodo).Result;

                return response.Content.ReadAsStringAsync();
            }
        }

        private static Task<bool> ConsumirServicioPost(object catalogo, string tokenAutorizacion, string uriMetodo)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiCache"];

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", tokenAutorizacion);

                var response = client.PostAsJsonAsync(endPoint + uriMetodo, catalogo).Result;

                return Task.FromResult(response.IsSuccessStatusCode);
            }
        }

        #endregion
    }
}
