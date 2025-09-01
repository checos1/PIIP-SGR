(function () {
    'use strict';
    focalizacionController.$inject = [
        '$sessionStorage',
        'gestionRecursosServicio',
        '$scope',
        '$anchorScroll',
        '$location',
        '$rootScope', '$compile'
    ];



    function focalizacionController(
        $sessionStorage,
        gestionRecursosServicio,
        $scope,
        $anchorScroll,
        $location,
        $rootScope,
        $compile


    ) {
        var vm = this;
        //vm.init = init;
        vm.user = {};
        vm.lang = "es";
        vm.NombreUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.Indicador = true;
        vm.CrucePolitica = true;
        vm.muestraCategoriaProductosPolitica = false;
        vm.muestraIndicadorPolitica = false;
        vm.muestraPliticaCrucePolitica = false;
        vm.habilitarCrucePolitica = true;

        vm.nombreComponente = "focalizaciongr";
        vm.notificacionCambiosCapitulos = null;
        vm.valido = false;
        vm.errores = [];
        vm.cargado = false;

        /* metodos*/

        vm.handlerComponentes = [
            { id: 1, componente: 'focalizaciongrcapitulo1', handlerValidacion: null, handlerCambios: null, esValido: true },
        ];

        vm.handlerComponentesChecked = {};


        vm.obtenerFocalizacionPoliticasTransversalesFuentes = obtenerFocalizacionPoliticasTransversalesFuentes;

        vm.PoliticasTFuentes = [{
            ProyectoId: 0,
            BPIN: "",
            FuentesFinanciacion: [{
                FuenteId: 0,
                EtapaId: 0,
                Etapa: "",
                FinaciadorId: 0,
                Finaciador: "",
                EntidadId: 0,
                Entidad: "",
                EntidadCorta: "",
                RecursoId: 0,
                Recurso: "",
                Texto: "",
                Politicas: [{
                    FocalizacionPoliticaId: 0,
                    PoliticaId: 0,
                    Politica: "",
                    Categoria: 0,
                    Indicador: 0,
                    CrucePolitica: 0

                }],
            }],
        }];



        //Inicio
        vm.init = function () {
            obtenerFocalizacionPoliticasTransversalesFuentes();
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });
          
        };

        function obtenerFocalizacionPoliticasTransversalesFuentes() {

            gestionRecursosServicio.obtenerFocalizacionPoliticasTransversalesFuentes($sessionStorage.idInstancia)
                .then(resultado => {
                    if (resultado != undefined && resultado.data.length > 0) {
                        vm.PoliticasTFuentes = jQuery.parseJSON(resultado.data);
                        if (vm.PoliticasTFuentes.FuentesFinanciacion[0].Politicas[0].Politica === "") {
                            guardarCapituloModificado();
                        }
                        if (vm.PoliticasTFuentes && vm.PoliticasTFuentes.FuentesFinanciacion &&
                            vm.PoliticasTFuentes.FuentesFinanciacion.length > 0) {
                            var p = vm.PoliticasTFuentes.FuentesFinanciacion[0].Politicas;
                            if (p && p.length === 1) {
                                vm.habilitarCrucePolitica = false;
                            }
                        }

                        for (const element of vm.PoliticasTFuentes.FuentesFinanciacion) {
                            if (element.Texto.length > 80) {
                                element.EntidadCorta = element.Texto.substring(0, 80) + "...";
                            }
                            else {
                                element.EntidadCorta = element.Texto;
                            }
                        }
                    }
                })
        };

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            obtenerFocalizacionPoliticasTransversalesFuentes();
        }

        //vm.notificacionCambiosCapitulos = function ({ nombreComponente, nombreComponenteHijo }) {
            
        //    for (var i = 0; i < vm.handlerComponentes.length; i++) {
        //        if (vm.handlerComponentes[i].handlerCambios != null) {
        //            vm.handlerComponentes[i].handlerCambios(nombreComponenteHijo);
        //        }
        //    }
        //};

        vm.notificacionCambiosCapitulosExternos = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].componente != nombreComponente) {
                    vm.handlerComponentes[i].handlerCambios = handler;
                    break;
                }
            }
        };
       
        $scope.scrollTo = function (id, elementoOrigen, politicaId, event) {

            function verPoliticas(id, elementoOrigen, iter = false) {
                var old = $location.hash();
                $location.hash(id);
                //$anchorScroll.yOffset = 50;
                $anchorScroll();
                /// Restablecer a antiguo para evitar que se active cualquier lógica de enrutamiento adicional
                $location.hash(old);
                /// Mostrar contenedor.
                $("#contenedorIndicadoresPolitica").css("visibility", "visible");
                $("#contenedorIndicadoresPolitica").css("display", "block");

                $("#contenedorCategoriaProductosPolitica").css("visibility", "hidden");
                $("#contenedorCategoriaProductosPolitica").css("display", "none");

                $("#contenedorPoliticasCrucePolitica").css("visibility", "visible");
                $("#contenedorPoliticasCrucePolitica").css("display", "block");

                $sessionStorage.nombreElementoOrigen = elementoOrigen;
                sessionStorage.setItem("nombreElementoOrigen", elementoOrigen);

                if (iter) {
                    verPoliticas(id, elementoOrigen, false);
                }
            }
            
            sessionStorage.setItem("politicaId", politicaId);

            var element = event.currentTarget;
            if (element) {
                var e = element.closest(".row");
                if (e) {
                    var p = e.parentNode;
                    if (p) {
                        var d = p.getElementsByClassName('dContedorPolitica');
                        if (d && d.length > 0) {
                            var elStr = ` <div id="contenedorIndicadoresPolitica" class="tab-pane cont col-md-12" style="padding-left: 10px !important; padding-top: 20px !important; width: 95%; font-size: 12pt; margin: 10px 0 20px 40px;">
                                <indicadores-politica></indicadores-politica>
                            </div>`;
                            d[0].innerHTML = elStr;

                            var el = angular.element(d[0]);
                            if (el) {
                                $compile(el.contents())($scope);
                            }
                        }
                    }
                }
            }

            if (!vm.muestraIndicadorPolitica) {
                vm.muestraIndicadorPolitica = true;
                setTimeout(() => verPoliticas(id, elementoOrigen, false), 3000);
            } else {
                verPoliticas(id, elementoOrigen, true);
            }
        };

        $scope.ocultarIndPolitica = function () {
            $("#contenedorIndicadoresPolitica").css("visibility", "hidden");
        };

        /// Categoria Productos Politica.
        $scope.scrollToCPP = function (id, elementoOrigen, politicaId, fuenteId, event) {
            vm.Fuente = fuenteId;
            sessionStorage.setItem("politicaId", politicaId);
            sessionStorage.setItem("fuenteId", fuenteId);

            $sessionStorage.nombreElementoOrigen = elementoOrigen;
            sessionStorage.setItem("nombreElementoOrigen", elementoOrigen);

            function verCPP(id, elementoOrigen, iter = false) {
                var old = $location.hash();
                $location.hash(id);
                $anchorScroll();
                /// Restablecer a antiguo para evitar que se active cualquier lógica de enrutamiento adicional
                $location.hash(old);
                /// Mostrar contenedor.
                $("#contenedorCategoriaProductosPolitica").css("visibility", "visible");
                $("#contenedorCategoriaProductosPolitica").css("display", "block");

                $("#contenedorIndicadoresPolitica").css("visibility", "hidden");
                $("#contenedorIndicadoresPolitica").css("display", "none");

                $("#contenedorPoliticasCrucePolitica").css("visibility", "visible");
                $("#contenedorPoliticasCrucePolitica").css("display", "block");

                //$sessionStorage.nombreElementoOrigen = elementoOrigen;
                //sessionStorage.setItem("nombreElementoOrigen", elementoOrigen);
                if (iter) {
                    verCPP(id, elementoOrigen, false);
                    
                }
                vm.cargado = true;
            }

            var element = event.currentTarget;
            if (element) {
                var e = element.closest(".row");
                if (e) {
                    var p = e.parentNode;
                    if (p) {
                        var d = p.getElementsByClassName('dContedorPolitica');
                        if (d && d.length > 0) {
                            var elStr = `<div id="contenedorCategoriaProductosPolitica" class="tab-pane cont col-md-12" style=" padding-left: 10px !important; padding-top: 20px !important; width: 95%; font-size: 12pt; margin: 10px 0px 20px 40px;">
                                <categoria-productos-politica guardadoevent="vm.guardado(nombreComponenteHijo)"
                                                          notificacionestado="vm.notificacionEstado(nombreComponente, esValido)"
                                                          notificacionvalidacion="vm.notificacionValidacionHijos(handler, nombreComponente)"
                                                          valido="{{vm.valido}}"
                                                          listaerrores="{{vm.errores}}"
                                                          cargado="{{vm.cargado}}" >
                                            </categoria-productos-politica>

                                                
                            </div>`;
                            d[0].innerHTML = elStr;
                          
                            var el = angular.element(d[0]);
                            if (el) {
                                $compile(el.contents())($scope);
                               
                            }
                        }
                    }
                }
            }

            //sessionStorage.setItem("politicaId", politicaId);
            //sessionStorage.setItem("fuenteId", fuenteId);
            //$scope.categoriaProductosPolitica_obtenerdatoscpp(politicaId, fuenteId);

            if (!vm.muestraCategoriaProductosPolitica) {
                vm.muestraCategoriaProductosPolitica = true;
                setTimeout(() => {
                    verCPP(id, elementoOrigen, false);
                    //$scope.categoriaProductosPolitica_obtenerdatoscpp(politicaId, fuenteId);
                }, 500);
            } else {
                verCPP(id, elementoOrigen, true);
            }
        };

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

      

        $scope.scrollToCrucePolitica = function (id, elementoOrigen, politicaId, fuenteId) {
            sessionStorage.setItem("politicaId", politicaId);
            sessionStorage.setItem("fuenteId", fuenteId);

            $sessionStorage.nombreElementoOrigen = elementoOrigen;
            sessionStorage.setItem("nombreElementoOrigen", elementoOrigen);
            vm.Fuente = fuenteId;

        function verPCP(id, elementoOrigen, iter = false) {
            var old = $location.hash();
            $location.hash(id);
            //$anchorScroll.yOffset = 50;
            $anchorScroll();
            /// Restablecer a antiguo para evitar que se active cualquier lógica de enrutamiento adicional
            $location.hash(old);
            /// Mostrar contenedor.
            // visualizacion informacio politicas cruce politica
            $("#contenedorPoliticasCrucePolitica").css("visibility", "visible");
            $("#contenedorPoliticasCrucePolitica").css("display", "block");

            $("#contenedorCategoriaProductosPolitica").css("visibility", "visible");
            $("#contenedorCategoriaProductosPolitica").css("display", "block");

            $("#contenedorIndicadoresPolitica").css("visibility", "hidden");
            $("#contenedorIndicadoresPolitica").css("display", "none");

         
                if (iter) {
                    verPCP(id, elementoOrigen, false);
                 }
                vm.cargado = true;
            }

            var element = event.currentTarget;
            if (element) {
                var e = element.closest(".row");
                if (e) {
                    var p = e.parentNode;
                    if (p) {
                        var d = p.getElementsByClassName('dContedorPolitica');
                        if (d && d.length > 0) {
                            var elStr = `<div id="contenedorPoliticasCrucePolitica" class="tab-pane cont col-md-12" style=" padding-left: 10px !important; padding-top: 20px !important; border: 1px solid #e6effd; width: 96%; font-size: 12pt; margin: 10px 0 20px 50px;">
                                         <politica-cruce-politica  guardadoevent="vm.guardado(nombreComponenteHijo)"
                                                    notificacionestado="vm.notificacionEstado(nombreComponente, esValido)"
                                                    notificacionvalidacion="vm.notificacionValidacionHijos(handler, nombreComponente)"
                                                    notificarrefresco="vm.notificarRefresco(handler, nombreComponente)"
                                                    valido="{{vm.valido}}"
                                                    listaerrores="{{vm.errores}}"
                                                    cargado="{{vm.cargado}}" >
                                                
                                        </politica-cruce-politica>
                                        </div>`;
                            d[0].innerHTML = elStr;

                            var el = angular.element(d[0]);
                            if (el) {
                                $compile(el.contents())($scope);
                               
                            }
                        }
                    }
                }
            }

          
            if (!vm.muestraPliticaCrucePolitica) {
                vm.muestraPliticaCrucePolitica = true;
                setTimeout(() => {
                    verPCP(id, elementoOrigen, false);
                   
                }, 500);
            } else {
                verPCP(id, elementoOrigen, true);
            }
            vm.filtrarDatosResult;
        };

        $scope.ocultarCPP = function () {
            $("#contenedorCategoriaProductosPolitica").css("visibility", "hidden");
        };

     
        $scope.ocultarCrucePolitica = function () {
            $("#contenedorPoliticasCrucePolitica").css("visibility", "hidden");
        };


        vm.filtrarDatosResult = function (politicaId) {
            /// Filtrar vista de datos.
            var secPoliticaCrucePolitica = document.getElementById('secPoliticaCrucePolitica');
            if (secPoliticaCrucePolitica && politicaId) {
                var s = secPoliticaCrucePolitica.getElementsByClassName('cDivMainPolCrucPol');
                if (s && s.length > 0) {
                    var encontro = false;
                    for (var i = 0; i < s.length; i++) {
                        var d = s[i];
                        var att = d.getAttribute("data-attribute");
                        if (att && att !== politicaId.toString()) {
                            d.style.display = 'none';
                        }
                        else {
                            d.style.display = 'block';
                            encontro = true;
                        }
                    }

                    /// No tiene datos.
                    if (!encontro) {
                        var cMensajePoliticacrucePolitca = secPoliticaCrucePolitica.getElementsByClassName('cMensajePoliticacrucePolitca');
                        if (cMensajePoliticacrucePolitca) {
                            cMensajePoliticacrucePolitca[0].innerHTML = '¡Sin datos de consulta!.';
                        }
                    }
                }
            }
        };


        /* --------------------------------- Marcado lunas ---------------------------*/

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'focalizaciongrcapitulo1': true
            };
        }


        ///**
        //* Función handler que contiene la referencia del binding notificacioncambios del componente justificacionCambios
        //* @param {any} param0
        //*/
        //vm.notificacionCambiosCapitulos = function ({ nombreComponente, nombreComponenteHijo }) {
        //    for (var i = 0; i < vm.handlerComponentes.length; i++) {
        //        if (vm.handlerComponentes[i].handlerCambios != null) {
        //            try {
        //                vm.handlerComponentes[i].handlerCambios(nombreComponenteHijo);
        //            } catch (error) {
        //                console.error('¡¡Tiene ERRORES - handlerCambios del componente = ' + vm.handlerComponentes[i].componente + '!!');
        //            }
        //        }
        //    }
        //};

        ///**
        // * Función que crea la referencia hanlder con los componentes hijos
        // * @param {any} handler
        // * @param {any} nombreComponente
        // */
        //vm.notificacionCambiosCapitulosExternos = function (handler, nombreComponente) {
        //    for (var i = 0; i < vm.handlerComponentes.length; i++) {
        //        if (vm.handlerComponentes[i].componente == nombreComponente) {
        //            vm.handlerComponentes[i].handlerCambios = handler;
        //            break;
        //        }
        //    }
        //};

        /* --------------------------------- Validaciones ---------------------------*/

        /**
         * Función que recibe listado de errores referentes a la sección de justificación
         * envía a sus hijos el listado de errores
         * @param {any} errores
         */
        vm.notificacionValidacionEvent = function (listErrores) {
            vm.valido = true;
            vm.errores = [];
            var erroresList = listErrores.errores.filter(p => p.Seccion == vm.nombreComponente);
            vm.errores = erroresList;
            vm.inicializarComponenteCheck();
            vm.esValido = true;
           // if (erroresList.length > 0) {
                for (var i = 0; i < vm.handlerComponentes.length; i++) {
                    
                    if (vm.handlerComponentes[i].handlerValidacion) vm.handlerComponentes[i].handlerValidacion(erroresList);
                }
            //}
            
        }

        /**
         * Función que crea las referencias de los métodos de los hijos con el padre. Este es llamado cuando se inicializa el componente hijo.
         * @param {any} handler función referenciada
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        vm.notificacionValidacionHijos = function (handler, nombreComponente) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.handlerComponentes[indx].handlerValidacion = handler;
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
            vm.showAlertError(nombreComponente, esValido);
        }


        /**
         * Función que visualiza alerta de error tab de componente
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        vm.showAlertError = function (nombreComponente, esValido) {
            
            //var idSpanAlertComponent = document.getElementById("alert-" + nombreComponente + '-' + fuente);
            //if (idSpanAlertComponent != undefined) {
            //    if (!esValido) {
            //        idSpanAlertComponent.classList.add("ico-advertencia");
            //    } else {
            //        idSpanAlertComponent.classList.remove("ico-advertencia");
            //    }
            //}
        }

        vm.capitulos = function (listadoCapitulos) {
            var listadoCapRecursos = listadoCapitulos.filter(p => p.SeccionModificado == vm.nombreComponente)
            listadoCapRecursos.forEach(function (item) {
                //var el = document.getElementById("name-capitulo-" + item.nombreComponente);
                var elidSeccionCapitulo = document.getElementById("id-capitulo-" + item.nombreComponente);
                var elAccordion = document.getElementById("accordion-" + item.nombreComponente);
                //if (el != undefined && el != null) {
                //    el.innerHTML = item.Capitulo;
                //}
                if (elAccordion != undefined && elAccordion != null) {
                    elAccordion.classList.remove("hidden");
                }
                if (elidSeccionCapitulo != undefined && elidSeccionCapitulo != null) {
                    elidSeccionCapitulo.innerHTML = item.SeccionCapituloId;
                }
            });
        };

        vm.guardado = function (nombreComponenteHijo) {
          //  vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
            vm.callback();
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-focalizaciongrcapitulo1');
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
            }

            gestionRecursosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardado({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }


    }

    angular.module('backbone').component('focalizacion', {
        templateUrl: "src/app/formulario/ventanas/gestionRecursos/componentes/focalizacion/focalizacion.html",
        controller: focalizacionController,
        controllerAs: "vm",
        bindings: {
            notificacionvalidacion: '&',
            notificacioncambios: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            callback: '&',
            guardadoevent: '&',
        }
    });

    


})();