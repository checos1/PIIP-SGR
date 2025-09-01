(function () {
    'use strict';

    firmaFormularioGestionRecursosSgpController.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        'firmaFormularioSgpServicio',
        '$timeout',
        'justificacionCambiosServicio',
        'usuariosInvolucradosSgpServicio',
        '$location'
    ];

    function firmaFormularioGestionRecursosSgpController(
        $scope,
        $sessionStorage,
        utilidades,
        firmaFormularioSgpServicio,
        $timeout,
        justificacionCambiosServicio,
        usuariosInvolucradosSgpServicio,
        $location
    ) {

        var vm = this;

        /*Varibales */
        vm.nombreArchivo = undefined;
        vm.boolExisteFirma = false;
        vm.desactivarGuardar = true;
        vm.desactivarFirmar = true;
        vm.IdInstancia = $sessionStorage.idInstancia;
        vm.tipoConceptoViabilidadId = 4;
        vm.usuarioSeleccionado = null;
        vm.cantidadUsuariosEmitio = 0;
        vm.cantidadUsuariosInvolucrados = 0;
        vm.cantidadUsuariosFirmado = 0;
        vm.cantidadUsuariosFirmadoEmitio = 0;
        vm.usuarioSesion = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.EntidadUsuarioFirmaId = 0;
        vm.habilitaSiguiente = false;

        /*declara metodos*/
        vm.examinar = examinar;
        vm.archivoSeleccionado = archivoSeleccionado;
        vm.firmar = firmar;
        vm.borrarSeleccionado = borrarSeleccionado;
        vm.limpiarArchivo = limpiarArchivo;
        vm.limpiarPrevisualizacion = limpiarPrevisualizacion;
        vm.redirectIndexPage = redirectIndexPage;
        vm.eliminarfirma = eliminarfirma;
        vm.init = init;

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;


        /*Funciones*/

        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
                // ObtenerSeccionCapitulo();
            }
        });

        function init() {
            vm.EntidadUsuarioFirmaId = $sessionStorage.idEntidad;
            //var accion = $sessionStorage.listadoAccionesTramite.find(f => f.Ventana == vm.ventanapadre);
            //if (accion != null) {
            //    vm.EntidadUsuarioFirmaId = accion.IdEntidad;
            //}
            obtenerUsuariosInvolucrados();
        }

        function obtenerUsuariosInvolucrados() {
            return usuariosInvolucradosSgpServicio.ObtenerProyectoViabilidadInvolucradosFirma(vm.IdInstancia, vm.tipoConceptoViabilidadId).then(
                function (involucrados) {
                    vm.involucrados = involucrados.data;
                    vm.cantidadUsuariosInvolucrados = vm.involucrados.length;

                    vm.involucrados.forEach(involucrado => {
                        if (involucrado.Accion === 'Emitió') {
                            vm.cantidadUsuariosEmitio++;

                            if (involucrado.Firmado) {
                                vm.cantidadUsuariosFirmadoEmitio++;
                            }
                        }

                        if (involucrado.Firmado) {
                            vm.cantidadUsuariosFirmado++;
                        }

                        if (involucrado.IdUsuarioDNP === vm.usuarioSesion) {
                            vm.usuarioSeleccionado = involucrado;
                            obtenerFirmaServicio();
                            if (involucrado.Accion === 'Emitió') {
                                vm.habilitaSiguiente = true;
                                vm.notificarsiguiente({ estado: vm.habilitaSiguiente });
                            } else {
                                vm.habilitaSiguiente = false;
                            }
                        }
                    });
                }
            );
        }

        function obtenerFirmaCargar(base64String) {
            vm.firmaConcepto = true;
            var base64img = getBase64Img(base64String);
            Base64ToImage(base64img);
            $scope.firmaCargada = true;
            vm.boolExisteFirma = true;
            vm.desactivarFirmar = false;
        }

        function obtenerFirmaServicio() {
            vm.firmaConcepto = true;

            firmaFormularioSgpServicio.validarSiExisteFirmaUsuario(vm.usuarioSeleccionado.IdUsuarioDNP).then(function (response) {
                if (response.data && response.data.Exito) {
                    var base64img = getBase64Img(response.data.Byte64);
                    Base64ToImage(base64img);
                    $scope.firmaCargada = true;
                    vm.boolExisteFirma = true;
                    vm.desactivarFirmar = false;
                    //guardarCapituloModificado();
                } else {
                    const img = document.getElementById('main');
                    img.innerHTML = '';
                    img.setAttribute("style", "width:80%;margin-left:2rem !important;border:inset;height:15rem;");
                    $scope.firmaCargada = false;
                    vm.boolExisteFirma = false;
                }
            });
        }

        function getBase64Img(imagebase64) {
            return "data:image/png;base64," + imagebase64;
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

        vm.asignarUsuarioFirmar = function (involucrado) {
            vm.usuarioSeleccionado = involucrado;
            obtenerFirmaServicio();
        }

        function archivoSeleccionado() {
            return firmaFormularioSgpServicio.cargarFirma(vm.data, '643bf52e-a37f-4a75-982b-64450014368a', vm.usuarioSeleccionado.IdUsuarioDNP).then(function (response) {
                if (response.data && response.data.Exito) {
                    parent.postMessage("cerrarModal", "*");
                    utilidades.mensajeSuccess("Ahora puede visualizarla en el campo 'Última firma cargada'", false, false, false, 'La imagen fue guardada con éxito.');
                    //guardarCapituloModificado();
                    limpiarArchivo();
                    obtenerFirmaCargar(vm.data);
                    $scope.firmaCargada = true;
                    vm.boolExisteFirma = true;
                    vm.desactivarFirmar = false;
                    $timeout(function () {
                        $scope.firmaCargada = true;
                    }, 100);
                } else {
                    swal('', "Error al realizar la operación", 'error');
                }
            });
        }

        function firmar() {

            utilidades.mensajeWarning(
                "¿Está seguro de continuar?",
                function funcionContinuar() {
                    return firmaFormularioSgpServicio.firmar(vm.IdInstancia, vm.tipoConceptoViabilidadId, vm.usuarioSeleccionado.IdUsuarioDNP, vm.EntidadUsuarioFirmaId).then(function (response) {
                        if (response.data && response.data.Exito) {
                            parent.postMessage("cerrarModal", "*");
                            //guardarCapituloModificado();
                            limpiarArchivo();
                            limpiarPrevisualizacion();
                            $scope.firmaCargada = true;
                            vm.boolExisteFirma = false;
                            vm.desactivarFirmar = true;
                            vm.cantidadUsuariosFirmado++;
                            if (vm.usuarioSeleccionado.Accion === 'Emitió') {
                                vm.cantidadUsuariosFirmadoEmitio++;

                                if (vm.cantidadUsuariosFirmadoEmitio === vm.cantidadUsuariosEmitio) {
                                    if (vm.cantidadUsuariosFirmado !== vm.cantidadUsuariosInvolucrados) {
                                        vm.notificarsiguiente({ estado: vm.habilitaSiguiente });                                       
                                    }
                                    vm.notificarsiguiente({ estado: vm.habilitaSiguiente });
                                    actualizarDatos();
                                    utilidades.mensajeSuccess('Recuerde que posterior a esto, el documento de Viabilidad definitivo será incluido en la sección de Soportes del proyecto.', false, false, false, 'Usted ha firmado el formulario con éxito. <br />Para concluir deberá validar y finalizar el formulario.');
                                    return;
                                }
                            }

                            vm.notificarsiguiente({ estado: vm.habilitaSiguiente });
                            actualizarDatos();
                            if (vm.usuarioSeleccionado.Accion === 'Emitió') {
                                utilidades.mensajeSuccess('Recuerde que posterior a esto, el documento de Viabilidad definitivo será incluido en la sección de Soportes del proyecto.', false, false, false, 'Usted ha firmado el formulario con éxito. <br />Para concluir deberá validar y finalizar el formulario.');
                            }
                            else {
                                utilidades.mensajeSuccess('Recuerde que la finalización del proceso estará a cargo del usuario responsable de "Emitir" el concepto y que posterior a eso, el documento de Viabilidad definitivo será incluido en la sección de Soportes del proyecto.', false, vm.redirectIndexPage, false, 'Usted ha firmado y concluido su parte del proceso con éxito. Será redirigido a la página principal de la PIIP.');
                            }

                            $timeout(function () {
                                $scope.firmaCargada = true;
                            }, 100);
                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
                    });
                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "Se registrará su firma en el formulario. Una vez ejecutada esta acción, este formulario no podrá modificarse.");
        }

        function actualizarDatos() {
            obtenerUsuariosInvolucrados();
            if (vm.cantidadUsuariosFirmado === vm.cantidadUsuariosInvolucrados) {
                guardarCapituloModificado();
            }
        }

        function redirectIndexPage() {
            $location.url("/");
        }

        function borrarSeleccionado() {

            utilidades.mensajeWarning(
                "¿Está seguro de continuar?",
                function funcionContinuar() {
                    firmaFormularioSgpServicio.borrarFirma('', vm.usuarioSeleccionado.IdUsuarioDNP).then(function (response) {
                        if (response.data && response.data.Exito) {
                            parent.postMessage("cerrarModal", "*");
                            utilidades.mensajeSuccess("La firma fue eliminada con éxito.", false, false, false);
                            limpiarArchivo();
                            limpiarPrevisualizacion();
                            vm.boolExisteFirma = false;
                            $timeout(function () {
                                $scope.firmaCargada = false;
                            }, 100);
                            eliminarCapitulosModificados();
                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
                    });
                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "Si se elimina la firma, es necesario cargar una nueva para firmar el formulario.");

        }

        function limpiarArchivo() {
            $scope.files = [];
            document.getElementById('fileFirma').value = "";
            vm.nombreArchivo = undefined;
            vm.desactivarGuardar = true;
            vm.desactivarFirmar = true;
        }

        function limpiarPrevisualizacion() {
            var imagen = document.getElementById('main');
            imagen.innerHTML = '';
            imagen.setAttribute("style", "width:80%;margin-left:2rem !important;border:inset;height:15rem;");
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

                var regex2 = /^([\ \(a-zA-Z0-9\s_\\.\-:\ \)])+(.jpg)$/;
                if (!regex2.test(nombre.toLowerCase()))  {
                    utilidades.mensajeError("El archivo debe tener algún formato de tipo  .png o .jpg !");
                    $scope.files = [];
                    $scope.nombreArchivo = '';
                    return false;
                }
                else {
                    return true;
                }
              
            }
            else {
                return true;
            }
        }


        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
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
        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById(`id-capitulo-${vm.nombrecomponentepaso}`);
            vm.seccionCapitulo = span.textContent;


        }

        function eliminarfirma() {

            utilidades.mensajeWarning(
                "¿Está seguro de continuar?",
                function funcionContinuar() {
                    return firmaFormularioSgpServicio.eliminarFirma(vm.IdInstancia, vm.tipoConceptoViabilidadId, vm.usuarioSeleccionado.IdUsuarioDNP, vm.EntidadUsuarioFirmaId).then(function (response) {
                        if (response.data && response.data.Exito) {
                            parent.postMessage("cerrarModal", "*");
                            //guardarCapituloModificado();
                            limpiarArchivo();
                            limpiarPrevisualizacion();
                            $scope.firmaCargada = false;
                            vm.boolExisteFirma = false;
                            vm.desactivarFirmar = true;
                            vm.cantidadUsuariosFirmado--;
                            if (vm.usuarioSeleccionado.Accion === 'Emitió') {
                                vm.cantidadUsuariosFirmadoEmitio--;

                                if (vm.cantidadUsuariosFirmadoEmitio === vm.cantidadUsuariosEmitio) {
                                    if (vm.cantidadUsuariosFirmado !== vm.cantidadUsuariosInvolucrados) {
                                        vm.notificarsiguiente({ estado: vm.habilitaSiguiente });
                                    }
                                    vm.notificarsiguiente({ estado: vm.habilitaSiguiente });
                                    actualizarDatos();
                                    return;
                                }
                            }

                            vm.notificarsiguiente({ estado: vm.habilitaSiguiente });
                            actualizarDatos();
                            utilidades.mensajeSuccess('Recuerde firmar nuevamente para poder continuar con el proceso.', false, false, false, 'Usted ha eliminado su firma con éxito.');

                            $timeout(function () {
                                $scope.firmaCargada = false;
                            }, 100);
                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
                    });
                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "Se eliminará su firma del formulario. Una vez ejecutada esta acción, este formulario no podrá modificarse.");
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function (errores) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.remove("ico-advertencia");
            }

            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-archivo-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
        }

        vm.notificacionValidacionPadre = function (errores) {
            //console.log("Validación  - CD Pvigencias futuras");
            vm.limpiarErrores(errores);
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == null ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                        var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
                        if (idSpanAlertComponent != undefined) {
                            idSpanAlertComponent.classList.add("ico-advertencia");
                        }
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

            }
        }



        vm.validarFirmaAclaracionLeyenda = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-archivo-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }





        vm.errores = {
            'TALP5001': vm.validarFirmaAclaracionLeyenda




        }

        /* ------------------------ FIN Validaciones ---------------------------------*/



    }

    angular.module('backbone').component('firmaFormularioGestionRecursosSgp', {

        templateUrl: "src/app/formulario/ventanas/SGP/comun/firma/firmaFormularioGestionRecursosSgp.html",
        controller: firmaFormularioGestionRecursosSgpController,
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
            ventanapadre: '@',
            notificarsiguiente: '&'
        }
    });
})();
