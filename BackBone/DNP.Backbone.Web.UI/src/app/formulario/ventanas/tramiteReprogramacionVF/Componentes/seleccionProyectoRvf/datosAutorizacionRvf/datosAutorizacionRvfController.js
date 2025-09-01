(function () {
    'use strict';

    datosAutorizacionRvfController.$inject = ['$scope', 'tramiteReprogramacionVfServicio', '$sessionStorage', 'utilidades', 'constantesBackbone'];

    function datosAutorizacionRvfController(
        $scope,
        tramiteReprogramacionVfServicio,
        $sessionStorage,
        utilidades,
        constantesBackbone
    ) {
        var vm = this;

        vm.BPIN = $sessionStorage.idObjetoNegocio;

        vm.disabled = true;
        vm.datosProyecto = {};
        vm.listaValores = {};
        vm.eliminarAsociacionA = eliminarAsociacionA;
        vm.habilitaBotones = $sessionStorage.nombreAccion.includes($sessionStorage.listadoAccionesTramite[0].Nombre) && !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1
        vm.init = init;

        function init() {

            $scope.$watch('vm.datosreprogramacion', function () {
                if (vm.datosreprogramacion !== "{}") {
                    //vm.obtenerDatosProyectoTramite(vm.tramiteid)                   
                    vm.datosReprogramacion = angular.fromJson(vm.datosreprogramacion);
                    vm.listaValores = vm.datosReprogramacion;
                    vm.listaValores.FechaAutorizacion = vm.listaValores.FechaAutorizacion.substring(8, 10) + '-' + vm.listaValores.FechaAutorizacion.substring(5, 7) + '-' + vm.listaValores.FechaAutorizacion.substring(0, 4);
                    $sessionStorage.tieneautorizacionasociada = true;
                    vm.autorizacionasociada = true;

                }
                else {
                    //vm.listaValores = {};
                    vm.autorizacionasociada = false;
                    $sessionStorage.tieneautorizacionasociada = false;
                }
            });

        };



        function eliminarAsociacionA() {
            if (vm.listaValores.Id === undefined || vm.listaValores.Id === '' || vm.listaValores.Id === 0) {
                utilidades.mensajeError('No hay autorizaciones asociadas.');

            }
            else {
                utilidades.mensajeWarning("Si realiza esta acción, deberá seleccionar una nueva autorización para crear el ajuste. ¿Esta seguro de continuar?",
                    function funcionContinuar() {
                        var reprogramacionDto = {
                            Id: vm.listaValores.ReprogramacionId,
                            AutorizacionVigenciasFuturasId: vm.listaValores.Id,
                        };
                        tramiteReprogramacionVfServicio.EliminaReprogramacionVF(reprogramacionDto)
                            //tramiteVigenciaFuturaServicio.eliminarAsociacion(eliminarAsociacionDto)
                            .then(function (response) {
                                if (response.status == "200") {
                                    $sessionStorage.proyectoId = 'e';
                                    utilidades.mensajeSuccess("", false, false, false, "La autorización del proyecto fue desasociada con éxito.");
                                    vm.listaValores = {};
                                    $sessionStorage.tieneautorizacionasociada = false;
                                    vm.autorizacionasociada = false;
                                    //para guardar los capitulos modificados y que se llenen las lunas, este modificado en 0
                                    //ObtenerSeccionCapitulo();
                                    //eliminarCapitulosModificados(vm.seccionCapituloAsociarid);
                                    //eliminarCapitulosModificados(vm.seccionCapituloCrear);

                                    //$sessionStorage.EstadoAjusteCreado = false;
                                    //vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                                    //$sessionStorage.EstadoAjusteFinalizado = false;
                                    //vm.estadoAjusteFinalizado = $sessionStorage.EstadoAjusteFinalizado;

                                    ///Ajuste Estabilizacion
                                    // $sessionStorage.proyectoId = 0;
                                    //$sessionStorage.BPIN = '';
                                    //$sessionStorage.nombreProyecto = '';
                                    //vm.BPIN = $sessionStorage.BPIN;
                                    //vm.proyecto = $sessionStorage.nombreProyecto;
                                }
                                else {
                                    //utilidades.mensajeError("Error al realizar la operación", false);
                                }
                            })
                            .catch(error => {
                                if (error.status == "400") {
                                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                                    return;
                                }
                                utilidades.mensajeError("Error al realizar la operación");
                            });
                    },
                    function funcionCancelar(reason) {
                        console.log("reason", reason);
                    },
                    "Aceptar",
                    "Cancelar",
                    "La autorización del proyecto será desasociada."
                )
            }
        };



    }

    angular.module('backbone').component('datosAutorizacionRvf', {
        templateUrl: "src/app/formulario/ventanas/tramiteReprogramacionVF/componentes/seleccionProyectoRvf/datosAutorizacionRvf/datosAutorizacionRvf.html",
        controller: datosAutorizacionRvfController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            datosreprogramacion: '@',
            autorizacionasociada: '='

        }
    });

})();