(function () {
    'use strict';
    angular.module('backbone').factory('focalizacionPoliticasServicio', focalizacionPoliticasServicio);

    focalizacionPoliticasServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function focalizacionPoliticasServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerFocalizacionProgramacionSeguimiento: ObtenerFocalizacionProgramacionSeguimiento,
            GuardarProductoCategoriaSeguimiento: GuardarProductoCategoriaSeguimiento,
            ObtenerFocalizacionProgramacionSeguimientoDetalle: ObtenerFocalizacionProgramacionSeguimientoDetalle
        };

        function ObtenerFocalizacionProgramacionSeguimiento(parametroConsulta) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.uriapiBackboneObtenerFocalizacionProgramacionSeguimiento + "?parametroConsulta=" + parametroConsulta;
            return $http.get(url);
        }

        function GuardarProductoCategoriaSeguimiento(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.uriapiBackboneGuardarFocalizacionProgramacionSeguimiento + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function ObtenerFocalizacionProgramacionSeguimientoDetalle(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.uriapiBackboneObtenerFocalizacionProgramacionSeguimientoDetalle + "?parametros=" + parametros + "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }

    }
})();