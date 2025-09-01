(function () {
    'use strict';
    angular.module('backbone').factory('resumenAvanceRegionalizacionCapServicio', resumenAvanceRegionalizacionCapServicio);

    resumenAvanceRegionalizacionCapServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function resumenAvanceRegionalizacionCapServicio($q, $http, $location, constantesBackbone) {
        return {
            consultarResumenAvanceRegionalizacion: consultarResumenAvanceRegionalizacion
        };

        function consultarResumenAvanceRegionalizacion(objetoParametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneResumenAvanceRegionalizacion;
            url += "?instanciaId=" + objetoParametros.instanciaId;
            url += "&proyectoId=" + objetoParametros.proyectoId;
            url += "&codigoBpin=" + objetoParametros.codigoBpin;

            return $http.get(url);
        }

    }
})();