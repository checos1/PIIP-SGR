(function () {

    'use strict';

    angular.module('backbone')
        .factory('servicioFichasProyectos', servicioFichasProyectos);

    servicioFichasProyectos.$inject = ['$http', 'appSettings', 'utilidades', '$httpParamSerializer', 'constantesBackbone'];

    function servicioFichasProyectos($http, appSettings, utilidades, $httpParamSerializer, constantesBackbone) {

        var servicios = {
            GenerarFicha: GenerarFicha,
            GenerarFichaSGR: GenerarFichaSGR,
            ObtenerIdFicha: ObtenerIdFicha
        }

        return servicios;

        ///////////////////////

        function GenerarFicha(params) {
            var config = {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                }
                /*,responseType: 'arraybuffer'*/
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiFichaProyectoFichaFisico, params, config)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function GenerarFichaSGR(params) {
            var config = {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                },
                responseType: 'arraybuffer'
            };

            return $http.post(apiFichasProyectosBaseUri + constantesBackbone.apiFichaProyectoFichaFisicoSGR, params, config)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }
        
        function ObtenerIdFicha(nombreFicha) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiFichaProyectoObtenerPlantillaPorNombre + "/" + nombreFicha)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }
    }
})();