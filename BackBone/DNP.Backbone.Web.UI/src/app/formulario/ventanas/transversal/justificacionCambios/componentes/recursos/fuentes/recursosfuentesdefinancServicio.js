(function () {
    'use strict';
    angular.module('backbone').factory('recursosfuentesdefinancServicio', recursosfuentesdefinancServicio);

    recursosfuentesdefinancServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    function recursosfuentesdefinancServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerResumenFuentesFinanciacion: obtenerResumenFuentesFinanciacion,
            obtenerCapitulosModificados: obtenerCapitulosModificados,
            obtenerCapitulosModificadosCapitoSeccion: obtenerCapitulosModificadosCapitoSeccion,
            obtenerDetalleAjustesFuenteFinanciacion: obtenerDetalleAjustesFuenteFinanciacion,
        };

        function obtenerResumenFuentesFinanciacion(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneResumenFuenteFinanciacion + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function obtenerCapitulosModificados(guiMacroproceso, idProyecto, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneCapitulosModificados;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function obtenerCapitulosModificadosCapitoSeccion(guiMacroproceso, idProyecto, idInstancia, capitulo, seccion, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneCapitulosModificadosCapitoSeccion;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            url += "&capitulo=" + capitulo;
            url += "&seccion=" + seccion;
            url += "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }

        function obtenerDetalleAjustesFuenteFinanciacion(Bpin, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneobtenerDetalleAjustesFuenteFinanciacion;
            url += "?Bpin=" + Bpin;
            url += "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }

    }
})();