(function () {
    'use strict';
    angular.module('backbone').factory('asignacionEntidadNacionalSgrServicio', asignacionEntidadNacionalSgrServicio);
    asignacionEntidadNacionalSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function asignacionEntidadNacionalSgrServicio($http, constantesBackbone) {

        return {
            registrarObservador: registrarObservador,
            notificarCambio: notificarCambio,   
            limpiarObservadores: limpiarObservadores,
            SGR_Proyectos_LeerEntidadesAdscritas: SGR_Proyectos_LeerEntidadesAdscritas,
            SGR_Proyectos_ActualizarEntidadAdscrita: SGR_Proyectos_ActualizarEntidadAdscrita,
            ObtenerUsuariosEntidadAdscrita: ObtenerUsuariosEntidadAdscrita,
            SGR_Proyectos_GuardarAsignacionUsuarioEncargado: SGR_Proyectos_GuardarAsignacionUsuarioEncargado,
            SGR_Proyectos_ValidarEntidadDelegada: SGR_Proyectos_ValidarEntidadDelegada,
            SGR_Proyectos_LeerAsignacionUsuarioEncargado: SGR_Proyectos_LeerAsignacionUsuarioEncargado
        };

        /*Inicio - Usado para comunicar los componentes hijos*/
        var observadores = [];

        function registrarObservador(callback) {
            if (!observadores) {
                observadores = [];
            }
            observadores.push(callback);     
        }

        function notificarCambio(datos) {
            observadores.forEach(function (observador) {
                observador(datos);
            });
        }

        function limpiarObservadores() {
            observadores = [];
        }
        /*Fin - Usado para comunicar los componentes hijos*/

        //Servicios de delegar viabilidad
        function SGR_Proyectos_LeerEntidadesAdscritas(proyectoId, tipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_LeerEntidadesAdscritas + "?proyectoId=" + proyectoId + "&tipoEntidad=" + tipoEntidad;
            return $http.get(url);
        }

        function SGR_Proyectos_ActualizarEntidadAdscrita(proyectoId, entityId, delegado) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneSGR_Proyectos_ActualizarEntidadAdscrita}?proyectoId=${proyectoId}&entityId=${entityId}&delegado=${delegado}`;
            return $http.post(url);
        }

        function SGR_Proyectos_ValidarEntidadDelegada(proyectoId, tipo) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_ValidarEntidadDelegada + "?proyectoId=" + proyectoId + "&tipo=" + tipo;
            return $http.get(url);
        }

        //Servicios de asignar viabilidad      
        function ObtenerUsuariosEntidadAdscrita(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerUsuariosInvolucrados, parametros);
        }

        function SGR_Proyectos_GuardarAsignacionUsuarioEncargado(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_GuardarAsignacionUsuarioEncargado, parametros);
        }

        function SGR_Proyectos_LeerAsignacionUsuarioEncargado(proyectoId, instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_LeerAsignacionUsuarioEncargado + "?proyectoId=" + proyectoId + "&instanciaId=" + instanciaId;
            return $http.get(url);
        }
    }
})();

