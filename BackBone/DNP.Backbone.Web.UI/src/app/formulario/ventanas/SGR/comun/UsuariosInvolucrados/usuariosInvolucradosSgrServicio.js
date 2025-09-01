(function () {
    'use strict';

    angular.module('backbone').factory('usuariosInvolucradosSgrServicio', usuariosInvolucradosSgrServicio);

    usuariosInvolucradosSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function usuariosInvolucradosSgrServicio($http, constantesBackbone) {

        return {
            guardarProyectoViabilidadInvolucrados: guardarProyectoViabilidadInvolucrados,
            ObtenerProyectoViabilidadInvolucrados: ObtenerProyectoViabilidadInvolucrados,
            EliminarProyectoViabilidadInvolucrados: EliminarProyectoViabilidadInvolucrados,
            ObtenerUsuariosInvolucrados: ObtenerUsuariosInvolucrados,
            ObtenerProyectoViabilidadInvolucradosFirma: ObtenerProyectoViabilidadInvolucradosFirma,
        };

        function ObtenerProyectoViabilidadInvolucrados(proyectoId, tipoConceptoViabilidadId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneSGRLeerProyectoViabilidadInvolucrados;
            var params = {
                'proyectoId': proyectoId,
                'tipoConceptoViabilidadId': tipoConceptoViabilidadId,
            };
            return $http.get(url, { params });
        }

        function guardarProyectoViabilidadInvolucrados(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneProyectoViabilidadInvolucradosAgregar, parametros);
        }

        function EliminarProyectoViabilidadInvolucrados(Id) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneProyectoViabilidadInvolucradosEliminar + "?id=" + Id;
            return $http.post(url);
        }

        function ObtenerUsuariosInvolucrados(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerUsuariosInvolucrados, parametros);
        }

        function ObtenerProyectoViabilidadInvolucradosFirma(instanciaId, tipoConceptoViabilidadId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneSGRLeerProyectoViabilidadInvolucradosFirma;
            var params = {
                'instanciaId': instanciaId,
                'tipoConceptoViabilidadId': tipoConceptoViabilidadId
            };
            return $http.get(url, { params });
        }
    }
})();