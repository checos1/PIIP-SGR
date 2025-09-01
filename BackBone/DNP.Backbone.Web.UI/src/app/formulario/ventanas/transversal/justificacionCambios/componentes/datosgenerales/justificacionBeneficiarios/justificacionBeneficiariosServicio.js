(function () {
    'use strict';
    angular.module('backbone').factory('justificacionBeneficiariosServicio', justificacionBeneficiariosServicio);

    justificacionBeneficiariosServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function justificacionBeneficiariosServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerCapitulosModificados: obtenerCapitulosModificados,
            ObtenerJustificacionbeneficiariosTotales: ObtenerJustificacionbeneficiariosTotales,
        };

        function ObtenerJustificacionbeneficiariosTotales(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerJustificacionProyectosBeneficiarios + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function obtenerCapitulosModificados(guiMacroproceso, idProyecto, idInstancia, seccionCapituloId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneCapitulosModificadosLocalizacion;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            url += "&seccionCapituloId=" + seccionCapituloId;
            return $http.get(url);
        }
    }
})();
