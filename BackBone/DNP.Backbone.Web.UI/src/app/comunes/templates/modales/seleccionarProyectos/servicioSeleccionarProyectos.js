(function () {
    'use strict';

    angular.module('backbone').factory('servicioSeleccionarProyectos', servicioSeleccionarProyectos);
    servicioSeleccionarProyectos.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function servicioSeleccionarProyectos($q, $http, $location, constantesBackbone) {

        return {
            obtenerInstanciasProyecto: obtenerInstanciasProyecto
        }

        function obtenerInstanciasProyecto(peticionObtenerInbox, proyectoFiltro) {
            const dto = {
                ProyectoParametrosDto: peticionObtenerInbox,
                ProyectoFiltroDto: proyectoFiltro,
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsolaProyectosInstancias, dto);
        }

    }
})();