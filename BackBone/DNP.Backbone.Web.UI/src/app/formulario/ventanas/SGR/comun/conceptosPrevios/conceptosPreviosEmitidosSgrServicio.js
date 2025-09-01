(function () {
    'use strict';

    angular.module('backbone').factory('conceptosPreviosEmitidosSgrServicio', conceptosPreviosEmitidosSgrServicio);

    conceptosPreviosEmitidosSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function conceptosPreviosEmitidosSgrServicio($http, constantesBackbone) {

        return {
            SGR_obtenerConceptosPreviosEmitidos: SGR_obtenerConceptosPreviosEmitidos,
        };

        function SGR_obtenerConceptosPreviosEmitidos(Bpin, TipoConcepto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConceptosPreviosEmitidos + "?bpin=" + Bpin + "&tipoconcepto=" + TipoConcepto;
            return $http.get(url);
        }
    }
})();