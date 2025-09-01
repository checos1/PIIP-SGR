(function () {
    'use strict';

    ///////////////////////////////////////////////////////////
    /* Servicio obtener datos CPP */
    ///////////////////////////////////////////////////////////

    angular.module('backbone')
        .factory('categoriaProductosPoliticaServicio', categoriaProductosPoliticaServicio);

    categoriaProductosPoliticaServicio.$inject = [
        '$http', 'constantesCategoriaProductosPolitica'
    ];

    /// Obtener información de categoria de productos por politica.
    function categoriaProductosPoliticaServicio($http, constantesCategoriaProductosPolitica) {
        function obtenerDatosCPP(id, fuenteId, politicaId) {
            var url = apiBackboneServicioBaseUri + constantesCategoriaProductosPolitica.apiBackboneObtenerDatosIP;
            var params = { bpin: id, fuenteId, politicaId };
            return $http.get(url, { params });
        }

        return { obtenerDatosCPP };
    }

    ///////////////////////////////////////////////////////////
    /* Servicio Guardar datos de solicitud de recursos. */
    ///////////////////////////////////////////////////////////

    angular.module('backbone')
        .factory('guardarDatosSolicitudRecursosServicio', guardarDatosSolicitudRecursosServicio);

    guardarDatosSolicitudRecursosServicio.$inject = [
        '$http', 'constantesCategoriaProductosPolitica'
    ];

    /// Guardar datos de solicitud de recursos.
    function guardarDatosSolicitudRecursosServicio($http, constantesCategoriaProductosPolitica) {
        function post(dat) {
            var url = apiBackboneServicioBaseUri + constantesCategoriaProductosPolitica.apiBackboneGuardarDatosSolicitudRecursos;
            var params = {
                BPIN: dat.BPIN, FuenteId: dat.FuenteId, ProyectoId: dat.ProyectoId,
                Politicas: dat.Politicas
            };
            return $http.post(url, params);
        }

        return { post };
    }
})();