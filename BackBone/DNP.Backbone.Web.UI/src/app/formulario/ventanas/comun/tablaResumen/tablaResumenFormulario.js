(function () {
    'use strict';

    tablaResumenFormulario.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        'trasladosServicio',
        '$timeout',
        'justificacionCambiosServicio',
        'comunesServicio'
    ];

    function tablaResumenFormulario(
        $scope,
        $sessionStorage,
        utilidades,
        trasladosServicio,
        $timeout,
        justificacionCambiosServicio,
        comunesServicio
    ) {

        var vm = this;

        /*Varibales */
        vm.instanciaId = $sessionStorage.instanciaId;
        vm.proyectoId = undefined;
        vm.tramiteid = undefined;
        vm.ConvertirNumero = ConvertirNumero;
        vm.pagina = 1;
        vm.Origen = 0;
        vm.nombreComponente = "reprogramacionvfreprogramacionporproducto";
        vm.handlerComponentes = [];
        vm.handlerComponentesChecked = {};

        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
                // ObtenerSeccionCapitulo();
            }
        });


        $scope.$watch('$sessionStorage.TramiteId', function () {
            if ($sessionStorage.TramiteId !== '' && $sessionStorage.TramiteId !== undefined && $sessionStorage.TramiteId !== null) {
                vm.tramiteid = $sessionStorage.TramiteId;
                cargaVariables();
            }

        });

        $scope.$watch('$sessionStorage.proyectoId', function () {
            if ($sessionStorage.proyectoId !== '' && $sessionStorage.proyectoId !== undefined && $sessionStorage.proyectoId !== null) {
                vm.proyectoId = $sessionStorage.proyectoId;
                cargaVariables();
            }

        });

        $scope.$watch('$sessionStorage.idInstancia', function () {
            if ($sessionStorage.idInstancia !== '' && $sessionStorage.idInstancia !== undefined && $sessionStorage.idInstancia !== null) {
                vm.instanciaId = $sessionStorage.idInstancia;
                cargaVariables();
            }

        });

        $scope.$watch('vm.modificodatos', function () {
            if (vm.modificodatos === '1') {
                cargaVariables();
                vm.modificodatos = '0';
            }

        });


        $scope.$watch('vm.vigenciaadicionada', function () {
            if (vm.vigenciaadicionada === '1') {
                cargaVariables();
                vm.vigenciaadicionada = '0';
            }

        });




        function cargaVariables() {
            if (vm.proyectoId !== undefined && vm.tramiteid !== undefined && vm.instanciaId !== undefined)
                ObtenerResumenReprogramacionPorVigencia();
        }

        function ObtenerResumenReprogramacionPorVigencia() {
            return comunesServicio.obtenerResumenReprogramacionPorProductoVigencia(vm.instanciaId, vm.proyectoId, vm.tramiteid).then(
                function (respuesta) {
                    if (respuesta.data !== '' && respuesta.data !== 'null' && respuesta.data !== null && respuesta.data !== undefined) {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.datos = jQuery.parseJSON(arreglolistas);
                        vm.Origen = 1;
                        if (!vm.datos.EsConstante) {
                            vm.datos.ResumenTramite[0].ValoresCorrientes = vm.datos.ResumenTramite[0].Valores;
                            vm.Origen = 2;
                        }
                        cargaTotales();
                    }
                    else {
                        vm.datos = {};
                    }
                });
        }


        /*declara metodos*/
        vm.initTablaResumen = initTablaResumen;
        vm.listaVigencias = [];
        vm.valoresTotales = {
            ValorUtilizadoNacion: 0,
            ValorUtilizadoPropios: 0,
            ValorReprogramadoNacion: 0,
            ValorReprogramadoPropios: 0,
            ValorReprogramadoProductoNacion: 0,
            ValorReprogramadoProductoPropios: 0,
            TotalValorUtilizado: 0,
            TotalValorReprogramado: 0,
            TotalValorReprogramadoProducto: 0
        };
        vm.ValoresTotalesCorrientes = {
            ValorUtilizadoNacion: 0,
            ValorUtilizadoPropios: 0,
            ValorReprogramadoNacion: 0,
            ValorReprogramadoPropios: 0,
            ValorReprogramadoProductoNacion: 0,
            ValorReprogramadoProductoPropios: 0,
            TotalValorUtilizado: 0,
            TotalValorReprogramado: 0,
            TotalValorReprogramadoProducto: 0
        };

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;


        /*Funciones*/




        function cargaTotales() {
            if (vm.datos != undefined) {
                limpiaValores();
                if (vm.datos.EsConstante) {
                    vm.datos.ResumenTramite[0].Valores.map(function (item) {
                        vm.valoresTotales.ValorUtilizadoNacion += item.UtilizadoNacion;
                        vm.valoresTotales.ValorUtilizadoPropios += item.UtilizadoPropios;
                        vm.valoresTotales.ValorReprogramadoNacion += item.ReprogramadoNacion;
                        vm.valoresTotales.ValorReprogramadoPropios += item.ReprogramadoPropios;
                        vm.valoresTotales.ValorReprogramadoProductoNacion += item.ReprogramadoNacionPorProducto;
                        vm.valoresTotales.ValorReprogramadoProductoPropios += item.ReprogramadoPropiosPorProducto;
                        vm.valoresTotales.TotalValorUtilizado += (item.UtilizadoNacion + item.UtilizadoPropios);
                        vm.valoresTotales.TotalValorReprogramado += (item.ReprogramadoNacion + item.ReprogramadoPropios);
                        vm.valoresTotales.TotalValorReprogramadoProducto += (item.ReprogramadoNacionPorProducto + item.ReprogramadoPropiosPorProducto);
                    });
                }
                else
                    vm.datos.ResumenTramite[0].ValoresCorrientes = vm.datos.ResumenTramite[0].Valores;

                vm.datos.ResumenTramite[0].ValoresCorrientes.map(function (item) {
                    vm.ValoresTotalesCorrientes.ValorUtilizadoNacion += item.UtilizadoNacion;
                    vm.ValoresTotalesCorrientes.ValorUtilizadoPropios += item.UtilizadoPropios;
                    vm.ValoresTotalesCorrientes.ValorReprogramadoNacion += item.ReprogramadoNacion;
                    vm.ValoresTotalesCorrientes.ValorReprogramadoPropios += item.ReprogramadoPropios;
                    vm.ValoresTotalesCorrientes.ValorReprogramadoProductoNacion += item.ReprogramadoNacionPorProducto;
                    vm.ValoresTotalesCorrientes.ValorReprogramadoProductoPropios += item.ReprogramadoPropiosPorProducto;
                    vm.ValoresTotalesCorrientes.TotalValorUtilizado += (item.UtilizadoNacion + item.UtilizadoPropios);
                    vm.ValoresTotalesCorrientes.TotalValorReprogramado += (item.ReprogramadoNacion + item.ReprogramadoPropios);
                    vm.ValoresTotalesCorrientes.TotalValorReprogramadoProducto += (item.ReprogramadoNacionPorProducto + item.ReprogramadoPropiosPorProducto);
                });

            }
        }

        function limpiaValores() {
            vm.valoresTotales.ValorUtilizadoNacion = 0;
            vm.valoresTotales.ValorUtilizadoPropios = 0;
            vm.valoresTotales.ValorReprogramadoNacion = 0;
            vm.valoresTotales.ValorReprogramadoPropios = 0;
            vm.valoresTotales.ValorReprogramadoProductoNacion = 0;
            vm.valoresTotales.ValorReprogramadoProductoPropios = 0;
            vm.valoresTotales.TotalValorUtilizado = 0;
            vm.valoresTotales.TotalValorReprogramado = 0;
            vm.valoresTotales.TotalValorReprogramadoProducto = 0;
            vm.ValoresTotalesCorrientes.ValorUtilizadoNacion = 0;
            vm.ValoresTotalesCorrientes.ValorUtilizadoPropios = 0;
            vm.ValoresTotalesCorrientes.ValorReprogramadoNacion = 0;
            vm.ValoresTotalesCorrientes.ValorReprogramadoPropios = 0;
            vm.ValoresTotalesCorrientes.ValorReprogramadoProductoNacion = 0;
            vm.ValoresTotalesCorrientes.ValorReprogramadoProductoPropios = 0;
            vm.ValoresTotalesCorrientes.TotalValorUtilizado = 0;
            vm.ValoresTotalesCorrientes.TotalValorReprogramado = 0;
            vm.ValoresTotalesCorrientes.TotalValorReprogramadoProducto = 0;

        }

        function initTablaResumen() {

            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            //vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });


        }


        vm.mostrarTab = function (origen) {
            vm.pagina = origen;

            switch (origen) {
                case 1:
                    {
                        //Constante
                        vm.Origen = 1;
                        break;
                    }
                case 2:
                    {
                        //Corriente
                        vm.Origen = 2;
                        break;
                    }
            }

            setTimeout(function () {
            }, 200);
        }


        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
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
        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
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



        vm.validarConstantes = function (errores) {
            vm.errores.constante = true;
            var campoObligatorioConstante = document.getElementById(vm.nombreComponente + "-error");
            if (campoObligatorioConstante != undefined) {
                campoObligatorioConstante.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioConstante.classList.remove('hidden');
            }
        }

        vm.validarCorrientes = function (errores) {
            vm.errores.corriente = true;
            var campoObligatorioCorriente = document.getElementById(vm.nombreComponente + "-error");
            if (campoObligatorioCorriente != undefined) {
                campoObligatorioCorriente.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioCorriente.classList.remove('hidden');
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
            vm.notificacionestado({ estado: estado, nombreComponente: vm.nombreComponente });
        });

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
            };
        }



        /* ------------------------ FIN Validaciones ---------------------------------*/



    }

    angular.module('backbone').component('tablaResumenFormulario', {

        templateUrl: "src/app/formulario/ventanas/comun/tablaResumen/tablaResumenFormulario.html",
        controller: tablaResumenFormulario,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
            modificodatos: '=',
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            nombrecomponentepaso: '@',
            vigenciaadicionada: '='

        }
    });
})();
