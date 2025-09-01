(function () {
    'use strict';

    verificadoliberacionController.$inject = [
        '$sessionStorage',
        '$scope',
        'verificadoliberacionServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        '$uibModal',
        'constantesBackbone'

    ];

    function verificadoliberacionController(
        $sessionStorage,
        $scope,
        verificadoliberacionServicio,
        utilidades,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        $uibModal,
        constantesBackbone
    ) {
        var vm = this;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.numeroTramite = $sessionStorage.idObjetoNegocio;
        vm.etapa = "ej";
        vm.TramiteId = $sessionStorage.tramiteId;//906
        vm.usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.idTipoTramite = $sessionStorage.TipoTramiteId;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.erroresActivos = null;
        vm.idProyecto = 0;// $sessionStorage.proyectoId;//98030
        vm.ObjetoVerMas = ObjetoVerMas;
        vm.nombreComponente = "aprobacionlbvfaprobacionvalvf";
        vm.notificacionCambiosCapitulos = null;
        vm.abrirMensajeInformacion = abrirMensajeInformacion;
        vm.Guardar = Guardar;
        vm.Cancelar = Cancelar;
        vm.Editar = Editar;
        vm.habilitaGuardar = false;
        vm.Verifica = Verifica;
        vm.ConvertirNumero = ConvertirNumero;
        vm.rolUsuario = $sessionStorage.usuario.roles.find((item) => item.Nombre.includes('R_Analista presupuestal'));
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;


        vm.handlerComponentes = [
        ];
        vm.handlerComponentesChecked = {};

        $scope.$watch('vm.tramiteid', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                ObtenerLiberacionVigenciaFutura();
            }
        });


        vm.init = function () {
            ObtenerLiberacionVigenciaFutura();
            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };

        function ObtenerLiberacionVigenciaFutura() {
            return verificadoliberacionServicio.ObtenerLiberacionVigenciaFutura(vm.idProyecto, vm.tramiteid).then(
                function (respuesta) {
                    //console.log(respuesta.data);
                    $scope.datos = respuesta.data[0];
                });
        }

        vm.convertToDate = function (stringDate) {
            var date = new Date(stringDate);
            return date;
        };

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2, maximumFractionDigits: 2,
            }).format(numero);
        }

        vm.changeBoton = function (tramite) {
            if (tramite.LabelBoton == '+') {
                tramite.LabelBoton = '-'
            } else {
                tramite.LabelBoton = '+'
            }
            return tramite.LabelBoton;
        }

        function ObjetoVerMas(tramite) {
            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModal.html',
                controller: 'objetivosIndicadorModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    Objetivo: function () {
                        return tramite.Objeto;
                    },
                    IdObjetivo: function () {
                        return '';
                    },
                    Tipo: function () {
                        return 'Objeto';
                    },
                    Titulo: function () {
                        return 'Liberación Vigencias Futuras';
                    }
                },
            });
        }

        function abrirMensajeInformacion() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' > Objetivos específicos</span>");
        }

        function Guardar(datos) {
            vm.habilitaGuardar = false;

            angular.forEach(datos.TramitesALiberar, function (series) {
                console.log(series);
                return verificadoliberacionServicio.InsertaAutorizacionVigenciasFuturas(series).then(
                    function (respuesta) {
                        vm.actualizacomponentes = vm.actualizacomponentes + '1';
                        if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                            //console.log(respuesta);
                            if (respuesta.data.Exito) {
                                series.EstadoAutorizacionOriginal = series.EstadoAutorizacion;
                                series.EditarTramiteLiberacion = false;
                            } else {
                                utilidades.mensajeError(respuesta.data.Mensaje);
                            }
                        } else {
                            utilidades.mensajeError("Error al realizar la operación");
                        }
                    });
            });

            guardarCapituloModificado();
            vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
            utilidades.mensajeSuccess("", false, false, false, "Los datos se han guardado con éxito");
            //vm.init();
        }

        function Verifica(tramite) {
            //console.log(tramite);
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = 627;// span.textContent;
        }

        function ObtenerSeccionCapitulo2() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo2 = 633;// span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.proyectoId,
                Justificacion: "",
                //SeccionCapituloId: vm.SeccionCapituloId,
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: false,
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

            //Se guarda el resumen para llenar la luna completa.
            ObtenerSeccionCapitulo2();
            var data2 = {
                ProyectoId: $sessionStorage.proyectoId,
                Justificacion: "",
                //SeccionCapituloId: vm.SeccionCapituloId,
                SeccionCapituloId: vm.seccionCapitulo2,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: false,
            }
            justificacionCambiosServicio.guardarCambiosFirme(data2)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function Cancelar(datos) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                vm.habilitaGuardar = false;
                angular.forEach(datos.TramitesALiberar, function (series) {
                    series.EstadoAutorizacion = series.EstadoAutorizacionOriginal;
                });

                return verificadoliberacionServicio.ObtenerLiberacionVigenciaFutura(vm.idProyecto, vm.TramiteId).then(
                    function (respuesta) {
                        if (respuesta.data != null && respuesta.data != "") {
                            utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                        }
                    });

            }, function funcionCancelar(reason) {
                //poner aquí q pasa cuando cancela
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function Editar(datos) {
            vm.habilitaGuardar = true;
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57)) {
                event.preventDefault();
            }
        }

        vm.getTotal = function (tramite) {
            var total = 0;
            angular.forEach(tramite.ValoresConstantesAutorizaLiberacion, function (series) {
                total = total + (series.UtilizadoConstanteNacion * series.Deflactor);
            });
            return total;
        };

        vm.getTotal2 = function (tramite) {
            var total = 0;
            angular.forEach(tramite.ValoresConstantesAutorizaLiberacion, function (series) {
                total = total + (series.UtilizadoConstantePropios * series.Deflactor);
            });
            return total;
        };

        /*------------------------------------Validaciones-----------------------------------*/
        /**
       * Listado de componentes hijos, obligatorio para estructura de validación
       * */


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

        vm.guardado = function (nombreComponenteHijo, deshabilitarRegresar, devolver) {
            vm.callback();
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo, deshabilitarRegresar: deshabilitarRegresar });

        }

        $scope.$watchCollection("vm.handlerComponentesChecked", function (newValue, oldValue) {
            var estado = true;
            var listHijos = Object.keys(vm.handlerComponentesChecked);
            if (listHijos.length == 0 || newValue === oldValue) {
                return;
            }
            listHijos.forEach(p => {
                if (vm.handlerComponentesChecked[p] == false) {
                    estado = false;
                }
            });
            vm.notificacionestado({ estado: estado, nombreComponente: vm.nombreComponente });
        });

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
            };
        }

        vm.deshabilitarBotonDevolverAsociarProyectoVF = function () {
            vm.callback();

        }

        vm.notificacionValidacionPadre = function (errores) {
            //debugger;
            if (errores != undefined) {
                var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
                vm.erroresActivos = erroresFiltrados.erroresActivos;

                var seccion2 = errores.find(x => x.Seccion == 'selecionarvigenciafutura' && x.Capitulo == 'valoresutilizados');
                if (seccion2 != null) {
                    let error = {};
                    if (($sessionStorage.sessionDocumentos == 0 || $sessionStorage.sessionDocumentos === undefined) && $sessionStorage.idNivel.toUpperCase() === constantesBackbone.idNivelSeleccionProyectos) {
                        error = {
                            Seccion: "selecionarvigenciafutura",
                            Capitulo: "valoresutilizadosdocs",
                            Errores: '{"selecionarvigenciafuturavaloresutilizadosdocs":[{"Error":"VFO006","Descripcion":"Diligencie los documentos obligatorios"}]}',
                        }
                    } else {
                        error = {
                            Seccion: "selecionarvigenciafutura",
                            Capitulo: "valoresutilizados",
                            Errores: null,
                        }
                    }
                    errores.push(error);
                }

                var erroresFiltrados2 = utilsValidacionSeccionCapitulosServicio.getErroresValidados('selecionarvigenciafuturavaloresutilizadosdocs', errores);
                if (erroresFiltrados2.erroresActivos.length > 0) {
                    vm.erroresActivos.push(erroresFiltrados2.erroresActivos[0]);
                }

                vm.ejecutarErrores();

                var isValid = (vm.erroresActivos.length <= 0);
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        };

        vm.ejecutarErrores = function () {
            vm.limpiarErrores();
            $scope.errores = vm.erroresActivos;
            vm.erroresActivos.forEach(p => {
                if (vm.errores[p.Error] != undefined) {
                    vm.errores[p.Error](p.Error, p.Descripcion, p.Data);
                }
            });
        }

        

        vm.limpiarErrores = function () {
            if ($scope.datos != null) {
                angular.forEach($scope.datos.TramitesALiberar, function (series) {
                    var autorizacionError001 = document.getElementById("validacion-VATL001-" + series.LiberacionVigenciasFuturasId);
                    if (autorizacionError001 != undefined) {
                        autorizacionError001.innerHTML = '';
                        autorizacionError001.classList.add('hidden');
                    }
                });
            }
        }

        vm.validacionVATL001 = function (errores, descripcion, data) {
            var indErr001 = JSON.parse(data);

            indErr001.forEach(p => {
                var autorizacionError001 = document.getElementById("validacion-VATL001-" + p.LiberacionVigenciasFuturasId);

                if (descripcion != '') {
                    if (autorizacionError001 != undefined) {
                        autorizacionError001.innerHTML = '<span> <img src="Img/u4630.svg"> ' + descripcion + "</span>";
                        autorizacionError001.classList.remove('hidden');
                    }
                } else {
                    if (autorizacionError001 != undefined) {
                        autorizacionError001.classList.add('hidden');
                    }
                }
            });
        }

        vm.errores = {
            'VATL001': vm.validacionVATL001
        }

        /* --------------------------------- Validaciones ---------------------------*/

        /**
        * Función que recibe los estados de los componentes hijos
        * @param {any} esValido true: valido, false: invalido
        * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
        */
        vm.notificacionEstado = function (nombreComponente, esValido) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.esValido = !esValido ? false : vm.esValido;
            vm.handlerComponentes[indx].esValido = esValido;
            vm.handlerComponentesChecked[nombreComponente] = esValido;
            //vm.showAlertError(nombreComponente, esValido, esValidoPaso4);
            vm.showAlertError(nombreComponente, esValido);
        }

        /**
         * Función que visualiza alerta de error tab de componente
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        //vm.showAlertError = function (nombreComponente, esValido, esValidoPaso4) {
        vm.showAlertError = function (nombreComponente, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + nombreComponente);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.capitulos = function (listadoCapitulos) {
            var listadoCapRecursos = listadoCapitulos.filter(p => p.SeccionModificado == vm.nombreComponente)
            listadoCapRecursos.forEach(function (item) {
                var el = document.getElementById("name-capitulo-" + item.nombreComponente);
                var elidSeccionCapitulo = document.getElementById("id-capitulo-" + item.nombreComponente);
                var elAccordion = document.getElementById("accordion-" + item.nombreComponente);
                if (el != undefined && el != null) {
                    el.innerHTML = item.Capitulo;
                }
                if (elAccordion != undefined && elAccordion != null) {
                    elAccordion.classList.remove("hidden");
                }
                if (elidSeccionCapitulo != undefined && elidSeccionCapitulo != null) {
                    elidSeccionCapitulo.innerHTML = item.SeccionCapituloId;
                }
            });
        };

        /*------------------------------------Fin Validaciones-----------------------------------*/

    }

    angular.module('backbone').component('verificadoliberacion', {
        templateUrl: "src/app/formulario/ventanas/tramiteLiberacion/componentes/verficadoliberacion/verificadoliberacion.html",
        controller: verificadoliberacionController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            actualizacomponentes: '=',
        }
    });
})();