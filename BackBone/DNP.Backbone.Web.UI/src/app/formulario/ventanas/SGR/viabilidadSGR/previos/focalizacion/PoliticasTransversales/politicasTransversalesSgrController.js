(function () {
    'use strict';

    politicasTransversalesSgrController.$inject = ['$scope', '$sessionStorage', '$uibModal', 'utilidades',
         '$timeout', 'focalizacionAjustesSgrServicio', 'justificacionCambiosServicio'
    ];

    function politicasTransversalesSgrController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        $timeout,
        focalizacionAjustesSgrServicio,
        justificacionCambiosServicio
    ) {
        

        var vm = this;
        vm.init = init;
        vm.nombreComponente = "sgrviabilidadpreviosfocalizacionpoliticas";
        vm.listaPoliticasProyecto = [];

        vm.abrirModalAgregarPoliticaTrans = abrirModalAgregarPoliticaTrans;
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        //vm.eliminarPolitica = eliminarPolitica;
        vm.disabled = false;
        vm.activar = true;
        vm.eliminarPolitica = eliminarPolitica;
        vm.abrirMensajePoliticasProyecto = abrirMensajePoliticasProyecto

        vm.notificacionErrores = null;
        vm.erroresActivos = [];
        var currentYear = new Date().getFullYear();

        vm.validacionGuardado = null;
        vm.recargaGuardado = null;
        vm.recargaGuardadoCostos = null;
        vm.seccionCapitulo = null;
        vm.erroresComponente = [];


        vm.componentesRefresh = [
            'focalizacioncrucepoliticast',
            'focalizacionconcepto',
            'sgrviabilidadpreviosrecursosfuentessgr'
        ];

        //vm.abrirMensajeFocalizacion = abrirMensajeFocalizacion;

        function init() {
            //vm.permiteEditar = false;
           
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            //vm.notificarrefresco({ handler: vm.refrescarResumenCostos, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacioninicio({ handlerInicio: vm.notificacionInicioPadre, nombreComponente: vm.nombreComponente, esValido: true });

        }
        vm.notificacionInicioPadre = function (nombreModificado, errores) {
            if (nombreModificado == vm.nombreComponente + '-1') {

                if (errores != null && errores != undefined) {
                    vm.erroresComponente = errores;
                }

                vm.model = {
                    modulos: {
                        administracion: false,
                        backbone: true
                    }
                }
                vm.obtenerPoliticasTransversales(vm.BPIN);
            }
        }
        $scope.$on("BloquearPorCTUSPendiente", function (evt, data) {
            bloquearControles();
        });

        function bloquearControles() {
            vm.disabled = true;
            vm.activar = true;
        }

        vm.obtenerPoliticasTransversales = function (bpin) {

            var idInstancia = $sessionStorage.idNivel;
            var idInstanciaProyecto = $sessionStorage.idInstancia;
            var idAccion = $sessionStorage.idNivel;
            var idFormulario = $sessionStorage.idNivel;
            var idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;

            return focalizacionAjustesSgrServicio.obtenerPoliticasTransversalesProyecto(idInstanciaProyecto, usuarioDNP, idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        //var cadena = '{"ProyectoId":null,"BPIN":"202200000000064","Politicas":[{"PoliticaId":1,"Politica":"ACTIVIDADES DE CIENCIA, TECNOLOGÍA E INNOVACIÓN","EnProyecto":true,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":2,"Politica":"CAMBIO CLIMÁTICO","EnProyecto":true,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":3,"Politica":"CAMPESINOS","EnProyecto":false,"EnSeguimiento":true,"EnFirme":false},{"PoliticaId":4,"Politica":"CONSTRUCCIÓN DE PAZ","EnProyecto":false,"EnSeguimiento":true,"EnFirme":false},{"PoliticaId":5,"Politica":"DESPLAZADOS","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":6,"Politica":"DISCAPACIDAD E INCLUSIÓN SOCIAL","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":7,"Politica":"EQUIDAD DE LA MUJER","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":8,"Politica":"GESTIÓN DE RIESGO DE DESASTRES","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":9,"Politica":"GRUPOS ÉTNICOS - COMUNIDADES RROM","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":10,"Politica":"GRUPOS ÉTNICOS - INDIGENAS","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":11,"Politica":"GRUPOS ÉTNICOS - POBLACIÓN AFROCOLOMBIANA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":12,"Politica":"GRUPOS ÉTNICOS - POBLACIÓN PALENQUERA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":13,"Politica":"GRUPOS ÉTNICOS - POBLACIÓN RAIZAL","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":14,"Politica":"PARTICIPACIÓN CIUDADANA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":15,"Politica":"PLAN NACIONAL DE CONSOLIDACIÓN","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":16,"Politica":"RED UNIDOS","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":17,"Politica":"SEGURIDAD ALIMENTARIA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":18,"Politica":"SEGURIDAD Y CONVIVENCIA CIUDADANA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":19,"Politica":"TECNOLOGÍAS DE INFORMACIÓN Y COMUNICACIONES","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":20,"Politica":"VÍCTIMAS","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":1506,"Politica":"POLITICA DE PRUEBA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":1507,"Politica":"CONTROL A LA DEFORESTACIÓN Y GESTIÓN DE LOS BOSQUES","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":1508,"Politica":"PRIMERA INFANCIA, INFANCIA Y ADOLESCENCIA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":1509,"Politica":"ZONAS FUTURO","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false}]}';
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        var arregloGeneral = jQuery.parseJSON(arreglolistas);
                        var arregloDatosGenerales = arregloGeneral.Politicas;

                        var listaPoliticasProy = [];

                        for (var pl = 0; pl < arregloDatosGenerales.length; pl++) {
                            var politicasProyecto = {
                                politicaId: arregloDatosGenerales[pl].PoliticaId,
                                politica: arregloDatosGenerales[pl].Politica,
                                enProyecto: arregloDatosGenerales[pl].EnProyecto,
                                enSeguimiento: arregloDatosGenerales[pl].EnSeguimiento,
                                TieneConceptoPendiente: arregloDatosGenerales[pl].TieneConceptoPendiente
                            }

                            //if (arregloDatosGenerales[pl].EnProyecto || arregloDatosGenerales[pl].EnSeguimiento)
                            listaPoliticasProy.push(politicasProyecto);
                        }

                        vm.listaPoliticasProyectos = angular.copy(listaPoliticasProy);
                        guardarCapituloModificado();

                        if (vm.erroresComponente != null && vm.erroresComponente != undefined) {
                            $timeout(function () {
                                vm.notificacionValidacionPadre(vm.erroresComponente);
                            }, 600);
                        }
                    }
                });
        }

        function abrirMensajePoliticasProyecto() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' >Políticas transversales asociadas</span>");
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.obtenerPoliticasTransversales(vm.BPIN);
            }
        }

        vm.notificacionValidacionHijo = function (handler) {
            vm.notificacionErrores = handler;
        }

        vm.validacionGuardadoHijo = function (handler) {
            vm.validacionGuardado = handler;
        }

        function recorrerObjetivosNumber(event) {

            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
            }

            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
            }

            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
            }

            event.target.value = parseFloat(event.target.value.replace(",", "."));
        }

        vm.getNombreReducido = function (data, maxCaracteres) {
            if (data.length > maxCaracteres) {
                var dataNueva = data.slice(0, maxCaracteres);
                return dataNueva + '...';
            } else return data
        }

        function abrirModalAgregarPoliticaTrans() {
            $uibModal.open({
                templateUrl: '/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/focalizacion/PoliticasTransversales/modalAgregarPoliticaTransversal.html',
                controller: 'modalAgregarPoliticaTransversalController',
            }).result.then(function (result) {
                guardarCapituloModificado();
                init();
            }, function (reason) {

            }), err => {
                //toastr.error("Ocurrió un error al consultar el idAplicacion");
            };

        }

        vm.testRefresco = function () {
            alert("Entro componente 1");
            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });

        }

        function eliminarPolitica(politicaId, politica) {

            var idInstancia = $sessionStorage.idNivel;
            var proyectoId = $sessionStorage.idProyectoEncabezado;

            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                const mensaje3 = "Los datos fueron eliminados con éxito.";
                return focalizacionAjustesSgrServicio.eliminarPoliticasProyecto(proyectoId, politicaId, usuarioDNP, idInstancia).then(
                    function (respuesta) {

                        let exito = jQuery.parseJSON(respuesta.data);
                        if (exito.ReasonPhrase == 'Guardado exitoso') {
                            //guardarCapituloModificado();

                            $timeout(function () {
                                utilidades.mensajeSuccess("", false, function funcionContinuar() {
                                    vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                                    vm.init();
                                }, false, mensaje3);

                            }, 600);



                        } else {
                            new utilidades.mensajeError("No se puede eliminar la politica seleccionada, se encuentra con valores en firme.");
                        }
                    });
            }, function funcionCancelar(reason) {
                //console.log("reason", reason);
            }, null, null, "Es posible que la política a desvincular tenga focalización registrada en sus categorías, al confirmar la desvinculación, se eliminarán los datos registrados.");
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.InstanciaSeleccionada.IdInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    } else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Políticas Transversales");
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var isValid = true;
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    if (vm.notificacionErrores != null && erroresJson != null) vm.notificacionErrores(erroresJson[vm.nombreComponente]);

                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.limpiarErrores = function () {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-categoriapolitica-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
            var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-valorcategoriapolitica-error");
            if (campoObligatorioProyectos != undefined) {
                campoObligatorioProyectos.innerHTML = "";
                campoObligatorioProyectos.classList.add('hidden');
            }
        }

        vm.validarCategoriasPoliticas = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-categoriapolitica-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValoresCategorias = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-valorcategoriapolitica-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            //'FUE001': vm.validarExitenciaFuentes;
            'FOCPOL001': vm.validarCategoriasPoliticas,
            'FOCPOL002': vm.validarValoresCategorias,
        }
    }

    angular.module('backbone').component('politicasTransversalesSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/focalizacion/PoliticasTransversales/politicasTransversalesSgr.html",
        controller: politicasTransversalesSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadoevent: '&',
            notificarrefresco: '&',
            notificacioncambios: '&',
            notificacioninicio: '&'
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
        })
        .directive('nksOnlyNumber', function () {
            return {
                restrict: 'EA',
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                        var spiltArray = String(newValue).split("");

                        if (attrs.allowNegative == "false") {
                            if (spiltArray[0] == '-') {
                                newValue = newValue.replace("-", "");
                                ngModel.$setViewValue(newValue);
                                ngModel.$render();
                            }
                        }

                        if (attrs.allowDecimal == "false") {
                            newValue = parseInt(newValue);
                            ngModel.$setViewValue(newValue);
                            ngModel.$render();
                        }

                        if (attrs.allowDecimal != "false") {
                            if (attrs.decimalUpto) {
                                var n = String(newValue).split(".");
                                if (n[1]) {
                                    var n2 = n[1].slice(0, attrs.decimalUpto);
                                    newValue = [n[0], n2].join(".");
                                    ngModel.$setViewValue(newValue);
                                    ngModel.$render();
                                }
                            }
                        }


                        if (spiltArray.length === 0) return;
                        if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                        if (spiltArray.length === 2 && newValue === '-.') return;

                        /*Check it is number or not.*/
                        if (isNaN(newValue)) {
                            ngModel.$setViewValue(oldValue || '');
                            ngModel.$render();
                        }
                    });
                }
            };
        });
})();