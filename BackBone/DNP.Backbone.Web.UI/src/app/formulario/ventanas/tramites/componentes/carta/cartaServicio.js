(function () {
    'use strict';
    angular.module('backbone').factory('cartaServicio', cartaServicio);

    cartaServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function cartaServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerPlantillaCarta: obtenerPlantillaCarta,
            obtenerDatosCartaPorSeccion: obtenerDatosCartaPorSeccion,
            actualizarCartaDatosIniciales: actualizarCartaDatosIniciales,
            obtenerUsuariosRegistrados: obtenerUsuariosRegistrados,
            obtenerCuerpoConceptoCDP: obtenerCuerpoConceptoCDP,
            obtenerCuerpoConceptoAutorizacion: obtenerCuerpoConceptoAutorizacion,
            obtenerCartaConceptoDatosDespedida: obtenerCartaConceptoDatosDespedida,
            actualizarCartaConceptoDatosDespedida: actualizarCartaConceptoDatosDespedida,
            ObtenerProyectosCartaTramite: ObtenerProyectosCartaTramite,
            ObtenerDetalleCartaAL: ObtenerDetalleCartaAL,
            consultarCarta: consultarCarta,
            ConsultarCartaConcepto: ConsultarCartaConcepto,

        }


        function obtenerPlantillaCarta(nombreSeccion, tipoTramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneObtenerPlantillaCarta}` + '?nombreSeccion=' + nombreSeccion + '&tipoTramite=' + tipoTramiteId;
            return $http.get(url);
        }

        function obtenerDatosCartaPorSeccion(tramiteId, plantillaSeccionId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneObtenerDatosCartaPorSeccion}` + '?tramiteId=' + tramiteId + '&plantillaSeccionId=' + plantillaSeccionId;
            return $http.get(url);
        }

        function verificaUsuarioDestinatario(usuarioTramite) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneVerificaUsuarioDestinatario;
            return $http.post(url, usuarioTramite);
        }

        function actualizarCartaDatosIniciales(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneActualizarCartaDatosIniciales;
            return $http.post(url, parametros);
        }

        function obtenerUsuariosRegistrados(tramiteId, numeroTramite) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneObtenerUsuariosRegistrados}` + '?tramiteId=' + tramiteId +
                '&numeroTramite=' + numeroTramite;
            return $http.get(url);

        }
        function obtenerCartaConceptoDatosDespedida(tramiteId, plantillaCartaSeccionId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneObtenerCartaConceptoDatosDespedida}` + '?tramiteId=' + tramiteId + '&plantillaCartaSeccionId=' + plantillaCartaSeccionId;
            return $http.get(url);
        }
        function actualizarCartaConceptoDatosDespedida(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneActualizarCartaConceptoDatosDespedida;
            return $http.post(url, parametros);
        }

        function obtenerCuerpoConceptoCDP(tramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneObtenerCuerpoConceptoCDP}` + '?tramiteId=' + tramiteId;
            return $http.get(url);
        }

        function obtenerCuerpoConceptoAutorizacion(tramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneObtenerCuerpoConceptoAutorizacion}` + '?tramiteId=' + tramiteId;
            return $http.get(url);
        }

        function ObtenerProyectosCartaTramite(tramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneObtenerProyectosCartaTramite}` + '?tramiteId=' + tramiteId;
            return $http.get(url);
        }

        function ObtenerDetalleCartaAL(tramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneObtenerDetalleCartaAL}` + '?tramiteId=' + tramiteId;
            return $http.get(url);
        }

        function consultarCarta(tramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneConsultarCarta}` + '?tramiteId=' + tramiteId;
            return $http.get(url);
        }

        function ConsultarCartaConcepto(tramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneConsultarCartaConcepto}` + '?tramiteId=' + tramiteId;
            return $http.get(url);
        }
    }
})();