(function () {
    'use strict';

    solicitudModificacionalController.$inject = [
        '$scope',
        '$sessionStorage',
        'aclaracionLeyendaServicio',
        'utilidades',
        'constantesBackbone',
        'justificacionCambiosServicio',
    ];
    function solicitudModificacionalController(
        $scope,
        $sessionStorage,
        aclaracionLeyendaServicio,
        utilidades,
        constantesBackbone,
        justificacionCambiosServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "informacionpresupuestalsolicitudmodifi";
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.datosProyecto;
        vm.Editar = 'EDITAR';
        vm.disabled = true;

        //Inicio
        vm.init = function () {

            $scope.$watch(function () {
                if (vm.proyectoId != $sessionStorage.proyectoId) {
                    vm.proyectoId = $sessionStorage.proyectoId;
                    if (vm.proyectoId == '')
                        vm.datosProyecto = {};
                    if (vm.tramiteid == '')
                        vm.tramiteid = $sessionStorage.tramiteId;
                    if (vm.proyectoId != 'e')
                        vm.cargarNombreProyecto();
                }
                return $sessionStorage;
            }, function (newVal, oldVal) {
            }, true);
            //Validaciones
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

        };

        vm.cargarNombreProyecto = function () {
            if ((vm.tramiteid != '' && vm.tramiteid != undefined) && (vm.proyectoId != '' && vm.proyectoId != undefined)) {
                aclaracionLeyendaServicio.obtenerModificacionLeyenda(vm.tramiteid, vm.proyectoId).then(function (result) {
                    if (result != null) {
                        vm.datosProyecto = result;
                        $sessionStorage.tramiteProyectoId = vm.datosProyecto.tramiteProyectoId;
                        vm.datosProyecto.NombreProyectoModificacion = vm.datosProyecto.NombreProyectoModificacion == null ? vm.datosProyecto.NombreProyectoOriginal : vm.datosProyecto.NombreProyectoModificacion;
                    }
                }, error => {
                    console.log(error);
                });
            }
        }

        vm.getEstiloBtnGuardar = function () {
            if (vm.disabled == false)
                return "btnguardarDNP";
            else
                return "btnguardarDisabledDNP";
        }

        vm.ActivarEditar = function () {
            var panel = document.getElementById('Guardar');
            if (vm.disabled == true) {
                vm.Editar = "CANCELAR";
                vm.disabled = false;
                //panel.classList.replace("btnguardarDisabledDNP", "btnguardarDNP");
            }
            else {
                vm.Editar = "EDITAR";
                vm.disabled = true;
                //panel.classList.replace("btnguardarDNP", "btnguardarDisabledDNP");
                vm.cargarNombreProyecto();
            }
        }

        vm.registrarModificacion = function () {
            var idSpanAlertComponent = document.getElementById("modificacion-error");
            if (vm.datosProyecto.NombreProyectoModificacion != '') {
                idSpanAlertComponent.classList.remove("ico-advertencia");
                idSpanAlertComponent.classList.add('hidden');
                if (vm.datosProyecto.NombreProyectoModificacion.length <= 1500) {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                    idSpanAlertComponent.classList.add('hidden');
                    vm.datosProyecto.tipoUpdate = 1;
                    vm.datosProyecto.ErrorTranscripcion = vm.datosProyecto.ErrorTranscripcion == null ? false : vm.datosProyecto.ErrorTranscripcion;
                    vm.datosProyecto.ErrorAritmetico = vm.datosProyecto.ErrorAritmetico == null ? false : vm.datosProyecto.ErrorAritmetico;

                    aclaracionLeyendaServicio.actualizaModificacionLeyenda(vm.datosProyecto).then(function (result) {
                        if (result.data == "OK") {
                            utilidades.mensajeSuccess("¡Solicitud de nuevo nombre de proyecto guardado con éxito!", false, false, false);
                            vm.ActivarEditar();
                            guardarCapituloModificado();
                        }
                        else {
                            utilidades.mensajeError('Ocurrio un error al registrar la solicitud de modificación de nombre del proyecto.');
                        }
                    }, error => {
                        console.log(error);
                        utilidades.mensajeError('Ocurrio un error al registrar la solicitud de modificación de nombre del proyecto.');
                    });
                }
                else {                   
                    if (idSpanAlertComponent != undefined) {
                        idSpanAlertComponent.classList.add("ico-advertencia");
                        idSpanAlertComponent.classList.remove('hidden');
                        idSpanAlertComponent.innerHTML = '<span> Máximo 1500 carácteres.</span>';
                    }
                }
            }
            else {
                if (idSpanAlertComponent != undefined) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                    idSpanAlertComponent.classList.remove('hidden');
                    idSpanAlertComponent.innerHTML = '<span> El campo no puede estar vacío.</span>';
                }
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
        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById(`id-capitulo-${vm.nombreComponente}`);
            vm.seccionCapitulo = span.textContent;


        }
        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            //console.log("Validación  - CD Pvigencias futuras");
            vm.limpiarErrores(errores);
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl !== undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }


        vm.limpiarErrores = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-pregunta-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-preguntaIgual-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-preguntaBlanco-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }


        }

        vm.validarValoresVigenciainformacionpresupuestalsolicitudmodifiVacio = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-pregunta-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValoresVigenciainformacionpresupuestalsolicitudmodifiIgual = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-preguntaIgual-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValoresVigenciainformacionpresupuestalsolicitudmodifiBlanco = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-preguntaBlanco-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            'AL003': vm.validarValoresVigenciainformacionpresupuestalsolicitudmodifiVacio,
            'AL004': vm.validarValoresVigenciainformacionpresupuestalsolicitudmodifiIgual,
            'AL005': vm.validarValoresVigenciainformacionpresupuestalsolicitudmodifiBlanco,



        }
    }

    angular.module('backbone').component('solicitudModificacional', {
        templateUrl: "src/app/formulario/ventanas/tramiteLeyenda/componentes/informacionPresupuestal/solicitudModificacion/solicitudModificacional.html",
    controller: solicitudModificacionalController,
    controllerAs: "vm",
    bindings: {
        callback: '&',
        tramiteid: '@',
        guardadoevent: '&',
        notificacionvalidacion: '&',
        notificacionestado: '&',


    }
});

}) ();