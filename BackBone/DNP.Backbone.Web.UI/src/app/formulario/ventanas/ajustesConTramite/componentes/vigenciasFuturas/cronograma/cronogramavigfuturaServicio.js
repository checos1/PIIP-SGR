(function () {
    'use strict';
    angular.module('backbone').factory('cronogramavigfuturaServicio', cronogramavigfuturaServicio);

    cronogramavigfuturaServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function cronogramavigfuturaServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerModalidadContratacion: ObtenerModalidadContratacion,
            ObtenerActividadesPrecontractualesModalidadesContratacion: ObtenerActividadesPrecontractualesModalidadesContratacion,
            apiBackBoneObtenerActividadesPrecontractualesProyectoTramite: apiBackBoneObtenerActividadesPrecontractualesProyectoTramite,
            ActualizarActividadesCronograma: ActualizarActividadesCronograma,
            Actividades: Actividades,
            ObtenerModalidadContratacionVigenciasFuturas: ObtenerModalidadContratacionVigenciasFuturas,
        };

        function ObtenerModalidadContratacion(mostrar) {
            if (mostrar === undefined) mostrar = 1;
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerModalidadesContratacion
            url += "?mostrar=" + mostrar;
            return $http.get(url);
        }

        function ObtenerActividadesPrecontractualesModalidadesContratacion(modalidadContratacionId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerActividadesPrecontractualesModalidadesContratacion + "?modalidadContratacionId=" + modalidadContratacionId;
            return $http.get(url);
        }
        function apiBackBoneObtenerActividadesPrecontractualesProyectoTramite(modalidadContratacionId, proyectoId, tramiteId, eliminarActividades) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerActividadesPrecontractualesProyectoTramite + "?modalidadContratacionId=" + modalidadContratacionId + "&proyectoId=" + proyectoId + "&tramiteId=" + tramiteId + "&eliminarActividades=" + eliminarActividades;;
            return $http.get(url).then((respuesta) => respuesta.data);
        }

        function ActualizarActividadesCronograma(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneActualizarActividadesCronograma;
            return $http.post(url, parametros);
        }

        function Actividades(modalidadContratacionId, tramiteProyectoId) {
            return $q.all([
                ObtenerActividadesPrecontractualesModalidadesContratacion(modalidadContratacionId),
                apiBackBoneObtenerActividadesPrecontractualesProyectoTramite(modalidadContratacionId, tramiteProyectoId)
            ])
        }

        function ObtenerModalidadContratacionVigenciasFuturas(proyectoId, tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerModalidadContratacionVigenciasFuturas;
            url += "?ProyectoId=" + proyectoId;
            url += "&TramiteId=" + tramiteId;
            return $http.get(url);



        }
    }
})();   