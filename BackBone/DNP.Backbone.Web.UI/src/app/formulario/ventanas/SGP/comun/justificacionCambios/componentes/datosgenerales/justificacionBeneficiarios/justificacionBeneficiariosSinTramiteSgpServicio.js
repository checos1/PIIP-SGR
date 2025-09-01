(function () {
    'use strict';
    angular.module('backbone').factory('justificacionBeneficiariosSinTramiteSgpServicio', justificacionBeneficiariosSinTramiteSgpServicio);

    justificacionBeneficiariosSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function justificacionBeneficiariosSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerCapitulosModificados: obtenerCapitulosModificados,
            ObtenerJustificacionbeneficiariosTotales: ObtenerJustificacionbeneficiariosTotales,
            ObtenerbeneficiariosTotales: ObtenerbeneficiariosTotales
        };

        function ObtenerJustificacionbeneficiariosTotales(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerJustificacionProyectosBeneficiarios + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function ObtenerbeneficiariosTotales(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosBeneficiariosSGP + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
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
