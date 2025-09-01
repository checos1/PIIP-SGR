(function () {
    'use strict';

    infoPresupuestalPoliticasMlFormulario.$inject = [
        '$sessionStorage',
        '$scope',
        'utilidades',
        '$uibModal',
        'modificacionLeyServicio',
        'comunesServicio'
    ];

    function infoPresupuestalPoliticasMlFormulario(
        $sessionStorage,
        $scope,
        utilidades,
        $uibModal,
        modificacionLeyServicio,
        comunesServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.EntidadDestinoId = $sessionStorage.InstanciaSeleccionada.entidadId;
        vm.ObjetoVerMas = ObjetoVerMas;
        vm.nombreComponente = "focalizacionfocalizacionfocalizacionpoliticastransversales";
        vm.notificacionCambiosCapitulos = null;
        vm.TramiteProyectoId = undefined;
        vm.mostrarPasoUno = false;
        vm.mostrarPasoTres = false;
        vm.mostrarPasoTresA = false;

        /*declara metodos*/
        vm.ConvertirNumero = ConvertirNumero;
        vm.abrirMensajeInformacion = abrirMensajeInformacion;

        vm.handlerComponentes = [];
        vm.handlerComponentesChecked = {};
        vm.habilitaBotones = $sessionStorage.nombreAccion.includes('Asociar Proyectos') && !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1

        $scope.$watch('vm.tramiteid', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                ObtenerSolicitudProyectosAsociadosTad();
            }
        });

        $scope.$watch('vm.modificodatos', function () {
            if (vm.modificodatos === '1') {
                ObtenerSolicitudProyectosAsociadosTad();
                vm.modificodatos = '0';
            }
        });

        $scope.$watch('vm.section', function () {
            switch (vm.section) {
                case 'Adición Presupuesto Paso1':
                    vm.mostrarPasoUno = true;
                    vm.mostrarPasoTres = false;
                    vm.mostrarPasoTresA = false;
                    break;
                case 'Adición Presupuesto Paso3':
                    vm.mostrarPasoUno = false;
                    vm.mostrarPasoTres = true;
                    vm.mostrarPasoTresA = false;
                    break;
                case 'Adición Analista paso3':
                    vm.mostrarPasoUno = false;
                    vm.mostrarPasoTres = false;
                    vm.mostrarPasoTresA = true;
                    break;
                default:
                    break;
            }
        });

        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };

        function ObtenerSolicitudProyectosAsociadosTad() {
            return modificacionLeyServicio.ObtenerInformacionPresupuestalMLEncabezado(vm.EntidadDestinoId, vm.tramiteid, vm.section).then(
                function (respuesta) {
                    if (respuesta.data !== '') {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        $scope.datos = jQuery.parseJSON(arreglolistas);
                    }
                    else {
                        $scope.datos = [];
                    }
                });
        }

        function ObjetoVerMas(resumen) {
            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModal.html',
                controller: 'objetivosIndicadorModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    Objetivo: function () {
                        return resumen.NombreProyecto;
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

        vm.changeBotonAsociados = function (asociados) {
            if (asociados.LabelBoton == '+') {
                asociados.LabelBoton = '-';
                comunesServicio.setProyectoCargado(asociados.ProyectoId);
                comunesServicio.setBpinCargado(asociados.CodigoBpin);
                vm.TramiteProyectoId = asociados.TramiteProyectoId;
            } else {
                asociados.LabelBoton = '+';
            }
            return asociados.LabelBoton;
        }

        function abrirMensajeInformacion() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' > Objetivos específicos</span>");
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
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
            vm.guardadocomponent({ nombreComponenteHijo: vm.nombreComponenteHijo });

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
            vm.limpiarErrores();
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                if (erroresJson != undefined) {
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.limpiarErrores = function () {

            let elementos = document.querySelectorAll('[id ^= "MLA-"]');
            if (elementos !== undefined) {
                let i;
                for (i = 0; i < elementos.length; i++) {
                    var idSpanAlertComponent = elementos[i];
                    if (idSpanAlertComponent != undefined) {
                        idSpanAlertComponent.innerHTML = "";
                        idSpanAlertComponent.classList.add('hidden');
                    }
                }
            }

            var campoObligatorioInformacionPresupuestal1 = document.getElementById("IPF001-error");
            if (campoObligatorioInformacionPresupuestal1 != undefined) {
                campoObligatorioInformacionPresupuestal1.innerHTML = "";
                campoObligatorioInformacionPresupuestal1.classList.add('hidden');
            }

            var campoObligatorioInformacionPresupuestal2 = document.getElementById("IPF002-error");
            if (campoObligatorioInformacionPresupuestal2 != undefined) {
                campoObligatorioInformacionPresupuestal2.innerHTML = "";
                campoObligatorioInformacionPresupuestal2.classList.add('hidden');
            }

            var campoObligatorioInformacionPresupuestal3 = document.getElementById("IPF003-error");
            if (campoObligatorioInformacionPresupuestal3 != undefined) {
                campoObligatorioInformacionPresupuestal3.innerHTML = "";
                campoObligatorioInformacionPresupuestal3.classList.add('hidden');
            } 

            var campoObligatorioInformacionPresupuestal4 = document.getElementById("IPF004-error");
            if (campoObligatorioInformacionPresupuestal4 != undefined) {
                campoObligatorioInformacionPresupuestal4.innerHTML = "";
                campoObligatorioInformacionPresupuestal4.classList.add('hidden');
            }



            if ($scope.datos.ProyectosAsociados !== undefined && $scope.datos.ProyectosAsociados !== null) {
                $scope.datos.ProyectosAsociados.forEach(p => {

                    var campoObligatorioInformacionPresupuestal = document.getElementById("IPF001-" + p.TramiteProyectoId);
                    if (campoObligatorioInformacionPresupuestal != undefined) {
                        campoObligatorioInformacionPresupuestal.innerHTML = "";
                        campoObligatorioInformacionPresupuestal.classList.add('hidden');
                    }
                });
            }
        }

        vm.validarIPF001 = function (errores) {
            var campoObligatorioInformacionPresupuestal = document.getElementById("IPF001-error");
            if (campoObligatorioInformacionPresupuestal != undefined) {
                campoObligatorioInformacionPresupuestal.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioInformacionPresupuestal.classList.remove('hidden');
            }
        }

        vm.validarIPF002 = function (errores) {
            var campoObligatorioInformacionPresupuestal2 = document.getElementById("IPF002-error");
            if (campoObligatorioInformacionPresupuestal2 != undefined) {
                campoObligatorioInformacionPresupuestal2.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioInformacionPresupuestal2.classList.remove('hidden');
            }
        }

        vm.validarIPF003 = function (errores) {
            var campoObligatorioInformacionPresupuestal = document.getElementById("IPF003-error");
            if (campoObligatorioInformacionPresupuestal != undefined) {
                campoObligatorioInformacionPresupuestal.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioInformacionPresupuestal.classList.remove('hidden');
            }
        }

        vm.validarIPF004 = function (errores) {
            var campoObligatorioInformacionPresupuestal = document.getElementById("IPF004-error");
            if (campoObligatorioInformacionPresupuestal != undefined) {
                campoObligatorioInformacionPresupuestal.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioInformacionPresupuestal.classList.remove('hidden');
            }
        }

        vm.validarIPF001Grill = function (errores) {
            var campoObligatorioInformacionPresupuestal = document.getElementById("IPF001-" + errores);
            if (campoObligatorioInformacionPresupuestal != undefined) {
                campoObligatorioInformacionPresupuestal.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioInformacionPresupuestal.classList.remove('hidden');
            }
        }

        vm.validarIPF003Grill = function (errores) {
            var campoObligatorioInformacionPresupuestal = document.getElementById("IPF003-" + errores);
            if (campoObligatorioInformacionPresupuestal != undefined) {
                campoObligatorioInformacionPresupuestal.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioInformacionPresupuestal.classList.remove('hidden');
            }
        }

        vm.validarIPF004Grill = function (errores) {
            var campoObligatorioInformacionPresupuestal = document.getElementById("IPF004-" + errores);
            if (campoObligatorioInformacionPresupuestal != undefined) {
                campoObligatorioInformacionPresupuestal.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioInformacionPresupuestal.classList.remove('hidden');
            }
        }

        vm.validarModificacionLey = function (errores) {
            var campoObligatorioModificacionLey = document.getElementById("MLA-" + errores);
            if (campoObligatorioModificacionLey != undefined) {
                campoObligatorioModificacionLey.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioModificacionLey.classList.remove('hidden');
            }
        }

        vm.errores = {
            'IPF001': vm.validarIPF001,
            'IPF001-': vm.validarIPF001Grill,//Este sirve para el error del 1,2 y del 4 del proyecto para que no se repitan los iconos
            'IPF002': vm.validarIPF002,
            'IPF002-': vm.validarIPF001Grill,
            'IPF002--': vm.validarModificacionLey,
            'IPF003-': vm.validarIPF003,
            'IPF003--': vm.validarIPF003Grill,
            'IPF004-': vm.validarIPF003,
            'IPF004--': vm.validarIPF003Grill,
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

        /*------------------------------------Fin Validaciones-----------------------------------*/
    }

    angular.module('backbone').component('infoPresupuestalPoliticasMlFormulario', {
        templateUrl: "src/app/formulario/ventanas/comun/modificacionLey/politicas/infoPresupuestalPoliticasMlFormulario.html",
        controller: infoPresupuestalPoliticasMlFormulario,
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
            modificodatos: '=',
            actualizacomponentes: '@'
        }
    })
})();