(function () {
	'use strict';
	angular.module('backbone').factory('reporteEstructuraActividadesServicio', reporteEstructuraActividadesServicio);

	reporteEstructuraActividadesServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

	function reporteEstructuraActividadesServicio($q, $http, $location, constantesBackbone) {
		return {
			obtenerReportesPowerBI: obtenerReportesPowerBI
		};

		function obtenerReportesPowerBI(filtro) {
			const embedParametrosDto = {
				EmbedFiltroDto: filtro
			};

			return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneobtenerReporteGantt, embedParametrosDto);
		}
	}
})();