(function () {
    'use strict';

    justificacionController.$inject = ['$scope', 'requerimientosTramitesServicio', '$sessionStorage', 'utilidades', 'constantesBackbone'];

    function justificacionController(
        $scope,
        requerimientosTramitesServicio,
        $sessionStorage,
        utilidades,
        constantesBackbone
    ) {
        var vm = this;
        vm.Justificaciones = [{
            TramiteId: 0,
            ProyectoId: 0,
            JustificacionId: 0,
            JustificacionPregunta: "",
            JustificacionRespuesta: null,
            TipoJustificacion: "",
            TipoRolId: 0
        }];
        vm.noJefePlaneacion = !$sessionStorage.jefePlaneacion;
        vm.disabledJefePlaneacion = $sessionStorage.jefePlaneacion;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.elaboracionConcepto = false;
        vm.aprobacionEntidad = false;

        vm.tipoJustificacion = "";

        vm.obtener = function () {
            var idTramite = $sessionStorage.TramiteId;
            var proyectoId = $sessionStorage.ProyectoId;
            var tipoTramiteId = $sessionStorage.TipoTramiteId;
            var tipoRolId = $sessionStorage.TipoRolId;
            vm.tipoJustificacion = "Justificación Técnico - Económica - Legal del trámite";

            //obtenerPreguntas(26,97652,4,1);
            obtenerPreguntas(idTramite, 0, tipoTramiteId, tipoRolId);
            if (vm.IdNivel.toLowerCase() == constantesBackbone.idNivelElaboracionConcepto.toLowerCase())
                vm.elaboracionConcepto = true;
            else if (vm.IdNivel.toLowerCase() == constantesBackbone.idNivelAprobacionEntidad.toLowerCase())
                vm.aprobacionEntidad = true;
        };


        function obtenerPreguntas(idTramite, proyectoId, tipoTramiteId, tipoRolId) {
            return requerimientosTramitesServicio.obtenerPreguntasJustificacion(idTramite, 0, tipoTramiteId, tipoRolId, vm.IdNivel).then(
                function (respuesta) {

                    vm.Justificaciones = respuesta.data;
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
            if (cont === 0) {
                for (const [key, value] of Object.entries(respuestas)) {
                    for (const element of vm.Justificaciones) {
                        if (element.JustificacionPreguntaId == key) {
                            element.JustificacionRespuesta = value;
                        }
                        element.NivelId = vm.IdNivel;
                        element.InstanciaId = $sessionStorage.idInstanciaIframe;
                        element.ProyectoId = null;
                    }
                }

                return requerimientosTramitesServicio.guardarRespuestasJustificacion(vm.Justificaciones).then(
                    function (response) {
                        //console.log(response);
                        if (response.data && (response.statusText === "OK" || response.status === 200)) {

                            if (response.data.Exito) {
                                parent.postMessage("cerrarModal", window.location.origin);
                                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            } else {
                                swal('', response.data.Mensaje, 'warning');
                            }

                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
                    }

                );

            } else return;
        }

    }

    angular.module('backbone').component('justificacion', {
        templateUrl: "src/app/formulario/ventanas/tramites/componentes/justificacion/justificacion.html",
        controller: justificacionController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });

})();