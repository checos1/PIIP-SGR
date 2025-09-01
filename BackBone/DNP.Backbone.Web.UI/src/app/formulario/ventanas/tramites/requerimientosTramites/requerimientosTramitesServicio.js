(function () {
    'use strict';
    angular.module('backbone').factory('requerimientosTramitesServicio', requerimientosTramitesServicio);

    requerimientosTramitesServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function requerimientosTramitesServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerPreguntasJustificacion: obtenerPreguntasJustificacion,
            guardarRespuestasJustificacion: guardarRespuestasJustificacion,
            ObtenerPreguntasProyectoActualizacion: ObtenerPreguntasProyectoActualizacion,
            ObtenerPreguntasProyectoActualizacionPaso: ObtenerPreguntasProyectoActualizacionPaso,
        };
        function obtenerPreguntasJustificacion(idTramite, proyectoId, tipoTramiteId, tipoRolId, idNivel) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasJustificacion;
            var params = {
                'TramiteId': idTramite,
                'ProyectoId': proyectoId,
                'TipoTramiteId': tipoTramiteId === null ? 0 : tipoTramiteId,
                'TipoRolId': tipoRolId,
                'IdNivel': idNivel
            };
            return $http.get(url, { params });
        }

        function guardarRespuestasJustificacion(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarRespuestasJustificacion, parametros);
        }

        function ObtenerPreguntasProyectoActualizacion(idTramite, proyectoId, tipoTramiteId, idNivel, tipoRolId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasProyectoActualizacion;
            var params = {
                'TramiteId': idTramite,
                'ProyectoId': proyectoId,
                'TipoTramiteId': tipoTramiteId == null ? 0 : tipoTramiteId,
                'IdNivel': idNivel,
                'TipoRolId': tipoRolId,
            };
            return $http.get(url, { params });
        }

        function ObtenerPreguntasProyectoActualizacionPaso(idTramite, proyectoId, tipoTramiteId, idNivel, tipoRolId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasProyectoActualizacionPaso;
            var params = {
                'TramiteId': idTramite,
                'ProyectoId': proyectoId,
                'TipoTramiteId': tipoTramiteId == null ? 0 : tipoTramiteId,
                'IdNivel': idNivel,
                'TipoRolId': tipoRolId,
            };
            return $http.get(url, { params });
        }


    }
})();