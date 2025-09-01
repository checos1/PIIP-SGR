(function () {
    'use strict';

    justificacioncostoactividadesSinTramiteSgpController.$inject = [
        '$scope',
        '$sessionStorage',
        'justificacioncostoactividadesSinTramiteSgpServicio',
        'utilidades',
        'justificacionCambiosServicio'
    ];

    function justificacioncostoactividadesSinTramiteSgpController(
        $scope,
        $sessionStorage,
        justificacioncostoactividadesSinTramiteSgpServicio,
        utilidades,
        justificacionCambiosServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "recursoscostosdelasacti";
        vm.titulo = "Modificación Costo de actividades";
        vm.init = init;
        vm.isEdicion;
        vm.mensaje = "";
        vm.longMaxText = 200;
        vm.vigenciaActual = new Date().getFullYear();
        vm.DetalleAjustes = [];
        vm.erroresActivos = [];

        vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.CapitulosModificados;
        vm.Capitulo = "COSTO DE ACTIVIDADES";
        vm.Seccion = "RECURSOS";
        vm.validacion = "";
        vm.mensajejustificacion = "Justifique la modificación* (Maximo 8.000 caracteres)";
        vm.ConvertirNumero = ConvertirNumero;

        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.soloLectura = false;

        function init() {

            vm.editar();

            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            vm.vigenciaActual = new Date().getFullYear();
            vm.justificacion = vm.justificacioncapitulo;
            vm.notificacioncambios({ handler: vm.notificacionJustificacion });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacion, nombreComponente: vm.nombreComponente, esValido: true });
            $sessionStorage.verDiferencia = true;

            obtenerDetalleAjustes();
        };

        function ConvertirNumero(numero) {
            if (numero == undefined) {
                numero = 0;
            }
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        function obtenerDetalleAjustes() {
            return justificacioncostoactividadesSinTramiteSgpServicio.ObtenerResumenObjetivosProductosActividadesJustificacion(vm.BPIN).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.DetalleAjustes = respuesta.data;
                        console.log(vm.DetalleAjustes);

                        CalcularTotales();
                    }
                });
        }

        function CalcularTotales() {
            vm.DetalleAjustes.Objetivos.forEach(objetivo => {
                objetivo.Productos.forEach(producto => {

                    producto.Etapas.forEach(etapa => {
                        etapa.TotalFirme = 0;
                        etapa.TotalAjuste = 0;
                        etapa.TotalDiferencia = 0;

                        etapa.Vigencias.forEach(vigencia => {
                            etapa.TotalAjuste += parseFloat(vigencia.CostoAjusteSinFormato);
                            etapa.TotalDiferencia += parseFloat(vigencia.DiferenciaSinFormato); 
                            etapa.TotalFirme += parseFloat(vigencia.CostoFirmeSinFormato);

                            if (vigencia.Diferencia == 0) {
                                vigencia.class = 'blockReferenciaDNP';
                            }
                            else {
                                vigencia.class = 'editReferenciaDNP';
                            }
                        });
                    });
                });
            });

            console.log("Calculando Totales => ");
            console.log(vm.DetalleAjustes);
        }

        vm.notificacionValidacion = function (errores) {
            console.log("Validación  - Justificación Costos de Actividades");
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => p.Capitulo == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                }
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson.errores.forEach(p => {
                            if (vm.errores[p.Error] != undefined) {
                                vm.erroresActivos.push({
                                    Error: p.Error,
                                    Descripcion: p.Descripcion
                                });
                                vm.errores[p.Error](p.Error, p.Descripcion);
                            }
                        });
                    }
                    vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        };

        vm.validarJustificacion = function (errores, descripcion) {
            var campoObligatorioJustificacion = document.getElementById("recursoscostosdelasacti-justificacion-error");
            var ValidacionFFR1Error = document.getElementById("recursoscostosdelasacti-justificacion-error-mns");

            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '<span>' + descripcion + "</span>";
                    campoObligatorioJustificacion.classList.remove('hidden');
                }
            }
        }

        vm.errores = {
            'JUST001': vm.validarJustificacion
        }

        vm.limpiarErrores = function () {
            var campoObligatorioJustificacion = document.getElementById("recursoscostosdelasacti-justificacion-error");
            var ValidacionFFR1Error = document.getElementById("recursoscostosdelasacti-justificacion-error-mns");

            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '';
                    campoObligatorioJustificacion.classList.add('hidden');
                }
            }
        }

        vm.guardar = function () {
            if (vm.justificacion == '' || vm.justificacion == undefined) {
                utilidades.mensajeError('Debe ingresar una justificación.');
                return false;
            }


            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: vm.justificacion,
                SeccionCapituloId: 906,
                InstanciaId: vm.idInstancia,
                AplicaJustificacion: 1,
            }

            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess(response.data.Mensaje);
                        document.getElementById("justificacion").value = vm.justificacion;
                        vm.justificacioncapitulo = vm.justificacion;
                        vm.isEdicion = false;
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje);
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
            vm.soloLectura = $sessionStorage.soloLectura;
            if (vm.isEdicion) {
                document.getElementById("justificacionca").disabled = false;
                document.getElementById("justificacionca").classList.remove('disabled');
                if (document.getElementById("btn-guardar-edicion-ca") != null) {
                    document.getElementById("btn-guardar-edicion-ca").classList.remove('disabled');
                }
                vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
            } else {

                if (estado == 'cancelar') {
                    utilidades.mensajeWarning("Los posibles datos que haya diligenciado se perderan. ¿Está seguro de continuar?", function funcionContinuar() {
                        OkCancelar();
                        document.getElementById("justificacionca").disabled = true;
                        document.getElementById("justificacionca").classList.add('disabled');
                        if (document.getElementById("btn-guardar-edicion-ca") != null) {
                            document.getElementById("btn-guardar-edicion-ca").classList.add('disabled');
                        }
                        document.getElementById("justificacionca").value = vm.justificacioncapitulo;
                        vm.ClasesbtnGuardar = "btn btnguardarHorizonte";

                    }, function funcionCancelar(reason) {
                        vm.isEdicion = true;
                        return;
                    }, null, null, "Advertencia");
                }
                else {
                    document.getElementById("justificacionca").disabled = true;
                    document.getElementById("justificacionca").classList.add('disabled');
                    if (document.getElementById("btn-guardar-edicion-ca") != null) {
                        document.getElementById("btn-guardar-edicion-ca").classList.add('disabled');
                    }
                    document.getElementById("justificacionca").value = vm.justificacioncapitulo;
                    vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
                }


              
            }
        }
    }

    angular.module('backbone').component('recursossgpcostosdelasactisintramitesgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/recursosSGP/costoactividades/justificacioncostoactividadesSinTramiteSgp.html",
        controller: justificacioncostoactividadesSinTramiteSgpController,
        controllerAs: "vm",
        location: "es-COP",
        bindings: {
            justificacioncapitulo: '@',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&'
        }
    });

})();