(function () {
    'use strict';

    cronogramavfController.$inject = [
        '$sessionStorage',
        'sesionServicios',
        'utilidades',
        'tramiteVigenciaFuturaServicio'
    ];

    function cronogramavfController(
        $sessionStorage,
        sesionServicios,
        utilidades,
        tramiteVigenciaFuturaServicio

    ) {
        var vm = this;
        vm.lang = "es";

        var listaPrecontractual = [];
        var listaContractual = [];
        vm.listaPrecontractual;
        vm.listaContractual;


        vm.TramiteId = $sessionStorage.tramiteId;
        vm.usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.instanciaId = $sessionStorage.idInstancia;


        vm.init = function init() {

            vm.obtenerDatosCronograma(vm.instanciaId);

            // var pruebaJson = '{"InstanciaId": "3E0750D4-DC36-4546-942F-72F8638B3E0A",'
            //+' "Actividades": [{"ActividadesCronogramaId": 38, "TramiteProyectoId": 12345, "NombreActividad": "Actividad precontractual de prueba numero 1",'
            // + ' "ActividadesPreContractualesId": 1, "FechaInicial": "09/2021", "FechaFinal": "10/2022" }]}'
            // var datosCronogramas = jQuery.parseJSON(pruebaJson);
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

                        for (var ls = 0; ls < arregloDatosValores.length; ls++) {

                            //var fechaInicialDate = new Date(Date.parse(arregloDatosValores[ls].FechaInicial));
                            //var fechaFinalDate = new Date(Date.parse(arregloDatosValores[ls].FechaFinal));
                            //var fechaInicialmy = (fechaInicialDate.getMonth() + 1) + "/" + (fechaInicialDate.getFullYear());
                            //var fechaFinalmy = (fechaFinalDate.getMonth() + 1) + "/" + (fechaFinalDate.getFullYear());

                            if (arregloDatosValores[ls].ActividadesPreContractualesId == null) {

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
                            else {

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

    angular.module('backbone').component('cronogramavf', {
        templateUrl: "src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/informacionPresupuestal/cronogramavf/cronogramavf.html",
        controller: cronogramavfController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&'

        }
    });

})();
