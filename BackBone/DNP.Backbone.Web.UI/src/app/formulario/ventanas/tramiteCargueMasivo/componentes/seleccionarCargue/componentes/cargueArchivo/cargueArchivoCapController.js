(function () {
    'use strict';

    cargueArchivoCapController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModal',
        'utilidades',
        'cargueArchivoCapServicio',
        'utilsValidacionSeccionCapitulosServicio'
    ];

    function cargueArchivoCapController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        cargueArchivoCapServicio,
        utilsValidacionSeccionCapitulosServicio

    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "seleccionarcarguecarguearchivocap";
        vm.disabled = true;
        vm.habilitaBotones = true;
        vm.habilitarFinal = false;
        vm.registrosExcel = null;
        vm.listaDatosExcel = [];
        vm.verDatosExcel = false;
        vm.codigoProceso = '';
        vm.totalCargueProceso = 0;
        vm.ConvertirNumero = ConvertirNumero;

        vm.init = function () {
            vm.consultarCargueExcel();
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });
        };

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'seleccionarcarguecarguearchivocap': true
            };
        }

        vm.abrilNivel = function (idElement) {
            console.log(idElement);
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                }
            }
        }


        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.refreshComponente();
            }
        };

        vm.actualizarCargue = function () {
            const objetoNegocioDto = {
                ObjetoNegocioId: $sessionStorage.idObjetoNegocio
            };

            cargueArchivoCapServicio.actualizarCargueMasivo(objetoNegocioDto).then(
                function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {

                        if (response.data.Exito) {
                            utilidades.mensajeSuccess('Cargue actualizado exitosamente.');
                            vm.verDatosExcel = false;
                        } else {
                            swal('', response.data.Mensaje, 'warning');
                        }
                    } else {
                        swal('', "Error al actualizar valores del cargue.", 'error');
                    }
                },
                function (error) {
                    if (error) {
                        utilidades.mensajeError(error);
                    }
                }
            );
        };


        vm.consultarCargueExcel = function () {
            const objetoNegocioDto = {
                ObjetoNegocioId: $sessionStorage.idObjetoNegocio
            };
            vm.listaDatosExcel = [];
            cargueArchivoCapServicio.consultarCargueExcel(objetoNegocioDto).then(
                function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {
                        var arreglolistas = jQuery.parseJSON(response.data);
                        vm.listaDatos = jQuery.parseJSON(arreglolistas);

                        vm.listaDatos.forEach(lista => {
                            vm.codigoProceso = lista.codigoProceso;
                            vm.totalCargueProceso = lista.totalCargue;
                            if (lista.DatosCargueEntidad && lista.DatosCargueEntidad.length > 0) {
                                const listaGrupoRegistros = lista.DatosCargueEntidad;
                                listaGrupoRegistros.forEach(registro => {
                                    vm.listaDatosExcel.push({
                                        codigoEntidad: registro.codigoEntidad,
                                        nombreEntidad: registro.nombreEntidad,
                                        totalEntidad: registro.totalEntidad
                                    });
                                });
                            }
                        });


                    } else {
                        swal('', "Error al obtener valores del cargue.", 'error');
                    }
                },
                function (error) {
                    if (error) {
                        utilidades.mensajeError(error);
                    }
                }
            );
        };

        vm.ActivarEditar = function () {
            vm.verDatosExcel = false;
            vm.callback({ arg: true, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });

        };


        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 4,
            }).format(numero);
        }



        /*--------------------- Validaciones ---------------------*/

        vm.notificacionValidacionPadre = function (errores) {

            vm.limpiarErrores();
            var isValid = true;
            if (errores != undefined) {
                var erroresCarguemasivo = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresCarguemasivo != undefined) {
                    var erroresJson = erroresCarguemasivo.Errores == "" ? [] : JSON.parse(erroresCarguemasivo.Errores);
                    if (erroresJson != undefined) {
                        isValid = (erroresJson == null || erroresJson.length == 0);
                        if (!isValid) {
                            erroresJson[vm.nombreComponente].forEach(p => {

                                if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                            });
                        }
                    }
                }

                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }

        }

        vm.notificacionValidacionEvent = function (listErrores) {
            vm.esValido = true;
        }



        vm.limpiarErrores = function () {
            var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-campoObligatorioProyectos-error");

            if (campoObligatorioProyectos != undefined) {
                campoObligatorioProyectos.innerHTML = '';
                campoObligatorioProyectos.classList.add('hidden');
            }
        };

        vm.ejecutarErrores = function () {
            vm.limpiarErrores();
            vm.erroresActivos.forEach(p => {
                if (vm.errores[p.Error] != undefined) {
                    vm.errores[p.Error]({
                        error: p.Error,
                        descripcion: p.Descripcion,
                        data: p.Data
                    });
                }
            });
        };

        vm.validarExistenciaProyectos = function (errores) {
            var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-campoObligatorioProyectos-error");
            if (campoObligatorioProyectos != undefined) {
                campoObligatorioProyectos.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioProyectos.classList.remove('hidden');
            }
        }

        vm.errores = {
            'CMO002': vm.validarExistenciaProyectos
        }

    }

    angular.module('backbone').component('cargueArchivoCap', {
        templateUrl: "src/app/formulario/ventanas/tramiteCargueMasivo/componentes/seleccionarCargue/componentes/cargueArchivo/cargueArchivoCap.html",
        controller: cargueArchivoCapController,
        controllerAs: "vm",
        bindings: {
            bpin: '@',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacioncambios: '&',
            notificacionestado: '&',
            callback: '&'
        }
    });

})();