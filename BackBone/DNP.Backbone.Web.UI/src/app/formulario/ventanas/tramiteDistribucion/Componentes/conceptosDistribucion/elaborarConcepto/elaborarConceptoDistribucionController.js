(function () {
    'use strict';

    elaborarConceptoDistribucionController.$inject = [
        '$sessionStorage',
        'archivoServicios',
        '$scope',
        'servicioAcciones',
        'trasladosServicio',
        'constantesCondicionFiltro',
        'sesionServicios',
        '$routeParams',
        'constantesBackbone',
        'tramiteVigenciaFuturaServicio',
        'utilidades',
        'FileSaver',
        'tramiteTrasladoOrdinarioServicio'
    ];

    function elaborarConceptoDistribucionController(
        $sessionStorage,
        archivoServicios,
        $scope,
        servicioAcciones,
        trasladosServicio,
        constantesCondicionFiltro,
        sesionServicios,
        $routeParams,
        constantesBackbone,
        tramiteVigenciaFuturaServicio,
        utilidades,
        FileSaver,
        tramiteTrasladoOrdinarioServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.tabActivo = 0;
        vm.firmaConcepto = false;

        vm.archivoSeleccionado = archivoSeleccionado;
        vm.limpiarArchivo = limpiarArchivo;
        vm.files = [];
        vm.examinar = examinar;
        vm.nombreArchivo = undefined;
        vm.desactivarGuardar = true;
        vm.data = undefined;
        vm.rolUsuario = $sessionStorage.usuario.roles.find((item) => item.Nombre.includes('Director'));
        vm.nivel = $sessionStorage.idNivel;
        vm.mostrarPDF = $sessionStorage.InstanciaSeleccionada.estadoId == 3 ? true : false;

        //declarar metodo
        vm.initElaborarConcepto = initElaborarConcepto;
        //Validaciones
        vm.nombreComponenteDatosIniciales = "solicitarconceptodatosiniciales";
        vm.nombreComponente = "conceptoselaborarconcep";
        //vm.nombreComponenteCuerpo = "solicitarconceptocuerpo";
        //vm.nombreComponentedatosdespedida = "solicitarconceptodatosdespedida";



        //metodos
        function initElaborarConcepto() {
            $scope.$watch('vm.tramiteid', function () {
                if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                    vm.tramiteId = vm.tramiteid;
                    vm.tipoTramite = vm.tipotramiteid;
                    vm.tabActivo = 1;
                    vm.mostrarTab(vm.tabActivo);
                    vm.rolAnalista = vm.rolanalista.toLowerCase() === 'true' ? true : false;

                    var roluser = $sessionStorage.usuario.roles;
                    if (!vm.mostrarPDF) {
                        if (vm.rolAnalista)
                            vm.mostrarPDF = true;
                        else if (roluser.find(x => x.Nombre.includes('Analista')) || roluser.find(x => x.Nombre.includes('Director')) ||
                            roluser.find(x => x.Nombre.includes('Subdirector')) || roluser.find(x => x.Nombre.includes('global')))
                            vm.mostrarPDF = true;
                        else
                            vm.mostrarPDF = false;
                    }
                }
            });
            //Validaciones
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            //vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponenteDatosIniciales, esValido: true });
            //vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponenteCuerpo, esValido: true });
            //vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponentedatosdespedida, esValido: true });
            //obtenerFirma();
            //vm.firmaConcepto = $sessionStorage.idNivel === constantesBackbone.idNivelAprobacionTramite.toLowerCase() ? true : false;
        }


        vm.mostrarTab = function (origen) {
            vm.tabActivo = origen;
            MostrarMensaje();

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
        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error_iniciales");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error-Cuerpo");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error-Despedida");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error-firma");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var idSpanAlertComponent = document.getElementById("alert-Inciales-" + vm.nombreComponenteDatosIniciales);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.remove("ico-advertencia");
            }

            var idSpanAlertComponent = document.getElementById("alert-Cuerpo-" + vm.nombreComponenteDatosIniciales);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.remove("ico-advertencia");
            }

            var idSpanAlertComponent = document.getElementById("alert-Despedida-" + vm.nombreComponenteDatosIniciales);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.remove("ico-advertencia");
            }





        }



        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores(errores);
            if (errores != undefined) {
                let valido = true;
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        valido = false;
                        erroresJson[vm.nombreComponenteDatosIniciales].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                    MostrarMensaje();
                    vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: valido });
                }


            }
        }

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

        vm.validarValoresVigenciasolicitarconceptodatosiniciales = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error_iniciales");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }

            var idSpanAlertComponent = document.getElementById("alert-Inciales-" + vm.nombreComponenteDatosIniciales);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.add("ico-advertencia");
            }
        }

        vm.validarValoresVigenciasolicitarconceptocuerpo = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error-Cuerpo");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                //campoObligatorioJustificacion.classList.remove('hidden');
            }

            var idSpanAlertComponent = document.getElementById("alert-Cuerpo-" + vm.nombreComponenteDatosIniciales);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.add("ico-advertencia");
            }

        }

        vm.validarValoresVigenciasolicitarconceptodatosdespedida = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error-Despedida");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                //campoObligatorioJustificacion.classList.remove('hidden');
            }
            var idSpanAlertComponent = document.getElementById("alert-Despedida-" + vm.nombreComponenteDatosIniciales);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.add("ico-advertencia");
            }

        }

        vm.validarValoresVigenciasolicitarconceptodatosElaboracionConcepto = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error-firma");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
            var idSpanAlertComponent = document.getElementById("alert-Despedida-" + vm.nombreComponenteDatosIniciales);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.add("ico-advertencia");
            }

        }


        vm.errores = {
            'VFO012': vm.validarValoresVigenciasolicitarconceptodatosiniciales,
            'VFO013': vm.validarValoresVigenciasolicitarconceptocuerpo,
            'VFO014': vm.validarValoresVigenciasolicitarconceptodatosdespedida,
            'VFO008': vm.validarValoresVigenciasolicitarconceptodatosElaboracionConcepto,
        }

        function MostrarMensaje() {
            switch (vm.tabActivo) {
                case 1:
                    var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error_iniciales");
                    if (campoObligatorioJustificacion != undefined) {
                        campoObligatorioJustificacion.classList.remove('hidden');
                    }

                    var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error-Cuerpo");
                    if (campoObligatorioJustificacion != undefined) {

                        campoObligatorioJustificacion.classList.add('hidden');
                    }

                    var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error-Despedida");
                    if (campoObligatorioJustificacion != undefined) {

                        campoObligatorioJustificacion.classList.add('hidden');
                    }
                    break;
                case 2:
                    var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error_iniciales");
                    if (campoObligatorioJustificacion != undefined) {

                        campoObligatorioJustificacion.classList.add('hidden');
                    }

                    var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error-Cuerpo");
                    if (campoObligatorioJustificacion != undefined) {
                        campoObligatorioJustificacion.classList.remove('hidden');
                    }

                    var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error-Despedida");
                    if (campoObligatorioJustificacion != undefined) {

                        campoObligatorioJustificacion.classList.add('hidden');
                    }
                    break;
                case 3:
                    var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error_iniciales");
                    if (campoObligatorioJustificacion != undefined) {

                        campoObligatorioJustificacion.classList.add('hidden');
                    }

                    var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error-Cuerpo");
                    if (campoObligatorioJustificacion != undefined) {

                        campoObligatorioJustificacion.classList.add('hidden');
                    }

                    var campoObligatorioJustificacion = document.getElementById(vm.nombreComponenteDatosIniciales + "-pregunta-error-Despedida");
                    if (campoObligatorioJustificacion != undefined) {
                        campoObligatorioJustificacion.classList.remove('hidden');
                    }
                    break;

            }
        }

        vm.guardado = function (nombreComponenteHijo) {
            vm.callback();
            vm.guardadoevent({ nombreComponenteHijo: nombreComponenteHijo });
        }

        /* ------------------------ FIN Validaciones ---------------------------------*/

        //Presenta el PDF de la carta concepto
        vm.verPdf = function () {
            const tramiteId = vm.tramiteid;
            tramiteTrasladoOrdinarioServicio.generarPdfCartaTramite(tramiteId, true, "ConceptoTraslDistribucion");
        }

    }

    angular.module('backbone').component('elaborarConceptoDistribucion', {
        templateUrl: "src/app/formulario/ventanas/tramiteDistribucion/componentes/conceptosDistribucion/elaborarConcepto/elaborarConceptoDistribucion.html",
        controller: elaborarConceptoDistribucionController,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
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
            deshabilitar: '@',
            rolanalista: '@',
        }
    });

})();