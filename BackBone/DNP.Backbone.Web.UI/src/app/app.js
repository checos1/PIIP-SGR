(function () {
    'use strict';

    var constantesFormularios = {
        "accionTipoEsconder": "accionTipoEsconder",
        "accionTipoMostrar": "accionTipoMostrar",
        "accionTipoEstablecerValorEnCampos": "accionTipoEstablecerValorEnCampos",
        "accionNoSePuedeModificar": "accionNoSePuedeModificar",
        "accionEstablecerValorFijo": "accionEstablecerValorFijo",
        "accionEstablecerValorMinimo": "accionEstablecerValorMinimo",
        "accionEstablecerValorMaximo": "accionEstablecerValorMaximo",
        "array": "array",
        "classCol": "col-xs-",
        "comment": "Comment",
        "condition": "condition",
        "contenedor": "contenedor",
        "controlWbs": "controlWbs",
        "controlPowerBI": "controlPowerBI",
        "contenedorDosColumnas": "contenedorDosColumnas",
        "contenedorTresColumnas": "contenedorTresColumnas",
        "controlFecha": "controlFecha",
        "controlSeleccionMultiple": "controlSeleccionMultiple",
        "controlLista": "controlSelect",
        "controlListaAutocompletable": "controlListaAutocompletable",
        "controlListaSeleccionMultiple": "controlListaSeleccionMultiple",
        "controlSelect": "controlSelect",
        "controlTexto": "controlTexto",
        "controlTextoSchemaForm": "string",
        "controlTextoLargo": "textarea",
        "controlNumerico": "number",
        "controlBoton": "botonControl",
        "ControlArchivo": "ControlArchivo",
        "controlNumericoSchemaForm": "number",
        "controlUrl": "urlLink",
        "controlTabla": "controlTabla",
        "controlEncabezadoFormulario": "encabezadoFormulario",
        "controlPiePaginaFormulario": "pieFormulario",
        "tipoListaMultiple": "tipoListaMultiple",
        "data": "data",
        "dateBeg": "dateBeg",
        "dateEnd": "dateEnd",
        "decimalNum": "decimalNum",
        "defaultValue": "defaultValue",
        "description": "description",
        "esInformativo": "esInformativo",
        "espacioBlanco": "espacioBlanco",
        "estiloBotonAceptar": "btn-success",
        "etiqueta": "etiqueta",
        "expresionTipoCampo": "campo",
        "expresionTipoLiteral": "literal",
        "expresionTipoPropiedad": "propiedad",
        "expresionTipoVigenciaActual": "vigenciaActual",
        "expresionTipoVigenciaFutura": "vigenciaFutura",
        "fechaActualPorDefecto": "fechaActualPorDefecto",
        "format": "format",
        "formatoValorBooleano": "booleano",
        "formatoValorFecha": "fecha",
        "formatoValorLista": "lista",
        "formatoValorNumero": "numero",
        "formatoValorTexto": "texto",
        "funcionEvaluar": "evaluar",
        "funcionObtenerValor": "obtenerValor",
        "funcionObtenerValorWBS": "obtenerValorWBS",
        "funcionEsconderCampos": "ocultarCampos",
        "funcionEstablecerValorEnCampos": "establecerValorEnCampos",
        "funcionMostrarCampos": "mostrarCampos",
        "funcionEstablecerValorFijo": "establecerValorFijo",
        "funcionEstablecerValorMaximo": "establecerValorMaximo",
        "funcionEstablecerValorMinimo": "establecerValorMinimo",
        "funcionNoSePuedeModificar": "noSePuedeModificar",
        "funcionControlarError": "controlarError",
        "funcionValidadVisibilidadCampo": "vm.esCampoVisible",
        "habilitarBusqueda": "habilitarBusqueda",
        "htmlClass": "htmlClass",
        "items": "items",
        "key": "key",
        "label": "label",
        "valor": "valor",
        "maxLenght": "maxLength",
        "maximum": "maximum",
        "minLenght": "minLength",
        "minimum": "minimum",
        "number": "number",
        "object": "object",
        "okMayuscula": "OK",
        "operadorComienzaCon": "operadorComienzaCon",
        "operadorContiene": "operadorContiene",
        "operadorEsIgualA": "operadorEsIgualA",
        "operadorFinalizaCon": "operadorFinalizaCon",
        "operadorLogicoAnd": "operadorLogicoAnd",
        "operadorLogicoOr": "operadorLogicoOr",
        "operadorMayorIgualQue": "operadorMayorIgualQue",
        "operadorMayorQue": "operadorMayorQue",
        "operadorMenorIgualQue": "operadorMenorIgualQue",
        "operadorMenorQue": "operadorMenorQue",
        "operadorNoComienzaCon": "operadorNoComienzaCon",
        "operadorNoContiene": "operadorNoContiene",
        "operadorNoEsIgualA": "operadorNoEsIgualA",
        "operadorNoFinalizaCon": "operadorNoFinalizaCon",
        "operadorTieneValor": "operadorTieneValor",
        "operadorEstaVacio": "operadorEstaVacio",
        "operadorAntes": "operadorAntes",
        "operadorVigenciaActual": "operadorVigenciaActual",
        "operadorVigenciaFutura": "operadorVigenciaFutura",
        "operadorDespues": "operadorDespues",
        "pattern": "pattern",
        "placeholder": "placeholder",
        "porcentaje": "porcentaje",
        "properties": "properties",
        "reglas": "reglas",
        "reglasAlActualizar": "reglasAlActualizar",
        "reglasAlAgregar": "reglasAlAgregar",
        "readonly": "readonly",
        "regexpattern": "regexpattern",
        "required": "required",
        "row": "row",
        "schema": "schema",
        "section": "section",
        "sections": "sections",
        "seleccionUnica": "checkbox",
        "string": "string",
        "style": "style",
        "submit": "submit",
        "subtitulo": "subtitulo",
        "tabindex": "tabindex",
        "tamano": "tamano",
        "target": "target",
        "textAling": "textAling",
        "textSelect": "textSelect",
        "title": "title",
        "titleMap": "titleMap",
        "titulo": "titulo",
        "tokenSeleccionarTodos": "todos",
        "tooltip": "tooltip",
        "type": "type",
        "urlEndPoint": "urlEndPoint",
        "urlGet": "urlGet",
        "valorMaximo": "valorMaximo",
        "valorMinimo": "valorMinimo",
        "valoresPorDefecto": "valoresPorDefecto",
        "value": "value",
        "valueSelect": "valueSelect",
        "variableCondicion": "condicion",
        "visible": "visible",
        "listaDependiente": "listaDependiente",
        "panels": "panels",
        "enableSorting": "enableSorting",
        "columnDefs": "columnDefs",
        "lang": "lang",
        "enableFiltering": "enableFiltering",
        "enableGridMenu": "enableGridMenu",
        "exporterMenuCsv": "exporterMenuCsv",
        "exporterCsvFilename": "exporterCsvFilename",
        "exporterMenuPdf": "exporterMenuPdf",
        "buttonTypeSave": "buttonTypeSave",
        "buttonTypeReturn": "buttonTypeReturn",
        "buttonTypeSearch": "buttonTypeSearch",
        "buttonTypeFicha": "buttonTypeFicha",
        "buttonTypeCustom": "buttonTypeCustom",
        "buttonTypeNext": "buttonTypeNext",
        "buttonTypeConditions": "buttonTypeConditions",
        "condicion": "condicion",
        "modeloServicioPreguntas": "modeloServicioPreguntas",
        "eventoCustom": "eventoCustom",
        "estado": {
            "pendiente": 0,
            "publicado": 1,
            "inactivo": 2
        },
        "tiposMetodos": {
            "get": "get",
            "post": "post",
            "put": "put",
            "delete": "delete"
        },
        "tipoDisenador": {
            "formulario": 0,
            "encabezado": 1
        },
        "tipoFormulario": {
            "cuerpoFormulario": 1,
            "encabezadoFormularioPorDefecto": 2,
            "piePaginaFormularioPorDefecto": 3,
            "encabezadoFormulario": 4,
            "piePaginaFormulario": 5
        },
        "encabezado": 0,
        "pieDePagina": 1,
        "habilitarEdicion": "habilitarEdicion",
        "tipoEdicion": "tipoEdicion",
        "habilitarTotalizacion": "habilitarTotalizacion",
        "habilitarPaginacion": "habilitarPaginacion",
        "habilitarFiltrado": "habilitarFiltrado",
        "habilitarEliminacion": "habilitarEliminacion",
        "modeloServicioPreguntasGrilla": "modeloServicioPreguntasGrilla",
        "PreguntasEspecificas": "PreguntasEspecificas",
        "PreguntasGenerales": "PreguntasGenerales",
        "tipoMensajeAlert": {
            "success": 1,
            "info": 2,
            "error": 3
        },
        "fichaCategoria": "fichaCategoria",
        "fichaPlantilla": "fichaPlantilla"
    }

    var constantesAcciones = {
        estado: {
            ejecutada: "Ejecutada",
            pasoEnProgreso: "PasoEnProgreso",
            porDefinir: "PorDefinir"
        },
        tipo: {
            accionInicial: 1,
            accionTipoTransaccional: 2,
            accionTipoAlmacenamiento: 3,
            accionTipoNotificacion: 4,
            accionTipoEnrutamiento: 5,
            accionFinal: 6,
            accionTipoAnidada: 7,
            scopeParalelas: 8,
            scopeRamas: 9
        }
    }

    var constantesTipoFiltro = {
        proyecto: 1,
        tramites: 2,
        inflexibilidades: 3,
        entidadProyecto: 4
    }

    var constantesCondicionFiltro = {
        igual: 0,
        diferente: 1,
        menor: 2,
        menorIgual: 3,
        mayor: 4,
        mayorIgual: 5,
        contiene: 6,
        comienza: 7,
        termina: 8
    }

    var constantesTipoBancoProyecto = {
        pgn: 1,
        sgr: 4,
        territorio: 5
    }

    var constantesTipoConceptoViabilidad = {
        Viabilidad: "VIABILIDAD",
        CTUS: "CTUS",
        Integrado: "CTUSINT"
    }

    ////////////

    angular.module('backbone', [
        'ngRoute',
        'ngStorage',
        'schemaForm',
        "ngAnimate",
        'slickCarousel',
        "ui.bootstrap",
        'ui.grid',
        'ui.grid.selection',
        'ui.grid.exporter',
        'ui.grid.edit',
        'ui.grid.rowEdit',
        'ui.grid.cellNav',
        'ui.select',
        'ui.grid.pagination',
        'ui.grid.moveColumns',
        'ui.grid.resizeColumns',
        'ui.grid.saveState',
        'ui.grid.expandable',
        'ui.grid.treeView',
        'sAlert',
        'ui.mask',
        'ui.utils.masks',
        'ngFileSaver',
        'ui.grid.draggable-rows',
        'btorfs.multiselect',
        'angular.chosen',
        'textAngular',
        //Modulos
        'backbone.core',
        'backbone.archivo',
        'backbone.usuarios',
        'backbone.formulario',
        'backbone.model',
        'backbone.extensions',
        'backbone.entidades'
    ])
        .constant('appSettings', {
            topePaginacionConsultaAplicaciones: topePaginacionConsultaAplicaciones, //Propiedad web.config
            topePaginacion: topePaginacion, //Propiedad web.config
            maximoPaginas: maximoPaginas, //Propiedad web.config
            orden: []
        })
        .constant('constantesFormularios', constantesFormularios)
        .constant('constantesAcciones', constantesAcciones)
        .constant('constantesTipoFiltro', constantesTipoFiltro)
        .constant('constantesCondicionFiltro', constantesCondicionFiltro)
        .constant('constantesTipoBancoProyecto', constantesTipoBancoProyecto)
        .constant('constantesTipoConceptoViabilidad', constantesTipoConceptoViabilidad)
        .config(bloqueConfig)
        .config(uibModalConfig)
        .config(uibTooltipProvider)
        .config(disableOnUnhandledRejections)
        .run(bloqueRun)
        .filter('to_trusted', ['$sce', function ($sce) {
            return function (text) {
                return $sce.trustAsHtml(text);
            }
        }]);

    ////////////

    bloqueConfig.$inject = ['blockUIConfig'];
    function bloqueConfig(blockUIConfig) {

        blockUIConfig.message = '';
        blockUIConfig.requestFilter = function (config) {
            //Perform a global, case-insensitive search on the request url for 'noblockui' ...
            if (config.url.match(/noblockui/gi)) {
                return false;
            }
        };
    }

    uibModalConfig.$inject = ['$uibModalProvider'];
    function uibModalConfig($uibModalProvider) {
        $uibModalProvider.options = {
            animation: false,
            controllerAs: "vm",
            size: 'lg',
            keyboard: true,
            backdrop: 'static'
        };
    }

    uibTooltipProvider.$inject = ['$uibTooltipProvider']
    function uibTooltipProvider($uibTooltipProvider) {
        $uibTooltipProvider.options({
            appendToBody: true
        })
    }

    function disableOnUnhandledRejections($qProvider) {
        $qProvider.errorOnUnhandledRejections(false);
    }

    bloqueRun.$inject = [
        '$rootScope',
        '$q',
        '$route',
        'sesionServicios',
        'autorizacionServicios',
        'backboneServicios',
        '$localStorage',
        '$timeout',
        'utilidades',
        'mensajeServicio',
        'servicioCentroAyuda',
        '$sessionStorage'
    ];

    function bloqueRun($rootScope, $q, $route, sesionServicios, autorizacionServicios, backboneServicios, $localStorage, $timeout, utilidades, mensajeServicio, servicioCentroAyuda, $sessionStorage) {

        $timeout(function () {
           //sesionServicios.limpiarUsuario();
            backboneServicios.obtenerToken()
                .then(guardarTokenAutorizacionEnLocalStorage)
                .then(obtenerPermisosPorEntidad)
                .then(guardarPermisosEnSessionStorage)
                //TODO: obtener los roles del objeto de sesion usuario.permisos.Entidades[].Roles
                .then(obtenerRolesUsuario)
                .then(guardarRolesUsuarioEnSessionStorage)
                .then(obtenerMensajesMantenimiento)
                .then(obtenerListaTemas)
                .then(confirmarAutorizacion)
                .then(leerListaParametros)
                .catch(mostrarError);
        });

        function leerListaParametros() {

            backboneServicios.obtenerListaParametros().then(function (respuesta) {

                $sessionStorage.parametros = respuesta.data;
            }, function (error) {
                console.log(error)
            });
        }

        function confirmarAutorizacion() {
            return $timeout(function () { $rootScope.$broadcast('AutorizacionConfirmada') });
        }

        function guardarRolesUsuarioEnSessionStorage(rolesUsuario) {
            var deferred = $q.defer();

            sesionServicios.setearUsuarioRoles(rolesUsuario);

            deferred.resolve(true);

            // retorna promesa
            return deferred.promise;
        }

        function guardarPermisosEnSessionStorage(permisos) {
            var deferred = $q.defer();
            sesionServicios.setearPermisos(permisos);
            deferred.resolve(true);
            return deferred.promise;
        }

        function guardarTokenAutorizacionEnLocalStorage(autorizationData) {
            var deferred = $q.defer();

            if (autorizationData.data.Exito) {
                $localStorage.authorizationData = autorizationData.data.Token;
                $localStorage.oldToken = autorizationData.data.OldToken;
            }

            deferred.resolve($localStorage.authorizationData);

            return deferred.promise;
        }

        function obtenerPermisosPorEntidad() {
            return autorizacionServicios.obtenerPermisosPorEntidad(usuarioDNP);
        }

        function obtenerRolesUsuario() {
            return autorizacionServicios.obtenerRolesPorUsuario(usuarioDNP);
        }

        function obtenerMensajesMantenimiento() {
            return mensajeServicio.obtenerUsuarioMensajesMantenimiento();
        }

        function obtenerListaTemas() {

            return servicioCentroAyuda.obtenerListaTemasBroadcast();
        }

        function cerrarSession() {
            window.location.href = "/Account/SignOut";
        }

        function mostrarError(e, f, g) {
            var mensaje = "";

            if (e)
                if (e.data) {
                    if (e.data.ExceptionMessage)
                        mensaje = e.data.ExceptionMessage;
                } else {
                    if (e.statusText)
                        mensaje = e.statusText;
                }

            if (usuarioDNP == '') {
                mensaje = "<h3>El usuario no tiene permisos para esta aplicación.<h3/> <small>Por favor comunicarse con el administrador del sistema.</small>"
                utilidades.mensajeError(mensaje, cerrarSession, '');
            } else {
                utilidades.mensajeError(mensaje);
            }

        }
    }
})();

