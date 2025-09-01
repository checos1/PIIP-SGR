(function () {
    'use strict';
    procesoFirmaViabilidadSgpSgpController.$inject = [
        'utilidades',
        '$sessionStorage',
        'justificacionCambiosServicio',
        'firmaEmisionSgpServicio',
        'servicioFichasProyectos',
        'FileSaver',
        'transversalSgpServicio'
    ];

    function procesoFirmaViabilidadSgpSgpController(
        utilidades,
        $sessionStorage,
        justificacionCambiosServicio,
        firmaEmisionSgpServicio,
        servicioFichasProyectos,
        FileSaver,
        transversalSgpServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgpfirmaresponsablesprocesofirmassgp';
        vm.ventanaPadre = 'viabilidadSgp';

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;

        vm.disabled = false;
        vm.activar = true;
        vm.desactivar = true
        vm.lista;

        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        };

        vm.mostrarBotonPdf = function () {
            var mostrar = false;
            if (vm.nombreComponente == "sgpfirmaresponsablesprocesofirmassgp") { mostrar = true; }
            return mostrar;
            ;

        };

        vm.verPdf = function () {
            transversalSgpServicio.SGPTransversalLeerParametro("GenerarFichaViabilidadSGR")
                .then(function (respuestaParametro) {
                    if (respuestaParametro.data.Valor == 'S') {
                        transversalSgpServicio.SGP_Transversal_ObtenerConfiguracionReportes($sessionStorage.idInstancia)
                            .then(function (configuracionReporte) {
                                vm.configuracionReporte = configuracionReporte.data;
                                const nombreFichaTramite = vm.configuracionReporte.NombreRdl;
                                const borrador = true;
                                const projectId = $sessionStorage.proyectoId;

                                servicioFichasProyectos.ObtenerIdFicha(nombreFichaTramite)
                                    .then(function (respuestaFicha) {
                                        var fichaPlantilla = {
                                            NombreReporte: nombreFichaTramite,
                                            IdReporte: respuestaFicha.ID,
                                            PARAM_BORRADOR: true,
                                            PARAM_BPIN: projectId,
                                            InstanciaId: $sessionStorage.idInstancia,
                                            NivelId: $sessionStorage.idNivel,
                                            TramiteId: 999
                                        };

                                        servicioFichasProyectos.GenerarFichaSGR($.param(fichaPlantilla))
                                            .then(function (respuesta) {
                                                if (borrador) {
                                                    const nombreArchivo = nombreFichaTramite.replace(/ /gi, "_") + '_' + projectId + '_' + moment().format("YYYYMMDDD_HHMMSS") + "pdf";
                                                    const blob = new Blob([respuesta], { type: 'application/pdf' });
                                                    const file = new File([blob], nombreArchivo, { type: 'application/pdf' });
                                                    FileSaver.saveAs(file, nombreArchivo);
                                                }
                                            }, function (error) {
                                                utilidades.mensajeError(error);
                                            });
                                    }, function (error) {
                                        utilidades.mensajeError(error);
                                    });
                            }, function (error) {
                                utilidades.mensajeError(error);
                            });
                    };
                }, function (error) {
                    utilidades.mensajeError(error);
                });

        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: vm.idInstancia,
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

        vm.habilitaFinalizar = function (estado) {
            vm.notificarsiguiente({ estado: estado });
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var isValid = true;
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            var nameArr = p.Error.split('-');
                            var TipoError = nameArr[0].toString();
                            if (TipoError == 'SGRVDPF1') {
                                vm.validarErrores(TipoError, p.Descripcion, false);
                            }
                            else {
                                vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                            }
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarValores = function (pregunta, errores, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarSeccion = function (tipoError, seccion, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError + seccion);
            if (campomensajeerror != undefined) {
                if (!esValido) {
                    campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                    campomensajeerror.classList.remove('hidden');
                } else {
                    campomensajeerror.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarErrores = function (tipoError, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError);
            if (campomensajeerror != undefined) {
                if (!esValido) {
                    campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                    campomensajeerror.classList.remove('hidden');
                } else {
                    campomensajeerror.classList.remove('ico-advertencia');
                }
            }
        }

        vm.limpiarErrores = function () {
            var errorElements = document.getElementsByClassName(`${vm.nombreComponente}-errores`);
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });
        }
    }


    angular.module('backbone').component('procesoFirmaViabilidadSgpSgp', {
        templateUrl: "/src/app/formulario/ventanas/SGP/viabilidadSGP/firmaEmision/procesoFirma/procesoFirmaSgp/procesoFirmaViabilidadSgpSgp.html",
        controller: procesoFirmaViabilidadSgpSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            namecomponent: '<',
            notificarsiguiente: '&'
        }
    })
        .directive('stringToNumber', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (value) {

                        return '' + value;
                    });
                    ngModel.$formatters.push(function (value) {
                        return parseFloat(value);
                    });
                }
            };
        });;
})();