(function () {
    'use strict';
    angular.module('backbone').factory('conpesServicio', conpesServicio);

    conpesServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function conpesServicio($q, $http, $location, constantesBackbone) {
        return {
            CargarConpes: CargarConpes,
            BuscarConpes: BuscarConpes,
            AdicionarConpes: AdicionarConpes,
            EliminarConpes: EliminarConpes,
            ObtenerConpesTramites: ObtenerConpesTramites,
            asociarCompesVigenciaFutura: asociarCompesVigenciaFutura,
            removerAsociacionConpesTramiteVigenciaFutura: removerAsociacionConpesTramiteVigenciaFutura
        };

        function CargarConpes(idProyecto, instanciaId, guiMacroproceso,NivelId,FlujoId) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneTramiteCargarProyectoConpes + "?proyectoid=" + idProyecto + "&InstanciaId=" + instanciaId + "&GuiMacroproceso=" + guiMacroproceso + "&NivelId=" + NivelId + "&FlujoId=" + FlujoId);
        }

        function BuscarConpes(conpes) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneTramiteObtenerProyectoConpes + "?conpes=" + conpes);
        }

        function AdicionarConpes(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneTramiteAdicionarProyectoConpes;
            return $http.post(url, parametros);
        }

        function EliminarConpes(proyecto,conpes) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneTramiteEliminarProyectoConpes + "?proyectoid=" + proyecto + "&conpesid=" + conpes);
        }

        function ObtenerConpesTramites(idTramite) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenrTramiteConpes + "/" + idTramite);
        }

        function asociarCompesVigenciaFutura(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneAsociarTramiteConpes, model);
        }

        function removerAsociacionConpesTramiteVigenciaFutura(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneRemoverAsociacionTramiteConpes, model);
        }
    }
})();