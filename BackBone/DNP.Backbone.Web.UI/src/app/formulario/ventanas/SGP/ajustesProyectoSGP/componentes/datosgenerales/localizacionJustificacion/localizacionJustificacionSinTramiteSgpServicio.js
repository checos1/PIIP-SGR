(function () {
	'use strict';
	angular.module('backbone').factory('localizacionJustificacionSinTramiteSgpServicio', localizacionJustificacionSinTramiteSgpServicio);

	localizacionJustificacionSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone', '$sessionStorage'];

	function localizacionJustificacionSinTramiteSgpServicio($q, $http, $location, constantesBackbone,$sessionStorage) {
		return {
			obtenerJustificacion: obtenerJustificacion,
			obtenerDepartamentos: obtenerDepartamentos,
			obtenerListaEntidades: obtenerListaEntidades,
			obtenerAgrupaciones: obtenerAgrupaciones,
			obtenerTipoAgrupaciones: obtenerTipoAgrupaciones,
			guardarLocalizacion: guardarLocalizacion,
			obtenerMunicipios: obtenerMunicipios,
			obtenerAgrupacionesCompleta: obtenerAgrupacionesCompleta,
			ObtenerCapitulosModificados: ObtenerCapitulosModificados,
			obtenerCapitulosModificadosLocalizacion: obtenerCapitulosModificadosLocalizacion
		};

		function obtenerJustificacion(idProyecto, idInstancia, guiMacroproceso) {
			var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneCapitulosModificados;
			url += "?guiMacroproceso=" + guiMacroproceso;
			url += "&idProyecto=" + idProyecto;
			url += "&idInstancia=" + idInstancia;
			return $http.get(url);
		}

		function obtenerDepartamentos() {
				var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDepartamentosLocalizacion;
			return $http.post(url);
		}

		function obtenerMunicipios(EntidadesPorCodigoParametrosDto,idDepartamento) {
			var peticion = {
				IdUsuario: usuarioDNP,
				IdObjeto: $sessionStorage.idProyecto,
				Aplicacion: null,
				ListaIdsRoles: null,
				idDepartamento: idDepartamento
			};

			var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerMunicipiosLocalizacion;
			return $http.post(url, peticion);
		}

		function obtenerListaEntidades(peticionobtenerProyectos, idTipoEntidad) {
			var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEntidadesTotal;
			return $http.post(url, peticionobtenerProyectos);
		}

		function obtenerAgrupaciones(peticion) {
			var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaAgrupaciones;
			return $http.post(url, peticion);
		}

		function obtenerTipoAgrupaciones(peticion) {
			var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaTipoAgrupaciones;
			return $http.post(url, peticion);
		}

		function guardarLocalizacion(LocalizacionProyectoAjusteDto, usuarioDNP) {
			var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarLocalizacionSGP + "?usuarioDNP=" + usuarioDNP;
			return $http.post(url, LocalizacionProyectoAjusteDto);
		}

		function obtenerAgrupacionesCompleta(peticion) {
			var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaAgrupacionesCompleta;
			return $http.post(url, peticion);
		}

		function ObtenerCapitulosModificados(guiMacroproceso, idProyecto, idInstancia) {
			var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneCapitulosModificados;
			url += "?guiMacroproceso=" + guiMacroproceso;
			url += "&idProyecto=" + idProyecto;
			url += "&idInstancia=" + idInstancia;
			return $http.get(url);
		}

		function obtenerCapitulosModificadosLocalizacion(guiMacroproceso, idProyecto, idInstancia, seccionCapituloId) {
			var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneCapitulosModificadosLocalizacion;
			url += "?guiMacroproceso=" + guiMacroproceso;
			url += "&idProyecto=" + idProyecto;
			url += "&idInstancia=" + idInstancia;
			url += "&seccionCapituloId=" + seccionCapituloId;
			return $http.get(url);
		}

	}

})();