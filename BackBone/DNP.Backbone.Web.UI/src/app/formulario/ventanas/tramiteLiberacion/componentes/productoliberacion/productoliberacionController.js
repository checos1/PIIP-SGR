(function () {
    'use strict';

    productoliberacionController.$inject = [
        '$sessionStorage',
        '$scope',
        'productoliberacionServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        '$uibModal',

    ];

    function productoliberacionController(
        $sessionStorage,
        $scope,
        productoliberacionServicio,
        utilidades,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        $uibModal,
    ) {
        var vm = this;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.numeroTramite = $sessionStorage.idObjetoNegocio;
        vm.etapa = "ej";
        vm.TramiteId = $sessionStorage.tramiteId;
        vm.usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.idTipoTramite = $sessionStorage.TipoTramiteId;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.erroresActivos = null;
        vm.idProyecto = 0;
        vm.ObjetoVerMas = ObjetoVerMas;
        vm.nombreComponente = "pasotresliberacionvfvaloresprolbvf";
        vm.notificacionCambiosCapitulos = null;
        vm.abrirMensajeInformacion = abrirMensajeInformacion;
        vm.Guardar = Guardar;
        vm.Cancelar = Cancelar;
        vm.Editar = Editar;
        vm.ConvertirNumero = ConvertirNumero;
        vm.vigenciaActual = new Date().getFullYear();
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;
        vm.handlerComponentes = [
        ];
        vm.handlerComponentesChecked = {};

        $scope.$watch('vm.tramiteid', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                ObtenerValUtilizadosLiberacionVigenciasFuturas();
            }
        });


        vm.init = function () {
            ObtenerValUtilizadosLiberacionVigenciasFuturas();
            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };

        function ObtenerValUtilizadosLiberacionVigenciasFuturas() {
            return productoliberacionServicio.ObtenerValUtilizadosLiberacionVigenciasFuturas(vm.idProyecto, vm.tramiteid).then(
                function (respuesta) {
                    console.log(respuesta.data);
                    $scope.datos = respuesta.data;
                });
        }


        vm.changeBoton = function (general) {
            if (general.LabelBoton == '+') {
                general.LabelBoton = '-'
            } else {
                general.LabelBoton = '+'
            }
            return general.LabelBoton;
        }

        vm.changeBotonProducto = function (producto) {
            if (producto.LabelBotonProducto == '+') {
                producto.LabelBotonProducto = '-'
            } else {
                producto.LabelBotonProducto = '+'
            }
            return producto.LabelBotonProducto;
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

        vm.convertToDate = function (stringDate) {
            var date = new Date(stringDate);
            return date;
        };

        function Guardar(producto) {
            var valida = false;

            if (!producto.EsConstante) {
                var validar = false;
                angular.forEach(producto.Vigencias, function (series) {
                    if (series.ValorUtilizado == "" || series.ValorUtilizado == null) {
                        series.ValorUtilizado = 0.00;
                    } else {
                        if (series.ValorUtilizado > series.ValorSolicitado) {
                            series.ValidacionValores = true;
                            validar = true;
                        }
                    }
                });

                if (validar) {
                    utilidades.mensajeError("El valor utilizado no podrá ser superior al valor solicitado por vigencia.");
                    return;
                }

                return productoliberacionServicio.InsertaValoresproductosLiberacionVFCorrientes(producto).then(
                    function (respuesta) {
                        if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                            if (respuesta.data.Exito) {
                                guardarCapituloModificado();
                                vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                                utilidades.mensajeSuccess("", false, false, false, "Los datos se han guardado con éxito");
                                producto.EditarTramiteLiberacion = false;

                                angular.forEach(producto.Vigencias, function (series) {
                                    series.ValorUtilizadoOriginal = series.ValorUtilizado;
                                });

                                vm.init();
                            } else {
                                utilidades.mensajeError(respuesta.data.Mensaje);
                            }
                        } else {
                            utilidades.mensajeError("Error al realizar la operación");
                        }
                    });
            }

            if (producto.EsConstante) {
                var validar = false;
                angular.forEach(producto.Vigencias, function (series) {
                    if (series.ValorUtilizadoConstantes == "" || series.ValorUtilizadoConstantes == null) {
                        series.ValorUtilizadoConstantes = 0.00;
                    } else {
                        if (series.ValorUtilizadoConstantes > series.ValorSolicitadoConstante) {
                            series.ValidacionValores = true;
                            validar = true;
                        }
                    }
                });

                if (validar) {
                    utilidades.mensajeError("El valor utilizado no podrá ser superior al valor solicitado por vigencia.");
                    return;
                }

                return productoliberacionServicio.InsertaValoresproductosLiberacionVFConstantes(producto).then(
                    function (respuesta) {
                        if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                            if (respuesta.data.Exito) {
                                guardarCapituloModificado();
                                vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                                utilidades.mensajeSuccess("", false, false, false, "Los datos se han guardado con éxito");
                                producto.EditarTramiteLiberacion = false;

                                angular.forEach(producto.Vigencias, function (series) {
                                    series.ValorUtilizadoConstantesOriginal = series.ValorUtilizadoConstantes;
                                });

                                vm.init();
                            } else {
                                utilidades.mensajeError(respuesta.data.Mensaje);
                            }
                        } else {
                            utilidades.mensajeError("Error al realizar la operación");
                        }
                    });
            }
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = 634;// span.textContent;
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

        function Cancelar(producto) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                producto.EditarTramiteLiberacion = false;

                if (!producto.EsConstante) {
                    angular.forEach(producto.Vigencias, function (series) {
                        series.ValorUtilizado = series.ValorUtilizadoOriginal;
                    });
                }

                if (producto.EsConstante) {
                    angular.forEach(producto.Vigencias, function (series) {
                        series.ValorUtilizadoConstantes = series.ValorUtilizadoConstantesOriginal;
                    });
                }

                return productoliberacionServicio.ObtenerValUtilizadosLiberacionVigenciasFuturas(vm.idProyecto, vm.TramiteId).then(
                    function (respuesta) {
                        if (respuesta.data != null && respuesta.data != "") {
                            utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                        }
                    });

            }, function funcionCancelar(reason) {
                //poner aquí q pasa cuando cancela
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function Editar(producto) {
            producto.EditarTramiteLiberacion = true;
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2, maximumFractionDigits: 2,
            }).format(numero);
        }

        vm.getValorUtilizado = function (vigencia) {
            var total = 0;
            total = vigencia.ValorUtilizadoConstantes * vigencia.Deflactor;
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2, maximumFractionDigits: 2,
            }).format(total);
        };

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }
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

        vm.actualizaFila = function (event, producto) {

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

            if (!producto.EsConstante) {
                angular.forEach(producto.Vigencias, function (series) {
                    acumula = acumula + parseFloat(series.ValorUtilizado);
                    producto.TotalValorUtilizado = parseFloat(acumula.toFixed(2));
                });
            }

            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
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
            //debugger;
            if ($scope.datos != null) {
                angular.forEach($scope.datos.TramitesVerificadosCorrientes, function (series) {
                    angular.forEach(series.DetalleObjetivosCorrientes, function (series2) {
                        angular.forEach(series2.DetalleProductosCorrientes, function (series3) {
                            var autorizacionError001 = document.getElementById("validacion-VUPTL001-" + series3.ProductoId);
                            if (autorizacionError001 != undefined) {
                                autorizacionError001.innerHTML = '';
                                autorizacionError001.classList.add('hidden');
                            }

                            var autorizacionError002 = document.getElementById("validacion-VUPTL002-" + series3.ProductoId);
                            if (autorizacionError002 != undefined) {
                                autorizacionError002.innerHTML = '';
                                autorizacionError002.classList.add('hidden');
                            }
                        });
                    });

                    var autorizacionError003 = document.getElementById("validacion-VUPTL003-" + series.TramiteLiberarId);
                    if (autorizacionError003 != undefined) {
                        autorizacionError003.innerHTML = '';
                        autorizacionError003.classList.add('hidden');
                    }

                });

                angular.forEach($scope.datos.TramitesVerificadosConstantes, function (series) {
                    angular.forEach(series.DetalleObjetivosConstantes, function (series2) {
                        angular.forEach(series2.DetalleProductosConstantes, function (series3) {
                            var autorizacionError004 = document.getElementById("validacion-VUPTL004-" + series3.ProductoId);
                            if (autorizacionError004 != undefined) {
                                autorizacionError004.innerHTML = '';
                                autorizacionError004.classList.add('hidden');
                            }

                            var autorizacionError005 = document.getElementById("validacion-VUPTL005-" + series3.ProductoId);
                            if (autorizacionError005 != undefined) {
                                autorizacionError005.innerHTML = '';
                                autorizacionError005.classList.add('hidden');
                            }
                        });
                    });

                    var autorizacionError006 = document.getElementById("validacion-VUPTL006-" + series.TramiteLiberarId);
                    if (autorizacionError006 != undefined) {
                        autorizacionError006.innerHTML = '';
                        autorizacionError006.classList.add('hidden');
                    }

                });
            }           
        }

        vm.validacionVUPTL001 = function (errores, descripcion, data) {
            //debugger;
            var indErr001 = JSON.parse(data);

            indErr001.forEach(p => {
                var autorizacionError001 = document.getElementById("validacion-VUPTL001-" + p.ProductoId);

                if (descripcion != '') {
                    if (autorizacionError001 != undefined) {
                        autorizacionError001.innerHTML = '<span> <img src="Img/u4630.svg"> ' + descripcion + "</span>";
                        autorizacionError001.classList.remove('hidden');
                    }
                } else {
                    if (autorizacionError001 != undefined) {
                        autorizacionError001.classList.add('hidden');
                    }
                }
            });
        }

        vm.validacionVUPTL002 = function (errores, descripcion, data) {
            //debugger;
            var indErr001 = JSON.parse(data);

            indErr001.forEach(p => {
                var autorizacionError002 = document.getElementById("validacion-VUPTL002-" + p.ProductoId);

                if (descripcion != '') {
                    if (autorizacionError002 != undefined) {
                        autorizacionError002.innerHTML = '<span> <img src="Img/u4630.svg"> ' + descripcion + "</span>";
                        autorizacionError002.classList.remove('hidden');
                    }
                } else {
                    if (autorizacionError002 != undefined) {
                        autorizacionError002.classList.add('hidden');
                    }
                }
            });
        }

        vm.validacionVUPTL003 = function (errores, descripcion, data) {
            //debugger;
            var indErr001 = JSON.parse(data);

            indErr001.forEach(p => {
                var autorizacionError003 = document.getElementById("validacion-VUPTL003-" + p.TramiteLiberarId);

                if (descripcion != '') {
                    if (autorizacionError003 != undefined) {
                        autorizacionError003.innerHTML = '<span> <img src="Img/u4630.svg"> ' + descripcion + "</span>";
                        autorizacionError003.classList.remove('hidden');
                    }
                } else {
                    if (autorizacionError003 != undefined) {
                        autorizacionError003.classList.add('hidden');
                    }
                }
            });
        }

        vm.validacionVUPTL004 = function (errores, descripcion, data) {
            //debugger;
            var indErr001 = JSON.parse(data);

            indErr001.forEach(p => {
                var autorizacionError004 = document.getElementById("validacion-VUPTL004-" + p.ProductoId);

                if (descripcion != '') {
                    if (autorizacionError004 != undefined) {
                        autorizacionError004.innerHTML = '<span> <img src="Img/u4630.svg"> ' + descripcion + "</span>";
                        autorizacionError004.classList.remove('hidden');
                    }
                } else {
                    if (autorizacionError004 != undefined) {
                        autorizacionError004.classList.add('hidden');
                    }
                }
            });
        }

        vm.validacionVUPTL005 = function (errores, descripcion, data) {
            //debugger;
            var indErr001 = JSON.parse(data);

            indErr001.forEach(p => {
                var autorizacionError005 = document.getElementById("validacion-VUPTL005-" + p.ProductoId);

                if (descripcion != '') {
                    if (autorizacionError005 != undefined) {
                        autorizacionError005.innerHTML = '<span> <img src="Img/u4630.svg"> ' + descripcion + "</span>";
                        autorizacionError005.classList.remove('hidden');
                    }
                } else {
                    if (autorizacionError005 != undefined) {
                        autorizacionError005.classList.add('hidden');
                    }
                }
            });
        }

        vm.validacionVUPTL006 = function (errores, descripcion, data) {
            //debugger;
            var indErr001 = JSON.parse(data);

            indErr001.forEach(p => {
                var autorizacionError006 = document.getElementById("validacion-VUPTL006-" + p.TramiteLiberarId);

                if (descripcion != '') {
                    if (autorizacionError006 != undefined) {
                        autorizacionError006.innerHTML = '<span> <img src="Img/u4630.svg"> ' + descripcion + "</span>";
                        autorizacionError006.classList.remove('hidden');
                    }
                } else {
                    if (autorizacionError006 != undefined) {
                        autorizacionError006.classList.add('hidden');
                    }
                }
            });
        }


        vm.errores = {
            'VUPTL001': vm.validacionVUPTL001,
            'VUPTL002': vm.validacionVUPTL002,
            'VUPTL003': vm.validacionVUPTL003,
            'VUPTL004': vm.validacionVUPTL004,
            'VUPTL005': vm.validacionVUPTL005,
            'VUPTL006': vm.validacionVUPTL006,
        }

        /* --------------------------------- Validaciones ---------------------------*/

        /**
        * Función que recibe los estados de los componentes hijos
        * @param {any} esValido true: valido, false: invalido
        * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
        */
        vm.notificacionEstado = function (nombreComponente, esValido) {
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

    angular.module('backbone').component('productoliberacion', {
        templateUrl: "src/app/formulario/ventanas/tramiteLiberacion/componentes/productoliberacion/productoliberacion.html",
        controller: productoliberacionController,
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
})();