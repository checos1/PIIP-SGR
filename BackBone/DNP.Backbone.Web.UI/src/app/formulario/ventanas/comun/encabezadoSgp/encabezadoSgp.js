(function () {
    'use strict';
    angular.module('backbone').directive('encabezadoSgp', encabezadoSgp);
    encabezadoSgpController.$inject = [
        'sesionServicios',
        '$uibModal',
        '$scope',
        'utilidades',
        '$sessionStorage',
        'constantesBackbone',
        'constantesTipoBancoProyecto',
        'encabezadoSgpServicio',
        'servicioAcciones',
        'transversalSgpServicio'
    ];

    function encabezadoSgpController(
        sesionServicios,
        $uibModal,
        $scope,
        utilidades,
        $sessionStorage,
        constantesBackbone,
        constantesTipoBancoProyecto,
        encabezadoSgpServicio,
        servicioAcciones,
        transversalSgpServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.rotated = false;
        vm.CodigoProceso = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.CodigoProceso != undefined ? $sessionStorage.InstanciaSeleccionada.CodigoProceso : $sessionStorage.InstanciaSeleccionada.numeroTramite : $sessionStorage.numeroTramite;
        vm.Fecha = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.FechaCreacion != undefined ? moment($sessionStorage.InstanciaSeleccionada.FechaCreacion).format("DD-MM-YYYY HH:mm:ss") : moment($sessionStorage.InstanciaSeleccionada.fecha).format("DD-MM-YYYY HH:mm:ss") : moment(new Date()).format("DD-MM-YYYY HH:MM:SS");
        vm.ProyectoId = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.ProyectoId : $sessionStorage.proyectoId;
        vm.CodigoBPIN = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.IdObjetoNegocio : "";
        vm.tramiteId = "";
        vm.IdAjusteConTramiteSgp = 0;

        vm.modelo = {
            CodigoProceso: "",
            Fecha: "",
            Tipo: "",
            proyectoId: 0,
            nombre: "",
            CodigoBPIN: "",
            vigenciaInicial: 0,
            vigenciaFinal: 0,
            entidad: "",
            estado: "",
            horizonte: "",
            sector: "",
            valor: 0.0,
            apropiacionInicial: 0.0,
            apropiacionVigente: 0.0,
            Alcanceproyecto: 0.0,
            CostoTotalProyecto: 0.0,
            Ejecutor: "",
            FechaRealInicio: "",
            EntidadPresenta: "",
            EntidadPresentado: "",
            ValorTotalInversion: 0.0,
            ValorTotalPreInversion: 0.0,
            ValorTotalOperacion: 0.0            
        }
        vm.abrirModalDetalle = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: '/src/app/formulario/ventanas/comun/encabezadoSgp/modalDetalleSgp.html',                
                controller: function flujoCompletoController($scope, $uibModalInstance) {
                    $scope.modal = $uibModalInstance;
                    $scope.detalle = vm.modelo;
                },
                size: 'lg',
                resolve: {
                }
            });
            modalInstance.result.then(function (accion) {
                $timeout(function () {

                }, 50);
            }, function () {

            });
        };
        cargarNuevo();

        function cargarNuevo() {
            vm.tramiteId = "";
            vm.etapa = $sessionStorage.etapa;
            vm.mostrarLinea1 = true;
            vm.mostrarLinea2 = true;
            vm.mostrarLinea3 = true;
            vm.parametros = {};
            vm.bpin = $sessionStorage.idObjetoNegocio;
            
            if ($sessionStorage.proyectoId != undefined && $sessionStorage.proyectoId != "") {
                if (vm.bpin.split('-')[0] == "EJ") {
                    vm.tramiteId = vm.bpin;
                    vm.bpin = "";
                } else {
                    //vm.bpin = $sessionStorage.idObjetoNegocio;
                    vm.bpin = $sessionStorage.proyectoId;
                    vm.tramiteId = "";
                }
            } else {
                vm.bpin = "";
                vm.tramiteId = $sessionStorage.idObjetoNegocio;
                if (vm.etapa == undefined && vm.tramiteId.split('-')[0] == "EJ") { vm.etapa = 'ej' }
            }
            vm.idNivel = getIdEtapa();
            vm.idInstancia = $sessionStorage.idInstancia;
            vm.idAccion = $sessionStorage.idAccion;
            vm.idFlujo = $sessionStorage.flujoSeleccionado;

            consultarEncabezado();
        }

        function consultarEncabezado() {
            vm.parametros.idInstancia = vm.idInstancia;
            vm.parametros.idFlujo = vm.idFlujo;
            vm.parametros.idNivel = vm.idNivel;
            vm.parametros.idProyecto = vm.bpin;
            vm.parametros.tramite = vm.tramiteId;

            if (vm.idFlujo == undefined) {
                getIdFlujo();
            } else {
                if (vm.bpin == "" && vm.tramiteId == "") {
                    getIdFlujo();
                } else {

                    return encabezadoSgpServicio.ObtenerEncabezadoSGP(vm.parametros).then(
                        function (res) {

                            //vm.CodigoProceso = vm.parametros.tramite;
                            //if (res.data.CodigoProceso != $sessionStorage.numeroTramite)
                            //    cargarNuevo();
                            //else {                           
                            if (res.data != null) {
                                vm.modelo = res.data;
                                // vm.Fecha = vm.modelo.Fecha != null ? moment(vm.modelo.Fecha).format("DD-MM-YYYY HH:MM:SS") : moment($sessionStorage.InstanciaSeleccionada.FechaCreacion).format("DD-MM-YYYY HH:mm:ss");
                                vm.CodigoProceso = (vm.modelo.CodigoProceso == null || vm.modelo.CodigoProceso == '' || vm.modelo.CodigoProceso == undefined) ? vm.CodigoProceso : vm.modelo.CodigoProceso;
                              
                                $sessionStorage.tipoTramiteId = res.data.TipoTramiteId;
                                $sessionStorage.TramiteId = res.data.TramiteId;
                                $sessionStorage.valorTotalProyectoEncabezado = res.data.Valor;
                                $sessionStorage.idProyectoEncabezado = $sessionStorage.proyectoId;
                                $sessionStorage.IdTipoTramitePresupuestal = res.data.IdTipoTramitePresupuestal;
                                if (vm.etapa == "pl" || vm.etapa == "ej" || vm.etapa == "ev") {
                                    if (vm.etapa == "ej") vm.mostrarValorAjuste = true;
                                    vm.mostrarLinea1 = true;
                                    vm.mostrarLinea2 = true;
                                    vm.mostrarLinea3 = true;
                                    vm.mostrarLinea4 = false;
                                    vm.mostrarLinea5 = false;                                   
                                    if (vm.modelo.TipoTramiteId == 92 && vm.modelo.TipoTramiteId != undefined) {
                                        vm.mostrarLinea3 = false;
                                        vm.mostrarLinea5 = true;
                                    }                                    
                                }
                                if (vm.etapa == "ej" && (vm.tramiteId != null && vm.tramiteId != "")) {
                                    vm.mostrarValorAjuste = true;
                                    vm.mostrarLinea1 = true;
                                    vm.mostrarLinea2 = false;
                                    vm.mostrarLinea3 = false;
                                    vm.mostrarLinea4 = true;
                                    vm.mostrarLinea5 = false;
                                }
                                if (vm.etapa == "gr") {
                                    vm.mostrarValorAjuste = false;
                                    vm.mostrarLinea1 = true;
                                    vm.mostrarLinea2 = true;
                                    vm.mostrarLinea3 = true;
                                    vm.mostrarLinea4 = false;
                                    vm.mostrarLinea5 = false;
                                }
                                if (vm.etapa != "pl" && (res.data.CodigoBPIN === null || res.data.CodigoBPIN === '')) {
                                    vm.mostrarValorAjuste = false;
                                    vm.mostrarLinea1 = true;
                                    vm.mostrarLinea2 = false;
                                    vm.mostrarLinea3 = false;
                                    vm.mostrarLinea4 = true;
                                    vm.mostrarLinea5 = true;
                                }
                            }
                            else {
                                vm.mostrarValorAjuste = false;
                                vm.mostrarLinea1 = true;
                                vm.mostrarLinea2 = false;
                                vm.mostrarLinea3 = false;
                                vm.mostrarLinea4 = false;
                                vm.mostrarLinea5 = false;
                            }
                            //}
                        });
                }
            }

        }

        transversalSgpServicio.registrarObservador(function (datos) {
            if (datos.actualizarFuentesAjusteSinTramite === true) {
                consultarEncabezado();
            }
        });

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
            servicioAcciones.ObtenerFlujoPorInstanciaTarea(parametros).then(
                function (respuesta) {
                    vm.idFlujo = respuesta.data.Id;
                    vm.tramiteId = respuesta.data.NumeroTramite == "" ? undefined : respuesta.data.NumeroTramite;
                    consultarEncabezado();
                });
        }
        setTimeout(() => {
            accordeon();
        }, 500)

        function accordeon() {
            var accSgr = document.getElementsByClassName("accordion-SGP");
            var i;
            for (i = 0; i < accSgr.length; i++) {
                accSgr[i].addEventListener("click", function () {
                    toggleAccordionSgp(this);
                });
            };
        }


        function toggleAccordionSgp(obj) {
            obj.classList.toggle("active");
            var panel = obj.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
            var div = document.getElementById('u4_img_sgp'),
                deg = vm.rotated ? 180 : 0;
            div.style.webkitTransform = 'rotate(' + deg + 'deg)';
            div.style.mozTransform = 'rotate(' + deg + 'deg)';
            div.style.msTransform = 'rotate(' + deg + 'deg)';
            div.style.oTransform = 'rotate(' + deg + 'deg)';
            div.style.transform = 'rotate(' + deg + 'deg)';
            vm.rotated = !vm.rotated;
        }
    }

    function encabezadoSgp() {
        return {
            restrict: 'E',
            transclude: true,
            scope: {
                modelovigencias: '='
            },
            templateUrl: 'src/app/formulario/ventanas/comun/encabezadoSgp/encabezadoSgp.html',            
            controller: encabezadoSgpController,
            controllerAs: 'vm',
            bindToController: true
        };
    }


})();