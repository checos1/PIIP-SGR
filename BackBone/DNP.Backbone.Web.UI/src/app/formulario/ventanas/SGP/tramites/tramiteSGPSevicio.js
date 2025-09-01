(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('tramiteSGPServicio', tramiteSGPServicio);

    tramiteSGPServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function tramiteSGPServicio($q, $http, $location, constantesBackbone) {

        return {
            actualizaEstadoAjusteProyecto: actualizaEstadoAjusteProyecto,
            eliminarProyectoTramite: eliminarProyectoTramite,
            eliminarInstanciaCerrada_AbiertaProyectoTramite: eliminarInstanciaCerrada_AbiertaProyectoTramite,
            actualizarTramitesFuentesPresupuestales: actualizarTramitesFuentesPresupuestales,
            obtenerListaProyectosFuentes: obtenerListaProyectosFuentes,
            guardarFuentesTramiteProyectoAprobacion: guardarFuentesTramiteProyectoAprobacion,
            obtenerListaProyectosFuentesAprobada: obtenerListaProyectosFuentesAprobada,
            actualizarTramitesRequisitos: actualizarTramitesRequisitos,
            obtenerProyectoRequisitosPorTramite: obtenerProyectoRequisitosPorTramite,
            obtenerContraCreditosSgp: obtenerContraCreditosSgp,
            obtenerCreditosSgp: obtenerCreditosSgp,
            obtenerPreguntasJustificacion: obtenerPreguntasJustificacion,
            guardarRespuestasJustificacion: guardarRespuestasJustificacion,
            registrarObservador: registrarObservador,
            removerObservador: removerObservador,
            notificarCambio: notificarCambio
        };

        function actualizaEstadoAjusteProyecto(tipoDevolucion, objetoNegocioId, tramiteId, observacion) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneActualizarEstadoAjusteProyectoSGP}` + '?tipoDevolucion=' + tipoDevolucion + '&ObjetoNegocioId=' + objetoNegocioId + '&tramiteId=' + tramiteId + '&observacion=' + observacion;
            return $http.post(url);
        }

        function eliminarProyectoTramite(peticionObtenerInbox, parametros) {
            const tramiteDto = {
                ParametrosInboxDto: peticionObtenerInbox,
                TramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    tramiteId: parametros.TramiteId,
                    filtroGradeDtos: [],
                    InstanciaId: parametros.TramiteId,
                    ProyectoId: parametros.ProyectoId
                },
                columnasVisibles: []
            };

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarProyectoTramiteNegocioSGP;
            return $http.post(url, tramiteDto);
        }

        function eliminarInstanciaCerrada_AbiertaProyectoTramite(instanciaTramite, Bpin) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneEliminarInstanciaCerrada_AbiertaProyectoTramiteSGP}` + '?instanciaTramite=' + instanciaTramite + '&Bpin=' + Bpin;
            return $http.post(url);
        }

        //Información Presupuestal
        function actualizarTramitesFuentesPresupuestales(TramiteFuentesPresupuestalesDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarTramitesFuentesPresupuestalesSGP;
            return $http.post(url, TramiteFuentesPresupuestalesDto);
        }

        function obtenerListaProyectosFuentes(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaProyectosFuentesSGP;
            url = url + '?tramiteId=' + tramiteId;
            return $http.get(url);
        }

        function guardarFuentesTramiteProyectoAprobacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarFuentesTramiteProyectoAprobacionSGP;
            return $http.post(url, parametros);
        }

        function obtenerListaProyectosFuentesAprobada(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaProyectosFuentesAprobadoSGP;
            url = url + '?tramiteId=' + tramiteId;
            return $http.get(url);
        }

        function actualizarTramitesRequisitos(TramiteRequisitosDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarTramitesRequisitosSGP;
            return $http.post(url, TramiteRequisitosDto);
        }

        function obtenerProyectoRequisitosPorTramite(ProyectoId, TramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerProyectoRequisitosPorTramiteSGP}` + '?pProyectoId=' + ProyectoId + '&pTramiteId=' + TramiteId + '&isCDP=' + true;
            return $http.get(url);
        }

        function obtenerContraCreditosSgp(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerContraCreditosSgp;
            return $http.post(url, parametros);
        }
        function obtenerCreditosSgp(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerCreditoSgp;
            return $http.post(url, parametros);
        }

        function obtenerPreguntasJustificacion(idTramite, proyectoId, tipoTramiteId, tipoRolId, idNivel) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasJustificacion;
            var params = {
                'TramiteId': idTramite,
                'ProyectoId': proyectoId,
                'TipoTramiteId': tipoTramiteId === null ? 0 : tipoTramiteId,
                'TipoRolId': tipoRolId,
                'IdNivel': idNivel
            };
            return $http.get(url, { params });
        }

        function guardarRespuestasJustificacion(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarRespuestasJustificacion, parametros);
        }

        /*Inicio - Usado para comunicar los componentes hijos*/
        var observadores = [];

        function registrarObservador(callback) {
            if (!observadores) {
                observadores = [];
            }
            observadores.push(callback);

            // Devolver una función de eliminación
            return () => removerObservador(callback);
        }

        function removerObservador(callback) {
            const indice = observadores.indexOf(callback);
            if (indice !== -1) {
                observadores.splice(indice, 1);
            }
        }

        function notificarCambio(datos) {
            if (observadores) {
                observadores.forEach(callback => callback(datos));
            }
        }
        /*Fin - Usado para comunicar los componentes hijos*/
    }
})()