(function () {
    'use strict';

    justificacionProyectoController.$inject = ['$scope', 'requerimientosTramitesServicio', '$sessionStorage'];

    function justificacionProyectoController(
        $scope,
        requerimientosTramitesServicio,
        $sessionStorage
    ) {
        var vm = this;
        vm.JustificacionTematica = [{
            Tematica: "",
            OrdenTematica: 0,
            justificaciones: [{
                TramiteId: 0,
                ProyectoId: 0,
                JustificacionId: 0,
                JustificacionPreguntaId: 0,
                OrdenJustificacionPregunta: 0,
                JustificacionPregunta: "",
                JustificacionRespuesta: null,
                ObservacionPregunta: "",
                OpcionesRespuesta: "",
                ObservacionRespuesta: "",
                Tematica: "",
                OrdenTematica: 0,
                NombreRol: "",
                NombreNivel: "",
                CuestionarioId: 0,
                TipoJustificacion: "",
                TipoRolId: 0
            }]
        }];


        vm.tipoJustificacion = "";

        vm.obtener = function () {
            var idTramite = $sessionStorage.TramiteId;
            var proyectoId = $sessionStorage.ProyectoId;
            var tipoTramiteId = $sessionStorage.TipoTramiteId;
            var idNivel = $sessionStorage.idNivel;
            var tipoRolId = $sessionStorage.TipoRolId;
            var TipoProyecto = $sessionStorage.TipoProyecto;
            vm.tipoJustificacion = "Justificacion del " + TipoProyecto;

            //obtenerPreguntas(26,97652,4,1);
            obtenerPreguntas(idTramite, proyectoId, tipoTramiteId, idNivel, tipoRolId);

        };


        function obtenerPreguntas(idTramite, proyectoId, tipoTramiteId, idNivel, tipoRolId) {
            var proyectoidtmp = proyectoId === undefined ? 0 : proyectoId;
            return requerimientosTramitesServicio.ObtenerPreguntasProyectoActualizacion(idTramite, proyectoidtmp, tipoTramiteId, idNivel, tipoRolId).then(
                function (respuesta) {
                    vm.JustificacionTematica = respuesta.data;
                }
            );
        }

        vm.registrorespuesta = function (response) {
            if (response == undefined) {
                return;
            }
            const respuestas = response.JustificacionRespuesta;
            var cont = 0;
            for (const [key, value] of Object.entries(respuestas)) {
                if (value == undefined) cont++;
            }
            if (cont == 0) {
                for (const [key, value] of Object.entries(respuestas)) {
                    for (const element of vm.Justificaciones) {
                        if (element.JustificacionId == key) {
                            element.JustificacionRespuesta = value;
                        }
                    }
                }

                return requerimientosTramitesServicio.guardarRespuestasJustificacion(vm.Justificaciones).then(
                    function (response) {
                        //console.log(response);
                    }

                );

            } else return;
        };

    }

    angular.module('backbone').component('justificacionProyecto', {
        templateUrl: "src/app/formulario/ventanas/tramites/componentes/justificacion/justificacionProyecto.html",
        controller: justificacionProyectoController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });

})();