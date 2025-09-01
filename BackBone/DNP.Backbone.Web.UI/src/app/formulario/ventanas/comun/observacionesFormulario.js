(function () {
    'use strict';

    observacionesFormulario.$inject = [
        'sesionServicios',
        '$uibModal',
        '$scope',
        'utilidades',
        '$sessionStorage',
        'constantesBackbone',
        'trasladosServicio',
        'servicioAcciones',
        '$interval',
        '$http'
    ];

    function observacionesFormulario(
        sesionServicios,
        $uibModal,
        $scope,
        utilidades,
        $sessionStorage,
        constantesBackbone,
        trasladosServicio,
        servicioAcciones,
        $interval,
        $http
    ) {
        var vm = this;
        vm.ActualizarObservacion = ActualizarObservacion;
        vm.registrarObservacion = registrarObservacion;
        vm.AccionDiferente = false;
        vm.disabled = true;
        vm.Editar = 'EDITAR';
        vm.VerEditarObervacion = true;


        $scope.$watch('vm.modelo', function () {
            vm.VerEditarObervacion = true;
            if ($sessionStorage.soloLectura) {
                vm.VerEditarObervacion = false;
            }
            if ($sessionStorage.BanderaDisabledEditarSGP) {
                vm.VerEditarObervacion = false;
            }

            if ($sessionStorage.idInstanciaIframe != undefined && $sessionStorage.idAccion != undefined) {
                trasladosServicio.obtenerObservacionesPasoPadre($sessionStorage.idInstanciaIframe, $sessionStorage.idAccion).then(
                    function (respuesta) {
                        if (respuesta.statusText == 'OK' || respuesta.status == '200') {
                            $sessionStorage.idAccionAnterior = respuesta.data.AccionId == '00000000-0000-0000-0000-000000000000' ? $sessionStorage.idAccion : respuesta.data.AccionId;
                            if ($sessionStorage.idAccion == $sessionStorage.idAccionAnterior) {
                                vm.observacion = respuesta.data.Observacion;
                                vm.observacionTemp = vm.observacion;
                                vm.AccionDiferente = false;
                                vm.ActualizarObservacion();
                                vm.modelo.tituloObservacionAnterior = '';
                            } else {                                
                                var listadoAcciones = $sessionStorage.listadoAccionesTramite;

                                vm.observacionAnterior = respuesta.data.UltimaObservacion;
                                vm.AccionDiferente = true;
                                vm.observacion = $sessionStorage.soloLectura ? respuesta.data.Observacion:'';
                                vm.ActualizarObservacion();
                                if (listadoAcciones != undefined) {
                                    var accion = listadoAcciones.find(x => x.Id == $sessionStorage.idAccionAnterior);
                                    vm.modelo.tituloObservacionAnterior = 'Observación ' + accion.Nombre;
                                }
                                if (vm.modelo.tituloObservacionAnterior == '' || vm.modelo.tituloObservacionAnterior == undefined)
                                    vm.modelo.tituloObservacionAnterior = 'Observación Paso Anterior';
                                if (vm.modelo.tituloObservacionAnterior == vm.modelo.tituloObservacion)
                                    vm.modelo.tituloObservacionAnterior = 'Observación Devolución';
                            }
                        }
                    });
            }
            else
                vm.observacion = '';
        });
        

        //$interval(ActualizarObservacion, 1000);

        function ActualizarObservacion() {
            vm.modelo.textoObservacionEnvio = vm.observacion;
        }

        function registrarObservacion() {
            if (vm.modelo.textoObservacionEnvio == undefined) {
                utilidades.mensajeError('Debe diligenciar las observaciones.');
            }
            else {
                if (vm.modelo.textoObservacionEnvio.length > 0) {
                    var parametrosEjecucionFlujo = new Object();
                    parametrosEjecucionFlujo.InstanciaId = $sessionStorage.idInstanciaFlujoPrincipal;
                    parametrosEjecucionFlujo.AccionId = $sessionStorage.idAccion;
                    parametrosEjecucionFlujo.Observacion = vm.modelo.textoObservacionEnvio;

                    $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiFlujosCrearTrazaAccionesPorInstancia, parametrosEjecucionFlujo).then(
                        function (respuesta) {
                            vm.observacionTemp = vm.observacion;
                            vm.ActivarEditar();                            
                            utilidades.mensajeSuccess("Observación guardada satisfactoriamente", false, false, false);
                        }
                    );
                }
                else {
                    utilidades.mensajeError('Debe diligenciar las observaciones de envío.');
                }
            }
        }

        vm.ActivarEditar = function () {
            if (vm.disabled == true) {
                vm.Editar = "CANCELAR";
                vm.disabled = false;
            }
            else {
                vm.Editar = "EDITAR";
                vm.disabled = true;
                vm.observacion = vm.observacionTemp;
                vm.ActualizarObservacion();
            }
        }


    }


    angular.module('backbone').controller('observacionesFormulario', observacionesFormulario);
    angular.module('backbone').component('observacionesFormulario', {
        restrict: 'E',
        transclude: true,
        bindings: {
            modelo: '='
        },
        templateUrl: "src/app/formulario/ventanas/comun/observacionesFormulario.html",
        controller: observacionesFormulario,
        controllerAs: "vm"
    });


})();