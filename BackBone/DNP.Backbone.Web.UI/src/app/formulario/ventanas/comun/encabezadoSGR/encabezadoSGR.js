(function () {
    'use strict';
    angular.module('backbone').directive('encabezadoSgr', encabezadoSgr);
    encabezadoSGRController.$inject = [
        'sesionServicios',
        '$uibModal',
        '$scope',
        'utilidades',
        '$sessionStorage',
        'constantesBackbone',
        'encabezadoSGRServicio',
        'servicioAcciones'
    ];

    function encabezadoSGRController(
        sesionServicios,
        $uibModal,
        $scope,
        utilidades,
        $sessionStorage,
        constantesBackbone,
        encabezadoSGRServicio,
        servicioAcciones
    ) {
        var vm = this;
        vm.lang = "es";
        vm.rotated = false;
        vm.CodigoProceso = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.CodigoProceso : $sessionStorage.numeroTramite;
        vm.Fecha = $sessionStorage.InstanciaSeleccionada != undefined ? moment($sessionStorage.InstanciaSeleccionada.FechaCreacion).format("DD-MM-YYYY HH:MM:SS") : moment(new Date()).format("DD-MM-YYYY HH:MM:SS");
        vm.ProyectoId = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.ProyectoId : 0;
        
        vm.CodBPIN = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.IdObjetoNegocio : "";
        vm.modelo = {
            CodigoProceso: "",
            Fecha: "",
            Tipo: "",
            proyectoId: 0,
            nombre: "",
            CodBPIN: "",
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
            EntidadPresentado: ""
        }
        vm.abrirModalDetalle = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: '/src/app/formulario/ventanas/comun/encabezadoSGR/modalDetalle.html',
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
            vm.parametros.idProyectoStr = vm.bpin;
            vm.parametros.tramite = vm.tramiteId;

            if (vm.idFlujo == undefined) {
                getIdFlujo();
            } else {
                if (vm.bpin == "" && vm.tramiteId == "") {
                    getIdFlujo();
                } else {

                    return encabezadoSGRServicio.ObtenerEncabezado(vm.parametros).then(
                        function (res) {
                            //vm.CodigoProceso = vm.parametros.tramite;
                            //if (res.data.CodigoProceso != $sessionStorage.numeroTramite)
                            //    cargarNuevo();
                            //else {
                            if (res.data != null) {
                                res.data.ProyectoTipo = res.data.ProyectoTipo === null ? 'NO' : res.data.ProyectoTipo;
                                vm.modelo = res.data;
                                vm.Fecha = vm.modelo.Fecha != null ? moment(vm.modelo.Fecha).format("DD-MM-YYYY HH:MM:SS") : moment($sessionStorage.InstanciaSeleccionada.FechaCreacion).format("DD-MM-YYYY HH:MM:SS");
                                vm.CodigoProceso = (vm.modelo.CodigoProceso == null || vm.modelo.CodigoProceso == '' || vm.modelo.CodigoProceso == undefined) ? vm.CodigoProceso : vm.modelo.CodigoProceso;

                                $sessionStorage.EntidadAdscritaId = res.data.EntidadAdscritaId;
                                $sessionStorage.TipoProyectoCTUSId = res.data.TipoProyectoCTUSId;
                                $sessionStorage.tipoTramiteId = res.data.TipoTramiteId;
                                $sessionStorage.TramiteId = res.data.TramiteId;
                                $sessionStorage.valorTotalProyectoEncabezado = res.data.Valor;
                                $sessionStorage.idProyectoEncabezado = $sessionStorage.proyectoId;
                                $sessionStorage.IdTipoTramitePresupuestal = res.data.IdTipoTramitePresupuestal;
                                $sessionStorage.NombrePhase = res.data.Fase;
                                $sessionStorage.IdInstanciaViabiliad = res.data.IdInstanciaViabiliad;
                                $sessionStorage.ProyectoTieneOPC = res.data.ProyectoTieneOPC;
                                $sessionStorage.TieneOPC = res.data.TieneOPC;
                                $sessionStorage.PorcentajeCredito = res.data.PorcentajeCredito;
                                $sessionStorage.ProyectoTieneInstanciasNoOPC = res.data.ProyectoTieneInstanciasNoOPC;

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
            var accSgr = document.getElementsByClassName("accordion-SGR");
            var i;
            for (i = 0; i < accSgr.length; i++) {
                accSgr[i].addEventListener("click", function () {
                    toggleAccordionSgr(this);
                });
            };
        }


        function toggleAccordionSgr(obj) {
            obj.classList.toggle("active");
            var panel = obj.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
            var div = document.getElementById('u4_img_sgr'),
                deg = vm.rotated ? 180 : 0;
            div.style.webkitTransform = 'rotate(' + deg + 'deg)';
            div.style.mozTransform = 'rotate(' + deg + 'deg)';
            div.style.msTransform = 'rotate(' + deg + 'deg)';
            div.style.oTransform = 'rotate(' + deg + 'deg)';
            div.style.transform = 'rotate(' + deg + 'deg)';
            vm.rotated = !vm.rotated;
        }
    }

    function encabezadoSgr() {
        return {
            restrict: 'E',
            transclude: true,
            scope: {
                modelovigencias: '='
            },
            templateUrl: 'src/app/formulario/ventanas/comun/encabezadoSGR/encabezadoSGR.html',
            controller: encabezadoSGRController,
            controllerAs: 'vm',
            bindToController: true
        };
    }


})();