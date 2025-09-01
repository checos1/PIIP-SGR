(function () {
    'use strict';

    datosProyectovfController.$inject = ['$scope', 'tramiteVigenciaFuturaServicio', '$sessionStorage', 'utilidades', 'constantesBackbone'];

    function datosProyectovfController(
        $scope,
        tramiteVigenciaFuturaServicio,
        $sessionStorage,
        utilidades,
        constantesBackbone
    ) {
        var vm = this;

        vm.BPIN = $sessionStorage.idObjetoNegocio;
       
        vm.disabled = true;
        vm.datosProyecto = {};
        vm.listaValores = [];
        vm.init = function () {
            $scope.$watch('vm.datosproyecto', function () {
                if (vm.datosproyecto !== "{}") {
                    //vm.obtenerDatosProyectoTramite(vm.tramiteid)
                    vm.datosProyecto = angular.fromJson(vm.datosproyecto);
                    vm.listaValores = vm.datosProyecto.listavalores;
                }
            });            
            
        };

        //vm.obtenerDatosProyectoTramite = function (tramiteId) {
        //    return tramiteVigenciaFuturaServicio.obtenerDatosProyectoTramite(tramiteId).then(
        //        //return gestionRecursosServicio.obtenerDatosGeneralesProyecto(ProyectoId, '88ea329d-f240-4868-9df7-86c74fb2ecfa').then(
        //        function (respuesta) {
        //            vm.datosProyecto = respuesta.data;

        //            var listaValores = [
        //                {
        //                    TipoRecurso: "Propios",
        //                    AprobacionInicial: new Intl.NumberFormat().format(vm.datosProyecto.ValorInicialPropios),
        //                    AprobacionVigente: new Intl.NumberFormat().format(vm.datosProyecto.ValorVigentePropios),
        //                    ValorDispinible: new Intl.NumberFormat().format(vm.datosProyecto.ValorDisponiblePropios),
        //                    VigenciasFuturas: new Intl.NumberFormat().format(vm.datosProyecto.ValorVigenciaFuturaPropios),

        //                },
        //                {
        //                    TipoRecurso: "Nación",
        //                    AprobacionInicial: new Intl.NumberFormat().format(vm.datosProyecto.ValorInicialNacion),
        //                    AprobacionVigente: new Intl.NumberFormat().format(vm.datosProyecto.ValorVigenteNacion),
        //                    ValorDispinible: new Intl.NumberFormat().format(vm.datosProyecto.ValorDisponibleNacion),
        //                    VigenciasFuturas: new Intl.NumberFormat().format(vm.datosProyecto.ValorVigenciaFuturaNacion),

        //                }
        //            ]

        //            vm.listaValores = listaValores;

        //        }
        //    );
        //}


      
    }

    angular.module('backbone').component('datosProyectovf', {
        templateUrl: "src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/asociarProyectovf/seleccionProyecto/datosProyectovf/datosProyectovf.html",
        controller: datosProyectovfController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            datosproyecto: '@'
            
        }
    });

})();