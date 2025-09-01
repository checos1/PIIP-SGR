(function () {
    'use strict';

    autorizacionliberacionController.$inject = [
        '$sessionStorage',
        '$scope',
        'autorizacionliberacionServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        '$uibModal'
    ];

    function autorizacionliberacionController(
        $sessionStorage,
        $scope,
        autorizacionliberacionServicio,
        utilidades,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        $uibModal
    ) {
        var vm = this;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.numeroTramite = $sessionStorage.idObjetoNegocio;
        vm.etapa = "ej";
        vm.TramiteId = $sessionStorage.tramiteId;
        vm.usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.idTipoTramite = $sessionStorage.TipoTramiteId;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.erroresActivos = null;
        vm.idProyecto = 0;// $sessionStorage.proyectoId;
        vm.ObjetoVerMas = ObjetoVerMas;
        vm.nombreComponente = "selecionarvigenciafuturaautorizacionminhacienda";
        vm.notificacionCambiosCapitulos = null;
        vm.abrirMensajeInformacion = abrirMensajeInformacion;
        vm.Guardar = Guardar;
        vm.Cancelar = Cancelar;
        vm.Editar = Editar;
        vm.rol = 'da595aa3-cf59-46d3-a22a-0d96da5c7371';
        vm.nombreComponenteHijo = "soportedocumentopaso";
        vm.listaArchivos = [];
        vm.handlerComponentesChecked = {};
        vm.rolUsuario = $sessionStorage.usuario.roles.find((item) => item.Nombre.includes('R_Presupuesto - preliminar'));
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;
        vm.handlerComponentes = [
            { id: 1, componente: 'soportedocumentopaso', handlerValidacion: null, handlerCambios: null, esValido: true }
        ];

        $scope.$watch('vm.tramiteid', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                if ($sessionStorage.tramiteAsociado)
                    ObtenerLiberacionVigenciaFutura();
            }
        });

        $scope.$watch(() => $sessionStorage.tramiteAsociado
            , (newVal, oldVal) => {
                if (newVal) {
                    ObtenerLiberacionVigenciaFutura();
                }

            }, true);

        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };

        function ObtenerLiberacionVigenciaFutura() {
            return autorizacionliberacionServicio.ObtenerLiberacionVigenciaFutura(vm.idProyecto, vm.tramiteid).then(
                function (respuesta) {
                    //console.log(respuesta.data);
                    $scope.datos = respuesta.data[0];
                });
        }

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'soportedocumentopaso': true
            };
        }

        vm.convertToDate = function (stringDate) {
            var date = new Date(stringDate);
            return date;
        };

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

        function Guardar(tramite) {

            if (tramite.CodigoAutorizacion == '' || tramite.CodigoAutorizacion == null || tramite.CodigoAutorizacion == '0') {
                utilidades.mensajeError("Debe diligenciar un valor para el código de autorización");
                return;
            }

            if (tramite.FechaAutorizacion == '' || tramite.FechaAutorizacion == null) {
                utilidades.mensajeError("Debe diligenciar un valor para la fecha de autorización");
                return;
            }

            return autorizacionliberacionServicio.InsertaAutorizacionVigenciasFuturas(tramite).then(
                function (respuesta) {
                    if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        //console.log(respuesta);
                        vm.actualizacomponentes = vm.actualizacomponentes + '1';
                        if (respuesta.data.Exito) {
                            guardarCapituloModificado();
                            vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                            utilidades.mensajeSuccess("", false, false, false, "Los datos han sido guardados con éxito");
                            tramite.EditarTramiteLiberacion = false;
                            tramite.FechaAutorizacionOriginal = tramite.FechaAutorizacion;
                            tramite.CodigoAutorizacionOriginal = tramite.CodigoAutorizacion;
                            vm.init();
                        } else {
                            utilidades.mensajeError(respuesta.data.Mensaje);
                        }
                    } else {
                        utilidades.mensajeError("Error al realizar la operación");
                    }
                });                
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = 412;//span.textContent;
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
        }

        function Cancelar(tramite) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                tramite.EditarTramiteLiberacion = false;
                tramite.FechaAutorizacion = tramite.FechaAutorizacionOriginal;
                tramite.CodigoAutorizacion = tramite.CodigoAutorizacionOriginal;


                return autorizacionliberacionServicio.ObtenerLiberacionVigenciaFutura(vm.idProyecto, vm.TramiteId).then(
                    function (respuesta) {
                        if (respuesta.data != null && respuesta.data != "") {
                            utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                        }
                    });

            }, function funcionCancelar(reason) {
                //poner aquí q pasa cuando cancela
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function Editar(tramite) {
            tramite.EditarTramiteLiberacion = true;
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57)) {
                event.preventDefault();
            }
        }

        /*------------------------------------Validaciones-----------------------------------*/
        /**
       * Listado de componentes hijos, obligatorio para estructura de validación
       * */

        /**
        * Función que recibe listado de errores referentes a la sección de justificación
        * envía a sus hijos el listado de errores
        * @param {any} errores
        */
        vm.notificacionValidacionEvent = function (listErrores) {
            debugger;
            var erroresList = listErrores.errores.filter(p => p.Seccion == vm.nombreComponente);
            vm.inicializarComponenteCheck();
            vm.esValido = true;
            if (erroresList.length > 0) {
                for (var i = 0; i < vm.handlerComponentes.length; i++) {
                    if (vm.handlerComponentes[i].handlerValidacion) vm.handlerComponentes[i].handlerValidacion(erroresList);
                }
            }
        }

        /**
        * Función que crea las referencias de los métodos de los hijos con el padre. Este es llamado cuando se inicializa el componente hijo.
        * @param {any} handler función referenciada
        * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
        */
        vm.notificacionValidacionHijos = function (handler, nombreComponente) {
            //debugger;
            var indx = vm.handlerComponentes.findIndex(p => p.componente == vm.nombreComponenteHijo);
            vm.handlerComponentes[indx].handlerValidacion = handler;

        };

        vm.changeArrow = function (nombreModificado) {
            debugger;
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
                'soportedocumentopaso': true
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

                var erroresFiltrados2 = utilsValidacionSeccionCapitulosServicio.getErroresValidados('selecionarvigenciafuturaautorizacionminhaciendadocs', errores);
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

            angular.forEach($scope.datos.TramitesALiberar, function (series) {
                var autorizacionError001 = document.getElementById("validacion-AMTL001-" + series.LiberacionVigenciasFuturasId);
                if (autorizacionError001 != undefined) {
                    autorizacionError001.innerHTML = '';
                    autorizacionError001.classList.add('hidden');
                }
            });

            var autorizacionError004 = document.getElementById("validacion-AMTL004-autorizacionliberacion");

            if (autorizacionError004 != undefined) {
                autorizacionError004.innerHTML = '';
                autorizacionError004.classList.add('hidden');
            }
            
        }

        vm.validacionAMTL001 = function (errores, descripcion, data) {
            var indErr001 = JSON.parse(data);

            indErr001.forEach(p => {
                var autorizacionError001 = document.getElementById("validacion-AMTL001-" + p.LiberacionVigenciasFuturasId);

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

        vm.validacionAMTL002 = function (errores, descripcion, data) {

            var indErr002 = JSON.parse(data);

            indErr002.forEach(p => {
                var autorizacionError002 = document.getElementById("validacion-AMTL002-" + p.LiberacionVigenciasFuturasId);

                if (descripcion != '') {
                    if (autorizacionError002 != undefined) {
                        autorizacionError002.innerHTML = '<span> <img src="Img/u4630.svg"> ' + descripcion + "</span>";
                        autorizacionError002.classList.remove('hidden');
                    }
                } else {
                    if (autorizacionError002 != undefined) {
                        autorizacionError002.classList.add('hidden');
                    }
                }
            });            
        }

        vm.validacionAMTL003 = function (errores, descripcion, data) {
            var indErr003 = JSON.parse(data);

            indErr003.forEach(p => {
                var autorizacionError003 = document.getElementById("validacion-AMTL003-" + p.LiberacionVigenciasFuturasId);

                if (descripcion != '') {
                    if (autorizacionError003 != undefined) {
                        autorizacionError003.innerHTML = '<span> <img src="Img/u4630.svg"> ' + descripcion + "</span>";
                        autorizacionError003.classList.remove('hidden');
                    }
                } else {
                    if (autorizacionError003 != undefined) {
                        autorizacionError003.classList.add('hidden');
                    }
                }
            });   
        }

        vm.validacionAMTL004 = function (errores, descripcion, data) {
            var autorizacionError004 = document.getElementById("validacion-AMTL004-autorizacionliberacion");

            if (autorizacionError004 != undefined) {
                autorizacionError004.innerHTML = '<span> <img src="Img/u4630.svg"> Debe adjuntar los documentos obligatorios </span>';
                autorizacionError004.classList.remove('hidden');
            }
        }


        vm.errores = {
            'AMTL001': vm.validacionAMTL001,
            'AMTL002': vm.validacionAMTL002,
            'AMTL003': vm.validacionAMTL003,
            'VFO006': vm.validacionAMTL004
        }

        /* --------------------------------- Validaciones ---------------------------*/

        vm.notificacionCambiosCapitulos = function ({ nombreComponente, nombreComponenteHijo }) {
            debugger;
            var x = 0;
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) {
                    vm.handlerComponentes[i].handlerCambios(nombreComponenteHijo);
                }
            }
        };

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

    angular.module('backbone').component('autorizacionliberacion', {
        templateUrl: "src/app/formulario/ventanas/tramiteLiberacion/componentes/autorizacionliberacion/autorizacionliberacion.html",
        controller: autorizacionliberacionController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacioncambios: '&',
            guardadocomponent: '&',
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