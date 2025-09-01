(function () {
	'use strict';
	angular.module('backbone').factory('justificacionHorizonteSinTramiteSgpServicio', justificacionHorizonteSinTramiteSgpServicio);

	justificacionHorizonteSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

	function justificacionHorizonteSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
		return {

			obtenerCambiosFirme: obtenerCambiosFirme,
			guardarCambiosFirme: guardarCambiosFirme,
			obtenerJustificacion: obtenerJustificacion
		};


		function obtenerCambiosFirme(idProyecto, instanciaId) {
			var url = apiBackboneServicioBaseUri + constantesBackbone.apiCambiosFirmeObtenerJustificacionHorizonte;
			url += "?idProyecto=" + idProyecto;
			return $http.get(url);
		}

		function guardarCambiosFirme(data) {
			var url = apiBackboneServicioBaseUri + constantesBackbone.apiCambiosFirmeguardarCambiosFirmeJustificacionHorizonte;
			return $http.post(url, data);
		}


		function obtenerJustificacion(idProyecto, idInstancia, guiMacroproceso) {
			var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneCapitulosModificados;
			url += "?guiMacroproceso=" + guiMacroproceso;
			url += "&idProyecto=" + idProyecto;
			url += "&idInstancia=" + idInstancia;
			return $http.get(url);
		}
	}

})();