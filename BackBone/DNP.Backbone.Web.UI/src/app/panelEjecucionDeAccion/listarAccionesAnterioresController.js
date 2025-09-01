angular.module('backbone').controller('listarAccionesAnterioresController', listarAccionesAnterioresController);

listarAccionesAnterioresController.$inject = ["$scope", "$uibModalInstance", "$filter", "appSettings", "uiGridConstants",
    "utilidades", "listaacciones", "servicioAcciones", "idInstancia", "idAccion", "idAplicacion", "existeFichaGenerar"];

function listarAccionesAnterioresController($scope, $uibModalInstance, $filter, appSettings, uiGridConstants,
    utilidades, listaacciones, servicioAcciones, idInstancia, idAccion,
    idAplicacion, existeFichaGenerar) {
    var vm = this;

    vm.IdAccionDevolucion = {};
    vm.listaAccionesAnteriores = listaacciones;
    vm.cancelarSeleccion = cancelarSeleccion;

    function accionDevolver() {
        var parametrosDevolverFlujo = new Object();
        parametrosDevolverFlujo.IdInstanciaFlujo = idInstancia;
        parametrosDevolverFlujo.IdAccionOrigen = idAccion;
        parametrosDevolverFlujo.IdAccionDestino = vm.IdAccionDevolucion;
        parametrosDevolverFlujo.IdAplicacion = idAplicacion;

        servicioAcciones.validacionDevolucionPaso(idInstancia, idAccion, vm.IdAccionDevolucion).then(
            function (resultado) {
                if (resultado.data.length == 0)
                    devovlerFlujo(parametrosDevolverFlujo);
                else
                    utilidades.mensajeError(resultado.data);
            });
    }

    function devovlerFlujo(parametrosDevolverFlujo) {
        servicioAcciones.DevolverFlujo(parametrosDevolverFlujo).then(
            function (resultado) {
                if (resultado.data.FlujoDevuelto) {
                    window.location.reload();
                }
            });
    }

    $scope.AccionSeleccionada = function () {
        utilidades.mensajeWarning("El devolver, inhabilita la acción actual y le habilita la acción seleccionada", accionDevolver, $uibModalInstance.close);
    };

    function cancelarSeleccion() {
        $uibModalInstance.close();
    }
}