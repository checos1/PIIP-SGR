(function () {
    'use strict';

    ctusAsignacionSgrController.$inject = [
        '$scope',
        'utilidades',
        '$sessionStorage',
        'ctusAsignacionSgrServicio',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        'constantesBackbone'
    ];

    function ctusAsignacionSgrController(
        $scope,
        utilidades,
        $sessionStorage,
        ctusAsignacionSgrServicio,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        constantesBackbone
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";

        vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
        vm.ProyectoId = $sessionStorage.idProyectoEncabezado;
        if (vm.ProyectoId === undefined)
            vm.ProyectoId = $sessionStorage.idObjetoNegocio;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.IdRol = constantesBackbone.idRolViabilidadDefinitiva;

        vm.notificarGuardado = notificarGuardado;
        vm.eventoValidar = eventoValidar;

        vm.ConfiguracionEntidades = [{
            ProyectoId: 0,
            FaseId: 0,
            Fase: "",
            AplicaTecnico: ""
        }];

        vm.init = function () {
            vm.inicializarComponenteCheck(true);
            vm.callback({ validacion: { evento: vm.eventoValidar }, arg: true, aprueba: false, titulo: '' });
            vm.accionPendiente = $sessionStorage.listadoAccionesTramite.some(x => x.AccionInstanciaId === null)
            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {
                vm.setCapitulosHijos(respuesta.data);
            });
        };

        vm.$onDestroy = function () {
            ctusAsignacionSgrServicio.limpiarObservadores();
        };

        $scope.tab = 1;

        $scope.setTab = function (newTab) {
            $scope.tab = newTab;
        };

        $scope.isSet = function (tabNum) {
            return $scope.tab === tabNum;
        };

        vm.validarFormulario = function () {
            eventoValidar();
        };

        function notificarGuardado() {
            vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '' });
        }

        /**
        * Listado de componentes hijos, obligatorio para estructura de validación
        * */
        vm.handlerComponentes = [
            { id: 1, componente: 'sgrctusasignacion', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null }

        ];

        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'sgrctusasignacionasignar': true
            };
        }


        vm.obtenerDetalleTramiteyRol = function () {

            if (vm.IdNivel === constantesBackbone.idNivelSeleccionProyectos.toLowerCase()) {
                var roles = sesionServicios.obtenerUsuarioIdsRoles();
                var rol = roles.find(x => x === constantesBackbone.idRPresupuesto.toLowerCase());
                if (rol !== undefined)
                    vm.IdRol = rol;
            }

            var roles = sesionServicios.obtenerUsuarioIdsRoles();
            trasladosServicio.obtenerDetallesTramite($sessionStorage.numeroTramite).then(function (result) {
                var x = result.data;
                if (x != null) {
                    vm.tramiteId = x.TramiteId;
                    vm.tipoTramiteId = x.TipoTramiteId;
                    $sessionStorage.tramiteId = x.TramiteId;
                    vm.tramiteDetail.tramiteId = x.TramiteId;
                    vm.tramiteDetail.tipoTramiteId = x.TipoTramiteId;
                }
            });
        }

        vm.changeArrow = function (nombreModificado) {
            var idSpanArrow = 'arrow-' + nombreModificado;
            var arrowCapitulo = document.getElementById(idSpanArrow);
            var arrowClasses = arrowCapitulo.classList;
            for (var i = 0; i < arrowClasses.length; i++) {
                if (arrowClasses[i] == vm.arrowIcoDown) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown);
                    break;
                } else if (arrowClasses[i] == vm.arrowIcoUp) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp);
                    break;
                }
            }
        }

        function eventoValidar() {
            vm.inicializarComponenteCheck();
            ctusAsignacionSgrServicio.obtenerErroresviabilidadSgr(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia).then(function (respuesta) {

                vm.cumple = '';
                vm.notificacionValidacionHijos(respuesta.data);
                var errorObservacion = false;
                var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
                if (indexobs != -1)
                    errorObservacion = respuesta.data[indexobs].Errores == null ? false : true;

                var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                var error = respuesta.data.findIndex(p => p.Errores != null && p.Seccion != '');

                if (!vm.accionPendiente) {
                    var erroresCumple = respuesta.data.filter(p => p.Errores != null && p.Seccion == '');
                    var erroresJson = erroresCumple.length == 0 ? [] : JSON.parse(erroresCumple[0].Errores);

                    if (error == -1 &&
                        (vm.visualizarCumple || !vm.visualizarCumple)) {
                        vm.cumple = erroresJson.length == 0 ? 'v' : erroresJson.Cumple[0].Error;
                        vm.textEstadoValidacion = erroresJson.length == 0 ? vm.textEstadoViable : erroresJson.Cumple.length > 1 ? erroresJson.Cumple[1].Descripcion : erroresJson.Cumple[0].Descripcion;
                        vm.textDetalleValidacion = erroresJson.length == 0 ? vm.detalleViable : erroresJson.Cumple.length > 1 ? erroresJson.Cumple[1].Detalle : erroresJson.Cumple[0].Detalle;
                        vm.siguienteDisabled = vm.cumple == 'obs';
                        vm.callback({ validacion: null, arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
                        return;
                    }
                }
                vm.visualizarAlerta((findErrors != -1 || error != -1), errorObservacion);
            });
        }

        vm.iconCumple = function (imagenSi, imagenNo, imagenObs, imagenNvObs) {
            if (vm.cumple == undefined || vm.cumple == '')
                return "";
            else if (vm.cumple == "v")
                return imagenSi;
            else if (vm.cumple == "obs")
                return imagenObs;
            else if (vm.cumple == "nvobs")
                return imagenNvObs;
            else
                return imagenNo;
        }

        vm.visualizarAlerta = function (error, errorObservacion) {
            if (error) utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
            else utilidades.mensajeSuccess("¡Validación exitosa!", false, false, false);

            var hijosCorrectos = (error == false);

            vm.siguienteDisabled = (hijosCorrectos == false);

            var ocultarDevolver = $sessionStorage.listadoAccionesTramite.some(x => x.AccionInstanciaId === null);

            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion
            }
            vm.callback({ validacion: validacion, arg: vm.siguienteDisabled, aprueba: false, titulo: '', ocultarDevolver: ocultarDevolver });
        }

        /* ---------------------- Validaciones ---------------*/

        vm.notificacionValidacion = function (handler, nombreComponente, handlerCapitulos) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.handlerComponentes[indx].handlerValidacion = handler;
            vm.handlerComponentes[indx].handlerCapitulos = handlerCapitulos;
        };

        vm.notificacionValidacionEstado = function (estado, nombreComponente) {
            vm.showAlertError(nombreComponente, estado);
            vm.handlerComponentesChecked[nombreComponente] = estado;
        }

        vm.setCapitulosHijos = function (listadoCapitulos) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCapitulos != null) vm.handlerComponentes[i].handlerCapitulos(listadoCapitulos);
            }
        };

        vm.notificacionValidacionHijos = function (errores) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerValidacion != null && errores != null) vm.handlerComponentes[i].handlerValidacion({ errores });
            }
        };

        /**
       * Función que visualiza alerta de error tab de componente
       * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
       */
        vm.showAlertError = function (nombreComponente, esValido) {
            var nomAlerta = "alert-" + nombreComponente;

            var idSpanAlertComponent = document.getElementById(nomAlerta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }

            if (nombreComponente === "sgrviabilidadsoportes") {
                var idDevError = document.getElementById("sgrviabilidadsoportesalojararchivo-archivo-error");
                if (idDevError != undefined) {
                    if (esValido) {
                        idDevError.classList.add("hidden");
                    } else {
                        idDevError.classList.remove("hidden");
                    }
                }

                idDevError = document.getElementById("alert-sgrviabilidadsoportesalojararchivo");
                if (idDevError != undefined) {
                    if (esValido) {
                        idDevError.classList.remove("ico-advertencia");
                    } else {
                        idDevError.classList.add("ico-advertencia");
                    }
                }
            }
        }

        $scope.$watchCollection("vm.handlerComponentesChecked", function (newValue, oldValue) {
            var estado = true;
            var listHijos = Object.keys(vm.handlerComponentesChecked);
            if (listHijos.length == 0 || newValue === oldValue) {
                return;
            }
            listHijos.forEach(p => {
                if (vm.handlerComponentesChecked[p] == false) {
                    estado = false;
                }
            });
            vm.validarSiguiente = estado;
            vm.callback({ arg: !estado, aprueba: false, titulo: '' });
        });

        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            //vm.notificacionCambiosAsociarProyecto(nombreComponente, nombreComponenteHijo);
        }

        vm.notificacionCambios = function (nombreComponente, nombreComponenteHijo) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) vm.handlerComponentes[i].handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
            }
        };

        vm.notificacionCambiosAjustes = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (handler != null && vm.handlerComponentes[i].componente == nombreComponente) vm.handlerComponentes[i].handlerCambios = handler;
            }
        };
    }

    angular.module('backbone').component('ctusAsignacionSgr', {
        templateUrl: "src/app/formulario/ventanas/SGR/ctus/ctusAsignacionSgr.html",
        controller: ctusAsignacionSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });
})();