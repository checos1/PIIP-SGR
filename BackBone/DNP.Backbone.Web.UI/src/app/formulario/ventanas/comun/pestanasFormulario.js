(function () {
    'use strict';
    pestanasFormularioController.$inject = [
        '$sessionStorage',
        '$rootScope',
        '$uibModal',
        '$timeout',
        '$interval',
        'pestanasFormularioServicio',
        'constantesAcciones',
        'justificacionCambiosServicio',
        'flujoServicios',
        'utilidades',
        '$location',
    ];

    function pestanasFormularioController($sessionStorage, $scope, $uibModal, $timeout, $interval, pestanasFormularioServicio, constantesAcciones, justificacionCambiosServicio, flujoServicios, utilidades, $location) {
        var vm = this;

        vm.IdInstancia = $sessionStorage.idInstancia;
        vm.abrirLogInstancias = abrirLogInstancias;
        vm.ImagenPermiso;
        vm.DehabilitaValidar = false;
        vm.Pestanas = [];
        vm.VerValidar = true;
        if ($sessionStorage.contieneSubpasos) { verificarSubPaso(); }
        vm.redirectIndexPage = redirectIndexPage;


        //Funciones

        vm.validarFormulario = function () {
            vm.onValidar();
        };

        vm.initPestana = function () {
            vm.callback({ evento: vm.cargarPestanas });
        };

        function redirectIndexPage() {
            $location.url("/proyectos/pl");
        }

        vm.cargarPestanas = function () {
            vm.IdMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
            vm.VerValidar = true;
            if ($sessionStorage.soloLectura) {
                vm.VerValidar = false;
            }
            pestanasFormularioServicio.obtenerSeccionesTramite(vm.IdMacroproceso, vm.IdInstancia, $sessionStorage.idNivel).then(function (respuesta) {
                var iguales = true;
                if (vm.Pestanas.length != respuesta.data.length || vm.Pestanas[0].Nombre != respuesta.data[0].Nombre ) {
                    vm.Pestanas = respuesta.data;
                }
                //if (respuesta.data.length == 1) {
                //    $scope.$watch(function () {
                //        vm.Pestanas = respuesta.data;
                //    });
                //}
                //else if (vm.Pestanas.length != respuesta.data.length) {
                //    vm.Pestanas = respuesta.data;
                //}
                else {
                    vm.Pestanas.forEach(function (value, index) {
                        var pestana = respuesta.data.filter(x => x.NombrePestana == value.NombrePestana);
                        if (pestana == undefined || pestana.length === 0)
                            iguales = false;
                    });

                    if (!iguales) {
                        vm.Pestanas = respuesta.data;
                    }
                }

                // if (vm.Pestanas.length === 0) {
                //vm.Pestanas = respuesta.data;
                //--}

                AgregarImagenRol(vm.valores.accion);
                vm.Pestanas.forEach(function (value, index) {
                    var pestanaNueva = respuesta.data.filter(x => x.NombrePestana == value.NombrePestana);
                    value.Porcentaje = pestanaNueva[0].Porcentaje;
                    if (value.Porcentaje == 0) {
                        vm.Pestanas[index].Imagen = "Img/etapas/luna_1.svg";
                    }
                    if (value.Porcentaje == 50) {
                        vm.Pestanas[index].Imagen = "Img/etapas/luna_2.svg";
                    }
                    if (value.Porcentaje == 100) {
                        vm.Pestanas[index].Imagen = "Img/etapas/luna_3.svg";
                    }
                });
                if (vm.valores.tieneError != undefined) {
                    if (!vm.valores.tieneError) {
                        vm.Pestanas.forEach(function (value, index) {
                            vm.Pestanas[index].Imagen = "Img/etapas/luna_4.svg";
                        });
                    }
                }
            });
        };

        vm.enviarSubPaso = function () {
            EnvioSiguienteSubPaso();
        };
        function EnvioSiguienteSubPaso() {
            if ($sessionStorage.visualizaEnviarSubpaso) {
                var data = {
                    InstanciaId: $sessionStorage.idInstancia,
                    RolId: $sessionStorage.idAccion,
                    idAccion: $sessionStorage.idAccion,
                    AvanceId: 1,
                    Usuario: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                    DireccionIp: "BackBone"
                }

                utilidades.mensajeWarning("Está seguro de continuar?",
                    function funcionContinuar() {
                        return flujoServicios.Flujos_SubPasoEjecutar(data).then(function (response) {
                            if (response.data) {
                                utilidades.mensajeSuccess("Se ha enviado el proyecto con éxito al siguiente paso del flujo", false, vm.redirectIndexPage, false);
                                vm.deshabilitaEnviarSubPaso = true;
                                vm.DehabilitaValidar = true;
                                $scope.$broadcast("HabilitarGuardarPaso", false);

                            } else {
                                utilidades.mensajeError("Error al realizar la operación", false);
                                vm.deshabilitaEnviarSubPasos = false;
                                vm.DehabilitaValidar = false;
                            }
                        })
                    },
                    function funcionCancelar() {
                    }, "Aceptar", "Cancelar", vm.mensajeWarningSubPaso)
            }

        }

        function verificarSubPaso() {
            flujoServicios.Flujos_SubPasosValidar($sessionStorage.idInstancia, $sessionStorage.idAccion, $sessionStorage.usuario.permisos.IdUsuarioDNP).then(
                function (response) {
                    if (response.data.Exitoso) {
                        $sessionStorage.HabilitarGuardarPaso = true;
                       }
                    else {
                        $sessionStorage.HabilitarGuardarPaso = false;
                           }
                    MostrarMensajeSubpasos(response.data.MensajeOperacion);
                    $scope.$broadcast("HabilitarGuardarPaso", $sessionStorage.HabilitarGuardarPaso);
                });
        }

        $scope.$on("visualizaEnviarSubpaso", function (evt, data) {
            vm.mostrarBtnEnviarSubpaso = data;
        });

        $interval(function () {
            if (vm.Pestanas != undefined && vm.Pestanas.length > 0) {
                var activas = 0;
                vm.Pestanas.forEach(function (value, index) {
                    var idPestana = document.getElementById("pestana-" + value.NombrePestana);
                    if (idPestana != null && idPestana.classList != null) {
                        if (idPestana.classList.contains("active")) {
                            activas++;
                        }
                    }
                });
                if (activas == 0) {
                    var primeraPestana = vm.Pestanas[0];
                    if (primeraPestana != null) {
                        var idPestanaUno = document.getElementById("pestana-" + primeraPestana.NombrePestana);
                        if (idPestanaUno != null) {
                            idPestanaUno.classList.add("active");
                        }
                    }
                }
            }
        }, 1000);

        function MostrarMensajeSubpasos(texto) {
            var MensajeSubpaso = document.getElementById('MensajeSubpaso');
            if ((MensajeSubpaso != undefined) || (texto.length>0)) {
                MensajeSubpaso.classList.remove('hidden');
                var msgSubPaso = document.getElementById('msgSubPaso');
                if (msgSubPaso != undefined) {
                    msgSubPaso.innerHTML = texto;
                    msgSubPaso.classList.remove('hidden');
                }
            }
            else {
                MensajeSubpaso.classList.add('hidden');
            }
        }


        function AgregarImagenRol(accion) {
            /*
             * Configuración
            */
            const conf = {
                urlEditarTooltip: "/Img/permiso1_barra_pest.svg",
                urlSoloLecturaTooltip: "/Img/permiso4_barra_pest.svg",
                urlEdicionBloqueadaTooltip: "/Img/permiso2_barra_pest.svg",
                urlEsperarEditarTooltip: "/Img/permiso3_barra_pest.svg"
            };
            /*Inicio tooltip*/
            vm.DehabilitaValidar = true;
            var imagenTooltip = conf.urlSoloLecturaTooltip;
            var tienePermisos = false;
            var roles = '';
            accion.Roles.map(function (item) {
                roles += item.NombreRol + ", ";
                var existe = $sessionStorage.usuario.roles.find(x => x.IdRol == item.IdRol);
                if (existe != null && existe != undefined)
                    tienePermisos = true;
            });
            roles = roles.substring(0, roles.length - 2);
            switch (accion.Estado) {
                case constantesAcciones.estado.ejecutada:
                    if (!tienePermisos)
                        imagenTooltip = conf.urlSoloLecturaTooltip;
                    else
                        imagenTooltip = conf.urlEdicionBloqueadaTooltip;
                    break;
                case constantesAcciones.estado.porDefinir:
                    if (!tienePermisos)
                        imagenTooltip = conf.urlSoloLecturaTooltip;
                    else
                        imagenTooltip = conf.urlEsperarEditarTooltip;
                    break;
                case constantesAcciones.estado.pasoEnProgreso:
                    if (!tienePermisos) {
                        imagenTooltip = conf.urlSoloLecturaTooltip;
                    }
                    else {
                        vm.DehabilitaValidar = false;
                        imagenTooltip = conf.urlEditarTooltip;
                    }
                    break;
                default:
                    imagenTooltip = conf.urlEditarTooltip;

            }
            vm.ImagenPermiso = imagenTooltip;

            if ($sessionStorage.contieneSubpasos) {
                if ($sessionStorage.HabilitarGuardarPaso) {
                    vm.DehabilitaValidar = false;}
                else { vm.DehabilitaValidar = true }
            }
           
        }

        vm.cargaNombreSeccion = function (seccion) {
            vm.seccionactiva = seccion;
        }

        function abrirLogInstancias() {


            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/comunes/log/modalLogInstanciasSubpasos.html',
                controller: 'modalLogInstanciasSubpasosController',
                controllerAs: "vm",
                openedClass: "consola-modal-soportesDNP",
                resolve: {
                    idInstancia: () => vm.IdInstancia,
                    /*
                    BPIN: () => row.IdObjetoNegocio,
                    nombreFlujo: () => row.NombreFlujo,
                    codigoProceso: () => row.CodigoProceso
                    */
                }
            });

            modalInstance.result.then(function (selectedItem) {


            }, function () {

            });
        }
    }
    angular.module('backbone').controller('observacionesFormulario', pestanasFormularioController);
    angular.module('backbone').component('pestanasFormulario', {
        controller: pestanasFormularioController,
        templateUrl: "src/app/formulario/ventanas/comun/pestanasFormulario.html",
        controllerAs: "vm",
        bindings: {
            valores: '=',
            onValidar: '&',
            callback: '&',
            seccionactiva:'='
        }
    });

})();