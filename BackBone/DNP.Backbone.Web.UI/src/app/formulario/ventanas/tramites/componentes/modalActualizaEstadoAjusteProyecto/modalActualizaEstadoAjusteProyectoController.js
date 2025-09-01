(function () {
    'use strict';

    modalActualizaEstadoAjusteProyectoController.$inject = [
        '$sessionStorage',
        '$uibModalInstance',
        'utilidades',
        'modalActualizaEstadoAjusteProyectoServicio'
    ];

    function modalActualizaEstadoAjusteProyectoController(
        $uibModalInstance,
        $sessionStorage,
        utilidades,
        modalActualizaEstadoAjusteProyectoServicio
    ) {
        var vm = this;
        vm.init = init;
        vm.ProyectoId = $uibModalInstance.ProyectoId;
        vm.TramiteId = $uibModalInstance.TramiteId;
        vm.BPIN = $uibModalInstance.BPIN;

        vm.cerrar = $sessionStorage.close;
        vm.guardar = guardar;
       

        


        function init() {
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
           
        }

       

        function guardar() {
            let tipoDevolucion = '';
            if (vm.model.devolucion !== "0" && vm.model.devolucion !== "1") {
                utilidades.mensajeError("Seleccione el tipo de devolución.", false); return false;
            }
            else
            {
                tipoDevolucion = vm.model.devolucion;
            }
            if (vm.model.observacion === undefined ) {
                utilidades.mensajeError("Incluir la observación", false); return false;
            }

            modalActualizaEstadoAjusteProyectoServicio.actualizaEstadoAjusteProyecto(tipoDevolucion, vm.BPIN, vm.TramiteId, vm.model.observacion)
                .then(function (response) {
                    let exito = response.data;
                    if (exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        $sessionStorage.close();
                    }
                    else {
                        utilidades.mensajeError("Error al realizar la operación", false);
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });


            
        }

      

    }


    angular.module('backbone').controller('modalActualizaEstadoAjusteProyectoController', modalActualizaEstadoAjusteProyectoController);

})();
