(function () {
    'use strict';

    agregarSectoresSgpController.$inject = ['$scope', 'requisitosSgpServicio', 'justificacionCambiosServicio', 'utilidades', '$sessionStorage'];

    function agregarSectoresSgpController(
        $scope,
        requisitosSgpServicio,
        justificacionCambiosServicio,
        utilidades,
        $sessionStorage,
    ) {
        var vm = this;
        vm.nombreComponente = "sgpviabilidadrequisitosdatosgeneralesagregarsectoressgp";
        vm.Bpin = null;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.soloLectura = false;

        vm.componentesRefresh = [
            "sgpviabilidadrequisitosverificacionrequisitossgp",
            "sgpviabilidadrequisitosdatosgeneralesagregarsectoressgp"
        ];

        vm.init = function () {
            vm.proyectoId = $sessionStorage.proyectoId;
            vm.nivelId = $sessionStorage.idNivel;
            vm.leerAcuerdoProyecto();
            vm.soloLectura = $sessionStorage.soloLectura;
            vm.notificarrefresco({ handler: vm.notificarRefrescoAcuerdo, nombreComponente: vm.nombreComponente });
            vm.Sectores = "";
            vm.Clasificadores = "";
            vm.disabledAgregar = true;
            vm.disabledCancelar = true;
            vm.tieneMensajeValidacion = false;
            vm.mensajeValidacion = "";
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };

        vm.leerAcuerdoProyecto = function () {
            requisitosSgpServicio.SGPAcuerdoLeerProyecto(vm.proyectoId, vm.nivelId)
                .then(function (response) {
                    if (response.data != null) {
                        vm.obj = response.data;
                        vm.Acuerdos = vm.obj[0].Acuerdos;
                        vm.MostrarGrid = false;
                        vm.Acuerdos.AcuerdoId = vm.Acuerdos[0].AcuerdoId;
                        vm.leerSectores();
                        vm.leerClasificadores();
                        //if (vm.Acuerdos.length == 1) {
                        //    vm.Acuerdos.AcuerdoId = vm.Acuerdos[0].AcuerdoId;      
                        //    vm.leerSectores();
                        //} else {
                        //    vm.Acuerdos.AcuerdoId = '';
                        //}                        

                        if (vm.obj[0].AcuerdoProyecto != null) {
                            if (vm.obj[0].AcuerdoProyecto.length > 0) {
                                vm.AcuerdoProyecto = vm.obj[0].AcuerdoProyecto;
                                vm.AcuerdoOriginalId = vm.obj[0].AcuerdoProyecto[0].AcuerdoId;
                                vm.MostrarGrid = true;
                            }
                        } else {
                            vm.AcuerdoProyecto = vm.obj[0].AcuerdoProyecto;
                        }

                        //if (vm.AcuerdoProyecto == null) {
                        //    eliminarCapitulosModificados();
                        //}

                    }
                }, function (error) {
                    utilidades.mensajeError('Ocurrió un problema al leer los acuerdos del proyecto.');
                });

        };

        vm.leerSectores = function () {
            if (vm.Acuerdos.AcuerdoId != null) {
                if (vm.Acuerdos.AcuerdoId != "") {
                    vm.Sectores = vm.obj[0].Sectores.filter(x => x.AcuerdoId === parseInt(vm.Acuerdos.AcuerdoId));
                }
            }
        };

        vm.notificarRefrescoAcuerdo = function () {
            vm.leerAcuerdoProyecto();
        }

        vm.leerClasificadores = function () {
            if (vm.Sectores.AcuerdoSectorId != null) {
                if (vm.Sectores.AcuerdoSectorId != "") {
                    vm.Clasificadores = vm.obj[0].Clasificadores.filter(x => x.AcuerdoSectorId === parseInt(vm.Sectores.AcuerdoSectorId));
                }
            }
        };

        vm.cambioClasificador = function () {
            vm.disabledAgregar = false;
            vm.disabledCancelar = false;
        };

        vm.Cancelar = function () {
            vm.Acuerdos.AcuerdoId = 0;
            vm.Sectores.AcuerdoSectorId = 0;
            vm.Clasificadores.AcuerdoSectorClasificadorId = 0;
            vm.disabledAgregar = true;
            vm.disabledCancelar = true;
            vm.mensajeValidacion = "";
            vm.tieneMensajeValidacion = false;
            vm.Sectores = "";
            vm.Clasificadores = "";
            vm.init();

        };

        vm.AgregarRegistro = function () {
            if ((parseInt(vm.Sectores.AcuerdoSectorId) > 0) && (parseInt(vm.Clasificadores.AcuerdoSectorClasificadorId) > 0)) {
                var result = vm.obj[0].Acuerdos.filter(x => x.AcuerdoId === parseInt(vm.Acuerdos.AcuerdoId));
                var AcuerdoNivelId = parseInt(result[0].AcuerdoNivelId);

                var data = {
                    Id: 0,
                    ProyectoId: vm.proyectoId,
                    AcuerdoNivelId: AcuerdoNivelId,
                    AcuerdoSectorClasificadorId: parseInt(vm.Clasificadores.AcuerdoSectorClasificadorId),
                    Activo: true,
                    TipoConcepto: 1
                }

                if (validarGuardar(data)) {
                    actualizar(data);
                    vm.disabledCancelar = true;
                }
                else {
                    vm.disabledCancelar = false;
                }
                vm.disabledAgregar = true;
            }
        };

        function validarGuardar(data) {
            var respuesta = true;
            if (vm.obj[0].AcuerdoProyecto != null) {
                vm.tieneMensajeValidacion = false;
                vm.mensajeValidacion = "";
                var result = vm.obj[0].AcuerdoProyecto.filter(x => x.AcuerdoSectorClasificadorId === parseInt(data.AcuerdoSectorClasificadorId));
                if (result.length > 0) {
                    vm.tieneMensajeValidacion = true;
                    vm.mensajeValidacion = "No se puede agregar esta tipología porque ya existe";
                    respuesta = false;
                }
                if (vm.AcuerdoOriginalId != parseInt(vm.Acuerdos.AcuerdoId)) {
                    vm.tieneMensajeValidacion = true;
                    vm.mensajeValidacion = "No se permiten tipologías de acuerdos diferentes";
                    respuesta = false;
                }
            }
            return respuesta;
        };



        function actualizar(data) {
            return requisitosSgpServicio.SGPAcuerdoGuardarProyecto(data).then(function (response) {
                if (response.data) {
                    vm.leerAcuerdoProyecto();
                    vm.Sectores = "";
                    vm.Clasificadores = "";
                    guardarCapituloModificado();
                    utilidades.mensajeSuccess("Los datos se han guardado con éxito", false, false, false);
                } else {
                    utilidades.mensajeError("Error al realizar la operación", false);
                    vm.disabledAgregar = true;
                }
            });
        };

        vm.clickIconoEliminar = function (Id) {
            var result = vm.obj[0].AcuerdoProyecto.filter(x => x.Id === parseInt(Id));

            var data = {
                Id: 0,
                ProyectoId: vm.proyectoId,
                AcuerdoNivelId: parseInt(result[0].AcuerdoNivelId),
                AcuerdoSectorClasificadorId: parseInt(result[0].AcuerdoSectorClasificadorId),
                Activo: false,
                TipoConcepto: 1
            }

            eliminar(data);

        };

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.leerAcuerdoProyecto();
            }
        }


        function eliminar(data) {
            utilidades.mensajeWarning("La línea de información seleccionada será desagregada de la tabla. Está seguro de continuar?",
                function funcionContinuar() {
                    return requisitosSgpServicio.SGPAcuerdoGuardarProyecto(data).then(function (response) {
                        if (response.data) {
                            /* guardarCapituloModificado();*/
                            if (vm.AcuerdoProyecto.length == 1) {
                                eliminarCapitulosModificados();
                            }
                            else {
                                vm.guardadocomponent({ nombreComponenteHijo: vm.nombreComponente });
                            }

                            utilidades.mensajeSuccess("Se ha desagregado con éxito la línea de información", false, false, false, "Éxito");                            
                            guardarCapituloModificado();
                        } else {
                            utilidades.mensajeError("Error al realizar la operación", false);
                            vm.disabledAgregar = true;
                        }
                    })
                },
                function funcionCancelar() {
                }, "Aceptar", "Cancelar", vm.mensajeWarning)
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: vm.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadocomponent({ nombreComponenteHijo: vm.nombreComponente });
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
                InstanciaId: vm.idInstancia,

            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadocomponent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

    }

    angular.module('backbone').component('agregarSectoresSgp', {
        templateUrl: "/src/app/formulario/ventanas/SGP/viabilidadSGP/datosGenerales/agregarSectores/agregarSectoresSgp.html",

        controller: agregarSectoresSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            guardadocomponent: '&',
            notificacioncambios: '&'
        },
    });

})();