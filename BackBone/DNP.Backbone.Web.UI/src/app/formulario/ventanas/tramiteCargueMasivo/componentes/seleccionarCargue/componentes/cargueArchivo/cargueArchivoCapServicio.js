(function () {
    'use strict';
    angular.module('backbone').factory('cargueArchivoCapServicio', cargueArchivoCapServicio);

    cargueArchivoCapServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function cargueArchivoCapServicio($q, $http, $location, constantesBackbone) {
        return {
            actualizarCargueMasivo: actualizarCargueMasivo,
            consultarCargueExcel: consultarCargueExcel
        };

        function actualizarCargueMasivo(objetoNegocioDto) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarCargueMasivo, objetoNegocioDto);
        }

        function consultarCargueExcel(objetoNegocioDto) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsultarCargueExcel, objetoNegocioDto);
        }
       
    }
})();