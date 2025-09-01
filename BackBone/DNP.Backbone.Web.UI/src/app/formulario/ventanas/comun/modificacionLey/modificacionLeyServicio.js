(function () {
    'use strict';
    angular.module('backbone').factory('modificacionLeyServicio', modificacionLeyServicio);

    modificacionLeyServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    var proyectoCargado = 0;
    var bpinCargado = "";

    function modificacionLeyServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerInformacionPresupuestalMLEncabezado: ObtenerInformacionPresupuestalMLEncabezado,
            ObtenerInformacionPresupuestalMLDetalle: ObtenerInformacionPresupuestalMLDetalle,
            GuardarInformacionPresupuestalML: GuardarInformacionPresupuestalML,
            ConsultarPoliticasTransversalesCategoriasModificaciones: ConsultarPoliticasTransversalesCategoriasModificaciones,
            GuardarPoliticasTransversalesCategoriasModificaciones: GuardarPoliticasTransversalesCategoriasModificaciones,
            ConsultarCatalogoIndicadoresPolitica: ConsultarCatalogoIndicadoresPolitica,
            GuardarModificacionesAsociarIndicadorPolitica: GuardarModificacionesAsociarIndicadorPolitica
        };

        function ObtenerInformacionPresupuestalMLEncabezado(EntidadDestinoId, TramiteId, origen) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerInformacionPresupuestalMLEncabezado + "?EntidadDestinoId=" + EntidadDestinoId + "&TramiteId=" + TramiteId + "&origen=" + origen;
            return $http.get(url);
        }

        function ObtenerInformacionPresupuestalMLDetalle(TramiteProyectoId, origen) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerInformacionPresupuestalMLDetalle + "?TramiteProyectoId=" + TramiteProyectoId + "&origen=" + origen;
            return $http.get(url);
        }

        function GuardarInformacionPresupuestalML(FuentesValores) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarInformacionPresupuestalML;
            return $http.post(url, FuentesValores);
        }

        function ConsultarPoliticasTransversalesCategoriasModificaciones(Bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarPoliticasTransversalesCategoriasModificaciones + "?bpin=" + Bpin;
            return $http.get(url);
        }

        function GuardarPoliticasTransversalesCategoriasModificaciones(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarPoliticasTransversalesCategoriasModificaciones;
            return $http.post(url, parametros);
        }

        function ConsultarCatalogoIndicadoresPolitica(PoliticaId, Criterio) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarCatalogoIndicadoresPolitica + "?PoliticaId=" + PoliticaId + "&Criterio=" + Criterio;
            return $http.get(url);
        }

        function GuardarModificacionesAsociarIndicadorPolitica(proyectoId, politicaId, categoriaId, indicadorId, accion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarModificacionesAsociarIndicadorPolitica + "?proyectoId=" + proyectoId + "&politicaId=" + politicaId + "&categoriaId=" + categoriaId + "&indicadorId=" + indicadorId + "&accion=" + accion;
            return $http.post(url);
        }
    }
})();