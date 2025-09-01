(function () {
    'use strict';

    angular.module('backbone').factory('usuariosInvolucradosSgpServicio', usuariosInvolucradosSgpServicio);

    usuariosInvolucradosSgpServicio.$inject = ['$http', 'constantesBackbone'];

    function usuariosInvolucradosSgpServicio($http, constantesBackbone) {

        return {
            guardarProyectoViabilidadInvolucrados: guardarProyectoViabilidadInvolucrados,
            ObtenerProyectoViabilidadInvolucrados: ObtenerProyectoViabilidadInvolucrados,
            EliminarProyectoViabilidadInvolucrados: EliminarProyectoViabilidadInvolucrados,
            ObtenerUsuariosInvolucrados: ObtenerUsuariosInvolucrados,
            ObtenerProyectoViabilidadInvolucradosFirma: ObtenerProyectoViabilidadInvolucradosFirma,
            ObtenerDependenciasFuncionarioInvolucrados: ObtenerDependenciasFuncionarioInvolucrados,
            ObtenerEntidadDestinoResponsableFlujo: ObtenerEntidadDestinoResponsableFlujo,
            ObtenerEntidadDestinoResponsableFlujoTramite: ObtenerEntidadDestinoResponsableFlujoTramite
        };

        function ObtenerProyectoViabilidadInvolucrados(proyectoId, instanciaId, tipoConceptoViabilidadId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneLeerProyectoViabilidadInvolucradosSGP;
            var params = {
                'proyectoId': proyectoId,
                'instanciaId': instanciaId,
                'tipoConceptoViabilidadId': tipoConceptoViabilidadId,
            };
            return $http.get(url, { params });
        }

        function ObtenerDependenciasFuncionarioInvolucrados(parametros) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneLeerDependenciasFuncionarioInvolucradoSGP);
        }

        function ObtenerEntidadDestinoResponsableFlujo(rolId, crTypeId, entidadResponsableId, proyectoId) {
            var params = {
                'rolId': rolId,
                'crTypeId': crTypeId,
                'entidadResponsableId': entidadResponsableId,
                'proyectoId': proyectoId
            };
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerEntidadDestinoResponsableFlujoSGP, { params });
        }

        function ObtenerEntidadDestinoResponsableFlujoTramite(rolId, entidadResponsableId, tramiteId) {
            var params = {
                'rolId': rolId,
                'entidadResponsableId': entidadResponsableId,
                'tramiteId': tramiteId
            };
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerEntidadDestinoResponsableFlujoTramiteSGP, { params });
        }

        function guardarProyectoViabilidadInvolucrados(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneProyectoViabilidadInvolucradosAgregarSGP, parametros);
        }

        function EliminarProyectoViabilidadInvolucrados(Id) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneProyectoViabilidadInvolucradosEliminarSGP + "?id=" + Id;
            return $http.post(url);
        }

        function ObtenerUsuariosInvolucrados(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerUsuariosInvolucrados, parametros);
        }

        function ObtenerProyectoViabilidadInvolucradosFirma(instanciaId, tipoConceptoViabilidadId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneLeerProyectoViabilidadInvolucradosFirmaSGP;
            var params = {
                'instanciaId': instanciaId,
                'tipoConceptoViabilidadId': tipoConceptoViabilidadId
            };
            return $http.get(url, { params });
        }
    }
})();