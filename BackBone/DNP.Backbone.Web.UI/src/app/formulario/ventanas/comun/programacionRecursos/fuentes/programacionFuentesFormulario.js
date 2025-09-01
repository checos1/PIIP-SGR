(function () {
    'use strict';

    programacionFuentesFormulario.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        '$timeout',
        'justificacionCambiosServicio',
        'comunesServicio'
    ];

    function programacionFuentesFormulario(
        $scope,
        $sessionStorage,
        utilidades,
        $timeout,
        justificacionCambiosServicio,
        comunesServicio
    ) {
        ///*Varibales */
        var vm = this;
        vm.lang = "es";
        vm.EAjCT = false;
        vm.nombreComponente = "fuentesfuentesdefinanc";
        vm.idNivel = $sessionStorage.idNivel;
        vm.seccionCapitulo = null;
        vm.mostrarCreditosPr = false;
        vm.mostrarCreditosFir = false;
        vm.mostrarDonacion = false;

        vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1

        /*declara metodos*/

        vm.CancelarValores = CancelarValores;
        vm.EditarValores = EditarValores;
        vm.GuardarValores = GuardarValores;
        vm.ConvertirNumero = ConvertirNumero;

        $scope.$watch('vm.tramiteproyectoid', function () {
            if (vm.tramiteproyectoid != '') {
                ObtenerDatosProgramacionDetalle();
            }
        });

        $scope.$watch('vm.calendariofuentes', function () {
            if (vm.calendariofuentes !== undefined && vm.calendariofuentes !== '')
                vm.habilitaBotones = vm.calendariofuentes === 'true' && !$sessionStorage.soloLectura ? true : false;

        });


        $scope.$watch('vm.modificardistribucion', function () {
            if (vm.modificardistribucion === '3') {
                ObtenerDatosProgramacionDetalle();
                vm.modificardistribucion = '4';
            }

        });

        vm.init = function () {
        };

        function ObtenerDatosProgramacionDetalle() {
            return comunesServicio.ObtenerDatosProgramacionDetalle(vm.tramiteproyectoid, vm.origen).then(
                function (respuesta) {
                    if (respuesta.data !== '') {
                        $scope.fuentes = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                        vm.mostrarCreditosPr = ($scope.fuentes.CreditosEnPreparacion != null) ? true : false;
                        vm.mostrarCreditosFir = ($scope.fuentes.CreditosFirmado != null) ? true : false;
                        vm.mostrarDonacion = ($scope.fuentes.DonacionFirmado != null) ? true : false;
                    }
                    else {
                        $scope.fuentes = [];
                    }                    
                });
        }

        function CancelarValores(fuentes) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                vm.EAjCT = false;

                return comunesServicio.ObtenerDatosProgramacionDetalle(vm.tramiteproyectoid, vm.origen).then(
                    function (respuesta) {
                        if (respuesta.data !== '') {
                            $scope.fuentes = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                            utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                        }
                        else {
                            $scope.fuentes = [];
                        }
                    });
            }, function funcionCancelar(reason) {
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function EditarValores(fuentes) {
            vm.EAjCT = true;
        }

        function GuardarValores(fuentes) {
            let Programacion = {};
            let ValoresFuente = [];
            let ValoresCredito = [];

            angular.forEach(fuentes.Recursos, function (series) {
                if (series.Codigo !== '13' && series.Codigo !== '14' && series.Codigo !== '15') {
                    let valores = {
                        FuenteId: series.fuenteid,
                        NacionCSF: series.NacionCSF,
                        NacionSSF: series.NacionSSF,
                        Propios: series.Propios
                    };

                    ValoresFuente.push(valores);
                }
            });

            if (fuentes.CreditosEnPreparacion != null) {
                angular.forEach(fuentes.CreditosEnPreparacion[0].CreditosEnPreparacionDetalle, function (series) {

                    let valores = {
                        FuenteId: fuentes.CreditosEnPreparacion[0].fuenteid,
                        CreditoId: series.Creditoid,
                        NacionCSF: series.NacionCSF,
                        NacionSSF: series.NacionSSF
                    };

                    ValoresCredito.push(valores);
                });
            }
            if (fuentes.CreditosFirmado != null) {
                angular.forEach(fuentes.CreditosFirmado[0].CreditosFirmadoDetalle, function (series) {

                    let valores = {
                        FuenteId: fuentes.CreditosFirmado[0].fuenteid,
                        CreditoId: series.Creditoid,
                        NacionCSF: series.NacionCSF,
                        NacionSSF: series.NacionSSF
                    };

                    ValoresCredito.push(valores);
                });
            }

            if (fuentes.DonacionFirmado != null) {
                angular.forEach(fuentes.DonacionFirmado[0].DonacionDetalle, function (series) {

                    let valores = {
                        FuenteId: fuentes.DonacionFirmado[0].fuenteid,
                        CreditoId: series.Creditoid,
                        NacionCSF: series.NacionCSF,
                        NacionSSF: series.NacionSSF
                    };

                    ValoresCredito.push(valores);
                });
            }

            ObtenerSeccionCapitulo();
            Programacion.TramiteProyectoId = fuentes.TramiteProyectoId;
            Programacion.NivelId = vm.idNivel;
            Programacion.SeccionCapitulo = vm.seccionCapitulo;
            Programacion.ValoresFuente = ValoresFuente;
            Programacion.ValoresCredito = ValoresCredito;

            return comunesServicio.GuardarDatosProgramacionFuentes(Programacion).then(
                function (respuesta) {
                    if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        if (respuesta.data.Exito) {
                            guardarCapituloModificado();
                            vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                            utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito");
                            vm.EAjCT = false;
                            vm.modificardistribucion = '1';
                            vm.init();
                            ObtenerDatosProgramacionDetalle();
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

        //para guardar los capitulos modificados y que se llenen las lunas

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 12;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 15;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 4);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 4) {
                        tamanioPermitido = n[0].length + 4;
                        event.target.value = n[0] + '.' + n[1].slice(0, 4);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            } else {
                if (tamanio > 12 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 12) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 15) {
                    event.preventDefault();
                }
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

        vm.actualizaFila = function (event, fuentes, campo, index) {
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

            var TNacionCSF = 0;
            var TNacionSSF = 0;
            var TPropios = 0;

            if (campo === 'NacionCSF') fuentes.Recursos[index].NacionCSF = event.target.value;
            if (campo === 'NacionSSF') fuentes.Recursos[index].NacionSSF = event.target.value;
            if (campo === 'Propios') fuentes.Recursos[index].Propios = event.target.value;

            angular.forEach(fuentes.Recursos, function (series) {
                series.TotalTR = parseFloat(series.NacionCSF) + parseFloat(series.NacionSSF) + parseFloat(series.Propios);
                TNacionCSF += parseFloat(series.NacionCSF);
                TNacionSSF += parseFloat(series.NacionSSF);
                TPropios += parseFloat(series.Propios);
            });

            fuentes.TNacionCSF = TNacionCSF;
            fuentes.TNacionSSF = TNacionSSF;
            fuentes.TPropios = TPropios;

            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
        }

        vm.actualizaFilaCredito = function (event, fuentes, tipo, campo, index) {
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

            var TNacionCSF = 0;
            var TNacionSSF = 0;

            switch (tipo) {
                case 1:
                    angular.forEach(fuentes.CreditosEnPreparacion, function (series) {

                        if (campo === 'NacionCSF') series.CreditosEnPreparacionDetalle[index].NacionCSF = event.target.value;
                        if (campo === 'NacionSSF') series.CreditosEnPreparacionDetalle[index].NacionSSF = event.target.value;

                        angular.forEach(series.CreditosEnPreparacionDetalle, function (seriesdetalle) {
                            TNacionCSF += parseFloat(seriesdetalle.NacionCSF);
                            TNacionSSF += parseFloat(seriesdetalle.NacionSSF);
                        })
                        series.TNacionCSF = parseFloat(TNacionCSF);
                        series.TNacionSSF = parseFloat(TNacionSSF);

                    });
                    break;
                case 2:
                    angular.forEach(fuentes.CreditosFirmado, function (series) {

                        if (campo === 'NacionCSF') series.CreditosFirmadoDetalle[index].NacionCSF = event.target.value;
                        if (campo === 'NacionSSF') series.CreditosFirmadoDetalle[index].NacionSSF = event.target.value;

                        angular.forEach(series.CreditosFirmadoDetalle, function (seriesdetalle) {
                            TNacionCSF += parseFloat(seriesdetalle.NacionCSF);
                            TNacionSSF += parseFloat(seriesdetalle.NacionSSF);
                        })
                        series.TNacionCSF = parseFloat(TNacionCSF);
                        series.TNacionSSF = parseFloat(TNacionSSF);

                    });
                    break;
                case 3:
                    angular.forEach(fuentes.DonacionFirmado, function (series) {

                        if (campo === 'NacionCSF') series.DonacionDetalle[index].NacionCSF = event.target.value;
                        if (campo === 'NacionSSF') series.DonacionDetalle[index].NacionSSF = event.target.value;

                        angular.forEach(series.DonacionDetalle, function (seriesdetalle) {
                            TNacionCSF += parseFloat(seriesdetalle.NacionCSF);
                            TNacionSSF += parseFloat(seriesdetalle.NacionSSF);
                        })
                        series.TNacionCSF = parseFloat(TNacionCSF);
                        series.TNacionSSF = parseFloat(TNacionSSF);

                    });
                    break;
            }

            //fuentes.TNacionCSF = TNacionCSF;
            //fuentes.TNacionSSF = TNacionSSF;
            //fuentes.TPropios = TPropios;

            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }
    }

    angular.module('backbone').component('programacionFuentesFormulario', {

        templateUrl: "src/app/formulario/ventanas/comun/programacionRecursos/fuentes/programacionFuentesFormulario.html",
        controller: programacionFuentesFormulario,
        controllerAs: "vm",
        bindings: {
            tramiteproyectoid: '@',
            origen: '@',
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            nivel: '@',
            rol: '@',
            actualizacomponentes: '@',
            calendariofuentes: '@',
            modificardistribucion:'=',
        }
    })
})();
