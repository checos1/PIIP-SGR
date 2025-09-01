(function () {
    'use strict';

    firmaLeyController.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        'trasladosServicio'
    ];

    function firmaLeyController(
        $scope,
        $sessionStorage,
        utilidades,
        trasladosServicio
    ){

        var vm = this;

        /*Varibales */
        vm.nombreArchivo = undefined;
        vm.nombreComponente = "aprobacionfinal";
        vm.nombreComponenteHijo = "aprobacionfinalfirmaconcepto";
        vm.notificacionCambiosCapitulos = null;

        vm.handlerComponentes = [
            { id: 1, componente: 'aprobacionfinalfirmaconcepto', handlerValidacion: null, handlerCambios: null, esValido: true }
        ];
        vm.handlerComponentesChecked = {};


        /*declara metodos*/
        vm.initFirmaConcepto = initFirmaConcepto;
        vm.examinar = examinar;

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'aprobacionfinalfirmaconcepto': true
            };
        }


        /*Funciones*/

        function initFirmaConcepto(){
            //Validaciones
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });
            obtenerFirma();
        }

        function obtenerFirma() {
            vm.firmaConcepto = true;
            trasladosServicio.validarSiExisteFirmaUsuario().then(function (response) {
                if (response.data && response.data.Exito) {
                    var base64img = getBase64Img(response);
                    Base64ToImage(base64img);
                    $timeout(function () {
                        $scope.firmaCargada = true;

                    }, 100);

                } else {
                    $timeout(function () {
                        $scope.firmaCargada = false;
                    }, 100);

                }


            });

        }

        function getBase64Img(response) {
            return "data:image/png;base64," + response.data.Byte64;
        }

        function Base64ToImage(base64img) {
            var img = new Image();
            img.onload = function () {
                const imagen = document.getElementById('main');
                imagen.innerHTML = '';
                imagen.appendChild(img);
                img.setAttribute("style", "width:100%;");
                imagen.setAttribute("style", "background-repeat: no-repeat;width:100%;border: inset");
            };
            img.src = base64img;
        }

        function archivoSeleccionado() {
            var x = trasladosServicio.cargarFirma(vm.data, '643bf52e-a37f-4a75-982b-64450014368a' /*vm.rolUsuario.IdRol*/).then(function (response) {
                if (response.data && response.data.Exito) {
                    parent.postMessage("cerrarModal", window.location.origin);
                    utilidades.mensajeSuccess("Ahora puede visualizarla en el campo 'Última firma cargada'", false, false, false, 'La imagen fué guardada con éxito.');
                    limpiarArchivo();
                    obtenerFirma();
                    $timeout(function () {
                        $scope.firmaCargada = true;
                    }, 100);
                } else {
                    swal('', "Error al realizar la operación", 'error');
                }
            });
        }

        function limpiarArchivo() {
            $scope.files = [];
            document.getElementById('fileFirma').value = "";
            vm.nombreArchivo = undefined;
            vm.desactivarGuardar = true;
            //vm.activarControles('inicio');
        }

        function examinar() {
            document.getElementById('fileFirma').value = "";
            document.getElementById('fileFirma').click();
        }

        $scope.fileFirmaNameChanged = function (input) {
            $scope.files = [];
            vm.data = undefined;
            if (input.files.length == 1) {
                var files = [];
                const fileByteArray = [];
                var reader = new FileReader();
                if (event != null) {
                    event.preventDefault();
                    files.push(input.files[0]);

                    if (input.files[0].size > 2048000) {
                        swal('', "Tamaño del archivo mayor a 2 megas", 'error');
                    }
                    else {

                        if ($scope.validaNombreArchivo(input.files[0].name)) {
                            $scope.files.push({ nombreArchivo: input.files[0].name, size: input.files[0].size, arhivo: input.files[0] });

                            reader.onload = function () {
                                vm.data = reader.result.replace("data:", "")
                                    .replace(/^.+,/, "");
                                vm.nombreArchivo = input.files[0].name;
                                vm.desactivarGuardar = false;
                            }
                            reader.readAsDataURL(input.files[0]);

                        }
                    }
                }

            }
            else {
                vm.desactivarGuardar = true;
                //vm.filename = input.files.length + " archivos"               
                //vm.activarControles('inicio');
            }
        }

        $scope.ChangeFirmaSet = function () {
            if (vm.nombreArchivo == "") {
                //vm.desactivarGuardar = true;
            }
        };

        $scope.validaNombreArchivo = function (nombre) {
            var regex = /^([\ \(a-zA-Z0-9\s_\\.\-:\ \)])+(.png)$/;
            if (!regex.test(nombre.toLowerCase())) {
                utilidades.mensajeError("El archivo debe tener algún formato de tipo  .png!");
                $scope.files = [];
                $scope.nombreArchivo = '';
                return false;
            }
            else {
                return true;
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

        vm.guardado = function (nombreComponenteHijo) {
            vm.callback();
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
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
                'aprobacionfinalfirmaconceptote': true
            };
        }

        vm.notificacionCambiosCapitulos = function ({ nombreComponente, nombreComponenteHijo }) {
            var x = 0;
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) {
                    vm.handlerComponentes[i].handlerCambios(nombreComponenteHijo);
                }
            }
        };

        vm.notificacionCambiosCapitulosExternos = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].componente != nombreComponente) {
                    vm.handlerComponentes[i].handlerCambios = handler;
                    break;
                }
            }
        };

        /**
        * Función que recibe listado de errores referentes a la sección de justificación
        * envía a sus hijos el listado de errores
        * @param {any} errores
        */
        vm.notificacionValidacionEvent = function (listErrores) {
            var erroresList = listErrores.errores.filter(p => p.Seccion == vm.nombreComponente);
            vm.inicializarComponenteCheck();
            vm.esValido = true;
            if (erroresList.length > 0) {
                for (var i = 0; i < vm.handlerComponentes.length; i++) {
                    if (vm.handlerComponentes[i].handlerValidacion) vm.handlerComponentes[i].handlerValidacion(erroresList);
                }
            }
        }

        /**
        * Función que crea las referencias de los métodos de los hijos con el padre. Este es llamado cuando se inicializa el componente hijo.
        * @param {any} handler función referenciada
        * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
        */
        vm.notificacionValidacionHijos = function (handler, nombreComponente) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.handlerComponentes[indx].handlerValidacion = handler;

        };


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





        /*------------------------------------Fin Validaciones-----------------------------------*/

        
    }
    angular.module('backbone').component('firmaLey', {

        templateUrl: "src/app/formulario/ventanas/tramiteTrasladoLey/componentes/firmaLey/firmaLey.html",
        controller: firmaLeyController,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&',
            guardadocomponent: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            nombrecomponentepaso: '@'
        }
    });
})();
