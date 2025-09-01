(function () {
    'use strict';

    angular.module('backbone').factory('designarEjecutorSgrServicio', designarEjecutorSgrServicio);

    designarEjecutorSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function designarEjecutorSgrServicio($http, constantesBackbone) {
        return {
            registrarObservador: registrarObservador,
            notificarCambio: notificarCambio,
            limpiarObservadores: limpiarObservadores,
            catalogoTodosTiposEntidades: catalogoTodosTiposEntidades,
            ObtenerEjecutorByTipoEntidad: ObtenerEjecutorByTipoEntidad,
            ObtenerEjecutores: ObtenerEjecutores,
            ObtenerEjecutoresAsociados: ObtenerEjecutoresAsociados,
            SGR_Procesos_ConsultarEjecutorbyTipo: SGR_Procesos_ConsultarEjecutorbyTipo,
            CrearEjecutorAsociado: CrearEjecutorAsociado,
            EliminarEjecutorAsociado: EliminarEjecutorAsociado,
            RegistrarRespuestaEjecutorSGR: RegistrarRespuestaEjecutorSGR,
            ObtenerRespuestaEjecutorSGR: ObtenerRespuestaEjecutorSGR,
            LeerValoresAprobacionSGR: LeerValoresAprobacionSGR,
            ActualizarValorEjecutorSGR: ActualizarValorEjecutorSGR,
            obtenerErroresProyecto: obtenerErroresProyecto,
            ObtenerValorCostosEstructuracionViabilidadSGR: ObtenerValorCostosEstructuracionViabilidadSGR
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

        function ActualizarValorEjecutorSGR(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Procesos_ActualizarValorEjecutorSGR, model);
        }

        function LeerValoresAprobacionSGR(proyectoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Procesos_LeerValoresAprobacionSGR + "?proyectoId=" + proyectoId;
            return $http.get(url);
        }

        function RegistrarRespuestaEjecutorSGR(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Procesos_RegistrarRespuestaEjecutorSGR, model);
        }

        function ObtenerRespuestaEjecutorSGR(campo, proyectoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Procesos_ObtenerRespuestaEjecutorSGR + "?campo=" + campo + "&proyectoId=" + proyectoId;
            return $http.get(url);
        }

        function ObtenerValorCostosEstructuracionViabilidadSGR(instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Procesos_ObtenerValorCostosEstructuracionViabilidadSGR + "?instanciaId=" + instanciaId;
            return $http.get(url);
        }

        function catalogoTodosTiposEntidades() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCatalogoTodosTipoEntidades;
            return $http.get(url);
        }

        function ObtenerEjecutorByTipoEntidad(idTipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEjecutorByTipoEntidad + "?idTipoEntidad=" + idTipoEntidad;
            return $http.get(url);
        }

        function ObtenerEjecutores(nit, tipoEntidadId, entidadId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEjecutores + "?nit=" + nit + "&tipoEntidadId=" + tipoEntidadId + "&entidadId=" + entidadId;
            return $http.get(url);
        }

        function ObtenerEjecutoresAsociados(proyectoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEjecutoresAsociados + "?proyectoId=" + proyectoId;
            return $http.get(url);
        }
        function SGR_Procesos_ConsultarEjecutorbyTipo(proyectoId, tipoEjecutorId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Procesos_ConsultarEjecutorbyTipo + "?proyectoId=" + proyectoId + "&tipoEjecutorId=" + tipoEjecutorId;
            return $http.get(url);
        }

        function CrearEjecutorAsociado(proyectoId, ejecutorId, tipoEjecutorId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCrearEjecutorAsociado + "?proyectoId=" + proyectoId + "&ejecutorId=" + ejecutorId + "&tipoEjecutorId=" + tipoEjecutorId;
            return $http.post(url);
        }

        function EliminarEjecutorAsociado(EjecutorAsociadoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarEjecutorAsociado + "?EjecutorAsociadoId=" + EjecutorAsociadoId;
            return $http.post(url);
        }

        function obtenerErroresProyecto(guiMacroproceso, idProyecto, idNivel, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }
    }

})();