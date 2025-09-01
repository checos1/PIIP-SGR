(function () {
    'use strict';

    cronogramaController.$inject = [
        '$sessionStorage',
        'utilidades',
        'tramiteVigenciaFuturaServicio'
    ];

    function cronogramaController(
        $sessionStorage,
        utilidades,
        tramiteVigenciaFuturaServicio

    ) {
        var vm = this;
        vm.lang = "es";

        var listaPrecontractual = [];
        var listaContractual = [];
        vm.listaPrecontractual;
        vm.listaContractual;
        vm.precontractual = false;
        vm.contractual = false;

        vm.TramiteId = $sessionStorage.tramiteId;
        vm.usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.instanciaId = $sessionStorage.idInstancia;

       

        vm.init = function init() {
            vm.precontractual = vm.mostrar === "ambos" || vm.mostrar ==="precontractual" ? true : false;
            vm.contractual = vm.mostrar === "ambos" || vm.mostrar === "contractual" ? true : false;
            vm.obtenerDatosCronograma(vm.instanciaId);
        }


        vm.obtenerDatosCronograma = function (instanciaId) {

            return tramiteVigenciaFuturaServicio.obtenerDatosCronograma(instanciaId).then(
                function (respuesta) {

                    listaPrecontractual = [];
                    listaContractual = [];

                    if (respuesta.data != "null" && respuesta.data != "") {

                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        var arregloGeneral = jQuery.parseJSON(arreglolistas)
                        var arregloDatosValores = arregloGeneral.Actividades;
                        vm.totalActividad = arregloDatosValores.length;
                        for (var ls = 0; ls < arregloDatosValores.length; ls++) {

                            if (arregloDatosValores[ls].ActividadesPreContractualesId == null && vm.contractual) {

                                var contractual = {
                                    ActividadesCronogramaId: arregloDatosValores[ls].ActividadesCronogramaId,
                                    TramiteProyectoId: arregloDatosValores[ls].TramiteProyectoId,
                                    NombreActividad: arregloDatosValores[ls].NombreActividad,
                                    ActividadesPreContractualesId: arregloDatosValores[ls].ActividadesPreContractualesId,
                                    FechaInicial: arregloDatosValores[ls].FechaInicialmy,//arregloDatosValores[ls].FechaInicial,
                                    FechaFinal: arregloDatosValores[ls].FechaFinalmy,//arregloDatosValores[ls].FechaInicial
                                };
                                listaContractual.push(contractual);

                            }
                            else if (vm.precontractual){

                                var precontractual = {
                                    ActividadesCronogramaId: arregloDatosValores[ls].ActividadesCronogramaId,
                                    TramiteProyectoId: arregloDatosValores[ls].TramiteProyectoId,
                                    NombreActividad: arregloDatosValores[ls].NombreActividad,
                                    ActividadesPreContractualesId: arregloDatosValores[ls].ActividadesPreContractualesId,
                                    FechaInicial: arregloDatosValores[ls].FechaInicialmy,//arregloDatosValores[ls].FechaInicial,
                                    FechaFinal: arregloDatosValores[ls].FechaFinalmy,//arregloDatosValores[ls].FechaInicial
                                };
                                listaPrecontractual.push(precontractual);
                            }

                        }

                    }
                    vm.listaPrecontractual = listaPrecontractual;
                    vm.listaContractual = listaContractual;
                    
                }
            );
        };



        vm.abrirTooltipPrec = function () {
            utilidades.mensajeInformacion('Visualización del cronograma de actividades con fechas estimadas para la etapa precontractual'
                + ' del proyecto asociado al trámite de VFO-PLN.'
                , false, "Información presupuestal")
        }

        vm.abrirTooltipCont = function () {
            utilidades.mensajeInformacion('Visualización del cronograma de actividades con fechas estimadas para la etapa contractual'
                + ' del proyecto asociado al trámite de VFO-PLN.'
                , false, "Información presupuestal")
        }


    }

    angular.module('backbone').component('cronograma', {
        templateUrl: "src/app/formulario/ventanas/comun/cronograma/cronograma.html",
        controller: cronogramaController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            mostrar: '@',
          
        }
    });

})();
