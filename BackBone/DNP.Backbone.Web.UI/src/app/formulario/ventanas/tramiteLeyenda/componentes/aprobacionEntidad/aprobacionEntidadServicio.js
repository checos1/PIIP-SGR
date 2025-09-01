(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('aprobacionEntidadServicio', aprobacionEntidadServicio);

    aprobacionEntidadServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function aprobacionEntidadServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerPreguntasAprobacionEntidad: obtenerPreguntasAprobacionEntidad,
            guardarRespuestasAprobacionRolPresupuesto: guardarRespuestasAprobacionRolPresupuesto,
            ObtenerSeccionCapitulo: ObtenerSeccionCapitulo,
            obtenerErroresProyecto: obtenerErroresProyecto,
            guardarCambiosFirme: guardarCambiosFirme


        };
      
        function obtenerPreguntasAprobacionEntidad(idTramite, proyectoId, tipoTramiteId, tipoRolId, idNivel) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerPreguntasAprobacionEntidad;
            var params = {
                'TramiteId': idTramite,
                'ProyectoId': proyectoId,
                'TipoTramiteId': tipoTramiteId === null ? 0 : tipoTramiteId,
                'TipoRolId': tipoRolId,
                'IdNivel': idNivel
            };
            return $http.get(url, { params });
        }

        function guardarRespuestasAprobacionRolPresupuesto(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarPreguntasAprobacionEntidadPresupuesto, parametros);
        }
       
        function ObtenerSeccionCapitulo(FaseGuid, Capitulo, Seccion, idUsuario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerSeccionCapitulo;
            url += "?FaseGuid=" + FaseGuid;
            url += "&Capitulo=" + Capitulo;
            url += "&Seccion=" + Seccion;
            url += "&idUsuario=" + idUsuario;
            return $http.get(url);
        }

        function ObtenerSeccionCapitulo(FaseGuid, Capitulo, Seccion, idUsuario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerSeccionCapitulo;
            url += "?FaseGuid=" + FaseGuid;
            url += "&Capitulo=" + Capitulo;
            url += "&Seccion=" + Seccion;
            url += "&idUsuario=" + idUsuario;
            return $http.get(url);
        }

        function obtenerErroresProyecto(guiMacroproceso, idProyecto, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function guardarCambiosFirme(data) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarCambiosFirmeRelacionPlanificacion;
            return $http.post(url, data);
        }
    }
})();