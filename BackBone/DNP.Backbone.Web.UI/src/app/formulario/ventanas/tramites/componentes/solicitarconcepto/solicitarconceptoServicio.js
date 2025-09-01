(function () {
    'use strict';
    angular.module('backbone').factory('solicitarconceptoServicio', solicitarconceptoServicio);

    solicitarconceptoServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function solicitarconceptoServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerSolicitarConcepto: ObtenerSolicitarConcepto,
            ObtenerDireccionTecnica: ObtenerDireccionTecnica,
            ObtenerSubDireccionTecnica: ObtenerSubDireccionTecnica,
            ObtenerAnalistasSubDireccionTecnica: ObtenerAnalistasSubDireccionTecnica,
            SolicitarConcepto: SolicitarConcepto,
            cargarSolicitudesConcepto: cargarSolicitudesConcepto,
            obtenerConceptoDireccionTecnicaTramite: obtenerConceptoDireccionTecnicaTramite,
            guardarConceptoDireccionTecnicaTramite: guardarConceptoDireccionTecnicaTramite,
            eliminarPermisos: eliminarPermisos,
            guardarConceptoDireccionTecnicaTramite: guardarConceptoDireccionTecnicaTramite,
            enviarConceptoDireccionTecnicaTramite: enviarConceptoDireccionTecnicaTramite
        };

        function ObtenerSolicitarConcepto(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerSolicitarConcepto, peticion);
        }

        function ObtenerDireccionTecnica(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDireccionTecnica, peticion);
        }

        function ObtenerSubDireccionTecnica(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerSubDireccionTecnica, peticion);
        }

        function ObtenerAnalistasSubDireccionTecnica(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerAnalistasSubDireccionTecnica, peticion);
        }

        function SolicitarConcepto(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSolicitarConcepto, peticion);
        }

        function cargarSolicitudesConcepto(tramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneCargarSolicitudesConcepto}` + '?tramiteId=' + tramiteId;
            return $http.post(url);
        }

        function obtenerConceptoDireccionTecnicaTramite(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConceptoDireccionTecnicaTramite, peticion);
        }
        function guardarConceptoDireccionTecnicaTramite(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarConceptoDireccionTecnicaTramite, peticion);
        }

        function eliminarPermisos(usuarioDestino, tramiteId, aliasNivel, InstanciaId  ) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneEliminarPermisosAccionesUsuarios}`;
            url = url + '?usuarioDestino=' + usuarioDestino + '&tramiteId=' + tramiteId + '&aliasNivel=' + aliasNivel + '&InstanciaId=' + InstanciaId;
            return $http.post(url);
        }

        function guardarConceptoDireccionTecnicaTramite(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarConceptoDireccionTecnicaTramite, peticion);
        }

        function enviarConceptoDireccionTecnicaTramite(tramiteId, usuarioDnp )
        {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneEnviarConceptoDireccionTecnicaTramite}`;
            url = url + '?tramiteId=' + tramiteId + '&usuarioDnp=' +  usuarioDnp ;
            return $http.post(url);
        }
    }
})();