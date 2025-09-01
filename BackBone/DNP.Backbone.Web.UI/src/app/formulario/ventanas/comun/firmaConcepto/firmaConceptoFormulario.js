(function () {
    'use strict';

    firmaConceptoFormulario.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        'trasladosServicio',
        '$timeout',
        'justificacionCambiosServicio',
    ];

    function firmaConceptoFormulario(
        $scope,
        $sessionStorage,
        utilidades,
        trasladosServicio,
        $timeout,
        justificacionCambiosServicio,
    ){

        var vm = this;

        /*Varibales */
        vm.nombreArchivo = undefined;
        vm.boolExisteFirma = false;
        vm.desactivarGuardar = true;

        /*declara metodos*/
        vm.initFirmaConcepto = initFirmaConcepto;
        vm.examinar = examinar;
        vm.archivoSeleccionado = archivoSeleccionado;
        vm.borrarSeleccionado = borrarSeleccionado;
        vm.limpiarArchivo = limpiarArchivo;

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

        function initFirmaConcepto(){
            //Validaciones
            //vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponenteDatosIniciales, esValido: true });
            obtenerFirma();
        }

        function obtenerFirma() {
            vm.firmaConcepto = true;
            trasladosServicio.validarSiExisteFirmaUsuario().then(function (response) {
                if (response.data && response.data.Exito) {
                    var base64img = getBase64Img(response);
                    Base64ToImage(base64img);
                    $scope.firmaCargada = true;
                    vm.boolExisteFirma = true;
                    guardarCapituloModificado();
                    

                } else {
                    const img = document.getElementById('main');
                    img.innerHTML = '';
                    img.setAttribute("style", "width:80%;margin-left:2rem !important;border:inset;height:15rem;");
                    $scope.firmaCargada = false;
                    vm.boolExisteFirma = false;
                    

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
                    guardarCapituloModificado();
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

        function borrarSeleccionado() {
            var x = trasladosServicio.borrarFirma('').then(function (response) {
                if (response.data && response.data.Exito) {
                    parent.postMessage("cerrarModal", window.location.origin);
                    utilidades.mensajeSuccess("La última firma cargada fue borrada", false, false, false, 'La imagen fué eliminada con éxito.');
                    limpiarArchivo();
                    obtenerFirma();
                   
                    $timeout(function () {
                        $scope.firmaCargada = false;
                    }, 100);
                    eliminarCapitulosModificados();
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
        

        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function (errores) {
            //var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
            //if (idSpanAlertComponent != undefined) {
            //    idSpanAlertComponent.classList.remove("ico-advertencia");
            //}

            var campoObligatorioJustificacion = document.getElementById("aprobacionfinalfirmaconceptote-archivo-error");
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
                        //var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
                        //if (idSpanAlertComponent != undefined) {
                        //    idSpanAlertComponent.classList.add("ico-advertencia");
                        //}
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

            }
        }



        vm.validarFirmaAclaracionLeyenda = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("aprobacionfinalfirmaconceptote-archivo-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

      
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;


        }


        vm.errores = {
            'TALP5001': vm.validarFirmaAclaracionLeyenda




        }

        /* ------------------------ FIN Validaciones ---------------------------------*/


        
    }

    angular.module('backbone').component('firmaConceptoFormulario', {

        templateUrl: "src/app/formulario/ventanas/comun/firmaConcepto/firmaConceptoFormulario.html",
        controller: firmaConceptoFormulario,
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
            nombrecomponentepaso: '@'
        }
    });
})();
