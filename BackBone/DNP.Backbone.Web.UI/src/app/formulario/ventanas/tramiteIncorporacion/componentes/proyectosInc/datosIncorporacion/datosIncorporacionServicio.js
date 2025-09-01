(function () {
    'use strict';
    angular.module('backbone').factory('datosIncorporacionServicio', datosIncorporacionServicio);

    datosIncorporacionServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function datosIncorporacionServicio($q, $http, $location, constantesBackbone)
    {
        return {
            ObtenerDatosIncorporacion: ObtenerDatosIncorporacion,
            ObtenerSectores: ObtenerSectores,
            ObtenerListaEntidades: ObtenerListaEntidades,
            GuardarDatosIncorporacion: GuardarDatosIncorporacion,
            EiliminarDatosIncorporacion: EiliminarDatosIncorporacion
        };

        function ObtenerDatosIncorporacion(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosIncorporacion + "?tramiteId=" + tramiteId;
           return $http.get(url);
        }

        function ObtenerSectores(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaSectoresEntity;
            return $http.post(url, parametros);
        }

        function ObtenerListaEntidades(peticionobtenerProyectos, idTipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEntidadesTotal;
            return $http.post(url, peticionobtenerProyectos);
        }

        function GuardarDatosIncorporacion(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarDatosIncorporacion + "?usuario=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function EiliminarDatosIncorporacion(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEiliminarDatosIncorporacion + "?usuario=" + usuarioDNP;
            return $http.post(url, parametros);
        }

    }
})();