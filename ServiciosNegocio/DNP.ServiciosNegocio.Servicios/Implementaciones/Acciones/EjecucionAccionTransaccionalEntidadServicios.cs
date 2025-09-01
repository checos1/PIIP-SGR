using System;
using DNP.ServiciosNegocio.Dominio.Dto.Acciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.Acciones;
using DNP.ServiciosNegocio.Comunes.Enum;
using DNP.ServiciosNegocio.Comunes.Interfaces;
using DNP.ServiciosNegocio.Dominio.Dto.Auditoria;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Entidades;
using DNP.ServiciosNegocio.Servicios.Interfaces.Entidades;
using DNP.ServiciosNegocio.Dominio.Dto.Entidades;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using DNP.ServiciosNegocio.Servicios.Properties;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Acciones
{
    public class EjecucionAccionTransaccionalEntidadServicios : IEjecucionAccionTransaccionalServicios
    {
        private readonly IEntidadServicios _entidadServicios;
        private readonly IAccionUtilidades _accionUtilidades;
        private readonly IAuditoriaServicios _auditoriaServicios;
        public string Usuario { get; set; }
        public string Ip { get; set; }
        public EjecucionAccionTransaccionalEntidadServicios(IEntidadServicios entidadServicios, IAccionUtilidades accionUtil, IAuditoriaServicios auditoriaServicios)
        {
            _entidadServicios = entidadServicios;
            _accionUtilidades = accionUtil;
            _auditoriaServicios = auditoriaServicios;
        }

        public bool EjecutarAccion(AccionFormularioDto accion)
        {
            switch ((EntidadEnum)accion.ObjetoDatos.IdEntidad)
            {
                case EntidadEnum.OpcionCatalagoTipoEntidad:
                    ValidacionesDeNegocio(accion);
                    var entidad = ArmarEntidad(accion);
                    ((OpcionCatalogoTipoEntidadServicios)_entidadServicios).InsertarEntidadBase(entidad);
                    GenerarAuditoriaOpcionCatalogoTipoEntidadServicios(Ip, Usuario, TipoMensajeEnum.Creacion, string.Format(Resources.AuditoriaEntidad, "OpcionCatalogoTipoEntidadDto", ((OpcionCatalogoTipoEntidadDto)entidad).Nombre), entidad.Id);
                    break;

                default:
                    return false;
            }
            return true;
        }

        private void ValidacionesDeNegocio(AccionFormularioDto accion)
        {
            // Validar si la instancia existe
            _accionUtilidades.ExisteInstancia(accion.IdInstanciaFlujo, Usuario);
            // Validar que la accion existe y esta activa
            _accionUtilidades.ExisteAccionActiva(accion.IdInstanciaAccion, Usuario);


        }

        private EntidadBase ArmarEntidad(AccionFormularioDto accion)
        {
            try
            {
                JObject objetoJson = JObject.Parse(accion.ObjetoDatos.ObjetoJson);

                switch ((EntidadEnum)accion.ObjetoDatos.IdEntidad)
                {
                    case EntidadEnum.OpcionCatalagoTipoEntidad:
                        OpcionCatalogoTipoEntidadDto entidadNueva = new OpcionCatalogoTipoEntidadDto();
                        foreach (var token in objetoJson)
                        {
                            var llave = token.Key;
                            var valor = token.Value;
                            if (llave == "Id")
                                entidadNueva.Id = Convert.ToInt32(valor);
                            if (llave == "Code")
                                entidadNueva.CodigoEntidad = valor?.ToString();
                            if (llave == "IsActive")
                                entidadNueva.EsActiva = string.IsNullOrEmpty(valor?.ToString())
                                    ? (bool?)null
                                    : Convert.ToBoolean(valor);
                            if (llave == "ParentId")
                                entidadNueva.IdPadre = string.IsNullOrEmpty(valor?.ToString())
                                    ? (int?)null
                                    : Convert.ToInt32(valor);
                            if (llave == "EntityTypeId")
                                entidadNueva.IdTipo = string.IsNullOrEmpty(valor?.ToString())
                                    ? (int?)null
                                    : Convert.ToInt32(valor);
                            if (llave == "Name")
                                entidadNueva.Nombre = valor?.ToString();
                            if (llave == "AtributosEntidad")
                            {
                                JObject objetoJasonAtributo = JObject.Parse(valor?.ToString());
                                AtributosEntidadDto atributo = new AtributosEntidadDto();
                                foreach (var tokenAtributo in objetoJasonAtributo)
                                {
                                    llave = tokenAtributo.Key;
                                    valor = tokenAtributo.Value;
                                    if (llave == "CabeceraSector")
                                        atributo.CabeceraSector = Convert.ToBoolean(valor);
                                    if (llave == "Orden")
                                        atributo.Orden = valor.ToString();
                                    if (llave == "SectorId")
                                        atributo.SectorId = string.IsNullOrEmpty(valor?.ToString())
                                            ? (int?)null
                                            : Convert.ToInt32(valor);
                                    if (llave == "FechaCreacion")
                                        atributo.FechaCreacion = Convert.ToDateTime(valor);
                                    if (llave == "FechaModificacion")
                                        atributo.FechaModificacion = Convert.ToDateTime(valor);
                                    if (llave == "CreadoPor")
                                        atributo.CreadoPor = valor?.ToString();
                                    if (llave == "ModificadoPor")
                                        atributo.ModificadoPor = valor?.ToString();
                                }

                                entidadNueva.Atributos = atributo;
                            }
                        }

                        if (entidadNueva.Atributos != null)
                            entidadNueva.Atributos.Id = entidadNueva.Id;
                        return entidadNueva;


                    default:
                        return null;
                }
            }
            catch
            {
                throw new AccionException(Resources.ErrorJsonInvalido);
            }
        }

        private void GenerarAuditoriaOpcionCatalogoTipoEntidadServicios(string ip, string usuario, TipoMensajeEnum tipoMensaje, string accion, int idEntidad)
        {
            var entidadEncontrada = ((OpcionCatalogoTipoEntidadServicios)_entidadServicios).ConsultarEntidadBasePorId(idEntidad);

            OpcionCatalogoTipoEntidadServiciosAuditoriaDto entidadAuditoria = new OpcionCatalogoTipoEntidadServiciosAuditoriaDto()
            {
                Mensaje = accion,
                Nombre = ((OpcionCatalogoTipoEntidadDto)entidadEncontrada).Nombre,
                IdTipo = ((OpcionCatalogoTipoEntidadDto)entidadEncontrada).IdTipo,
                IdPadre = ((OpcionCatalogoTipoEntidadDto)entidadEncontrada).IdPadre,
                CodigoEntidad = ((OpcionCatalogoTipoEntidadDto)entidadEncontrada).CodigoEntidad,
                EsActiva = ((OpcionCatalogoTipoEntidadDto)entidadEncontrada).EsActiva,
                CabeceraSector = ((OpcionCatalogoTipoEntidadDto)entidadEncontrada).Atributos?.CabeceraSector,
                Orden = ((OpcionCatalogoTipoEntidadDto)entidadEncontrada).Atributos?.Orden,
                SectorId = ((OpcionCatalogoTipoEntidadDto)entidadEncontrada).Atributos?.SectorId,
                FechaCreacion = ((OpcionCatalogoTipoEntidadDto)entidadEncontrada).Atributos?.FechaCreacion,
                FechaModificacion = ((OpcionCatalogoTipoEntidadDto)entidadEncontrada).Atributos?.FechaModificacion,
                CreadoPor = ((OpcionCatalogoTipoEntidadDto)entidadEncontrada).Atributos?.CreadoPor,
                ModificadoPor = ((OpcionCatalogoTipoEntidadDto)entidadEncontrada).Atributos?.ModificadoPor
            };

            var mensaje = JsonConvert.SerializeObject(entidadAuditoria);
            //se quitan las llaves adicionales que genero el newtonsoft
            var contenidoMensaje = mensaje.Substring(1, mensaje.Length - 2);
            Task.Run(() => _auditoriaServicios.RegistrarTrazabilidadAuditoriaServiciosNegocio(contenidoMensaje, ip, usuario, tipoMensaje));
        }
    }
}
