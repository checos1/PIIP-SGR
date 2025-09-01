(function () {
    'use strict';

    recursosfuentesdefinancSinTramiteSgpController.$inject = [
        '$scope',
        '$sessionStorage',
        'recursosfuentesdefinancSinTramiteSgpServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'relacionPlanificacionServicio'
    ];

    function recursosfuentesdefinancSinTramiteSgpController($scope,
        $sessionStorage,
        recursosfuentesdefinancSinTramiteSgpServicio,
        utilidades,
        justificacionCambiosServicio,
        relacionPlanificacionServicio) {

        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "recursosfuentesdefinanc";
        vm.titulo = "Modificación Fuentes de Financiacion";
        vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.listaResumen;
        vm.isEdicion;
        vm.CapitulosModificados;
        vm.Capitulo = "FUENTES DE FINANCIACION";
        vm.Seccion = "RECURSOS";
        vm.validacion = "";
        vm.mensajejustificacion = "Justifique la modificación* (Maximo 8.000 caracteres)";

        const listaResumen_inv = [];
        const listaResumen_prei = [];
        const listaResumen_ope = [];
        let resumenSubtotal_ope = {};
        let resumenSubtotal_prei = {};
        let resumenSubtotal_inv = {};
        vm.DetalleAjustes = [];
        vm.mensaje = "";
        vm.soloLectura = false;

        vm.init = function () {
            obtenerDetalleAjustesFuenteFinanciacion();
            vm.editar();
            vm.justificacion = vm.justificacioncapitulo;
            vm.notificacioncambios({ handler: vm.notificacionJustificacion });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacion, nombreComponente: vm.nombreComponente, esValido: true });
            $sessionStorage.verDiferencia = true;
        };

        function obtenerDetalleAjustesFuenteFinanciacion() {
            return recursosfuentesdefinancSinTramiteSgpServicio.obtenerDetalleAjustesFuenteFinanciacion($sessionStorage.idInstancia, vm.idUsuario).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.DetalleAjustes = respuesta.data.split('|');
                        vm.soloLectura = $sessionStorage.soloLectura;
                    }
                });
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }


        vm.editar = function (estado) {
            vm.isEdicion = null;
            vm.isEdicion = estado == 'editar';
            if (vm.isEdicion) {
                document.getElementById("justificacionff").disabled = false;
                document.getElementById("justificacionff").classList.remove('disabled');
                document.getElementById("btn-guardar-edicion-ff").classList.remove('disabled');
                vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
            } else {
                if (estado == 'cancelar') {
                    
                    utilidades.mensajeWarning("Los posibles datos que haya diligenciado se perderan. ¿Está seguro de continuar?", function funcionContinuar() {
                        OkCancelar();
                        document.getElementById("justificacionff").disabled = true;
                        document.getElementById("justificacionff").classList.add('disabled');
                        /*document.getElementById("btn-guardar-edicion-ff").classList.add('disabled');*/
                        document.getElementById("justificacionff").value = vm.justificacioncapitulo;
                        vm.ClasesbtnGuardar = "btn btnguardarHorizonte";

                    }, function funcionCancelar(reason) {
                        vm.isEdicion = true;
                        return;
                    }, null, null, "Advertencia");
                }
                else {
                    document.getElementById("justificacionff").disabled = true;
                    document.getElementById("justificacionff").classList.add('disabled');
                    /*document.getElementById("btn-guardar-edicion-ff").classList.add('disabled');*/
                    document.getElementById("justificacionff").value = vm.justificacioncapitulo;
                    vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
                }



            }
        }
        vm.guardar = function () {
            if (vm.justificacion == '' || vm.justificacion == undefined) {
                utilidades.mensajeError('Debe ingresar una justificación.');
                return false;
            }

            var seccionCapitulo = document.getElementById("seccion-capitulo-recursossgpfuentesdefinancsintramitesgp");
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: vm.justificacion,
                SeccionCapituloId: seccionCapitulo.value,
                InstanciaId: vm.idInstancia,
                AplicaJustificacion: 1,
            }

            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess(response.data.Mensaje);
                        document.getElementById("justificacionff").value = vm.justificacion;
                        vm.justificacioncapitulo = vm.justificacion;
                        vm.editar('');
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje);
                    }
                });
        }

        vm.validar = function () {
            if (vm.justificacion == '' || vm.justificacion == undefined) {
                vm.validacion = "Debe diligenciar la justificación del capítulo Horizonte dentro de la pestaña Justificación.";
                return false;
            }
            vm.validacion = "";
            return true;
        }

        vm.notificacionValidacion = function (errores) {
            console.log("Validación  - Justificación Fuentes de financiación");
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => p.Capitulo == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                }
                var isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    erroresJson.errores.forEach(p => {
                        if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                    });
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        };

        vm.limpiarErrores = function () {
            var campoObligatorioJustificacion = document.getElementById("recursossgpfuentesdefinancsintramite-justificacion-error");
            var ValidacionFFR1Error = document.getElementById("recursossgpfuentesdefinancsintramite-justificacion-error-mns");

            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '';
                    campoObligatorioJustificacion.classList.add('hidden');
                }
            }
        }

        vm.validarJustificacion = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("recursossgpfuentesdefinancsintramite-justificacion-error");
            var ValidacionFFR1Error = document.getElementById("recursossgpfuentesdefinancsintramite-justificacion-error-mns");

            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion.classList.remove('hidden');
                }
            }
        }

        vm.errores = {
            'JUST001': vm.validarJustificacion
        }
    }

    angular.module('backbone').component('recursossgpfuentesdefinancsintramite', {
        templateUrl: "src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/recursosSGP/fuentes/recursosfuentesdefinancSinTramiteSgp.html",
        controller: recursosfuentesdefinancSinTramiteSgpController,
        controllerAs: "vm",
        bindings: {
            justificacioncapitulo: '@',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&'
        }
    });
})();