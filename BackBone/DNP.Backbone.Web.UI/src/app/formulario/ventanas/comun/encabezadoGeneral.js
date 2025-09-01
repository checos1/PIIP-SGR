(function() {
    'use strict';
    angular.module('backbone').directive('encabezadoGeneral', encabezadoGeneral);
    encabezadoGeneralController.$inject = [
        'sesionServicios',
        '$uibModal',
        '$scope',
        'utilidades',
        '$sessionStorage',
        'constantesBackbone',
        'trasladosServicio',
        'servicioAcciones',
        'modalPNDServicio'
    ];

    function encabezadoGeneralController(
        sesionServicios,
        $uibModal,
        $scope,
        utilidades,
        $sessionStorage,
        constantesBackbone,
        trasladosServicio,
        servicioAcciones,
        modalPNDServicio
    ) {
        var vm = this;
        vm.abriPND = abriPND;
        vm.lang = "es";
        vm.CodigoProceso = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.CodigoProceso : $sessionStorage.numeroTramite;
        vm.Fecha = $sessionStorage.InstanciaSeleccionada != undefined ? moment($sessionStorage.InstanciaSeleccionada.FechaCreacion).format("DD-MM-YYYY") : moment(new Date()).format("DD-MM-YYYY");
        vm.ProyectoId = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.ProyectoId : 0;
        vm.CodBPIN = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.IdObjetoNegocio : "";
        
        vm.modelo = {
            CodigoProceso: "",
            Fecha: "",
            Tipo: "",
            proyectoId: 0,
            nombreProyecto: "",
            codBPIN: "",
            vigenciaInicial: 0,
            vigenciaFinal: 0,
            entidad: "",
            estado: "",
            horizonte: "",
            sector: "",
            valorTotal: 0.0,
            apropiacionInicial: 0.0,
            apropiacionVigente: 0.0,
            Alcanceproyecto: 0.0,
            CostoTotalProyecto: 0.0,
            Ejecutor: "",
            FechaRealInicio: "",
            PeriodoAbierto: "",
            FechaInicioReporte: "",
            FechaLimiteReporte: ""
        }

        cargarNuevo();

        function cargarNuevo() {
            vm.etapa = $sessionStorage.etapa;
            vm.mostrarLinea1 = true;
            vm.mostrarLinea2 = true;
            vm.mostrarLinea3 = true;
            vm.parametros = {};
            vm.bpin = $sessionStorage.idObjetoNegocio;
            if ($sessionStorage.numeroTramite == undefined || $sessionStorage.numeroTramite == "") {
                if (vm.bpin.split('-')[0] == "EJ") {
                    vm.tramiteId = vm.bpin;
                    vm.bpin = "";
                } else {
                    vm.bpin = $sessionStorage.idObjetoNegocio;
                    vm.tramiteId = "";
                }
            } else {
                vm.bpin = "";
                vm.tramiteId = $sessionStorage.idObjetoNegocio;
                if (vm.etapa == undefined && vm.tramiteId.split('-')[0] == "EJ") { vm.etapa = 'ej'}
            }
            vm.idNivel = getIdEtapa();
            vm.idInstancia = $sessionStorage.idInstancia;
            vm.idAccion = $sessionStorage.idAccion;
            vm.idFlujo = $sessionStorage.idFlujoIframe;

            consultarEncabezado();
            modalPNDServicio.obtenerPlanNacionalDesarrollo(vm.ProyectoId).then(function (resultado) {
                vm.PND = resultado.data;
            });
        }

        //vm.mostrarOcultar = function (ProyectoId) {

        //    $("#panelAlcanceProyecto" + vm.ProyectoId).toggleClass('hidden');
        //    $("#panelAlcanceProyecto" + vm.ProyectoId).toggleClass('hidden');
        //}

        function consultarEncabezado() {
            vm.parametros.idInstancia = vm.idInstancia;
            vm.parametros.idFlujo = vm.idFlujo;
            vm.parametros.idNivel = vm.idNivel;
            vm.parametros.idProyectoStr = vm.ProyectoId;
            vm.parametros.tramite = vm.tramiteId;

            if (vm.idFlujo == undefined) {
                getIdFlujo();
            } else {
                if (vm.bpin == "" && vm.tramiteId == "") {
                    getIdFlujo();
                } else {

                    return trasladosServicio.ObtenerEncabezado(vm.parametros).then(
                        function(res) {
                            vm.CodigoProceso = vm.parametros.tramite;
                            $sessionStorage.idProyectoEncabezado = $sessionStorage.proyectoId;
                            //if (res.data.CodigoProceso != $sessionStorage.numeroTramite)
                            //    cargarNuevo();
                            //else {
                            if (res.data != null && res.data.TipoId != null) {
                                vm.modelo = res.data;
                                vm.Fecha = vm.modelo.Fecha != null ? moment(vm.modelo.Fecha).format("DD-MM-YYYY") : moment($sessionStorage.InstanciaSeleccionada.FechaCreacion).format("DD-MM-YYYY");
                                vm.CodigoProceso = (vm.modelo.CodigoProceso == null || vm.modelo.CodigoProceso == '' || vm.modelo.CodigoProceso == undefined) ? vm.CodigoProceso : vm.modelo.CodigoProceso;
                                //vm.FechaRealInicio = moment(vm.modelo.FechaRealInicio).format("DD-MM-YYYY");
                                vm.FechaRealInicio = vm.modelo.FechaRealInicio != null ? moment(vm.modelo.FechaRealInicio).format("DD-MM-YYYY") : "";
                                vm.PeriodoAbierto = vm.modelo.PeriodoAbierto != null ? vm.modelo.PeriodoAbierto: "";
                                vm.FechaInicioReporte = vm.modelo.FechaInicioReporte != null ? vm.modelo.FechaInicioReporte : "";
                               // vm.FechaLimiteReporte = vm.modelo.FechaLimiteReporte.substring(0, vm.modelo.FechaLimiteReporte.trim().length - 1) != null ? vm.modelo.FechaLimiteReporte.substring(0, vm.modelo.FechaLimiteReporte.trim().length - 1): "";
                                vm.FechaLimiteReporte = vm.modelo.FechaLimiteReporte != null ? vm.modelo.FechaLimiteReporte.substring(0, vm.modelo.FechaLimiteReporte.trim().length - 1) : "";


                                $sessionStorage.tipoTramiteId = res.data.TipoId;
                                $sessionStorage.TramiteId = res.data.TramiteId;
                                $sessionStorage.valorTotalProyectoEncabezado = res.data.valorTotal;
                                
                                $sessionStorage.IdTipoTramitePresupuestal = res.data.IdTipoTramitePresupuestal;
                                if (vm.etapa == "pl" || vm.etapa == "ej" || vm.etapa == "ev") {
                                    vm.mostrarLinea1 = true;
                                    vm.mostrarLinea2 = true;
                                    vm.mostrarLinea3 = true;
                                }
                                if (vm.etapa == "gr") {
                                    vm.mostrarLinea1 = false;
                                    vm.mostrarLinea2 = true;
                                    vm.mostrarLinea3 = true;
                                }
                                if (res.data.CodBPIN === null || res.data.CodBPIN === '') {
                                    vm.mostrarLinea1 = true;
                                    vm.mostrarLinea2 = false;
                                    vm.mostrarLinea3 = false;
                                }
                            }
                            else {
                                vm.mostrarLinea1 = true;
                                vm.mostrarLinea2 = false;
                                vm.mostrarLinea3 = false;
                            }
                            //}
                        });
                }
            }

        }

        function getIdEtapa() {
            var idEtapa = [];
            switch (vm.etapa) {
                case 'pl':
                    idEtapa = constantesBackbone.idEtapaPlaneacion;
                    break;
                case 'pr':
                    idEtapa = constantesBackbone.idEtapaProgramacion;
                    break;
                case 'gr':
                    idEtapa = constantesBackbone.idEtapaGestionRecursos;
                    break;
                case 'ej':
                    idEtapa = constantesBackbone.idEtapaEjecucion;
                    break;
                case 'se':
                    idEtapa = [];
                    break;
                case 'ev':
                    idEtapa = constantesBackbone.idEtapaEvaluacion;
                    break;
            }
            return idEtapa;
        }


        function getIdFlujo() {
            var parametros = {
                nombreAplicacion: nombreAplicacionBackbone,
                usuario: usuarioDNP,
                idInstancia: $sessionStorage.idInstanciaFlujoPrincipal
            };
            if ($sessionStorage.InstanciaSeleccionada.FlujoId !== undefined) {
                vm.idFlujo = $sessionStorage.InstanciaSeleccionada.FlujoId;
                vm.tramiteId = $sessionStorage.InstanciaSeleccionada.tramiteId;
                consultarEncabezado();
            }
            else {
                servicioAcciones.ObtenerFlujoPorInstanciaTarea(parametros).then(
                    function (respuesta) {
                        vm.idFlujo = respuesta.data.Id;
                        vm.tramiteId = respuesta.data.NumeroTramite == "" ? undefined : respuesta.data.NumeroTramite;
                        consultarEncabezado();
                    });
            }
        }
        function abriPND() {
            var modalInstance = $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/comun/modalPND/modalPND.html',
                controller: 'modalPNDController',
                controllerAs: "vm",
                openedClass: "consola-modal-soportesDNP",
                resolve: {
                    PND: vm.PND
                }
            });

            modalInstance.result.then(function (selectedItem) {


            }, function () {

            });
        }

    }

    //angular.module('backbone').controller('encabezadoGeneralController', encabezadoGeneralController);
    //angular.module('backbone').component('encabezadoGeneral', {
    //    templateUrl: "src/app/formulario/ventanas/comun/encabezadoGeneral.html",
    //    controller: encabezadoGeneralController,
    //    controllerAs: "vm",
    //    bindings: {
    //        callback: '&'
    //    }
    //});
    function encabezadoGeneral() {
        return {
            restrict: 'E',
            transclude: true,
            scope: {
                modelovigencias: '='
            },
            templateUrl: 'src/app/formulario/ventanas/comun/encabezadoGeneral.html',
            controller: encabezadoGeneralController,
            controllerAs: 'vm',
            bindToController: true
        };
    }
    

})();