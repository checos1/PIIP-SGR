(function () {
    'use strict';

    //angular.module('backbone').controller('ejecucionFormulariosController', ejecucionFormulariosController);

    angular.module('backbone').factory('$dialogConfirm', function ($uibModal, $q) {
        return function (message, title) {
            var deferred = $q.defer();
            var modal = $uibModal.open({
                size: '',
                templateUrl: '/src/app/acciones/consultarAcciones/modales/modalConfirmacion.html',
                controller: function ($scope, $uibModalInstance) {
                    $scope.modal = $uibModalInstance;
                    if (angular.isObject(message)) {
                        angular.extend($scope, message);
                    } else {
                        $scope.message = message;
                        $scope.title = angular.isUndefined(title) ? 'Mensagem' : title;
                    }
                }
            });
            modal.result.then(function () {
                deferred.resolve(true);
            }, function () {
                deferred.resolve(false);
            });
            return deferred.promise;
        }
    }).component('ejecucionFormulariosController', {
        templateUrl: '/src/app/acciones/consultarAcciones/ejecucionFormularios.html',
        controller: ejecucionFormulariosController,
        controllerAs: 'vm',
        bindings: {
            deshabilitarControles: '<'
        }
    });

    ejecucionFormulariosController.$inject = ['$q', '$scope', '$sessionStorage', "constantesFormularios", 'servicioVisualizacionFormulario', "ejecutorReglasServicios",
        "servicioAcciones", "servicioFichasProyectos", "$uibModal", "utilidades", "$filter", "$http", "constantesBackbone", "sAlert", '$dialogConfirm', '$window', '$location',
        "FileSaver", "constantesAcciones", "archivoServicios", "$interpolate"];

    function ejecucionFormulariosController($q, $scope, $sessionStorage, constantesFormularios, servicioVisualizacionFormulario, ejecutorReglasServicios,
        servicioAcciones, servicioFichasProyectos, $uibModal, utilidades, $filter, $http, constantesBackbone, sAlert, $dialogConfirm, $window, $location,
        FileSaver, constantesAcciones, archivoServicios, $interpolate) {
        var vm = this;
        vm.encabezadoColapsado = true;
        vm.idFormulario = $sessionStorage.idFormulario;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.idObjetoNegocio = $sessionStorage.idObjetoNegocio;
        vm.idEntidad = $sessionStorage.idEntidad;
        vm.idAplicacion = null;
        vm.idNivel = null;
        vm.model = null;
        vm.schema = null;
        vm.form = null;
        vm.modelEncabezado = null;
        vm.formEncabezado = null;
        vm.schemaEncabezado = null;
        vm.modelPie = null;
        vm.formPie = null;
        vm.schemaPie = null;
        vm.mensaje = "";
        vm.ExisteDevolver = false;
        vm.controlarError = controlarError;
        vm.ocultarCampos = ocultarCampos;
        vm.mostrarCampos = mostrarCampos;
        vm.establecerValorEnCampos = establecerValorEnCampos;
        vm.obtenerValor = obtenerValor;
        vm.obtenerValorWBS = obtenerValorWBS;
        vm.evaluar = evaluar;
        vm.esCampoVisible = esCampoVisible;
        vm.validarFormulario = validarFormulario;
        vm.AccionesDevolucion = [];
        vm.BuscarAccionesDevolucion = buscarAccionesDevolucion;
        vm.accionDevolverSeleccionada = null;
        vm.confirmarAccionSiguiente = confirmarAccionSiguiente;
        vm.refrescarPagina = refrescarPagina;
        vm.obtenerDatosFormulario = obtenerDatosFormulario;
        vm.validarCamposFormulario = validarCamposFormulario;
        vm.verificarValoresDefecto = verificarValoresDefecto;
        vm.GuardarFormulario = GuardarFormulario;
        vm.FijaColumnaAccionALaDerecha = FijaColumnaAccionALaDerecha;
        $scope.$on("$locationChangeStart", function (event, next, prev) {
            event.preventDefault();
            $dialogConfirm('Al abandonar el espacio de trabajo, los datos no guardados se perderán.', '¿Desea salir?').then(function (result) {
                if (result) {
                    $window.location.href = next;
                }
            });
        });

        function redirect(next) {
            $location.path(next);
        }
        //Boton Custom
        vm.buttomCustom = {
            form: null,
            schema: null,
            model: null
        };
        //

        this.$onInit = function () {
            validarFormulario()
                .then(buscarAccionesDevolucion)
                .then(obtenerConfiguracionFormulario)
                .then(configurarFormulario)
                .then(cargarCabeceraFormulario)
                .then(cargarCuerpoFomrulario)
                .then(cargarPieFormulario)
                .catch(mostrarError);
        };

        function FijaColumnaAccionALaDerecha() {
            vm.form.forEach((section) => {
                section.items.forEach((item) => {
                    item.items.forEach((i) => {
                        if (i.type === "grid") {
                            i.columnDefs.forEach(c => {
                                if (c.name == 'ColumnaAcciones') {
                                    c.pinnedRight = true;
                                }
                            })
                        }
                    })
                })
            });
        }

        function AlteraTamanoColumnas() {
            vm.form.forEach((section) => {
                section.items.forEach((item) => {
                    item.items.forEach((i) => {
                        if (i.type === "grid") {
                            i.columnDefs.forEach(c => {
                                c.width = '20%'
                            })
                        }
                    })
                })
            });
        }

        function cargarPieFormulario(resultadoFormulario) {
            var deferred = $q.defer();

            deferred.resolve(resultadoFormulario);

            var urlContenido = resultadoFormulario.data.FormularioPie.UrlContenido;
            if (urlContenido) {
                obtenerDatosDeServicioEncabezadoPieDePagina(urlContenido)
                    .then(function (datosPie) {
                        vm.schemaPie = JSON.parse(resultadoFormulario.data.FormularioPie.Schema);
                        vm.formPie = JSON.parse(resultadoFormulario.data.FormularioPie.Form);
                        vm.modelPie = datosPie;
                        $scope.$watch("vm.modelPie", ejecutarRegla, true);
                    }).catch(mostrarError);
            }

            return deferred.promise;
        }

        function cargarCuerpoFomrulario(resultadoFormulario) {
            var deferred = $q.defer();

            deferred.resolve(resultadoFormulario);

            obtenerDatosFormulario(resultadoFormulario.data.Url)
                .then(function (datosServicio) {
                    vm.schema = JSON.parse(resultadoFormulario.data.Schema);
                    vm.form = JSON.parse(resultadoFormulario.data.Form);

                    if (vm.form !== null) {
                        angular.forEach(vm.form, function (value, $index) {
                            if (value.type === "submit") {
                                vm.form.splice($index, 1);
                            }
                        });
                    }

                    try {
                        for (let i = 0; i < vm.form.length; i++) {
                            for (let i2 = 0; i2 < vm.form[i].items.length; i2++) {
                                for (let i3 = 0; i3 < vm.form[i].items[i2].items.length; i3++) {
                                    var type = vm.form[i].items[i2].items[i3].type;
                                    var key = vm.form[i].items[i2].items[i3].key;
                                    if (["grid", "tabla"].includes(type)) {
                                        vm.form[i].items[i2].items[i3].data = datosServicio.data[key];
                                        vm.form[i].items[i2].items[i3].agregarPreguntasRequisitos = datosServicio.data['AgregarPreguntasRequisitos']
                                    }
                                }
                            }
                        }
                    } catch (e) {

                    }

                    aplicarDeshabilitarControles(vm.form);
                    FijaColumnaAccionALaDerecha();
                    AlteraTamanoColumnas();

                    vm.model = datosServicio.data;
                    $scope.$watch("vm.model", ejecutarRegla, true);
                }).catch(mostrarError);

            return deferred.promise;
        }

        function cargarCabeceraFormulario(resultadoFormulario) {
            var deferred = $q.defer();

            deferred.resolve(resultadoFormulario);

            var urlContenido = resultadoFormulario.data.FormularioEncabezado.UrlContenido;
            if (urlContenido) {
                obtenerDatosDeServicioEncabezadoPieDePagina(urlContenido)
                    .then(function (datosEncabezado) {
                        vm.schemaEncabezado = JSON.parse(resultadoFormulario.data.FormularioEncabezado.Schema);
                        vm.formEncabezado = JSON.parse(resultadoFormulario.data.FormularioEncabezado.Form);
                        vm.modelEncabezado = JSON.parse(resultadoFormulario.data.FormularioEncabezado.Model);
                        vm.modelEncabezado = datosEncabezado;
                        $scope.$watch("vm.modelEncabezado", ejecutarRegla, true);
                    }).catch(mostrarError);
            }

            return deferred.promise;
        }

        function configurarFormulario(resultadoFormulario) {
            var deferred = $q.defer();

            $sessionStorage.IdAplicacion = resultadoFormulario.data.IdAplicacion;
            $sessionStorage.IdNivel = resultadoFormulario.data.IdNivel;
            vm.idAplicacion = $sessionStorage.IdAplicacion;
            vm.idNivel = $sessionStorage.IdNivel;
            vm.mensaje = "La respuesta fue exitosa.";

            if (resultadoFormulario.data.Model === "" || resultadoFormulario.data.Form === "" || resultadoFormulario.data.Schema === "") {
                vm.mensaje = "Falta información para renderizar.";
                deferred.reject("Falta información para renderizar.");
            } else {
                deferred.resolve(resultadoFormulario);
            }

            return deferred.promise;
        }

        function obtenerConfiguracionFormulario() {
            return servicioVisualizacionFormulario.formulario(vm.idFormulario);
        }

        function validarFormulario() {
            var deferred = $q.defer();

            switch (vm.idFormulario) {
                case '00000000-0000-0000-0000-000000000000':
                case null:
                case undefined:
                    vm.mensaje = "No se puede renderizar el formulario.";
                    deferred.reject("No se puede renderizar el formulario.");
                    break;
                default:
                    deferred.resolve(true);
            }
            return deferred.promise;
        }

        function aplicarDeshabilitarControles(form) {
            if (form && form.length > 0) {
                _.each(form, function (seccion) {
                    if (seccion.items) {
                        _.each(seccion.items, function (seccionInterna) {
                            if (seccionInterna.items) {
                                _.each(seccionInterna.items, function (control) {
                                    control.deshabilitarControles = vm.deshabilitarControles;
                                });
                            }
                        });
                    }
                });
            }
        }

        function mostrarError(e) {
            var mensaje = "";

            if (e.data)
                if (e.data.ExceptionMessage)
                    mensaje = e.data.ExceptionMessage;
            mensaje = mensaje === "" ? e : mensaje;
            utilidades.mensajeError($filter('language')('ErrorCargaFormulario') + " " + mensaje);
        }

        function obtenerDatosDeServicioEncabezadoPieDePagina(url) {
            return $q(function (resolve, reject) {
                if (url) {
                    if (validarUrl(url)) {
                        return servicioVisualizacionFormulario.obtenerDatosServiciosFormulario(url, vm.idObjetoNegocio, $sessionStorage.IdNivel, vm.idInstancia, vm.idAccion, vm.idAplicacion, vm.idFormulario).then(
                            function (resultado) {
                                if (!resultado.data) {
                                    reject($filter("language")("ErrorLecturaContenido"));
                                    return;
                                }
                                if (resultado.data.length < 1) {
                                    reject($filter("language")("ApiArregloVacio"));
                                    return;
                                }
                                resolve(resultado.data);
                            },
                            function () {
                                reject($filter("language")("ErrorUrl"));
                            });
                    } else {
                        reject($filter("language")("FormatoURL"));
                    }
                } else {
                    reject($filter("language")("UrlNoAsignada"));
                }
            });
        }

        function validarUrl(url) {
            var patron = /^(https?:\/\/)([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \?=.-]*)*\/?$/;
            if (!url || url.match(patron) === null) {
                return true;
            }
            return true;
        }

        function ejecutarRegla(newValue, oldValue) {
            if (vm.schema && vm.schema.reglas)
                eval(vm.schema.reglas);
        }

        function controlarError(error) {
            ejecutorReglasServicios.controlarError(error);
        }

        function ocultarCampos(campos) {
            vm.form = ejecutorReglasServicios.ocultarCampos(vm.form, campos);
        }

        function mostrarCampos(campos) {
            vm.form = ejecutorReglasServicios.mostrarCampos(vm.form, campos);
        }

        function establecerValorEnCampos(campos, valor) {
            vm.model = ejecutorReglasServicios.establecerValorEnCampos(vm.model, campos, valor);
        }

        function obtenerValor(valor, formato) {
            return ejecutorReglasServicios.obtenerValor(vm.model, valor, formato);
        }

        function obtenerValorWBS(valor, formato, WBS) {
            return ejecutorReglasServicios.obtenerValorWBS(vm.model, valor, formato, WBS);
        }

        function evaluar(expresion1, operador, expresion2) {
            return ejecutorReglasServicios.evaluar(expresion1, operador, expresion2);
        }

        function esCampoVisible(campo) {
            var visible = true;
            if (campo) {
                var objeto = ejecutorReglasServicios.obtenerCampoDelFormulario(vm.form, campo);
                if (objeto) {
                    if (objeto.type === constantesFormularios.controlBoton) {
                        if (objeto.buttonTypeReturn && !vm.ExisteAccionesDevolver) {
                            return objeto.visible = false;
                        }
                        else if (objeto.buttonTypeCustom && objeto.condicion && Object.getOwnPropertyNames(vm.model).length > 0 && Object.getOwnPropertyNames(objeto.condicion).length > 0) {
                            Object.getOwnPropertyNames(vm.model).forEach(function (val, idx, array) {
                                if (val === objeto.condicion.expresion1.valor) {
                                    visible = evaluar(vm.model[val], objeto.condicion.operador.tipo, objeto.condicion.expresion2.valor);
                                    return objeto.visible = visible;
                                }
                            });
                        }
                    }
                    return objeto.visible;
                }
            }
            return visible;
        }

        $scope.$on('onbuttonTypeCustomClick', function (event, data) {
            logicaBotonCustom(data);
        });

        $scope.$on('onbuttonTypeReturnClick', function (event, data) {
            seleccionarAccionDevolver();
        });

        $scope.$on('onbuttonTypeSaveClick', function (event, data) {
            GuardarFormulario(false);
        });

        $scope.$on('onbuttonTypeSearchClick', function (event, data) {

        });

        $scope.$on('onbuttonTypeNextClick', function (event, data) {
            if (validarCamposFormulario()) {
                utilidades.mensajeWarning($filter('language')('ConfirmarAccionSiguiente'),
                    vm.confirmarAccionSiguiente,
                    null,
                    $filter('language')('Aceptar'));
            }
        });

        $scope.$on('onbuttonTypeFichaClick', function (event, fichaPlantilla) {
            var esBorrador = $sessionStorage.estadoAccion === constantesAcciones.estado.pasoEnProgreso;

            if (esBorrador) {
                var fichaBorrador = _.clone($sessionStorage.fichaPlantilla);
                fichaBorrador.PARAM_BORRADOR = true;

                utilidades.mensajeWarning(
                    $filter('language')('ConfirmarFichaBorrador'),
                    function () {
                        //procesarFichasBorrador().then(function () {
                        crearDocumento(fichaBorrador).then(function (fichaTemporal) {
                            FileSaver.saveAs(fichaTemporal, fichaTemporal.name);
                            //utilidades.mensajeSuccess($filter('language')('FichaGeneradaCorrectamente'), null, null, null);
                        }, function (error) {
                            utilidades.mensajeError(error);
                        });
                        //});
                    },
                    null,
                    $filter('language')('Aceptar'));
            }
        });

        $scope.$on('onDevolverAccion', function (event, data) {
            procesarFichasBorrador().then(function () {
                crearDocumento($sessionStorage.fichaPlantilla).then(function () {

                    $sessionStorage.fichaPlantilla = undefined;
                    $sessionStorage.Ficha = undefined;

                    utilidades.mensajeSuccess($filter('language')('ExitoGuardadoFormulario'),
                        false,
                        window.location.reload(),
                        null);
                }, function (error) {
                    utilidades.mensajeError(error);
                });
            });
        });

        function validarCamposFormulario() {
            var data = ejecutorReglasServicios.validarCamposRequeridos(vm.model, vm.schema.required);
            if (!data) {
                utilidades.mensajeError("Por favor diligencie los campos marcados con *");
            }
            else {
                return true;
            }
            
        }

        function confirmarAccionSiguiente() {
            GuardarFormulario(true);
        }

        function GuardarFormulario(postDefinitivo) {
            var parametrosEjecucionFlujo = new Object();
            parametrosEjecucionFlujo.IdInstanciaFlujo = $sessionStorage.idInstanciaFlujoPrincipal;//= vm.idInstancia;
            parametrosEjecucionFlujo.IdAccion = vm.idAccion;
            parametrosEjecucionFlujo.PostDefinitivo = postDefinitivo;
            parametrosEjecucionFlujo.ObjetoContexto = new Object();
            //parametrosEjecucionFlujo.ObjetoContexto.IdRol = '1dd225f4-5c34-4c55-b11d-e5856a68839b';
            parametrosEjecucionFlujo.ObjetoContexto.IdUsuario = usuarioDNP;
            parametrosEjecucionFlujo.ObjetoDatos = new Object();


            for (let i = 0; i < vm.form.length; i++) {
                for (let i2 = 0; i2 < vm.form[i].items.length; i2++) {
                    for (let i3 = 0; i3 < vm.form[i].items[i2].items.length; i3++) {
                        var type = vm.form[i].items[i2].items[i3].type;
                        var key = vm.form[i].items[i2].items[i3].key;
                        var datosNuevos = vm.form[i].items[i2].items[i3].originalData;
                        var data = vm.form[i].items[i2].items[i3].data;
                        //RCacheOriginalValue
                        if (["grid", "tabla"].includes(type)) {
                            if (['PreguntasGenerales', 'PreguntasEspecificas'].includes(key[0])) {
                                vm.model[key] = datosNuevos.filter(p => p.IdPregunta);
                                data.forEach(d => {
                                    d.subGridOptions.data.forEach(s => {
                                        if (s.OpcionesRespuestasSeleccionado) {
                                            datosNuevos.find(dn => dn.IdPregunta === s.IdPregunta).OpcionesRespuestasSeleccionado = s.OpcionesRespuestasSeleccionado;
                                        }
                                        if (s.ObservacionPregunta) {
                                            datosNuevos.find(dn => dn.IdPregunta === s.IdPregunta).ObservacionPregunta = s.ObservacionPregunta;
                                        }

                                    });
                                });
                                if (datosNuevos.AgregarRequisitos)
                                    vm.model.AgregarRequisitos = true;
                            }

                            for (let iRow = 0; iRow < vm.model[key].length; iRow++) {
                                var tempRow = vm.model[key][iRow];
                                Object.keys(tempRow).forEach(function (k) {
                                    if (k.indexOf("RCacheOriginalValue") > -1) {
                                        tempRow[k.replace("RCacheOriginalValue", "")] = tempRow[k];
                                    }
                                })
                            }

                        }
                        if (["controlWbs"].includes(type)) {
                            try {
                                for (let iRow = 0; iRow < vm.model[key].length; iRow++) {
                                    var tempRow = vm.model[key][iRow];
                                    Object.keys(tempRow).forEach(function (k) {
                                        if (k.indexOf("RCacheOriginalValue") > -1) {
                                            tempRow[k.replace("RCacheOriginalValue", "")] = tempRow[k];
                                        }
                                    })
                                }
                            } catch (ex) {
                                console.log(ex)
                            }
                        }
                    }
                }
            }
            parametrosEjecucionFlujo.ObjetoDatos.IdEntidad = vm.idEntidad;
            parametrosEjecucionFlujo.ObjetoDatos.ObjetoJson = JSON.stringify(vm.model);

            $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiEjecutarFlujo, parametrosEjecucionFlujo).then(

                function (resultado) {
                    if (resultado.data !== null) {
                        if (postDefinitivo) {
                            if (resultado.status === 200) {
                                if ($sessionStorage.fichaPlantilla != undefined) {
                                    procesarFichasBorrador().then(function () {
                                        crearDocumento($sessionStorage.fichaPlantilla).then(function () {
                                            $sessionStorage.fichaPlantilla = undefined;
                                            $sessionStorage.Ficha = undefined;
                                            $sessionStorage.guardadoPrevio = true;

                                            utilidades.mensajeSuccess($filter('language')('ExitoGuardadoFormulario'),
                                                false,
                                                vm.refrescarPagina,
                                                null);
                                        }, function (error) {
                                            utilidades.mensajeError(error);
                                        });
                                    });
                                } else {
                                    $sessionStorage.guardadoPrevio = true;
                                    utilidades.mensajeSuccess($filter('language')('ExitoGuardadoFormulario'),
                                        false,
                                        vm.refrescarPagina,
                                        null);
                                }
                            }
                            else {
                                utilidades.mensajeError($filter('language')('ErrorGuardadoFormulario'));
                            }
                        }
                        else {
                            if (resultado.status === 200) {
                                sAlert.success($filter('language')('GuardadoTemporal'), 'mensaje').autoRemove();
                            }
                            else {
                                utilidades.mensajeError($filter('language')('ErrorGuardadoFormulario'));
                            }
                        }
                    }
                    else {
                        utilidades.mensajeError($filter('language')('ErrorGuardadoFormulario'));
                    }
                }
            ).catch(function (e) {

                var mensaje;
                var mensajeRecurso = $filter('language')('ErrorGuardadoTemporal');

                if (e.data === undefined)
                    mensaje = e.message;
                else {
                    if (e.data.ExceptionMessage && (e.data.ExceptionMessage.startsWith("{"))) {
                        try {
                            var excepcion = angular.fromJson(e.data.ExceptionMessage);
                            mensaje = utilidades.generarLogExcepcionHTML(excepcion);
                        } catch (e) {
                            mensaje = $filter('language')('ImposiblePresentarError');
                        }
                    } else
                        mensaje = e.data.ExceptionMessage;
                }
                utilidades.mensajeError(mensajeRecurso.replace("[0]", mensaje));

            });
        }

        function refrescarPagina() {
            location.reload();
        }

        function DevolverCustom(url) {
            var postDefinitivo = true;
            var parametrosEjecucionFlujo = new Object();
            parametrosEjecucionFlujo.IdInstanciaFlujo = $sessionStorage.idInstanciaFlujoPrincipal;
            parametrosEjecucionFlujo.IdAccion = vm.idAccion;
            parametrosEjecucionFlujo.PostDefinitivo = postDefinitivo;
            parametrosEjecucionFlujo.ObjetoContexto = new Object();
            parametrosEjecucionFlujo.ObjetoContexto.IdUsuario = usuarioDNP;
            parametrosEjecucionFlujo.ObjetoDatos = new Object();


            for (let i = 0; i < vm.form.length; i++) {
                for (let i2 = 0; i2 < vm.form[i].items.length; i2++) {
                    for (let i3 = 0; i3 < vm.form[i].items[i2].items.length; i3++) {
                        var type = vm.form[i].items[i2].items[i3].type;
                        var key = vm.form[i].items[i2].items[i3].key;
                        var datosNuevos = vm.form[i].items[i2].items[i3].originalData;
                        var data = vm.form[i].items[i2].items[i3].data;
                        //RCacheOriginalValue
                        if (["grid", "tabla"].includes(type)) {
                            if (['PreguntasGenerales', 'PreguntasEspecificas'].includes(key[0])) {
                                vm.model[key] = datosNuevos.filter(p => p.IdPregunta);
                                data.forEach(d => {
                                    d.subGridOptions.data.forEach(s => {
                                        if (s.OpcionesRespuestasSeleccionado) {
                                            datosNuevos.find(dn => dn.IdPregunta === s.IdPregunta).OpcionesRespuestasSeleccionado = s.OpcionesRespuestasSeleccionado;
                                        }
                                        if (s.ObservacionPregunta) {
                                            datosNuevos.find(dn => dn.IdPregunta === s.IdPregunta).ObservacionPregunta = s.ObservacionPregunta;
                                        }

                                    });
                                });
                                if (datosNuevos.AgregarRequisitos)
                                    vm.model.AgregarRequisitos = true;
                            }

                            for (let iRow = 0; iRow < vm.model[key].length; iRow++) {
                                var tempRow = vm.model[key][iRow];
                                Object.keys(tempRow).forEach(function (k) {
                                    if (k.indexOf("RCacheOriginalValue") > -1) {
                                        tempRow[k.replace("RCacheOriginalValue", "")] = tempRow[k];
                                    }
                                })
                            }

                        }
                        if (["controlWbs"].includes(type)) {
                            try {
                                for (let iRow = 0; iRow < vm.model[key].length; iRow++) {
                                    var tempRow = vm.model[key][iRow];
                                    Object.keys(tempRow).forEach(function (k) {
                                        if (k.indexOf("RCacheOriginalValue") > -1) {
                                            tempRow[k.replace("RCacheOriginalValue", "")] = tempRow[k];
                                        }
                                    })
                                }
                            } catch (ex) {
                                console.log(ex)
                            }
                        }
                    }
                }
            }
            parametrosEjecucionFlujo.ObjetoDatos.IdEntidad = vm.idEntidad;
            parametrosEjecucionFlujo.ObjetoDatos.ObjetoJson = JSON.stringify(vm.model);

            $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiEjecutarFlujo, parametrosEjecucionFlujo).then(
                function (resultado) {
                    if (resultado.data !== null) {
                        if (resultado.status === 200) {
                            var datos = {
                                Bpin: vm.model.CodigoBPIN,
                                ProyectoId: 0,
                                Observacion: vm.model.ObservacionCuestionario == null ? 'No registra' : vm.model.ObservacionCuestionario,
                                DevolverId: true,
                                EstadoDevolver: 7
                            };
                            $http({
                                method: 'POST',
                                url: url,
                                data: datos,
                                headers: {
                                    'piip-idInstanciaFlujo': $sessionStorage.idInstanciaFlujoPrincipal,
                                    'piip-idAccion': vm.idAccion,
                                    'piip-idAplicacion': vm.idAplicacion,
                                    'piip-idFormulario': vm.idFormulario,
                                }
                            }).then(
                                function (resultado) {
                                    if (resultado.status === 200) {
                                        $window.location.href = '/proyectos/pl';
                                    }
                                }
                            ).catch(function (e) {

                                var mensaje;
                                var mensajeRecurso = $filter('language')('ErrorGuardadoTemporal');

                                if (e.data === undefined)
                                    mensaje = e.message;
                                else {
                                    if (e.data.ExceptionMessage && (e.data.ExceptionMessage.startsWith("{"))) {
                                        try {
                                            var excepcion = angular.fromJson(e.data.ExceptionMessage);
                                            mensaje = utilidades.generarLogExcepcionHTML(excepcion);
                                        } catch (e) {
                                            mensaje = $filter('language')('ImposiblePresentarError');
                                        }
                                    } else
                                        mensaje = e.data.ExceptionMessage;
                                }
                                utilidades.mensajeError(mensajeRecurso.replace("[0]", mensaje));

                            });

                        }
                        else {
                            utilidades.mensajeError($filter('language')('ErrorGuardadoFormulario'));
                        }
                    }
                    else {
                        utilidades.mensajeError($filter('language')('ErrorGuardadoFormulario'));
                    }
                }
            ).catch(function (e) {

                var mensaje;
                var mensajeRecurso = $filter('language')('ErrorGuardadoTemporal');

                if (e.data === undefined)
                    mensaje = e.message;
                else {
                    if (e.data.ExceptionMessage && (e.data.ExceptionMessage.startsWith("{"))) {
                        try {
                            var excepcion = angular.fromJson(e.data.ExceptionMessage);
                            mensaje = utilidades.generarLogExcepcionHTML(excepcion);
                        } catch (e) {
                            mensaje = $filter('language')('ImposiblePresentarError');
                        }
                    } else
                        mensaje = e.data.ExceptionMessage;
                }
                utilidades.mensajeError(mensajeRecurso.replace("[0]", mensaje));

            });
        }

        function seleccionarAccionDevolver() {
            if (!vm.accionDevolverSeleccionada) {
                vm.accionDevolverSeleccionada = vm.accionSeleccionada
                    ? angular.copy(vm.accionSeleccionada.Flujo)
                    : null;
            }

            vm.seleccionarAccionModal = $uibModal.open({
                animation: true,
                templateUrl: '/src/app/panelEjecucionDeAccion/listarAccionesAnteriores.html',
                controller: 'listarAccionesAnterioresController',
                controllerAs: "vm",
                keyboard: false,
                backdrop: false,
                scope: $scope,
                size: "sm",

                resolve: {
                    listaacciones: $q.resolve(vm.AccionesDevolucion),
                    idInstancia: $q.resolve(vm.idInstancia),
                    idAccion: $q.resolve(vm.idAccion),
                    idAplicacion: $q.resolve(vm.idAplicacion),
                    existeFichaGenerar: $q.resolve($sessionStorage.fichaPlantilla !== undefined)
                }
            });

        }

        function buscarAccionesDevolucion() {
            if (vm.idInstancia === undefined || vm.idInstancia === null)
                vm.idInstancia = $sessionStorage.idInstanciaFlujoPrincipal;//= vm.idInstancia;
            return servicioAcciones.ObtenerAccionesDevolucion(vm.idInstancia, vm.idAccion).then(
                function (resultado) {
                    if (resultado.data && resultado.data.Result.length > 0) {

                        vm.ExisteAccionesDevolver = true;
                        vm.AccionesDevolucion = resultado.data.Result;
                    }
                    else
                        vm.ExisteAccionesDevolver = false;
                });
        }

        function obtenerDatosFormulario(url) {
            return servicioVisualizacionFormulario.obtenerDatosServiciosFormulario(url, vm.idObjetoNegocio, $sessionStorage.IdNivel, $sessionStorage.idInstanciaFlujoPrincipal, vm.idAccion, vm.idAplicacion, vm.idFormulario);
        }


        function verificarValoresDefecto(model, data) {
            var indices = Object.keys(data);
            indices.forEach(function (index) {
                if (model !== undefined) {
                    if (model[index] && (data[index] == null || data[index] === "")) {
                        data[index] = model[index];
                    }
                }
            });
            return data;
        }


        //#region Boton Custom

        function logicaBotonCustom(data) {
            if (data.condicion) {
                data.visible = evaluarCondicionDeVisibilidad();
            } else if (data.eventoCustom) {
                obtenerFormularioHTML(data);
            }
        }

        function obtenerFormularioHTML(data) {

            if (data.eventoCustom.Formulario) {

                var idFormulario = data.eventoCustom.Formulario;
                servicioVisualizacionFormulario.formulario(idFormulario).then(function (response) {
                    convertirASchemaForm(response.data);
                    abrirModal(data.eventoCustom);
                });
            } else if (data.eventoCustom.Url) {
                if (data.eventoCustom.Url.indexOf("seleccionDeProyectos") > -1 || data.eventoCustom.Url.indexOf("resumenDeProyectos") > -1)
                    data.eventoCustom.Url = data.eventoCustom.Url.replace("')", "")
                else
                    data.eventoCustom.Url = data.eventoCustom.Url + '?output=embed';

                abrirModal(data.eventoCustom);
            } else if (data.eventoCustom.Servicio) {

                confirmarEjecucion(data.eventoCustom.Servicio);
            } else {
                utilidades.mensajeError($filter('language')('errorTipoAccionCustom'));
            }
        }

        function confirmarEjecucion(idServicio) {
            swal({
                title: '',
                text: '¿Está seguro de que desea ejecutar el Servicio?',
                type: 'warning',
                showCancelButton: true,
                confirmButtonText: "Continuar",
                cancelButtonText: "Cancelar",
                closeOnConfirm: true
            }, function (isConfirm) {

                if (isConfirm) {

                    servicioVisualizacionFormulario.obtenerServicio(idServicio)
                        .then(obtenerParametros)
                        .then(ejecutarServicio)
                        .catch(mostrarError);
                }
            });

        }

        function obtenerParametros(servicioCustom) {

            var deferred = $q.defer();
            var parametrosConDatos = new Object();
            var valor = null;
            var parametros = null;

            if (servicioCustom.data.Tipo == 1 || servicioCustom.data.Tipo == 'GET') {
                parametros = JSON.parse(getParametrosfromUrl(servicioCustom.data.Detalle));
            } else {
                parametros = new Array(JSON.parse(servicioCustom.data.Parametros));
            }
            parametrosConDatos.datosUrl = servicioCustom.data;
            parametrosConDatos.params = {};

            if (parametros[0][0] != undefined) {
                angular.forEach(parametros[0], function (value, key) {
                    if (value == null || value == 'null') {
                        valor = obtenerValorParametro(vm.model, (key + ''));
                    }

                    if (valor) {
                        parametrosConDatos.params[key] = valor;
                    } else {
                        var mensaje = "El parametro " + key + " no se ha encontrado. Contacte al Administrador.";
                        deferred.reject(mensaje);
                    }

                });

            }

            deferred.resolve(parametrosConDatos);

            return deferred.promise;

        }

        function getParametrosfromUrl(url) {
            var params = {};
            var parser = document.createElement('a');
            parser.href = url;

            var query = parser.search.substring(1);


            var vars = query.split('&');

            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }

            return JSON.stringify(new Array(params));
        }


        function obtenerValorParametro(form, campo) {
            var valor = form[campo];
            return valor;
        }


        function ejecutarServicio(servicio) {
            if (servicio.datosUrl.Nombre == 'Devolver') {
                DevolverCustom(servicio.datosUrl.Detalle.split('?')[0]);
            }
            else {
                var servicioParse = {
                    url: servicio.datosUrl.Detalle.split('?')[0],
                    metodo: servicio.datosUrl.Tipo,
                    parametros: servicio.params
                }
                servicioVisualizacionFormulario.ejecutarServicio(servicioParse).then(function (resultado) {
                    if (resultado.status === 200)
                        utilidades.mensajeSuccess('Se ejecutó correctamente el Servicio.', false, null, null);
                    else
                        utilidades.mensajeError('Ocurrió un error al ejecutar el servicio. Contacte al Administrados.');
                }, mostrarError);
            }
            
        }

        function convertirASchemaForm(formulario) {
            vm.buttomCustom.form = formulario.Form;
            vm.buttomCustom.schema = formulario.Schema;
            vm.buttomCustom.model = formulario.Model;
            vm.buttomCustom.nombre = formulario.NombreFormulario;
            vm.buttomCustom.formEncabezado = formulario.FormularioEncabezado.Form;
            vm.buttomCustom.schemaEncabezado = formulario.FormularioEncabezado.Schema;
            vm.buttomCustom.modelEncabezado = formulario.FormularioEncabezado.Model;
            vm.buttomCustom.formPie = formulario.FormularioPie.Form;
            vm.buttomCustom.schemaPie = formulario.FormularioPie.Schema;
            vm.buttomCustom.modelPie = formulario.FormularioPie.Model;
        }


        function evaluarCondicionDeVisibilidad() {
            var condicion = $scope.form.condicion;

            if ($scope.form.modeloServicioPreguntas.hasOwnProperty(condicion.expresion1.valor)) {
                var valorExpresion1 = $scope.form.modeloServicioPreguntas[condicion.expresion1.valor];
                var valorExpresion2 = condicion.expresion2.valor;

                return ejecutorReglasServicios.evaluar(valorExpresion1, condicion.operador.tipo, valorExpresion2);
            }

            return true;
        }


        function abrirModal(eventoCustom) {
            var modal = $uibModal.open({
                animation: true,
                templateUrl: '/Scripts/schema-form/directives/modales/modalEvento.html',
                controller: 'modalEventoController',
                controllerAs: "vm",
                backdrop: false,
                keyboard: false,
                size: eventoCustom.Modal,
                windowClass: 'my-class-popup',
                resolve: {
                    eventoCustom: $q.resolve(eventoCustom),
                    formulario: $q.resolve(vm.buttomCustom),
                }
            });

            modal.result.then(function (condicion) {
                vm.buttomCustom.form = null;
                vm.buttomCustom.schema = null;
                vm.buttomCustom.model = null;
                //      
            });
        }

        //#endregion

        //#region Metodos para la generación y consulta de fichas de proyectos
        /**
         * Metodo que genera el documento usando la api de fichas de proyectos.
         * @param fichaPlantilla
         * @returns {*}
         */
        function crearDocumento(fichaPlantilla) {
            var extension = '.pdf';
            var nombreArchivo = $sessionStorage.Ficha.Nombre.replace(/ /gi, "_") + '_' + $sessionStorage.idObjetoNegocio + '_' + moment().format("YYYYMMDDD_HHMMSS") + extension;

            return $q(function (resolve, reject) {
                servicioFichasProyectos.ObtenerIdFicha($sessionStorage.Ficha.Nombre).then(function (respuestaFicha) {

                    servicioFichasProyectos.GenerarFicha($.param(fichaPlantilla)).then(function (respuesta) {
                        /*var blob = new Blob([respuesta], { type: 'application/pdf' });*/
                        const blob = utilidades.base64toBlob(respuesta, { type: 'application/pdf' });
                        var fileOfBlob = new File([blob], nombreArchivo, { type: 'application/pdf' });
                        var archivo = {};

                        var metadatos = {
                            NombreAccion: $sessionStorage.nombreAccion,
                            IdAplicacion: $sessionStorage.IdAplicacion,
                            IdNivel: $sessionStorage.idNivel,
                            IdInstancia: $sessionStorage.idInstancia,
                            IdAccion: $sessionStorage.idAccion,
                            IdInstanciaFlujoPrincipal: $sessionStorage.idInstanciaFlujoPrincipal,
                            IdObjetoNegocio: $sessionStorage.idObjetoNegocio,
                            Size: blob.size,
                            ContenType: 'application/pdf',
                            Extension: extension,
                            FechaCreacion: new Date(),
                            Tipo: 'Ficha',
                            NombreFicha: respuestaFicha.Nombre,
                            TipoFicha: respuestaFicha.Descripcion
                        }

                        archivo = {
                            FormFile: fileOfBlob,
                            Nombre: nombreArchivo,
                            Metadatos: metadatos
                        };

                        if (fichaPlantilla.PARAM_BORRADOR) {
                            resolve(fileOfBlob);
                        } else {
                            archivoServicios.cargarArchivo(archivo, $sessionStorage.IdAplicacion).then(function (response) {
                                if (response === undefined || typeof response === 'string') {
                                    reject(response);
                                } else {
                                    resolve(fileOfBlob);
                                }
                            }, function (error) {
                                reject(error);
                            });
                        }
                    }, function (error) {
                        reject(error);
                    });

                }, function (error) {
                    reject(error);
                });
            });
        }

        /**
         * Metodo general que realiza el procesamiento de buscar, eliminar fichas no definitivas y generacion de fichas.
         * @returns {*}
         */
        function procesarFichasBorrador() {
            var parametrosArchivos = {
                IdAccion: $sessionStorage.idAccion,
                IdInstancia: $sessionStorage.idInstancia,
                Tipo: 'Ficha'
            };

            return $q(function (resolve, reject) {
                archivoServicios.obtenerListadoArchivos(parametrosArchivos, $sessionStorage.IdAplicacion).then(function (response) {
                    var datosOrigen = _.filter(response, function (r) { return r.status !== 'Descartado'; });

                    if (_.size(datosOrigen) > 0) {
                        eliminarFichasBorrador(datosOrigen).then(function () {
                            resolve();
                        }, function (error) {
                            reject(error);
                        });
                    } else {
                        resolve();
                    }
                }, function (error) {
                    reject(error);
                });
            });
        }

        /**
         * Elimina las fichas que no son definitivas.
         * @param datosOrigen
         * @returns {*}
         */
        function eliminarFichasBorrador(datosOrigen) {
            return $q(function (resolve, reject) {
                _.each(_.filter(datosOrigen, function (d) { return d.status !== 'Descartado'; }), function (val) {
                    archivoServicios.eliminarArchivo(val.id.toString(), 'Descartado', $sessionStorage.IdAplicacion)
                        .then(function (response) { }, function (error) {
                            reject(error);
                        });
                });
                resolve();
            });
        }

        /**
         * Muestra el modal con las fichas tanto No Definitivas como las definitivas creadas en firme si existen.
         */
        function mostrarModalFichas() {
            let modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'src/app/fichasProyectos/template/modales/fichaTemplate.html',
                controller: 'FichaTemplateController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "dialog-modal-archivo",
                resolve: {
                    params: {}
                },
            });

            modalInstance.result.then(data => { });
        }
        //#endregion

    }
}());






