(function () {
    'use strict';
    angular.module('backbone').factory('ctusIntegradoAsignacionSgrServicio', ctusIntegradoAsignacionSgrServicio);
    ctusIntegradoAsignacionSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function ctusIntegradoAsignacionSgrServicio($http, constantesBackbone) {

        return {
            registrarObservador: registrarObservador,
            limpiarObservadores: limpiarObservadores,
            notificarCambio: notificarCambio,
            SGR_Proyectos_LeerEntidadesAdscritas: SGR_Proyectos_LeerEntidadesAdscritas,
            SGR_Proyectos_ActualizarEntidadAdscritaCTUS: SGR_Proyectos_ActualizarEntidadAdscritaCTUS,
            ObtenerUsuariosEntidadAdscrita: ObtenerUsuariosEntidadAdscrita,           
            SGR_Proyectos_ValidarEntidadDelegada: SGR_Proyectos_ValidarEntidadDelegada,
            SGR_Proyectos_LeerAsignacionUsuarioEncargado: SGR_Proyectos_LeerAsignacionUsuarioEncargado,
            SGR_Proyectos_LeerProyectoCtus: SGR_Proyectos_LeerProyectoCtus,
            SGR_CTUS_GuardarAsignacionUsuarioEncargado: SGR_CTUS_GuardarAsignacionUsuarioEncargado
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

        function SGR_Proyectos_ActualizarEntidadAdscritaCTUS(proyectoId, entityId, tipo) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneSGR_CTUS_ActualizarEntidadAdscritaCTUS}?proyectoId=${proyectoId}&entityId=${entityId}&tipo=${tipo}`;
            return $http.post(url);
        }

        function SGR_Proyectos_ValidarEntidadDelegada(proyectoId, tipo) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_ValidarEntidadDelegada + "?proyectoId=" + proyectoId + "&tipo=" + tipo;
            return $http.get(url);
        }

        //Servicios compartidos
        function SGR_Proyectos_LeerProyectoCtus(proyectoId, instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_LeerProyectoCtus + "?proyectoId=" + proyectoId + "&instanciaId=" + instanciaId;
            return $http.get(url);
        }
        
        //Servicios de asignar viabilidad      
        function ObtenerUsuariosEntidadAdscrita(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerUsuariosInvolucrados, parametros);
        }

        function SGR_Proyectos_LeerAsignacionUsuarioEncargado(proyectoId, instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_LeerAsignacionUsuarioEncargado + "?proyectoId=" + proyectoId + "&instanciaId=" + instanciaId;
            return $http.get(url);
        }

        function SGR_CTUS_GuardarAsignacionUsuarioEncargado(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_CTUS_GuardarAsignacionUsuarioEncargado, data);
        }
    }
})();

