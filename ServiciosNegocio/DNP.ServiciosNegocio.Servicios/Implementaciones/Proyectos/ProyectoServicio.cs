namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Proyectos
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Threading.Tasks;
	using Comunes.Dto.ObjetosNegocio;
	using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
	using DNP.ServiciosNegocio.Comunes.Excepciones;
	using DNP.ServiciosNegocio.Dominio.Dto.Conpes;
	using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
	using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
	using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
	using DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
	using Dominio.Dto.Proyectos;
	using Interfaces.Proyectos;
	using Interfaces.Transversales;
	using Persistencia.Interfaces.Proyectos;
    using System.Net.NetworkInformation;

	public class ProyectoServicio : IProyectoServicio
	{
		private readonly ICacheServicio _cacheServicio;
		private readonly IProyectoPersistencia _proyectoPersistencia;
		public string Usuario { get; set; }
        public string Ip { get; set; }

		public ProyectoServicio(ICacheServicio cacheServicio, IProyectoPersistencia proyectoPersistencia)
		{
			_cacheServicio = cacheServicio;
			_proyectoPersistencia = proyectoPersistencia;
		}

		public Task<ProyectoDto> ObtenerProyecto(string bpin, string tokenAutorizacion)
		{
			return _cacheServicio.ObtenerProyecto(bpin, tokenAutorizacion);
		}

		public Task<ProyectoDto> ObtenerProyectoPreview()
		{
			return Task.FromResult(_proyectoPersistencia.ObtenerProyectoPreview());
		}

		public Task<List<ProyectoEntidadDto>> ConsultarProyectosPorEntidadesYEstados(ParametrosProyectosDto parametros)
		{
			var proyectosEntidades = new List<ProyectoEntidadDto>();
			var entidadesPendientes = new List<int>();
			entidadesPendientes.AddRange(parametros.IdsEntidades);

			if (entidadesPendientes.Count > 0)
			{
				proyectosEntidades = _proyectoPersistencia.ObtenerProyectosEntidad(parametros.IdsEntidades, parametros.NombresEstadosProyectos);
			}

			if (proyectosEntidades.Count == 0) return Task.FromResult<List<ProyectoEntidadDto>>(null);
			return Task.FromResult(proyectosEntidades);
		}

		public Task<List<ProyectoPriorizarDto>> ObtenerProyectosPriorizar(String IdUsuarioDNP)
		{
            var proyectos = new List<ProyectoPriorizarDto>();

            if (IdUsuarioDNP != null && IdUsuarioDNP.Length > 0)
                proyectos = _proyectoPersistencia.ObtenerProyectosPriorizar(IdUsuarioDNP);

            //if (proyectos.Count == 0) return Task.FromResult<List<ProyectoPriorizarDto>>(null);

            return Task.FromResult(proyectos);
        }

        public Task<List<ProyectoEntidadDto>> ConsultarProyectosPorBPINs(BPINsProyectosDto bpins)
		{
			var proyectos = new List<ProyectoEntidadDto>();

			if (bpins != null && bpins.BPINs.Count > 0)
				proyectos = _proyectoPersistencia.ObtenerProyectosPorBPINs(bpins);

			if (proyectos.Count == 0) return Task.FromResult<List<ProyectoEntidadDto>>(null);

			return Task.FromResult(proyectos);
		}

		public Task<List<EntidadDto>> ConsultarEntidadesPorIds(List<string> idsEntidades)
		{
			var entidades = new List<EntidadDto>();

			if (idsEntidades != null && idsEntidades.Count > 0)
				entidades = _proyectoPersistencia.ObtenerEntidadesPorIds(idsEntidades);

			if (entidades.Count == 0) return Task.FromResult<List<EntidadDto>>(null);

			return Task.FromResult(entidades);
		}

		public Task<List<ProyectoEntidadDto>> ConsultarProyectosPorIds(List<int> ids)
		{
			var proyectos = new List<ProyectoEntidadDto>();

			if (ids != null && ids.Count > 0)
				proyectos = _proyectoPersistencia.ObtenerProyectosPorIds(ids);

			if (proyectos.Count == 0) return Task.FromResult<List<ProyectoEntidadDto>>(null);

			return Task.FromResult(proyectos);
		}
		public Task<List<CrTypeDto>> ObtenerCRType()
		{
			return Task.FromResult(_proyectoPersistencia.ObtenerCRType());
		}

		public Task<List<FaseDto>> ObtenerFase()
		{
			return Task.FromResult(_proyectoPersistencia.ObtenerFase());
		}

		public Task<RespuestaGeneralDto> MantenimientoMatrizFlujo(List<MatrizEntidadDestinoAccionDto> flujos)
		{
			return Task.FromResult(_proyectoPersistencia.MantenimientoMatrizFlujo(flujos));
		}
		public Task<List<MatrizEntidadDestinoAccionDto>> ObtenerMatrizFlujo(int entidadResponsableId)
		{
			return Task.FromResult(_proyectoPersistencia.ObtenerMatrizFlujo(entidadResponsableId));
		}

		public IEnumerable<ProyectoCreditoDto> ObtenerProyectosContracredito(string tipoEntidad, int? idEntidad, Guid idFLujo, int? idEntidadFiltro, string bpin, string nombreProyecto)
		{
			return _proyectoPersistencia.ObtenerProyectosContracredito(tipoEntidad, idEntidad, idFLujo, idEntidadFiltro, bpin, nombreProyecto);
		}

		public IEnumerable<ProyectoCreditoDto> ObtenerProyectosCredito(string tipoEntidad, int idEntidad, Guid idFLujo, string bpin, string nombreProyecto)
		{
			return _proyectoPersistencia.ObtenerProyectosCredito(tipoEntidad, idEntidad, idFLujo, bpin, nombreProyecto);
		}


		#region AuditoriaEntidades

		/// <summary>
		/// Inserta un nuevo registro de cambio de entidad del proyecto actual
		/// </summary>
		/// <param name="auditoriaEntidad">Información del nuevo cambio de entidad como una instancia de la clase <see cref="AuditoriaEntidadDto"/></param>
		/// <returns></returns>
		public Task<RespuestaGeneralDto> InsertarAuditoriaEntidad(AuditoriaEntidadDto auditoriaEntidad)
		{
			try
			{
				var valorRetorno = _proyectoPersistencia.InsertarAuditoriaEntidad(auditoriaEntidad);
				return Task.FromResult(
						// retornar una respuesta al controlador con el valor retorno
						new RespuestaGeneralDto { IdRegistro = new Guid().ToString(), Exito = true, Mensaje = String.Empty, Registros = new List<Object> { valorRetorno } }
					);
			}
			catch (Exception exception)
			{
				return Task.FromResult(
					// retornar una respuesta al controlador con la excepción generada
					new RespuestaGeneralDto
					{
						IdRegistro = new Guid().ToString(),
						Exito = false,
						Mensaje = $"ProyectoServicio.InsertarAuditoriaEntidad: {exception.Message}\n{exception.InnerException?.Message ?? String.Empty}",
					}
					);
			}
		}

		/// <summary>
		///         Obtiene el historial de cambio de entidades del proyecto actual
		/// </summary>
		/// <param name="proyectoId">Identificador del proyecto actual</param>
		/// <returns></returns>
		public Task<RespuestaGeneralDto> ObtenerAuditoriaEntidad(int proyectoId)
		{
			try
			{
				var resultado = _proyectoPersistencia.ObtenerAuditoriaEntidad(proyectoId).ToList<object>();
				return Task.FromResult(
					 // retornar una respuesta al controlador con el valor retorno
					 new RespuestaGeneralDto { IdRegistro = new Guid().ToString(), Exito = true, Mensaje = String.Empty, Registros = resultado }
				 );
			}
			catch (Exception exception)
			{
				return Task.FromResult(
					   // retornar una respuesta al controlador con la excepción generada
					   new RespuestaGeneralDto
					   {
						   IdRegistro = new Guid().ToString(),
						   Exito = false,
						   Mensaje = $"ProyectoServicio.ObtenerAuditoriaEntidad: {exception.Message}\n{exception.InnerException?.Message ?? String.Empty}",
					   }
				);
			}
		}
		#endregion AuditoriaEntidades

		#region Métodos Privados

		public List<ProyectoEntidadDto> FiltrarProyectosPorEstados(ParametrosProyectosDto parametros, IEnumerable<ProyectoEntidadDto> proyectosEntidades)
		{
			var idsProyectos = proyectosEntidades.Select(c => c.ProyectoId).Distinct().ToList();

			var proyectosEstados = _proyectoPersistencia.ObtenerProyectosPorEstados(idsProyectos, parametros.NombresEstadosProyectos);

			if (proyectosEstados == null || proyectosEstados.Count == 0) return null;

			var interseccionProyectos = (from pest in proyectosEstados
										 where idsProyectos.Any(u => u == pest.ProyectoId)
										 select pest).Distinct().ToList();

			return interseccionProyectos;
		}

		private async Task ConsultarProyectosPorEntidad(ParametrosProyectosDto parametros, int idEntidad,
														List<ProyectoEntidadDto> proyectosEntidades)
		{
			var proyectosEntidadCache = await _cacheServicio.ConsultarProyectosEntidad(idEntidad, parametros.TokenAutorizacion);
			proyectosEntidades.AddRange(proyectosEntidadCache);

			if (proyectosEntidadCache == null)
			{
				//Si no existe en caché se consulta en MGA y se guarda en caché.
				//var proyectosEntidadMga = _proyectoPersistencia.ObtenerProyectosEntidad(idEntidad);

				if (proyectosEntidades == null || proyectosEntidades.Count == 0) return;

				var tiempo = Convert.ToInt32(ConfigurationManager.AppSettings["TTL_Proyectos"]);
				var tiempoExpiracion = new TimeSpan(tiempo, 0, 0).Ticks;

				await _cacheServicio.GuardarProyectosEntidad(idEntidad,
															 proyectosEntidades,
															 parametros.TokenAutorizacion,
															 tiempoExpiracion);

				//proyectosEntidades.AddRange(proyectosEntidades);
			}
			else
			{
				proyectosEntidades.AddRange(proyectosEntidadCache);
			}
		}



		public CapituloConpes ObtenerProyectoConpes(int proyectoId, Guid InstanciaId, string GuiMacroproceso,string NivelId, string FlujoId)
		{
			Guid guidNivelID = Guid.Parse(NivelId);
			Guid guidFlujoId = Guid.Parse(FlujoId);
			return _proyectoPersistencia.ObtenerProyectoConpes(proyectoId, InstanciaId, GuiMacroproceso, guidNivelID, guidFlujoId);
		}

		#endregion

		Task<RespuestaGeneralDto> IProyectoServicio.ActualizarHorizonte(HorizonteProyectoDto datosHorizonteProyecto, string usuario)
		{
			return Task.FromResult(_proyectoPersistencia.ActualizarHorizonte(datosHorizonteProyecto, usuario));
		}

		public Task<RespuestaGeneralDto> AdicionarProyectoConpes(CapituloConpes Conpes, string usuario)
		{
			return Task.FromResult(_proyectoPersistencia.AdicionarProyectoConpes(Conpes, usuario));
		}

		public List<DocumentoCONPESDto> EliminarProyectoConpes(int proyectoId, int conpesId)
		{
			return _proyectoPersistencia.EliminarProyectoConpes(proyectoId, conpesId);
		}

		public ObjectivosAjusteDto ObtenerResumenObjetivosProductosActividades(string bpin)
		{
			return _proyectoPersistencia.ObtenerResumenObjetivosProductosActividades(bpin);
		}

        public Task<ReponseHttp> GuardarAjusteCostoActividades(ProductoAjusteDto producto, string usuario)
        {
            try
            {
                _proyectoPersistencia.GuardarAjusteCostoActividades(producto, usuario);
                return Task.FromResult<ReponseHttp>(new ReponseHttp() { Status = true });
            }
            catch (ServiciosNegocioException e)
            {
                return Task.FromResult<ReponseHttp>(new ReponseHttp()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }

		public void AgregarEntregable(AgregarEntregable[] entregables, string usuario)
		{
			_proyectoPersistencia.AgregarEntregable(entregables, usuario);
		}

		public void EliminarEntregable(EntregablesActividadesDto entregable)
		{
			_proyectoPersistencia.EliminarEntregable(entregable);
		}

		public ObjectivosAjusteJustificacionDto ObtenerResumenObjetivosProductosActividadesJustificacion(string bpin)
		{
			return _proyectoPersistencia.ObtenerResumenObjetivosProductosActividadesJustificacion(bpin);
		}

		public LocalizacionJustificacionProyectoDto ObtenerJustificacionLocalizacionProyecto(int idProyecto)
		{
			return _proyectoPersistencia.ObtenerJustificacionLocalizacionProyecto(idProyecto);
		}

		public List<ProyectoInstanciaDto> ObtenerInstanciaProyectoTramite(string InstanciaId, string BPIN)
		{
			Guid guidInstancia = new Guid(InstanciaId);
			return _proyectoPersistencia.ObtenerInstanciaProyectoTramite(guidInstancia, BPIN);
		}

        public string ObtenerProyectosBeneficiarios(string bpin)
        {
            return _proyectoPersistencia.ObtenerProyectosBeneficiarios(bpin);
        }

        /// <summary>
        /// Validacion previa a la devolución de un paso
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <param name="accionId"></param>   
        /// <param name="accionDevolucionId"></param>   
		/// <param name="usuario"></param>   
        /// <returns>string</returns> 
        public string ValidacionDevolucionPaso(Guid instanciaId, Guid accionId, Guid accionDevolucionId, string usuario)
		{
			return _proyectoPersistencia.ValidacionDevolucionPaso(instanciaId, accionId, accionDevolucionId, usuario);
		}

        public string ObtenerProyectosBeneficiariosDetalle(string json)
        {
			return _proyectoPersistencia.ObtenerProyectosBeneficiariosDetalle(json);
        }

        public string ObtenerJustificacionProyectosBeneficiarios(string bpin)
        {
            return _proyectoPersistencia.ObtenerJustificacionProyectosBeneficiarios(bpin);
        }

        public void GuardarBeneficiarioTotales(BeneficiarioTotalesDto beneficiario, string usuario)
		{
			_proyectoPersistencia.GuardarBeneficiarioTotales(beneficiario, usuario);
		}

        public List<ConfiguracionUnidadMatrizDTO> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto dto, string usuario)
        {
            return _proyectoPersistencia.ObtenerMatrizEntidadDestino(dto, usuario);
        }

        public Task<RespuestaGeneralDto> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto dto, string usuario)
        {
            return Task.FromResult(_proyectoPersistencia.ActualizarMatrizEntidadDestino(dto, usuario));
        }

        public void GuardarBeneficiarioProducto(BeneficiarioProductoDto beneficiario, string usuario)
		{
			_proyectoPersistencia.GuardarBeneficiarioProducto(beneficiario, usuario);
		}

		public void GuardarBeneficiarioProductoLocalizacion(BeneficiarioProductoLocalizacionDto beneficiario, string usuario)
		{
			_proyectoPersistencia.GuardarBeneficiarioProductoLocalizacion(beneficiario, usuario);
		}

		public void GuardarBeneficiarioProductoLocalizacionCaracterizacion(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuario)
		{
			_proyectoPersistencia.GuardarBeneficiarioProductoLocalizacionCaracterizacion(beneficiario, usuario);
		}

        public string GetCategoriasSubcategorias_JSON(int padreId, int? entidadId, int esCategoria, int esGruposEtnicos)
        {
			return _proyectoPersistencia.GetCategoriasSubcategorias_JSON(padreId,entidadId,esCategoria,esGruposEtnicos);
        }

		public Task<List<ProyectoEntidadDto>> ConsultarProyectosASeleccionar(ParametrosProyectosDto parametros)
		{
			var proyectosEntidades = new List<ProyectoEntidadDto>();
			//var entidadesPendientes = new List<int>();
			//entidadesPendientes.AddRange(parametros.IdsEntidades);

			if (parametros.IdsEntidades.Count > 0)
			{
				proyectosEntidades = _proyectoPersistencia.ConsultarProyectosASeleccionar(parametros);
			}

			if (proyectosEntidades.Count == 0) return Task.FromResult<List<ProyectoEntidadDto>>(null);
			return Task.FromResult(proyectosEntidades);
		}

		public Task<RespuestaGeneralDto> GuardarReprogramacionPorProductoVigencia(List<ReprogramacionValores> reprogramacionValores, string usuario)
        {
			if(reprogramacionValores != null && reprogramacionValores[0].ProyectoId == 0)
            {
				RespuestaGeneralDto respuesta = new RespuestaGeneralDto();
				respuesta.Exito = false;
				respuesta.Mensaje = "No se guada la información";
				return Task.FromResult(respuesta);

			}
			var rta = _proyectoPersistencia.GuardarReprogramacionPorProductoVigencia(reprogramacionValores, usuario);
			return Task.FromResult(rta);

		}



        public SoportesDto ObtenerDocumentosProyecto(FiltroDocumentosDto filtroDocumentos)
        {
            return _proyectoPersistencia.ObtenerDocumentosProyecto(filtroDocumentos);
        }

        public PlanNacionalDesarrolloDto ObtenerPND(int idProyecto)
        {
			return _proyectoPersistencia.ObtenerPND(idProyecto);
        }
    }
}
