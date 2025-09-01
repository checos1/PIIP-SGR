(function () {
    'use strict';

    modificacionLeyCreditosController.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        '$timeout',
        'justificacionCambiosServicio',
        'modificacionLeyServicio'
    ];

    function modificacionLeyCreditosController(
        $scope,
        $sessionStorage,
        utilidades,
        $timeout,
        justificacionCambiosServicio,
        modificacionLeyServicio
    ) {
        ///*Varibales */
        var vm = this;
        vm.lang = "es";
        vm.Editable = false;
        vm.nombreComponente = "informacionpresupuestalsolicitudmodificaciondeleyadicion";
        vm.idNivel = $sessionStorage.idNivel;
        vm.seccionCapitulo = null;

        vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1

        /*declara metodos*/

        vm.CancelarValores = CancelarValores;
        vm.EditarValores = EditarValores;
        vm.GuardarValores = GuardarValores;
        vm.ConvertirNumero = ConvertirNumero;

        $scope.$watch('vm.tramiteproyectoid', function () {
            if (vm.tramiteproyectoid != '') {
                ObtenerInformacionPresupuestalDetalle();
                switch (vm.origen) {
                    case 'Adición Presupuesto Paso1':
                        vm.nombreComponente = "informacionpresupuestalsolicitudmodificaciondeleyadicion";
                        break;
                    case 'Reducción presupuesto paso1':
                        vm.nombreComponente = "informacionpresupuestalsolicitudmodificaciondeleyreduccion";
                        break;
                    default:
                        break;
                }
            }
        });    

        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        };

        function ObtenerInformacionPresupuestalDetalle() {
            return modificacionLeyServicio.ObtenerInformacionPresupuestalMLDetalle(vm.tramiteproyectoid, vm.origen).then(
                function (respuesta) {
                    if (respuesta.data !== '') {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        $scope.fuentes = jQuery.parseJSON(arreglolistas);
                    }
                    else {
                        $scope.fuentes = [];
                    }
                });
        }

        function CancelarValores() {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                vm.Editable = false;

                return modificacionLeyServicio.ObtenerInformacionPresupuestalMLDetalle(vm.tramiteproyectoid, vm.origen).then(
                    function (respuesta) {
                        if (respuesta.data !== '') {
                            var arreglolistas = jQuery.parseJSON(respuesta.data);
                            $scope.fuentes = jQuery.parseJSON(arreglolistas);
                            utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                        }
                        else {
                            $scope.fuentes = [];
                        }
                    });
            }, function funcionCancelar(reason) {
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function EditarValores() {
            vm.Editable = true;
        }

        function GuardarValores(fuentes) {
            let Creditos = {};
            let ValoresFuente = [];

            angular.forEach(fuentes.Creditos, function (series) {
                let valores = {
                    FuenteId: series.fuenteid,
                    NacionCSF: series.origen == 'N' ? series.ValorAcreditarCSF : 0,
                    NacionSSF: series.origen == 'N' ? series.ValorAcreditarSSF : 0,
                    Propios: series.origen == 'P' ? series.ValorAcreditarCSF: 0
                };

                ValoresFuente.push(valores);
            });

            ObtenerSeccionCapitulo();
            Creditos.TramiteProyectoId = fuentes.TramiteProyectoId;
            Creditos.NivelId = vm.idNivel;
            Creditos.SeccionCapitulo = vm.seccionCapitulo;
            Creditos.Origen = vm.origen;
            Creditos.ValoresFuente = ValoresFuente;

            return modificacionLeyServicio.GuardarInformacionPresupuestalML(Creditos).then(
                function (respuesta) {
                    if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        if (respuesta.data.Exito) {
                            guardarCapituloModificado();
                            vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                            utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito");
                            vm.Editable = false;
                            vm.modificodatos = '1';
                            vm.init();
/*                            ObtenerInformacionPresupuestalDetalle();*/
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

            event.target.value = event.target.value.replace(",", ".");

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 16;
            var tamanio = event.target.value.length;
            var decimal = false;
            decimal = event.target.value.toString().includes(".");
            if (decimal) {
                tamanioPermitido = 19;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    event.target.value = newValue;
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
                else {
                    var n2 = "";
                    newValue = [n[0], n2].join(".");
                    event.target.value = newValue;
                }
            }
            else {
                if (tamanio > tamanioPermitido && event.keyCode != 44) {
                    event.target.value = event.target.value.slice(0, tamanioPermitido);
                    event.preventDefault();
                }
            }
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }
    }

    angular.module('backbone').component('modificacionLeyCreditos', {

        templateUrl: "src/app/formulario/ventanas/comun/modificacionLey/creditos/modificacionLeyCreditos.html",
        controller: modificacionLeyCreditosController,
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
            modificodatos: '=',
        }
    })
})();
