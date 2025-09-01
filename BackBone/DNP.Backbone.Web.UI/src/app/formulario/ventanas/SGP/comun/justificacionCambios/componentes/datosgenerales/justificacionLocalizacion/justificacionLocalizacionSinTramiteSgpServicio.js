(function () {
	'use strict';
	angular.module('backbone').factory('justificacionLocalizacionSinTramiteSgpServicio', justificacionLocalizacionSinTramiteServicio);

	justificacionLocalizacionSinTramiteServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

	function justificacionLocalizacionSinTramiteServicio($q, $http, $location, constantesBackbone) {
		return {

			obtenerJustificacion: obtenerJustificacion,
			ObtenerJustificacionLocalizacionProyecto: ObtenerJustificacionLocalizacionProyecto
		};


		function obtenerJustificacion(idProyecto, idInstancia, guiMacroproceso) {
			var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneCapitulosModificados;
			url += "?guiMacroproceso=" + guiMacroproceso;
			url += "&idProyecto=" + idProyecto;
			url += "&idInstancia=" + idInstancia;
			return $http.get(url);
		}

		function ObtenerJustificacionLocalizacionProyecto(idProyecto, usuarioDNP) {
			var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerJustificacionLocalizacionProyecto;
			url += "?proyectoId=" + idProyecto;
			return $http.get(url, usuarioDNP);
		}
	}

})();