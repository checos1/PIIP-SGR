(function () {
    'use strict';

    angular.module('backbone').factory('requisitosSinTramiteSgpServicio', requisitosSinTramiteSgpServicio);

    requisitosSinTramiteSgpServicio.$inject = ['$http', 'constantesBackbone'];

    function requisitosSinTramiteSgpServicio($http, constantesBackbone) {

        return {
            
            obtenerPreguntasPersonalizadasComponente: obtenerPreguntasPersonalizadasComponente,            
            guardarRespuestas: guardarRespuestas,                                                
            obtenerErroresviabilidadSgp: obtenerErroresviabilidadSgp,
            ConsultarAccionPorInstancia: ConsultarAccionPorInstancia,
            obtenerPreguntasPersonalizadas: obtenerPreguntasPersonalizadas,
            DevolverCuestionarioProyecto: DevolverCuestionarioProyecto,            
            obtenerPreguntasPersonalizadasComponenteSGP: obtenerPreguntasPersonalizadasComponenteSGP,
            guardarRespuestasCustomSGP: guardarRespuestasCustomSGP,            
        };
       

        function obtenerPreguntasPersonalizadasComponente(bPin, nivelId, instanciaId, nombreComponente, listaRoles) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasPersonalizadasComponente;
            var params = {
                'bPin': bPin,
                'nivelId': nivelId,
                'instanciaId': instanciaId,
                'nombreComponente': nombreComponente,
                'listaRoles': listaRoles,
            };
            return $http.get(url, { params });
        }

              
        function guardarRespuestas(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarPreguntasPersonalizadas, parametros);
        }         

        function obtenerErroresviabilidadSgp(guiMacroproceso, idProyecto, idNivel, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function ConsultarAccionPorInstancia(instanciaId, idAccion) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneConsultarAccionPorInstancia}?instanciaId=${instanciaId}&idAccion=${idAccion}`;
            return $http.get(url);
        }

        function obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasPersonalizadasSGP;
            var params = {
                'bPin': bPin,
                'nivelId': nivelId,
                'instanciaId': instanciaId,
                'listaRoles': listaRoles,
            };
            return $http.get(url, { params });
        }

        function DevolverCuestionarioProyecto(nivelId, instanciaId, estadoAccionesPorInstancia) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneDevolverCuestionarioProyectoSGP}?nivelId=${nivelId}&instanciaId=${instanciaId}&estadoAccionesPorInstancia=${estadoAccionesPorInstancia}`;
            return $http.post(url);
        }

        function obtenerPreguntasPersonalizadasComponenteSGP(bPin, nivelId, instanciaId, nombreComponente, listaRoles) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasPersonalizadasComponenteSGP;
            var params = {
                'bPin': bPin,
                'nivelId': nivelId,
                'instanciaId': instanciaId,
                'nombreComponente': nombreComponente,
                'listaRoles': listaRoles,
            };
            return $http.get(url, { params });
        }

        function guardarRespuestasCustomSGP(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarPreguntasPersonalizadasCustomSGP, parametros);
        }

    }
})();
