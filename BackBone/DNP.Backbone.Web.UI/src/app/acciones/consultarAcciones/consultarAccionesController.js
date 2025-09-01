(function () {
    'use strict';

    angular.module('backbone').controller('consultarAccionesController', consultarAccionesController);
    consultarAccionesController.$inject = ['$scope', '$timeout', 'servicioAcciones', '$sessionStorage', 'constantesAcciones', 'constantesTipoBancoProyecto', '$uibModal', '$location', 'utilidades', '$compile', 'servicioUsuarios', '$filter']

    function consultarAccionesController($scope, $timeout, servicioAcciones, $sessionStorage, constantesAcciones, constantesTipoBancoProyecto, $uibModal, $location, utilidades, $compile, servicioUsuarios, $filter) {

        var vm = this;
        //EventosValidarFormulario
        vm.eventoValidar;
        vm.eventoGuardado;
        $sessionStorage.ventanaAnterior = null;
        //Metodos
        vm.abrirMigaPan = abrirMigaPan;
        vm.cerrarMigaPan = cerrarMigaPan;
        vm.seleccionarAccion = seleccionarAccion;
        vm.tieneMigaPan = tieneMigaPan;
        vm.abrirAccion = abrirAccion;
        vm.migaPanFlujos = [];
        //Variables Globales
        vm.acciones = [];
        vm.listadoAcciones = [];
        // variables para el nuevo diagrama
        vm.accionesDiagrama = [];
        vm.idAccion = 0;
        vm.informacionDiagrama = {
            usuario: '',
            rol: '',
            entidad: '',
            fechaCreacion: ''
        };
        $sessionStorage.vigenciaHorizonte = '';
        $sessionStorage.soloLectura = false;
        $sessionStorage.BanderaDisabledEditarSGP = false;
        vm.fechaCreacion = null;
        vm.nombreEntidad = '';
        vm.papel = null;
        vm.usuario = '';
        vm.modeloBotones = { handler: null, disabled: true, visible: false, observacionEnvio: false, tituloObservacion: '', ocultarDevolver: false, nombreAccion: '', descripcionAccion: '', textoObservacionEnvio: '', nombreAccionSiguiente: 'SIGUIENTE' };
        vm.listadoMenu = [];
        vm.listadoMiga = [];
        vm.indiceAccionSeleccionada = 0;
        vm.validaHabilita = validaHabilita;
        vm.constantesAcciones = constantesAcciones;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.nombreProyecto = $sessionStorage.nombreProyecto;
        vm.idProyecto = $sessionStorage.idObjetoNegocio;
        vm.nombreFlujo = '';
        vm.nombreAccion = '';
        vm.descripcionAccion = '';
        vm.estadoPaso = '';
        vm.selectores = [];
        $sessionStorage.accionDeshabilitada = false;
        vm.cargarFormulario = false;
        vm.vistaPersonalizada = false;
        vm.nombreFormularioCustom = '';
        vm.controllerFormularioCustom = '';
        vm.abrirMiga = false;
        vm.cerrarMiga = false;
        vm.habilitarFormulario = false;
        vm.ProyectoId = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.ProyectoId : 0;
        vm.nombreComponente = "observaciones";
        vm.ocultarObservaciones = "false";
        vm.accionSeleccionada = null;
        vm.seccionActiva = undefined;
        vm.flujoId = '';
        vm.crtypeId = $sessionStorage.crtypeId;
        vm.resourceGroupId = $sessionStorage.resourceGroupId;
        vm.isSGR = vm.resourceGroupId == constantesTipoBancoProyecto.sgr;
        vm.isSGP = vm.resourceGroupId == undefined ? $sessionStorage.nombreFlujo.includes('SGP') : vm.resourceGroupId == constantesTipoBancoProyecto.territorio;
        vm.rol = null;
        vm.slickConfigMenu = {
            method: {},
            speed: 300,
            adaptiveHeight: true,
            slidesToShow: 5,
            slidesToScroll: 1,
            infinite: false,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 5,
                        slidesToScroll: 1
                    }
                },
                {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 4,
                        slidesToScroll: 1
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1
                    }
                }
            ],
            event: {
                beforeChange: function () {

                },
                afterChange: function () {

                }
            },
            prevArrow: '<button type="button" class="slick-prev" data-role="none" aria-label="Previous" tabindex="0" role="button"> <img src="Img/arrow-left.png" alt="Izquierda"> </button>',
            nextArrow: '<button type="button" class="slick-next" data-role="none" aria-label="Next" tabindex="0" role="button"> <img src="Img/arrow-right.png" alt="Derecha"> </button>'
        };

        vm.slickConfigMiga = {
            method: {},
            speed: 300,
            adaptiveHeight: true,
            slidesToShow: 6,
            slidesToScroll: 1,
            infinite: false,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 6,
                        slidesToScroll: 1
                    }
                },
                {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 4,
                        slidesToScroll: 1
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1
                    }
                }
            ],
            event: {
                beforeChange: function () {

                },
                afterChange: function () {

                }
            }
        };
        vm.abrirMGA = function () {
            servicioAcciones.ObtenerTokenMGA(vm.idProyecto, tipoUsuarioAutenticado).then(function (respuesta) {
                window.open(respuesta.data, '_blank').focus();
            });
        };
        vm.formularioGuardado = function (evento) {
            if (evento != undefined)
                vm.eventoGuardado = evento;
        };
        vm.validarFormulario = function () {
            vm.eventoValidar();
        };
        vm.abrirModalFlujos = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: '/src/app/acciones/consultarAcciones/modales/modalFlujo.html',
                controller: 'modalFlujoController',
                size: 'lg',
                resolve: {
                    items: function () {
                        return vm.accionesDiagrama;
                    },
                    idAccion: function () {
                        return vm.idAccion;
                    },
                    nombreFlujo: function () {
                        return vm.nombreFlujo;
                    },
                    info: function () {
                        return vm.informacionDiagrama;
                    }
                }
            });
            modalInstance.result.then(function (accion) {
                $timeout(function () {
                    let arrayAccion = [accion];
                    arrayAccion.push(accion);
                    let accionSeleccionada = crearListadoAcciones(arrayAccion)[0];
                    vm.idAccion = 0;
                    vm.indiceAccionSeleccionada = 0;
                    vm.seleccionarAccion(accionSeleccionada);
                    return false;
                }, 50);
            }, function () {
                vm.idAccion = 0;
            });
        };



        function validaHabilita(validacion, arg, aprueba, titulo = '', ocultarDevolver = false, nombreAccion = '', descripcionAccion = '', observacionEnvio = '', textoObservacionEnvio = '') {
            vm.modeloBotones = { handler: null, disabled: true, visible: false, observacionEnvio: false, tituloObservacion: '', ocultarDevolver: false, nombreAccion: '', descripcionAccion: '', textoObservacionEnvio: '', nombreAccionSiguiente: 'Siguiente' };

            if (arg === undefined)
                arg = true;

            vm.modeloBotones.disabled = arg;
            vm.modeloBotones.visible = !arg;
            vm.modeloBotones.observacionEnvio = aprueba;
            vm.modeloBotones.tituloObservacion = titulo;
            vm.modeloBotones.ocultarDevolver = ocultarDevolver;
            vm.modeloBotones.nombreAccion = nombreAccion;
            vm.modeloBotones.descripcionAccion = descripcionAccion;
            vm.modeloBotones.textoObservacionEnvio = textoObservacionEnvio;

            if (vm.acciones[0] != undefined) {
                if (vm.acciones[0].OrdenVisualizacion == "1") {
                    vm.modeloBotones.ocultarDevolver = true;
                }
                if (vm.listadoAcciones.length.toString() == vm.acciones[0].OrdenVisualizacion) {
                    vm.modeloBotones.nombreAccionSiguiente = "FINALIZAR";
                }
                else {
                    vm.modeloBotones.nombreAccionSiguiente = "SIGUIENTE";
                }
                if (vm.acciones[0].Estado === constantesAcciones.estado.ejecutada)
                    vm.modeloBotones.disabled = true;

                if (vm.acciones[0].Ventana == "aclaracionleyendaPasoDos") {
                    vm.modeloBotones.ocultarDevolver = true;
                    if (validacion != null)
                        if (validacion.validacionPaso2)
                            vm.modeloBotones.ocultarDevolver = false;
                }

                if (vm.acciones[0].Ventana == "aclaracionleyendaPasoCuatro") {
                    vm.modeloBotones.ocultarDevolver = true;
                    if (validacion != null)
                        if (validacion.validacionPaso4)
                            vm.modeloBotones.ocultarDevolver = false;
                }

                if (vm.acciones[0].Ventana == "aclaracionleyendaPasoQuinto") {
                    vm.modeloBotones.ocultarDevolver = true;
                    if (validacion.tieneError != null)
                        vm.modeloBotones.ocultarDevolver = false;
                }

            }

            if (ocultarDevolver == false) {
                //vm.modeloBotones.disabled = false;
                if (vm.modeloBotones.visible == false) {
                    vm.modeloBotones.tituloObservacionAnterior = 'TEXTO DEVOLUCIÓN';
                }
                else {
                    if ($sessionStorage.AccionesDevolucion != undefined) {
                        vm.modeloBotones.tituloObservacionAnterior = 'Observación ' + $sessionStorage.AccionesDevolucion[0].NombreAccionDevolucion;
                    }
                }
            }

            if (titulo == '') {
                vm.modeloBotones.tituloObservacion = 'Observación ' + vm.modeloBotones.nombreAccion;
            }

            if ($sessionStorage.vigenciaHorizonte != '') {
                vm.vigencias = $sessionStorage.vigenciaHorizonte;
            }
            if (validacion != undefined) {
                if (validacion.evento != undefined)
                    vm.eventoValidar = validacion.evento;
                if (validacion.tieneError != undefined)
                    vm.tieneError = validacion.tieneError;
                if (validacion.tieneErrorObservacion != undefined)
                    vm.showAlertError(vm.nombreComponente, validacion.tieneErrorObservacion != undefined ? validacion.tieneErrorObservacion : false, validacion.descObservacion);
            }
            if (vm.eventoGuardado != undefined)
                vm.eventoGuardado();
        }

        function abrirModalFlujoCompleto() {
            var modalFlujoInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: '/src/app/acciones/consultarAcciones/modales/modalConfirmarCompletado.html',
                controller: function flujoCompletoController($scope, $uibModalInstance) {
                    $scope.modal = $uibModalInstance;
                },
                size: '',
                resolve: {
                }
            });
            modalFlujoInstance.result.then(function () {
                $location.url("/");
                //window.location.href = "/";
            }, function () {
                $location.url("/");
                //window.location.href = "/";
            });
        }
        function inizializar() {
            $sessionStorage.IdMacroproceso = undefined;
            obtenerAccionesServicio();
            $scope.$on('acciones:recargar', function (evento) {
                evento.stopPropagation();
                recargarMenu();
            });
        }

        function recargarMenu() {
            limpiarMenu();
            obtenerAccionesServicio();
        }

        function limpiarMenu() {
            vm.listadoMenu = [];
            vm.listadoMiga = [];
        }

        function tieneMigaPan(accion) {
            return [vm.constantesAcciones.tipo.accionTipoAnidada, vm.constantesAcciones.tipo.scopeRamas].indexOf(accion.TipoAccion) > -1;
        }

        function abrirMigaPan(evento, accion) {
            evento.stopPropagation();

            accion.Animar = true;
            $timeout(function () {
                var miga = angular.copy(vm.listadoMiga);
                var listaAccionesCopia = vm.listadoMenu.map(function (accionCopia) {
                    accionCopia.Seleccionado = false;
                    accionCopia.Animar = false;
                    if (accionCopia.IdAccion === accion.IdAccion) {
                        mostrarMenu(crearListadoAcciones(accion.AccionesHijas));
                    }
                    return accionCopia;
                });

                accion.Padre = listaAccionesCopia;
                miga.push(accion);
                mostrarMiga(miga);
                vm.indiceAccionSeleccionada = 0;
                vm.listadoMenu[0].Seleccionado = true;
                accion.Animar = false;
                vm.cargarFormulario = false;
            }, 200);
        }

        function cerrarMigaPan(accion) {
            vm.abrirMiga = false;
            vm.cerrarMiga = true;
            $timeout(function () {
                var indice = vm.listadoMiga.indexOf(accion);
                var miga = angular.copy(vm.listadoMiga);
                miga.splice(indice, miga.length);
                mostrarMenu(accion.Padre);
                mostrarMiga(miga);
                vm.indiceAccionSeleccionada = 0;
                vm.listadoMenu[0].Seleccionado = true;
                vm.cargarFormulario = false;
            }, 200);
        }
        function mostrarMenu(data) {
            vm.recargarMenu = false;
            vm.listadoMenu = data;
            $timeout(function () {
                vm.recargarMenu = true;
            }, 50);
        }

        function mostrarMiga(data) {
            vm.cerrarMiga = false;
            vm.abrirMiga = true;
            vm.recargarMiga = false;
            vm.listadoMiga = data;
            $timeout(function () {
                vm.recargarMiga = true;
            }, 50);

        }

        function dibujarDiagrama(acciones) {
            validarFlujoCompleto(acciones);
            // declaro un objeto constante de algunos valores globales para el diagrama
            const conf = {
                radioEvento: 15, // radio de los circulos de inicio y fin
                azul: "#004884",
                verde: "#069169",
                blanco: "#FFFFFF",
                gris: "#E3EAFC",
                rojo: "#F11515",
                anchoContenedor: $(".carrusel").width(),
                altoContainer: 70,
                cursor: "pointer",
                estiloFlecha: "block-wide-long",
                posicionX: 20,
                posicionY: 45,
                anchoTarea: 40,
                altoTarea: 32,
                radius: 5,
                grosor1: 1,
                grosor2: 2,
                tamanoFuente: 12,
                tamanoFuenteEvento: 16,
                //fuente: "Work Sans",
                fuente: "Montserrat",
                fuenteEventos: "Work Sans",
                opacidadInicio: 0.2,
                opacidadFinal: 0.5,
                urlLapiz: "/Img/pencil.png",
                urlSeparador: "/Img/u472.svg",
                urlCheck: "/Img/check.svg",
                urlEditarTooltip: "/Img/Editar_tooltip.svg",
                urlSoloLecturaTooltip: "/Img/SoloLectura_tooltip.svg",
                urlEdicionBloqueadaTooltip: "/Img/EdicionBloqueada_tooltip.svg",
                urlEsperarEditarTooltip: "/Img/EspararEditar_tooltip.svg",
                anchoImagen: 10,
                altoImagen: 8,
                esTarea: false,
                tamanoEntrada: 20,
                espacioEntradaTarea: 5,
                esInicio: true
            };

            conf.posicionY = (conf.altoContainer / 2);
            var papel = Raphael('paper', conf.anchoContenedor, conf.altoContainer);
            vm.papel = papel;
            var eventoInicial = dibujarEvento(papel, conf.posicionX, conf.posicionY, conf.radioEvento, conf.verde, conf.verde, conf.grosor2, conf.cursor);
            eventoInicial.hover(function () {
                this.animate({ "fill-opacity": conf.opacidadFinal }, 1000, "backIn");
            }, function () {
                this.animate({ "fill-opacity": conf.opacidadInicio }, 1000, "backOut");
            });
            dibujarTexto(papel, (conf.posicionX - conf.radioEvento), (conf.posicionY - conf.radioEvento - 10), 'Inicio', conf.verde, conf.tamanoFuente, conf.fuenteEventos, null, null, false);

            conf.posicionX += conf.radioEvento;
            var cantidadEjecutadas = 0;
            for (var i = 0; i < acciones.length; i++) {
                var accion = acciones[i];
                if (accion.Estado === constantesAcciones.estado.ejecutada) {
                    cantidadEjecutadas++;
                }
                var coordFlechas = "M" + conf.posicionX + " " + (conf.altoContainer / 2) + "L" + (conf.posicionX += 80) + " " + (conf.altoContainer / 2);
                switch (accion.TipoAccion) {
                    case constantesAcciones.tipo.accionTipoTransaccional:

                        var tarea = dibujarTarea(papel, conf.posicionX, (conf.posicionY - (conf.altoTarea / 2)), conf.anchoTarea, conf.altoTarea, conf.radius, conf.grosor2, null, "pointer", accion.Id);

                        var idTarea = tarea.node.id.replace("task_", "");
                        var selectorX = tarea.node.x.animVal.value + (conf.anchoTarea / 4);
                        var selectorY = (tarea.node.y.animVal.value + (conf.altoTarea)) + 5;
                        var selectorAncho = conf.anchoTarea / 2;
                        var selectorAlto = conf.altoTarea / 2;
                        AgregarTooltip(accion, conf, idTarea, selectorX, selectorY, selectorAncho, selectorAlto);

                        var textPosicionX = conf.posicionX + conf.tamanoFuenteEvento - 2;

                        tarea.node.onclick = function (i) {
                            var taskId = this.id.replace("task_", "");
                            var selectorX = this.x.animVal.value + (conf.anchoTarea / 4);
                            var selectorY = (this.y.animVal.value + (conf.altoTarea)) + 5;
                            var selectorAncho = conf.anchoTarea / 2;
                            var selectorAlto = conf.altoTarea / 2;
                            vm.abrirAccion(taskId, selectorX, selectorY, selectorAncho, selectorAlto, conf.azul, conf.grosor1);
                        }

                        var colorTarea = conf.azul;
                        $sessionStorage.soloLectura = false;
                        if (accion.Estado === constantesAcciones.estado.ejecutada) {
                            $sessionStorage.soloLectura = true;
                            agregarImagen(papel, accion.Estado === constantesAcciones.estado.ejecutada ? conf.urlCheck : conf.urlLapiz, (conf.posicionX + (conf.anchoTarea / 3)), (conf.posicionY - conf.altoTarea), conf.anchoImagen, conf.altoImagen, conf.cursor);
                            colorTarea = conf.verde;
                        }

                        var textoAccion = dibujarTexto(papel, (textPosicionX + 6), (conf.posicionY), accion.OrdenVisualizacion, colorTarea, conf.tamanoFuenteEvento, conf.fuente, null, "center", true);
                        textoAccion.toBack();

                        agregarColorTareaPorEstatus(tarea, accion.Estado, conf);
                        conf.posicionX += conf.anchoTarea;
                        break;
                    case constantesAcciones.tipo.accionTipoEnrutamiento:
                        //
                        let diagonalRomboEnruta = Math.sqrt(2) * conf.tamanoEntrada;
                        var rotateEnruta = "r-45," + conf.posicionX + "," + conf.posicionY;
                        var iconoEnruta = papel.rect(conf.posicionX, conf.posicionY, conf.tamanoEntrada, conf.tamanoEntrada).attr({ stroke: conf.azul }).animate({ transform: rotateEnruta }, 0.5);
                        iconoEnruta.node.onclick = (function (i) {
                            return function () {
                                vm.idAccion = acciones[i].Id;
                                vm.abrirModalFlujos();
                                return false;
                            }
                        })(i);

                        var iconoCaminoEnruta = dibujarTexto(papel, conf.posicionX + 5, conf.posicionY, "O", conf.azul, 25, conf.fuente, conf.cursor);
                        iconoCaminoEnruta.node.onclick = (function (i) {
                            return function () {
                                vm.idAccion = acciones[i].Id;
                                vm.abrirModalFlujos();
                                return false;
                            }
                        })(i);

                        dibujarTexto(papel, conf.posicionX, (conf.posicionY + conf.altoTarea + conf.tamanoFuente), accion.Nombre, conf.azul, conf.tamanoFuente, conf.tamanoFuente);
                        //
                        conf.posicionX += diagonalRomboEnruta;
                        //
                        break;
                    //
                    case constantesAcciones.tipo.accionTipoAnidada:
                        var entrada = papel.rect(conf.posicionX, (conf.posicionY - (conf.tamanoEntrada / 2)), conf.tamanoEntrada, conf.tamanoEntrada);
                        entrada.attr({ stroke: conf.azul });
                        let iconoMas = dibujarTexto(papel, (conf.posicionX + conf.tamanoEntrada / 2), conf.posicionY, "+", conf.azul, 40, conf.fuente, conf.cursor, "middle");
                        iconoMas.node.onclick = (function (i) {
                            return function () {
                                vm.idAccion = acciones[i].Id;
                                vm.abrirModalFlujos();
                                return false;
                            }
                        })(i);
                        conf.posicionX += conf.tamanoEntrada;
                        let coordMiniConector = "M" + conf.posicionX + " " + (conf.altoContainer / 2) + "L" + (conf.posicionX += conf.espacioEntradaTarea) + " " + (conf.altoContainer / 2);
                        dibujarFlecha(papel, coordMiniConector, conf.azul, conf.grosor2, "none");
                        let tareaCamino = dibujarTarea(papel, conf.posicionX, conf.posicionY - (conf.altoTarea / 2), conf.anchoTarea, conf.altoTarea, conf.radius, conf.grosor2, conf.azul);
                        tareaCamino.node.innerHTML = `<title>Nombre: ${accion.Nombre} \nEntidad: ${vm.informacionDiagrama.entidad} \nRol: ${vm.informacionDiagrama.rol}</title>`;
                        tareaCamino.node.onclick = (function (i) {
                            return function () {
                                vm.idAccion = acciones[i].Id;
                                vm.abrirModalFlujos();
                                return false;
                            }
                        })(i);
                        agregarColorTareaPorEstatus(tareaCamino, accion.Estado, conf);
                        if (accion.Estado === constantesAcciones.estado.pasoEnProgreso) {
                            dibujarTexto(papel, conf.posicionX, (conf.posicionY + conf.altoTarea), accion.Nombre, conf.azul, conf.tamanoFuente, conf.tamanoFuente);
                        }
                        let imagenLapizAnidada = agregarImagen(papel, accion.Estado === constantesAcciones.estado.ejecutada ? conf.urlCheck : conf.urlLapiz, (conf.posicionX + (conf.anchoImagen / 2)), (conf.posicionY - (conf.altoImagen / 2)), conf.anchoImagen, conf.altoImagen, conf.cursor);
                        imagenLapizAnidada.toBack();
                        //dibujarTexto(papel, conf.posicionX, (conf.posicionY + conf.altoTarea), accion.OrdenVisualizacion, conf.azul, conf.tamanoFuente - 1, conf.fuente);
                        conf.posicionX += conf.anchoTarea;
                        break;
                    case constantesAcciones.tipo.scopeParalelas:
                        let diagonalRombo = Math.sqrt(2) * conf.tamanoEntrada;
                        var rotate = "r-45," + conf.posicionX + "," + conf.posicionY;
                        var icono = papel.rect(conf.posicionX, conf.posicionY, conf.tamanoEntrada, conf.tamanoEntrada).attr({ stroke: conf.azul }).animate({ transform: rotate }, 0.5);
                        let visualizacionPNombre = dibujarTexto(papel, (conf.posicionX + conf.tamanoEntrada / 2), conf.posicionY + conf.tamanoEntrada + conf.espacioEntradaTarea, accion.OrdenVisualizacion, conf.azul, conf.tamanoFuente - 1, conf.fuente);
                        icono.node.onclick = (function (i) {
                            return function () {
                                vm.idAccion = acciones[i].Id;
                                vm.abrirModalFlujos();
                                return false;
                            }
                        })(i);
                        var iconoCamino = dibujarTexto(papel, conf.posicionX, conf.posicionY, "+", conf.azul, 50, conf.fuente, conf.cursor);
                        iconoCamino.node.onclick = (function (i) {
                            return function () {
                                vm.idAccion = acciones[i].Id;
                                vm.abrirModalFlujos();
                                return false;
                            }
                        })(i);

                        //
                        conf.posicionX += diagonalRombo;
                        let coordMiniParalelas = "M" + conf.posicionX + " " + (conf.altoContainer / 2) + "L" + (conf.posicionX += conf.espacioEntradaTarea) + " " + (conf.altoContainer / 2);
                        dibujarFlecha(papel, coordMiniParalelas, conf.azul, conf.grosor2, "none");
                        let tareaCaminoParalelas = dibujarTarea(papel, conf.posicionX, conf.posicionY - (conf.altoTarea / 2), conf.anchoTarea, conf.altoTarea, conf.radius, conf.grosor2, conf.azul);
                        let imagenLapizParalela = agregarImagen(papel, accion.Estado === constantesAcciones.estado.ejecutada ? conf.urlCheck : conf.urlLapiz, (conf.posicionX + (conf.anchoImagen / 2)), (conf.posicionY - (conf.altoImagen / 2)), conf.anchoImagen, conf.altoImagen, conf.cursor);
                        imagenLapizParalela.node.innerHTML = `<title>Nombre: ${accion.Nombre} \nEntidad: ${vm.informacionDiagrama.entidad} \nRol: ${vm.informacionDiagrama.rol}</title>`;
                        agregarColorTareaPorEstatus(tareaCaminoParalelas, acciones[i].Estado, conf);
                        imagenLapizParalela.node.onclick = (function (i) {
                            return function () {
                                vm.idAccion = acciones[i].Id;
                                vm.abrirModalFlujos();
                                return false;
                            }
                        })(i);
                        //
                        const totalHijas = accion.AccionesParalelas.length;
                        dibujarTexto(papel, conf.posicionX, conf.posicionY + conf.tamanoEntrada, (totalHijas > 1 ? totalHijas + " acciones más" : totalHijas + " acción más"), conf.azul, conf.tamanoFuente - 1, conf.fontFamily);
                        let requeridas = 0;
                        for (var m = 0; m < totalHijas; m++) {
                            if (accion.AccionesParalelas[m].EsObligatoria) {
                                requeridas += 1;
                            }
                        }
                        dibujarTexto(papel, conf.posicionX, (conf.posicionY + conf.tamanoEntrada + conf.tamanoFuente), (requeridas > 1 ? requeridas + " obligatorias" : requeridas + " obligatoria"), conf.rojo, conf.tamanoFuente - 1, conf.fontFamily);
                        conf.posicionX += conf.anchoTarea;
                        break;
                    default:
                }
                agregarFlechaPorEstatus(papel, coordFlechas, accion.Estado, conf);
            }
            var coordFlechaFin = "M" + (conf.posicionX + conf.grosor2) + " " + (conf.altoContainer / 2) + "L" + (conf.posicionX += 73) + " " + (conf.altoContainer / 2);
            var colorFin = conf.blanco;
            var colorBordeFin = conf.azul;
            if (cantidadEjecutadas === acciones.length) {
                colorFin = conf.verde;
                colorBordeFin = conf.verde;
            }
            var flechaFin = dibujarFlecha(papel, coordFlechaFin, colorBordeFin, conf.grosor2, conf.estiloFlecha);
            var eventoFinal = dibujarEvento(papel, conf.posicionX + conf.radioEvento, conf.posicionY, conf.radioEvento, colorFin, colorBordeFin, conf.grosor2, conf.cursor);
            eventoFinal.hover(function () {
                this.animate({ "fill-opacity": conf.opacidadFinal }, 1000, "backIn");
            }, function () {
                this.animate({ "fill-opacity": conf.opacidadInicio }, 1000, "backOut");
            });
            dibujarTexto(papel, (conf.posicionX + (conf.radioEvento / 2)), conf.posicionY - conf.radioEvento - 10, 'Fin', colorBordeFin, conf.tamanoFuente, conf.fuenteEventos, null, null, false);
        }

        function abrirAccion(taskId, selectorX, selectorY, selectorAncho, selectorAlto, color, grosor) {
            vm.nombreFormularioCustom = "";

            if (vm.accionSeleccionada !== taskId) {
                let arrayAccion = vm.accionesDiagrama.filter(x => x.Id == taskId);
                let accionSeleccionada = crearListadoAcciones(arrayAccion)[0];

                vm.accionSeleccionada = accionSeleccionada.IdAccion;

                angular.forEach(vm.selectores, function (value, key) {
                    vm.selectores[key].node.style.display = "none";
                });
                dibujarSelector(vm.papel, selectorX, selectorY, selectorAncho, selectorAlto, 0, 0, color, grosor);
                if (accionSeleccionada.Estado == 'PorDefinir')
                    return false;
                if (!(accionSeleccionada.Ventana == null || accionSeleccionada.Ventana === "")) {
                    var roles = [];
                    $sessionStorage.usuario.roles.forEach(element => roles.push(element.IdRol));
                    if (accionSeleccionada.Estado == 'Ejecutada') {
                        vm.indiceAccionSeleccionada = 0;
                        vm.seleccionarAccion(accionSeleccionada);
                        vm.descripcionAccion = accionSeleccionada.Descripcion;
                        vm.modeloBotones.descripcionAccion = accionSeleccionada.Descripcion;
                        vm.modeloBotones.nombreAccion = accionSeleccionada.Nombre;
                        vm.accionSeleccionada = null;
                        $sessionStorage.soloLectura = true;
                        return false;
                    }
                    else {
                        //var parametros = {
                        //    Roles: roles,
                        //    accionId: accionSeleccionada.IdAccion,
                        //    InstanciaId: $sessionStorage.idInstancia 
                        //};
                        var parametros = {
                            IdInstancia: $sessionStorage.idInstancia,
                            IdAcccion: accionSeleccionada.IdAccion,
                            ObjetoNegocioId: $sessionStorage.idObjetoNegocio,
                            UsuarioDNP: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                            ObjetoJson: ''
                        };
                        //servicioAcciones.obtenerValidacionVerAccion(parametros).then(function (respuesta) {
                        servicioAcciones.obtenerPermisosAccionPaso(parametros).then(function (respuesta) {

                            //if (respuesta.data) {
                            vm.indiceAccionSeleccionada = 0;
                            vm.seleccionarAccion(accionSeleccionada);
                            vm.descripcionAccion = accionSeleccionada.Descripcion;
                            vm.modeloBotones.descripcionAccion = accionSeleccionada.Descripcion;
                            vm.modeloBotones.nombreAccion = accionSeleccionada.Nombre;
                            vm.accionSeleccionada = null;
                            //}
                            //else {
                            //    utilidades.mensajeError('No tienes permiso para acceder a esta acción.');
                            //    return;
                            //}

                            if (!respuesta.data || respuesta.data === "null") {
                                $sessionStorage.soloLectura = true;
                            }
                            else {
                                if (accionSeleccionada.Estado !== 'Ejecutada') {
                                    $sessionStorage.soloLectura = false;
                                }
                            }

                            return false;

                        });
                    }
                }
            }
        }

        function agregarColorTareaPorEstatus(tarea, estatus, conf) {
            switch (estatus) {
                case constantesAcciones.estado.ejecutada:
                    tarea.attr({
                        stroke: conf.verde,
                        fill: conf.verde,
                        "fill-opacity": 0.2
                    });
                    break;
                case constantesAcciones.estado.porDefinir:
                    tarea.attr({
                        stroke: conf.azul,
                        fill: conf.blanco,
                        "fill-opacity": 0.2
                    });
                    break;
                case constantesAcciones.estado.pasoEnProgreso:
                    tarea.attr({
                        stroke: conf.azul,
                        fill: conf.azul,
                        "fill-opacity": 0.2,
                        "stroke-dasharray": "-",
                    });
                    break;
                default:
                    tarea.attr({
                        stroke: conf.azul,
                        fill: conf.blanco,
                        "fill-opacity": 0.2
                    });
            }
        }
        function AgregarTooltip(accion, conf, taskId, selectorX, selectorY, selectorAncho, selectorAlto) {
            /*Inicio tooltip*/
            var imagenTooltip = conf.urlSoloLecturaTooltip;
            var textoTooltip = "Edición disponible";

            var fechaFinalizacion = new Date();
            var fechaCreacion = new Date();
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

                    textoTooltip = "Edición finalizada";
                    if (accion.FechaModificacion != null && accion.EstadoAccionPorInstanciaId != 0) {
                        fechaFinalizacion = $filter('date')(new Date(accion.FechaModificacion), 'dd/MM/yyyy.HH:MM:ss');
                    }
                    else { fechaFinalizacion = 'NO FINALIZADA'; }
                    if (accion.FechaCreacion != null) {
                        fechaCreacion = $filter('date')(new Date(accion.FechaCreacion), 'dd/MM/yyyy.HH:MM:ss');
                    }
                    else { fechaCreacion = 'SIN INICIAR'; }
                    break;
                case constantesAcciones.estado.porDefinir:
                    if (!tienePermisos)
                        imagenTooltip = conf.urlSoloLecturaTooltip;
                    else
                        imagenTooltip = conf.urlEsperarEditarTooltip;

                    textoTooltip = "Edición no permitida";
                    fechaFinalizacion = 'NO FINALIZADA';
                    fechaCreacion = 'SIN INICIAR';

                    break;
                case constantesAcciones.estado.pasoEnProgreso:
                    if (!tienePermisos) {

                        textoTooltip = "Edición no disponible";
                        imagenTooltip = conf.urlSoloLecturaTooltip;
                    }
                    else {
                        textoTooltip = "Edición disponible";
                        imagenTooltip = conf.urlEditarTooltip;
                    }

                    fechaFinalizacion = 'NO FINALIZADA';
                    if (accion.FechaCreacion != null) {
                        fechaCreacion = $filter('date')(new Date(accion.FechaCreacion), 'dd/MM/yyyy.HH:MM:ss');
                    }
                    else { fechaCreacion = 'SIN INICIAR'; }
                    break;
                default:
                    imagenTooltip = conf.urlEditarTooltip;
                    textoTooltip = "Edición disponible";
            }

            var posiciondvx = conf.posicionX - 8;
            var posiciondvy = conf.posicionY - 15;
            var elemento = angular.element(document.getElementById('paper'));
            var usuario = "";
            var usuarioCedula = "";
            var entidad = "";
            if (accion.Entidad != null) {
                entidad = `<p style="text-transform: capitalize; "><span>Entidad: ${accion.Entidad}</span></p>`;
            }


            if (accion.ModificadoPor != null) {
                usuarioCedula = `<p><span>Nombre del Usuario: ${accion.NombreUsuario} - ${accion.ModificadoPor}</span></p>`;
            }

            var toolTip = `<div class='tooltipInfo'>
                                        <div>
                                            <div>
                                                <div>
								                    <img style="width:16px; height:16px" src="${imagenTooltip}">
								                    <img src="${conf.urlSeparador}">
								                    <span class="titleTooltip">${textoTooltip}</span>
							                    <div>
							                    <br/>
                                                <br/>
                                                <p style="font-weight:600"><span>Paso: ${accion.Nombre}</span></p>
                                                <p><span>Roles: ${roles}</span></p>
                                                ${usuarioCedula}
                                                ${entidad}
                                                <p><span>Fecha Inicio: ${fechaCreacion}</span></p>
                                                <p><span>Fecha Fin: ${fechaFinalizacion}</span></p>
                                            </div>
                                        </div >
                                    </div >`;
            var html = `<div    name='tooltipInfo' ng-click="vm.abrirAccion('${taskId}', ${selectorX}, ${selectorY}, ${selectorAncho}, ${selectorAlto}, '${conf.azul}', ${conf.grosor1})"
                                            class='wrapperTooltip'
                                            style='position:absolute; left:${posiciondvx}px; top:${posiciondvy}px; width:${conf.anchoTarea * 2}px;'>
                                            ${toolTip}
                                    </div >`;


            var angularHtml = angular.element(html);
            elemento.append(angularHtml);
            if (angularHtml) {
                $compile(elemento)($scope);
            }

            //var angularHtml = angular.element(html);
            //if (angularHtml) {
            //    $compile(angularHtml.contents())($scope);

            //}

            /*Fin tooltip */
        }
        function agregarFlechaPorEstatus(papel, coordFlechas, status, conf) {
            var color = null;
            switch (status) {
                case constantesAcciones.estado.ejecutada:
                    color = conf.verde;
                    break;
                case constantesAcciones.estado.porDefinir:
                    color = conf.azul;
                    break;
                case constantesAcciones.estado.pasoEnProgreso:
                    color = conf.verde;
                    break;
                default:
                    color = conf.azul;
            }
            var flechaInicial = dibujarFlecha(papel, coordFlechas, color, conf.grosor2, conf.estiloFlecha);

        }

        function dibujarEvento(papel, posicionx, posiciony, radio, color, colorLinea, grosorLinea, cursor) {
            var evento = papel.circle(posicionx, posiciony, radio).attr({
                fill: color, stroke: colorLinea, "stroke-width": grosorLinea, cursor: cursor, "fill-opacity": 0.2
            });
            return evento;
        }
        function dibujarTexto(papel, posicionx, posiciony, texto, color, fontSize = 12, fontFamily = 'Work Sans', cursor = null, alineacionTexto = "start", bold = false) {
            var weight = "400";
            if (bold)
                weight = "700";

            var texto = papel.text(posicionx, posiciony, texto).attr({
                'font-family': fontFamily,
                'font-size': fontSize,
                'fill': color,
                'text-anchor': alineacionTexto,
                'font-weight': weight
            });
            if (cursor !== null) {
                texto.attr({ cursor: cursor });
            }
            return texto;
        }
        function dibujarFlecha(papel, coordenadas, colorLinea, grosorLinea, estiloFlecha) {
            var flecha = papel.path(coordenadas).attr({
                stroke: colorLinea,
                'stroke-width': grosorLinea,
                'arrow-end': estiloFlecha
            });
            return flecha;
        }
        function dibujarTarea(papel, posicionx, posiciony, ancho, alto, bordoRedondeado, grosorLinea, color, cursor = "pointer", id) {


            var task = papel.rect(posicionx, posiciony, ancho, alto, bordoRedondeado).attr({ "stroke-width": grosorLinea, fill: color, 'fill-opacity': 0.2, cursor: cursor }).hover(function () {
                this.animate({
                    "fill-opacity": 0.5
                }, 0.5, "backIn");
            }, function () {
                this.animate({ "fill-opacity": 0.2 }, 0.5, "backOut");
            });
            task.node.id = "task_" + id;

            return task;
        }

        function dibujarSelector(papel, posicionx, posiciony, ancho, alto, bordoRedondeado, grosorLinea, color, cursor = "pointer") {

            var selector = papel.rect(posicionx, posiciony, ancho, alto, bordoRedondeado).attr({ "stroke-width": grosorLinea, fill: color });
            vm.selectores.push(selector);
            return selector;
        }

        function agregarImagen(papel, rutaImage, posicionx, posiciony, ancho, alto, cursor = 'pointer') {
            var image = papel.image(rutaImage, posicionx, posiciony, ancho, alto).attr({ cursor: cursor });
            return image;
        }

        function draw_tooltip(papel, popup, object, show, text, x, y) {
            if (show == 0) {
                popup.remove();
                return popup;
            }
            var posX = object.node.attributes[0].value;
            var posY = object.node.attributes[2].value - 60;
            popup = dibujarTarea(papel, posX, posY, 50, 50, 5, 2);
            popup.toFront();
            return popup;
        }
        function agregarInformacion(datos) {
            try {
                var roles = $sessionStorage.usuario.roles;
                let entidades = JSON.parse($sessionStorage.entidades);
                vm.informacionDiagrama.entidad = entidades.find(e => e.Id === datos.IdEntidad).Name;
                vm.informacionDiagrama.rol = roles.find(r => r.IdRol === datos.RolId).Nombre;
                vm.informacionDiagrama.usuario = datos.Usuario;
                vm.informacionDiagrama.fechaCreacion = datos.FechaHoraCreacion
            } catch { }
        }
        function validarFlujoCompleto(acciones) {
            let numeroTareasEjecutadas = 0;
            for (var h = 0; h < acciones.length; h++) {
                if (acciones[h].Estado === constantesAcciones.estado.ejecutada) {
                    numeroTareasEjecutadas += 1;
                }
            }
            if (numeroTareasEjecutadas === acciones.length && $sessionStorage.guardadoPrevio !== undefined && $sessionStorage.guardadoPrevio) {
                abrirModalFlujoCompleto();
                $sessionStorage.guardadoPrevio = false;
            }
        }

        function obtenerAccionesServicio() {
            var parametros = {
                nombreAplicacion: nombreAplicacionBackbone,
                usuario: usuarioDNP,
                //idInstancia: vm.idInstancia
                idInstancia: $sessionStorage.idInstanciaFlujoPrincipal
            };
            servicioAcciones.ObtenerFlujoPorInstanciaTarea(parametros).then(function (respuesta) {
                if (respuesta.data && respuesta.data.Acciones.length) {
                    vm.fechaCreacion = respuesta.data.FechaHoraCreacion;
                    vm.accionesDiagrama = respuesta.data.Acciones;
                    vm.nombreFlujo = $sessionStorage.nombreFlujo ? $sessionStorage.nombreFlujo : respuesta.data.Nombre;
                    vm.flujoId = respuesta.data.Id;
                    if (vm.flujoId == '5653ea15-f2ad-46ee-b006-8a697244c560') {
                        servicioAcciones.obtenerEstadoOcultarObservacionesGenerales().then(function (result) {
                            vm.ocultarObservaciones = result.data;
                        });
                    }                  
                    agregarInformacion(respuesta.data);
                    $sessionStorage.idInstanciaFlujoPrincipal = respuesta.data.IdInstancia;
                    $sessionStorage.idInstanciaIframe = respuesta.data.IdInstancia;
                    $sessionStorage.idFlujoIframe = respuesta.data.Id;
                    $sessionStorage.descripcionTramite = respuesta.data.Descripcion;
                    $sessionStorage.numeroTramite = respuesta.data.NumeroTramite == null ? "" : respuesta.data.NumeroTramite;
                    $sessionStorage.listadoAccionesTramite = respuesta.data.Acciones;
                    vm.listadoAcciones = crearListadoAcciones(respuesta.data.Acciones);
                    dibujarDiagrama(vm.accionesDiagrama);
                }

                mostrarMenu(vm.listadoAcciones);
                _.each(vm.listadoAcciones, function (accion) {
                    if (accion.Estado === constantesAcciones.estado.pasoEnProgreso) {
                        accion.Seleccionado = true;
                        vm.indiceAccionSeleccionada = vm.listadoAcciones.indexOf(accion);
                    }
                });
            });
        }

        function crearListadoAcciones(listaAcciones) {
            vm.acciones = [];
            angular.forEach(listaAcciones, function (accion) {
                switch (accion.TipoAccion) {
                    case constantesAcciones.tipo.scopeParalelas:
                        angular.forEach(accion.AccionesParalelas, function (rama, indice) {
                            var numeroDeRama = indice + 1;
                            var accionRama = crearObjetoAccionRama(accion, rama, numeroDeRama);
                            crearObjetoAcciones(accionRama);
                        });

                        break;
                    default:
                        crearObjetoAcciones(accion);
                }
            });
            return vm.acciones;
        }

        function crearObjetoAcciones(accion) {
            var objetoAccion = {
                IdAccion: accion.Id,
                TipoAccion: accion.TipoAccion,
                Nombre: accion.Nombre,
                Descripcion: accion.Descripcion,
                FechaModificacion: accion.FechaModificacion,
                ModificadoPor: accion.ModificadoPor,
                FlujoAnidado: accion.FlujoAnidado,
                Estado: accion.Estado,
                IdFormulario: accion.IdFormulario,
                RolId: accion.RolId,
                Roles: accion.Roles,
                Padre: null,
                ClaseEstado: claseEstado(accion),
                IconoEstado: iconoEstado(accion),
                EstadoDescripcion: accion.Estado === constantesAcciones.estado.pasoEnProgreso ? 'PASO EN PROGRESO' : 'POR DEFINIR',
                Seleccionado: false,
                Animar: false,
                NumeroAccionesRama: accion.NumeroAccionesRama,
                EsObligatoria: accion.EsObligatoria,
                Ventana: accion.Ventana,
                IdNivel: accion.IdNivel,
                DescripcionNivel: accion.DescripcionNivel,
                OrdenVisualizacion: accion.OrdenVisualizacion,
                RequiereInfoNivelAnterior: accion.RequiereInfoNivelAnterior,
                VisualizarCumple: accion.VisualizarCumple,
                VisualizaEnviarSubpaso: accion.VisualizaEnviarSubpaso,
                SubPasos :accion.SubPasos
            }

            switch (accion.TipoAccion) {
                case constantesAcciones.tipo.scopeRamas:
                    objetoAccion.AccionesHijas = accion.Acciones;
                    break;
                case constantesAcciones.tipo.accionTipoAnidada:
                    objetoAccion.AccionesHijas = accion.FlujoAnidado.Acciones;
                    break;
                default:
                    objetoAccion.AccionesHijas = null;
            }

            vm.acciones.push(objetoAccion);
        }

        function claseEstado(accion) {
            return accion.Estado === constantesAcciones.estado.ejecutada ? 'checked' : accion.Estado === constantesAcciones.estado.pasoEnProgreso ? 'edition' : '';
        }

        function iconoEstado(accion) {
            return accion.Estado === constantesAcciones.estado.ejecutada ? 'icon-tick' : accion.Estado === constantesAcciones.estado.pasoEnProgreso ? 'icon-edit' : 'icon-see';
        }

        function recursividadIdInstanciaAnidada(flujoAnidado, accion) {
            for (var i = 0; i < flujoAnidado.Acciones.length; i++) {
                if (accion.IdAccion === flujoAnidado.Acciones[i].Id) {
                    return flujoAnidado.IdInstancia;
                }
                if (flujoAnidado.Acciones[i].FlujoAnidado !== null) {
                    var idInstancia = recursividadIdInstanciaAnidada(flujoAnidado.Acciones[i].FlujoAnidado, accion);
                    if (idInstancia !== null)
                        return idInstancia;
                }
            }
            return null;
        }

        function recursividadIdInstancia(accion) {
            for (var i = 0; i < vm.listadoAcciones.length; i++) {
                if (accion.IdAccion === vm.listadoAcciones[i].IdAccion) {
                    return vm.idInstancia;
                }
                if (vm.listadoAcciones[i].FlujoAnidado !== null && vm.listadoAcciones[i].FlujoAnidado !== undefined) {
                    var idInstancia = recursividadIdInstanciaAnidada(vm.listadoAcciones[i].FlujoAnidado, accion);
                    if (idInstancia !== null)
                        return idInstancia;
                }
            }
            return null;
        }

        function seleccionarAccion(accion) {
            vm.nombreAccion = accion.Nombre;
            $sessionStorage.visualizarCumple = accion.VisualizarCumple;
            $sessionStorage.requiereInfoNivelAnterior = accion.RequiereInfoNivelAnterior;
            $sessionStorage.soloLectura = false;

            vm.limpiarErrores();

            if (!tieneMigaPan(accion)) {
                vm.estadoPaso = accion.Estado;
                $sessionStorage.accionDeshabilitada = false;
                if (vm.estadoPaso == 'Ejecutada') {
                    $sessionStorage.soloLectura = true;
                    $sessionStorage.accionDeshabilitada = true;
                }
                $sessionStorage.nombreAccion = accion.Nombre;
                $sessionStorage.jefePlaneacion = false;

                if ($sessionStorage.idInstanciaFlujoPrincipal === undefined || $sessionStorage.idInstanciaFlujoPrincipal === "")
                    $sessionStorage.idInstanciaFlujoPrincipal = $sessionStorage.idInstancia;
                else {
                    $sessionStorage.idInstancia = vm.idInstancia = $sessionStorage.idInstanciaFlujoPrincipal;
                }
                $sessionStorage.idAccion = accion.IdAccion;
                $sessionStorage.idNivel = accion.IdNivel;
                $sessionStorage.DescripcionAccionNivel = accion.DescripcionNivel;

                if ((vm.flujoId == '5e5b4600-caf9-46ab-a404-73f0758b47db' && $sessionStorage.idNivel == '28b7e007-5e01-4dac-849f-1e2e405d646b') || (vm.flujoId == 'a2b51530-559c-47c4-97d4-e433619268aa' && $sessionStorage.idNivel == '9788b3f4-bee3-4386-99da-3f2cd6caa61c')) {
                    var rolFinalizacionGestionRecursos = utilidades.obtenerParametroTransversal('RolFinalizacionGestionRecursosSGP');
                    if ((!$sessionStorage.usuario.roles.find(x => x.IdRol == rolFinalizacionGestionRecursos) && vm.flujoId == '5e5b4600-caf9-46ab-a404-73f0758b47db') || (!$sessionStorage.usuario.roles.find(x => x.Nombre.includes('Viabilidad definitiva')) && vm.flujoId == 'a2b51530-559c-47c4-97d4-e433619268aa') ) {
                        $sessionStorage.BanderaDisabledEditarSGP = true;
                    }
                    else {
                        $sessionStorage.BanderaDisabledEditarSGP = false;
                    }
                }
                else {
                    $sessionStorage.BanderaDisabledEditarSGP = false;
                }


                var instanciaAccionActual = recursividadIdInstancia(accion);
                if (instanciaAccionActual !== $sessionStorage.idInstancia) {
                    vm.idInstancia = instanciaAccionActual;
                    $sessionStorage.idInstancia = instanciaAccionActual;
                }
                accion.Seleccionado = true;
                vm.listadoMenu[vm.indiceAccionSeleccionada].Seleccionado = false;
                vm.indiceAccionSeleccionada = vm.listadoMenu.indexOf(accion);
                vm.cargarFormulario = false;

                if (accion.SubPasos.length > 0 && accion.VisualizaEnviarSubpaso == true) { $sessionStorage.contieneSubpasos = true; } else { $sessionStorage.contieneSubpasos = false; }

                $timeout(function () {
                    $sessionStorage.idFormulario = accion.IdFormulario;
                    $sessionStorage.idAccion = accion.IdAccion;
                    $sessionStorage.estadoAccion = accion.Estado;
                    if (accion.Estado === vm.constantesAcciones.estado.pasoEnProgreso) {
                        vm.habilitarFormulario = false;
                    } else {
                        vm.habilitarFormulario = true;
                    }
                    
                    if (accion.TipoAccion == 2 && accion.Ventana != null) {
                        servicioAcciones.ObtenerAccionesDevolucion($sessionStorage.idInstancia, accion.IdAccion).then(
                            function (resultado) {
                                if (resultado.data && resultado.data.length > 0) {

                                    $sessionStorage.ExisteAccionesDevolver = true;
                                    $sessionStorage.AccionesDevolucion = resultado.data;
                                }
                                else
                                    $sessionStorage.ExisteAccionesDevolver = false;
                            });
                        vm.vistaPersonalizada = true;
                        if ($sessionStorage.ventanaAnterior == accion.Ventana && vm.estadoPaso == 'Ejecutada' && accion.Ventana == 'PreguntasPersonalizadas') {
                            accion.Ventana = accion.Ventana + 'Verificacion';
                        }
                        else {
                            $sessionStorage.ventanaAnterior = accion.Ventana;
                        }

                        vm.nombreFormularioCustom = accion.Ventana;
                        if (accion.Ventana == 'JefePlaneacion')
                            $sessionStorage.jefePlaneacion = true;
                    }
                    else {
                        vm.cargarFormulario = true;
                    }

                }, 50);
            }
        }

        vm.limpiarErrores = function () {

            let elementos = document.querySelectorAll('[id ^= "alert-"]');
            if (elementos !== undefined) {
                let i;
                for (i = 0; i < elementos.length; i++) {
                    var idSpanAlertComponent = elementos[i];
                    if (idSpanAlertComponent != undefined) {
                        idSpanAlertComponent.classList.remove("ico-advertencia");
                    }
                }
            }

            var campoObligatorioObservacion = document.getElementById("observacion-pregunta-error");
            if (campoObligatorioObservacion != undefined) {
                campoObligatorioObservacion.innerHTML = "";
                campoObligatorioObservacion.classList.add('hidden');
            }
        }

        function crearObjetoAccionRama(accionPadre, rama, numeroDeRama) {
            var accionRama = angular.copy(rama);

            if (rama != null && rama.Nombre) {
                accionRama.Nombre = rama.Nombre;
            }
            else {
                accionRama.Nombre = 'Rama ' + numeroDeRama;
            }
            // Se tiene en cuenta el estado de la acción padre (scope) para las ramas.
            accionRama.Estado = accionPadre.Estado;
            accionRama.NumeroAccionesRama = rama.NumeroAcciones;
            accionRama.EsObligatoria = rama.EsObligatoria;

            return accionRama;
        }

        vm.showAlertError = function (nombreComponente, tieneError, descObservacion) {
            var idSpanAlertComponent = document.getElementById("alert-" + nombreComponente);
            if (idSpanAlertComponent != undefined) {
                if (tieneError) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
            mostrarErrorObservaciones(tieneError, descObservacion);
        }

        function mostrarErrorObservaciones(tieneError, descObservacion) {
            var campoObligatorioObservacion = document.getElementById("observacion-pregunta-error");
            if (tieneError) {

                if (campoObligatorioObservacion != undefined) {
                    if (descObservacion != undefined)
                        campoObligatorioObservacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span> <span>" + descObservacion +"</span>";
                    else
                        campoObligatorioObservacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span> <span>El campo de observación es de obligatorio diligenciamiento.</span>";

                    campoObligatorioObservacion.classList.remove('hidden');
                }
            } else {

                if (campoObligatorioObservacion != undefined) {
                    campoObligatorioObservacion.innerHTML = "";
                    campoObligatorioObservacion.classList.add('hidden');
                }
            }
        }

        vm.verLogArchivosSoporte = function () {
            console.log($sessionStorage);
            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/formulario/ventanas/comun/documentoSoporte/modalHistoricoVersiones/historicoArhivosSoporteModal.html',
                controller: 'historicoArhivosSoporteModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    IdInstancia: () => vm.idInstancia,
                    IdNivel: () => vm.IdNivel,
                    CodigoProceso: () => $sessionStorage.InstanciaSeleccionada.CodigoProceso,
                    NombreProceso: () => $sessionStorage.InstanciaSeleccionada.NombreFlujo,
                    IdObjetoNegocio: () => $sessionStorage.InstanciaSeleccionada.IdObjetoNegocio,
                    IdAccion: () => $sessionStorage.idAccion
                },
            });
        }

        vm.arrowIcoDown = "glyphicon-chevron-down";
        vm.arrowIcoUp = "glyphicon-chevron-up";
        vm.changeArrow = function (nombreModificado) {
            var idSpanArrow = 'arrow-' + nombreModificado;
            var arrowCapitulo = document.getElementById(idSpanArrow);
            var arrowClasses = arrowCapitulo.classList;
            for (var i = 0; i < arrowClasses.length; i++) {
                if (arrowClasses[i] == vm.arrowIcoDown) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown);
                    break;
                } else if (arrowClasses[i] == vm.arrowIcoUp) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp);
                    break;
                }
            }
        }
        inizializar();
    }
})();