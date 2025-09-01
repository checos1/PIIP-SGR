(function () {
    'use strict';

    utilizadoliberacionController.$inject = [
        '$sessionStorage',
        '$scope',
        'utilizadoliberacionServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        '$uibModal'
    ];

    function utilizadoliberacionController(
        $sessionStorage,
        $scope,
        utilizadoliberacionServicio,
        utilidades,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        $uibModal
    ) {
        var vm = this;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.numeroTramite = $sessionStorage.idObjetoNegocio;
        vm.etapa = "ej";
        vm.TramiteId = $sessionStorage.tramiteId;//906
        vm.usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.idTipoTramite = $sessionStorage.TipoTramiteId;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.erroresActivos = null;
        vm.idProyecto = 0;// $sessionStorage.proyectoId;//98030
        vm.ObjetoVerMas = ObjetoVerMas;
        vm.nombreComponente = "selecionarvigenciafuturavaloresutilizados";
        vm.notificacionCambiosCapitulos = null;
        vm.abrirMensajeInformacion = abrirMensajeInformacion;
        vm.Guardar = Guardar;
        vm.Cancelar = Cancelar;
        vm.Editar = Editar;
        vm.vigenciaActual = new Date().getFullYear();
        vm.handlerComponentes = [
        ];
        vm.handlerComponentesChecked = {};
        vm.rolUsuario = $sessionStorage.usuario.roles.find((item) => item.Nombre.includes('R_Presupuesto - preliminar'));
        vm.ConvertirNumero = ConvertirNumero;
        vm.documentosCargados = false;
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;

        vm.modelo = {
            coleccion: "tramites", ext: ".pdf", codigoProceso: $sessionStorage.numeroTramite, descripcionTramite: $sessionStorage.descripcionTramite, idInstancia: $sessionStorage.idInstancia,
            idAccion: $sessionStorage.idAccion, section: vm.section, idTipoTramite: vm.tipotramiteid
        };

        $scope.$watch('vm.tramiteid', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                if ($sessionStorage.tramiteAsociado)
                    ObtenerLiberacionVigenciaFutura();
            }
        });

        $scope.$watch(() => $sessionStorage.tramiteAsociado
            , (newVal, oldVal) => {
                if (newVal) {
                    ObtenerLiberacionVigenciaFutura();
                }

            }, true);

        $scope.$watch(() => $sessionStorage.proyectoId
            , (newVal, oldVal) => {                
                    ObtenerLiberacionVigenciaFutura();               

            }, true);

        $scope.$watch('vm.actualizacomponentes', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                ObtenerLiberacionVigenciaFutura();
            }
        });


        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };

        function ObtenerLiberacionVigenciaFutura() {
            vm.idProyecto = (vm.idProyecto === 0 || vm.idProyecto === undefined) ? $sessionStorage.proyectoId : vm.idProyecto;
            if (vm.idProyecto !== 0 && vm.idProyecto !== undefined) {
                return utilizadoliberacionServicio.ObtenerLiberacionVigenciaFutura(vm.idProyecto, vm.tramiteid).then(
                    function (respuesta) {
                        console.log(respuesta.data);
                        $scope.datos = respuesta.data[0];
                    });
            }
        }

        vm.convertToDate = function (stringDate) {
            var date = new Date(stringDate);
            return date;
        };

        vm.changeBoton = function (tramite) {
            if (tramite.LabelBoton == '+') {
                tramite.LabelBoton = '-'
            } else {
                tramite.LabelBoton = '+'
            }
            return tramite.LabelBoton;
        }

        function ObjetoVerMas(tramite) {
            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModal.html',
                controller: 'objetivosIndicadorModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    Objetivo: function () {
                        return tramite.Objeto;
                    },
                    IdObjetivo: function () {
                        return '';
                    },
                    Tipo: function () {
                        return 'Objeto';
                    },
                    Titulo: function () {
                        return 'Liberación Vigencias Futuras';
                    }
                },
            });
        }

        function abrirMensajeInformacion() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' > Objetivos específicos</span>");

        }

        function Guardar(tramite) {
            var valida = false;
            if (!tramite.EsConstante) {
                angular.forEach(tramite.ValoresCorrientesAutorizaLiberacion, function (series) {
                    if (series.UtilizadoNacion > series.AprobadoNacion) {
                        valida = true;
                    }
                    if (series.UtilizadoPropios > series.AprobadoPropios) {
                        valida = true;
                    }
                });
            }

            if (tramite.EsConstante) {
                angular.forEach(tramite.ValoresConstantesAutorizaLiberacion, function (series) {
                    if (series.UtilizadoConstanteNacion > series.AprobadoConstanteNacion) {
                        valida = true;
                    }
                    if (series.UtilizadoConstantePropios > series.AprobadoConstantePropios) {
                        valida = true;
                    }
                });
            }

            if (valida) {
                utilidades.mensajeError("El valor utilizado por vigencia y tipo de recurso no puede ser superior al valor autorizado para la misma vigencia y tipo de recurso.");
                return;
            }

            return utilizadoliberacionServicio.InsertaValoresUtilizadosLiberacionVF(tramite).then(
                function (respuesta) {
                    if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        //console.log(respuesta);
                        if (respuesta.data.Exito) {
                            guardarCapituloModificado();
                            vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                            utilidades.mensajeSuccess("", false, false, false, "Los datos han sido guardados con éxito");
                            tramite.EditarTramiteLiberacion = false;
                            if (!tramite.EsConstante) {
                                angular.forEach(tramite.ValoresCorrientesAutorizaLiberacion, function (series) {
                                    series.UtilizadoNacionOriginal = series.UtilizadoNacion;
                                    series.UtilizadoPropiosOriginal = series.UtilizadoPropios;
                                });
                            }

                            if (tramite.EsConstante) {
                                angular.forEach(tramite.ValoresCorrientesAutorizaLiberacion, function (series) {
                                    series.UtilizadoConstanteNacionOriginal = series.UtilizadoConstanteNacion;
                                    series.UtilizadoConstantePropiosOriginal = series.UtilizadoConstantePropios;
                                });
                            }
                            
                            vm.init();
                        } else {
                            utilidades.mensajeError(respuesta.data.Mensaje);
                        }

                        
                    } else {
                        utilidades.mensajeError("Error al realizar la operación");
                    }

                });
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = 413;// span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.proyectoId,
                Justificacion: "",
                //SeccionCapituloId: vm.SeccionCapituloId,
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: false,
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function Cancelar(tramite) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                tramite.EditarTramiteLiberacion = false;

                if (!tramite.EsConstante) {
                    angular.forEach(tramite.ValoresCorrientesAutorizaLiberacion, function (series) {
                        series.UtilizadoNacion = series.UtilizadoNacionOriginal;
                        series.UtilizadoPropios = series.UtilizadoPropiosOriginal;
                    });
                }

                if (tramite.EsConstante) {
                    angular.forEach(tramite.ValoresCorrientesAutorizaLiberacion, function (series) {
                        series.UtilizadoConstanteNacion = series.UtilizadoConstanteNacionOriginal;
                        series.UtilizadoConstantePropios = series.UtilizadoConstantePropiosOriginal;
                    });
                }

                return utilizadoliberacionServicio.ObtenerLiberacionVigenciaFutura(vm.idProyecto, vm.TramiteId).then(
                    function (respuesta) {
                        if (respuesta.data != null && respuesta.data != "") {
                            utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                        }
                    });

            }, function funcionCancelar(reason) {
                //poner aquí q pasa cuando cancela
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function Editar(tramite) {
            tramite.EditarTramiteLiberacion = true;
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }
        }

        vm.getTotal = function (tramite) {
            var total = 0;
            angular.forEach(tramite.ValoresConstantesAutorizaLiberacion, function (series) {
                total = total + (series.UtilizadoConstanteNacion * series.Deflactor);
            });
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2, maximumFractionDigits: 2,
            }).format(total);
        };

        vm.getTotal2 = function (tramite) {
            var total = 0;
            angular.forEach(tramite.ValoresConstantesAutorizaLiberacion, function (series) {
                total = total + (series.UtilizadoConstantePropios * series.Deflactor);
            });
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2, maximumFractionDigits: 2,
            }).format(total);
        };

        vm.getCorrientesNacion = function (vigencia) {
            var total = 0;
            total = vigencia.UtilizadoConstanteNacion * vigencia.Deflactor;
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2, maximumFractionDigits: 2,
            }).format(total);
        };

        vm.getCorrientesPropios = function (vigencia) {
            var total = 0;
            total = vigencia.UtilizadoConstantePropios * vigencia.Deflactor;
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2, maximumFractionDigits: 2,
            }).format(total);
        };

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2, maximumFractionDigits: 2,
            }).format(numero);
        }

        vm.actualizaFila = function (event, tramite) {

            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            event.target.value = parseFloat(event.target.value.replace(",", "."));

            var acumula = 0;
            var acumula2 = 0;

            if (!tramite.EsConstante) {
                angular.forEach(tramite.ValoresCorrientesAutorizaLiberacion, function (series) {
                    acumula = acumula + parseFloat(series.UtilizadoNacion);
                    acumula2 = acumula2 + parseFloat(series.UtilizadoPropios);
                    tramite.TotalCorrientesUtilizadosNacion = parseFloat(acumula.toFixed(2));
                    tramite.TotalCorrientesUtilizadosPropios = parseFloat(acumula2.toFixed(2));
                });
            }

            if (tramite.EsConstante) {
                angular.forEach(tramite.ValoresConstantesAutorizaLiberacion, function (series) {
                    acumula = acumula + parseFloat(series.UtilizadoConstanteNacion);
                    acumula2 = acumula2 + parseFloat(series.UtilizadoConstantePropios);
                    tramite.TotalConstantesUtilizadosNacion = parseFloat(acumula.toFixed(2));
                    tramite.TotalConstantesUtilizadosPropios = parseFloat(acumula2.toFixed(2));
                });
            }

            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
        }    

        vm.validarTamanio = function (event) {

            if (Number.isNaN(event.target.value)) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == null) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == "") {
                event.target.value = "0"
                return;
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > 4) {
                }
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            }
        }

        /*------------------------------------Validaciones-----------------------------------*/
        /**
       * Listado de componentes hijos, obligatorio para estructura de validación
       * */


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

        vm.guardado = function (nombreComponenteHijo, deshabilitarRegresar, devolver) {
            vm.callback();
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo, deshabilitarRegresar: deshabilitarRegresar });

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
            vm.notificacionestado({ estado: estado, nombreComponente: vm.nombreComponente });
        });

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
            };
        }

        vm.deshabilitarBotonDevolverAsociarProyectoVF = function () {
            vm.callback();

        }

        vm.notificacionValidacionPadre = function (errores) {
            //debugger;
            if (errores != undefined) {
                var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
                vm.erroresActivos = erroresFiltrados.erroresActivos;

                var erroresFiltrados2 = utilsValidacionSeccionCapitulosServicio.getErroresValidados('selecionarvigenciafuturavaloresutilizadosdocs', errores);
                if (erroresFiltrados2.erroresActivos.length > 0) {
                    vm.erroresActivos.push(erroresFiltrados2.erroresActivos[0]);
                }

                vm.ejecutarErrores();

                var isValid = (vm.erroresActivos.length <= 0);
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        };

        vm.ejecutarErrores = function () {
            vm.limpiarErrores();
            $scope.errores = vm.erroresActivos;
            vm.erroresActivos.forEach(p => {
                if (vm.errores[p.Error] != undefined) {
                    vm.errores[p.Error](p.Error, p.Descripcion, p.Data);
                }
            });
        }

        vm.limpiarErrores = function () {

            var autorizacionError004 = document.getElementById("validacion-VATL004-valoresutilizados");

            if (autorizacionError004 != undefined) {
                autorizacionError004.innerHTML = '';
                autorizacionError004.classList.add('hidden');
            }
        }

        vm.validacionVUTL004 = function () {
            //debugger;
            var autorizacionError004 = document.getElementById("validacion-VATL004-valoresutilizados");

            if (autorizacionError004 != undefined) {
                autorizacionError004.innerHTML = '<span> <img src="Img/u4630.svg"> Debe adjuntar los documentos obligatorios </span>';
                autorizacionError004.classList.remove('hidden');
            }
        }

        vm.errores = {
            'VFO006': vm.validacionVUTL004
        }
        

        /* --------------------------------- Validaciones ---------------------------*/

        /**
        * Función que recibe los estados de los componentes hijos
        * @param {any} esValido true: valido, false: invalido
        * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
        */
        vm.notificacionEstado = function (nombreComponente, esValido) {
            //debugger;
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.esValido = !esValido ? false : vm.esValido;
            vm.handlerComponentes[indx].esValido = esValido;
            vm.handlerComponentesChecked[nombreComponente] = esValido;
            //vm.showAlertError(nombreComponente, esValido, esValidoPaso4);
            vm.showAlertError(nombreComponente, esValido);
        }

        /**
         * Función que visualiza alerta de error tab de componente
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        //vm.showAlertError = function (nombreComponente, esValido, esValidoPaso4) {
        vm.showAlertError = function (nombreComponente, esValido) {
            debugger;
            var idSpanAlertComponent = document.getElementById("alert-" + nombreComponente);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.capitulos = function (listadoCapitulos) {
            var listadoCapRecursos = listadoCapitulos.filter(p => p.SeccionModificado == vm.nombreComponente)
            listadoCapRecursos.forEach(function (item) {
                var el = document.getElementById("name-capitulo-" + item.nombreComponente);
                var elidSeccionCapitulo = document.getElementById("id-capitulo-" + item.nombreComponente);
                var elAccordion = document.getElementById("accordion-" + item.nombreComponente);
                if (el != undefined && el != null) {
                    el.innerHTML = item.Capitulo;
                }
                if (elAccordion != undefined && elAccordion != null) {
                    elAccordion.classList.remove("hidden");
                }
                if (elidSeccionCapitulo != undefined && elidSeccionCapitulo != null) {
                    elidSeccionCapitulo.innerHTML = item.SeccionCapituloId;
                }
            });
        };

        /*------------------------------------Fin Validaciones-----------------------------------*/

    }

    angular.module('backbone').component('utilizadoliberacion', {
        templateUrl: "src/app/formulario/ventanas/tramiteLiberacion/componentes/utilizadoliberacion/utilizadoliberacion.html",
        controller: utilizadoliberacionController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            actualizacomponentes: '@'
        }
    }).directive('stringToNumber', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$parsers.push(function (value) {

                    return '' + value;
                });
                ngModel.$formatters.push(function (value) {
                    return parseFloat(value);
                });
            }
        };
    })
        .directive('nksOnlyNumber', function () {
            return {
                restrict: 'EA',
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                        var spiltArray = String(newValue).split("");

                        if (attrs.allowNegative == "false") {
                            if (spiltArray[0] == '-') {
                                newValue = newValue.replace("-", "");
                                ngModel.$setViewValue(newValue);
                                ngModel.$render();
                            }
                        }

                        if (attrs.allowDecimal == "false") {
                            newValue = parseInt(newValue);
                            ngModel.$setViewValue(newValue);
                            ngModel.$render();
                        }

                        if (attrs.allowDecimal != "false") {
                            if (attrs.decimalUpto) {
                                var n = String(newValue).split(".");
                                if (n[1]) {
                                    var n2 = n[1].slice(0, attrs.decimalUpto);
                                    newValue = [n[0], n2].join(".");
                                    ngModel.$setViewValue(newValue);
                                    ngModel.$render();
                                }
                            }
                        }


                        if (spiltArray.length === 0) return;
                        if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                        if (spiltArray.length === 2 && newValue === '-.') return;

                        /*Check it is number or not.*/
                        if (isNaN(newValue)) {
                            ngModel.$setViewValue(oldValue || '');
                            ngModel.$render();
                        }
                    });
                }
            };
        });

    }) ();