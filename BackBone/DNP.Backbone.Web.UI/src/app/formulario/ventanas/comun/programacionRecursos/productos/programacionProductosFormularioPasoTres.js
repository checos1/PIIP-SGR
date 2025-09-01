(function () {
    'use strict';

    programacionProductosFormularioPasoTres.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        '$timeout',
        'comunesServicio'
    ];

    function programacionProductosFormularioPasoTres(
        $scope,
        $sessionStorage,
        utilidades,
        $timeout,
        comunesServicio
    ) {
        ///*Varibales */
        var vm = this;
        vm.lang = "es";
        vm.EAjCT = false;
        vm.nombreComponente = "productosproductos";
        vm.idNivel = $sessionStorage.idNivel;
        vm.seccionCapitulo = null;
        vm.mostrarCreditosPr = false;
        vm.mostrarCreditosFir = false;
        vm.mostrarDonacion = false;
        vm.tramiteid = $sessionStorage.InstanciaSeleccionada.tramiteId;
        //vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1
        vm.habilitaBotones = false;
        /*declara metodos*/
        vm.CancelarValores = CancelarValores;
        vm.EditarValores = EditarValores;
        vm.GuardarValores = GuardarValores;
        vm.ConvertirNumero = ConvertirNumero;


        vm.init = function () {
            ObtenerDatosProgramacionProducto()

            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };

        function ObtenerDatosProgramacionProducto() {
            //vm.tramiteid = 2290;
            return comunesServicio.ObtenerDatosProgramacionProducto(vm.tramiteid).then(
                function (respuesta) {
                    if (respuesta.data !== '') {
                        $scope.productos = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                    }
                    else {
                        $scope.productos = [];
                    }
                });
        }

        vm.abrirTooltip = function () {
            utilidades.mensajeInformacion('Detalle de la cuantificación de productos, tanto para metas como para recursos.'
                , false, "Programación")
        }

        $scope.$watch('vm.calendarioproductos', function () {
            if (vm.calendarioproductos !== undefined && vm.calendarioproductos !== '')
                vm.habilitaBotones = vm.calendarioproductos === 'true' && !$sessionStorage.soloLectura ? true : false;

        });

        $scope.$watch('vm.modificardistribucionprod', function () {
            if (vm.modificardistribucionprod === '1') {

                vm.modificardistribucionprod = '0';
                ObtenerDatosProgramacionProducto();
            }

        });

        function CancelarValores(productos) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                vm.EAjCT = false;

                return comunesServicio.ObtenerDatosProgramacionProducto(vm.tramiteid).then(
                    function (respuesta) {
                        if (respuesta.data !== '') {
                            $scope.productos = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                            utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                        }
                        else {
                            $scope.productos = [];
                        }
                    });
            }, function funcionCancelar(reason) {
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function EditarValores(productos) {
            vm.EAjCT = true;
        }

        function GuardarValores(productos) {
            let Programacion = {};
            let ValoresProducto = [];


            angular.forEach(productos.Productos, function (series) {
                let valores = {
                    ProductCatalogId: series.ProductCatalogId,
                    Meta: series.MetaAprobado,
                    Recurso: series.RecursoAprobado
                };

                ValoresProducto.push(valores);
            });

            ObtenerSeccionCapitulo();
            Programacion.TramiteId = productos.TramiteId;
            Programacion.NivelId = vm.idNivel;
            Programacion.SeccionCapitulo = vm.seccionCapitulo;
            Programacion.ProgramacionProductos = ValoresProducto;

            return comunesServicio.GuardarDatosProgramacionProducto(Programacion).then(
                function (respuesta) {
                    if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        if (respuesta.data.Exito) {
                            guardarCapituloModificado();
                            vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                            utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito");
                            vm.EAjCT = false;
                            vm.modificodatos = '2';
                            vm.init();
                        }
                        else {
                            utilidades.mensajeError(respuesta.data.Mensaje);
                        }
                    } else {
                        utilidades.mensajeError("", null, "Error al realizar la operación");
                    }
                });
        }

        function guardarCapituloModificado() {
            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        vm.actualizaFila = function (event, productos, campo, index) {
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

            if (campo === 'MetaAprobado') productos.Productos[index].MetaAprobado = event.target.value;
            if (campo === 'RecursoAprobado') productos.Productos[index].RecursoAprobado = event.target.value;

            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
        }

        //para guardar los capitulos modificados y que se llenen las lunas

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
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

        /* ------------------------ Validaciones ---------------------------------*/

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
            vm.showAlertError(nombreComponente, esValido);
        }

        /**
         * Función que visualiza alerta de error tab de componente
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
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

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    if (erroresJson != undefined) {
                        isValid = (erroresJson == null || erroresJson.length == 0);
                        if (!isValid) {
                            vm.erroresJson = erroresJson;
                            erroresJson[vm.nombreComponente].forEach(p => {

                                if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                            });
                        }
                    }
                }

                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.limpiarErrores = function () {
            var campoObligatorioprogramacion1 = document.getElementById(vm.nombreComponente + "-error1");
            if (campoObligatorioprogramacion1 != undefined) {
                campoObligatorioprogramacion1.innerHTML = "";
                campoObligatorioprogramacion1.classList.add('hidden');
            }

            var campoObligatorioprogramacion2 = document.getElementById(vm.nombreComponente + "-error2");
            if (campoObligatorioprogramacion2 != undefined) {
                campoObligatorioprogramacion2.innerHTML = "";
                campoObligatorioprogramacion2.classList.add('hidden');
            }

            var campoObligatorioprogramacion3 = document.getElementById(vm.nombreComponente + "-error3");
            if (campoObligatorioprogramacion3 != undefined) {
                campoObligatorioprogramacion3.innerHTML = "";
                campoObligatorioprogramacion3.classList.add('hidden');
            }

            var campoObligatorioprogramacion4 = document.getElementById(vm.nombreComponente + "-error4");
            if (campoObligatorioprogramacion4 != undefined) {
                campoObligatorioprogramacion4.innerHTML = "";
                campoObligatorioprogramacion4.classList.add('hidden');
            }

            if ($scope.productos.Productos !== undefined && $scope.productos.Productos !== null)
                $scope.productos.Productos.forEach(p => {
                    var campoObligatorioJustificacion2Detalle = document.getElementById(vm.nombreComponente + "-" + p.ProductCatalogId);
                    if (campoObligatorioJustificacion2Detalle != undefined) {
                        campoObligatorioJustificacion2Detalle.innerHTML = "";
                        campoObligatorioJustificacion2Detalle.classList.add('hidden');
                    }
                }
                );
        }

        vm.validarValoresProyectosRegistrados1 = function (errores) {
            var campoObligatorioprogramacion1 = document.getElementById(vm.nombreComponente + "-error1");
            if (campoObligatorioprogramacion1 != undefined) {
                campoObligatorioprogramacion1.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span><br />";
                campoObligatorioprogramacion1.classList.remove('hidden');
            }
        }

        vm.validarValoresProyectosRegistrados2 = function (errores) {
            var campoObligatorioprogramacion2 = document.getElementById(vm.nombreComponente + "-error2");
            if (campoObligatorioprogramacion2 != undefined) {
                campoObligatorioprogramacion2.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span><br />";
                campoObligatorioprogramacion2.classList.remove('hidden');
            }
        }

        vm.validarValoresProyectosRegistrados3 = function (errores) {
            var campoObligatorioprogramacion3 = document.getElementById(vm.nombreComponente + "-error3");
            if (campoObligatorioprogramacion3 != undefined) {
                campoObligatorioprogramacion3.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span><br />";
                campoObligatorioprogramacion3.classList.remove('hidden');
            }
        }

        vm.validarValoresProyectosRegistrados4 = function (errores) {
            var campoObligatorioprogramacion4 = document.getElementById(vm.nombreComponente + "-error4");
            if (campoObligatorioprogramacion4 != undefined) {
                campoObligatorioprogramacion4.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span><br />";
                campoObligatorioprogramacion4.classList.remove('hidden');
            }
        }

        vm.validarValoresProyectosRegistrados2Detalle = function (errores) {
            var campoObligatorioJustificacion2Detalle = document.getElementById(vm.nombreComponente + "-" + errores);
            if (campoObligatorioJustificacion2Detalle != undefined) {
                campoObligatorioJustificacion2Detalle.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion2Detalle.classList.remove('hidden');
            }
        }

        vm.errores = {
            'PDI001': vm.validarValoresProyectosRegistrados1,
            'PDI002': vm.validarValoresProyectosRegistrados2,
            'PDI003': vm.validarValoresProyectosRegistrados3,
            'PDI004': vm.validarValoresProyectosRegistrados4,
            'PDI002-': vm.validarValoresProyectosRegistrados2Detalle,
            'PDI003-': vm.validarValoresProyectosRegistrados2Detalle,
            'PDI004-': vm.validarValoresProyectosRegistrados2Detalle,
        }

        /* ------------------------ FIN Validaciones ---------------------------------*/
    }

    angular.module('backbone').component('programacionProductosFormularioPasoTres', {

        templateUrl: "src/app/formulario/ventanas/comun/programacionRecursos/productos/programacionProductosFormularioPasoTres.html",
        controller: programacionProductosFormularioPasoTres,
        controllerAs: "vm",
        bindings: {
            tramiteproyectoid: '@',
            origen: '@',
            modificodatos: '=',
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            actualizacomponentes: '@',
            calendarioproductos: '@',
            modificardistribucionprod: '=',
        }
    })
})();
