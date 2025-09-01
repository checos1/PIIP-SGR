(function () {
    'use strict';
    angular.module('backbone').factory('politicasTransversalesFormularioServicio', politicasTransversalesFormularioServicio);

    politicasTransversalesFormularioServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

   

    function politicasTransversalesFormularioServicio($q, $http, $location, constantesBackbone) {
        return {            
            consultarPoliticasTransversalesProgramacion: consultarPoliticasTransversalesProgramacion,
            eliminarPoliticasProyectoProgramacion: eliminarPoliticasProyectoProgramacion            

        };

        //Politicas Transversales
        function consultarPoliticasTransversalesProgramacion(Bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarPoliticasTransversalesProgramacion + "?bpin=" + Bpin;
            return $http.get(url);
        }   
        function eliminarPoliticasProyectoProgramacion(proyectoId, politicaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneEliminarPoliticasProyectoProgramacion + "?proyectoId=" + proyectoId + "&politicaId=" + politicaId;
            return $http.post(url);
        }
        
    }
})();