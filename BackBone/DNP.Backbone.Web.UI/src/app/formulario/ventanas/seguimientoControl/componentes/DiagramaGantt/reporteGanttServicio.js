(function () {
	'use strict';
	angular.module('backbone').factory('reporteGanttServicio', reporteGanttServicio);

	reporteGanttServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

	function reporteGanttServicio($q, $http, $location, constantesBackbone) {
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