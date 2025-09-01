(function () {

    'use strict';

    angular.module('backbone')
        .factory('formularioServicios', formularioServicios);

    formularioServicios.$inject = ['$http', 'appSettings', 'utilidades', '$filter', 'constantesFormularios'];

    function formularioServicios($http, appSettings, utilidades, $filter, constantesFormularios) {

        var servicios = {
            actualizarFormulario: actualizarFormulario,
            obtenerFormularios: obtenerFormularios,
            crearFormulario: crearFormulario,
            obtenerFormulariosActivos: obtenerFormulariosActivos,
            obtenerFormulariosPorFiltros: obtenerFormulariosPorFiltros,
            obtenerFormulariosporId: obtenerFormulariosporId,
            obtenerPropiedadesApiExterna: obtenerPropiedadesApiExterna,
            obtenerExpresionesRegulares: obtenerExpresionesRegulares,
            crearExpresionRegular: crearExpresionRegular,
            eliminarExpresionRegular: eliminarExpresionRegular,
            eliminarFormularioPorId: eliminarFormularioPorId,
            actualizarExpresionRegular: actualizarExpresionRegular,
            obtenerPosiblesTiposDeControl: obtenerPosiblesTiposDeControl,
            ObtenerTipoCampoPropiedad: obtenerTipoCampoPropiedad,
            obtenerOperadoresCondicionalesDeReglas: obtenerOperadoresCondicionalesDeReglas,
            obtenerAccionesDeReglas: obtenerAccionesDeReglas,
            obtenerTipoOperadorLogicoDeReglas: obtenerTipoOperadorLogicoDeReglas,
            obtenerListadoSwaggers: obtenerListadoSwaggers,
            obtenerCamposEntidad: obtenerCamposEntidad,
            obtenerFormulariosPlantilla: obtenerFormulariosPlantilla,
            obtenerControlesActualesParaAuditoria: obtenerControlesActualesParaAuditoria,
            obtenerControlesEditados: obtenerControlesEditados,
            esPublicable: esPublicable,
            esInactivable: esInactivable,
            cambiarEstado: cambiarEstado,
            estaElLogDeErroresVacio: estaElLogDeErroresVacio,
            construirLogActualizarFormularios: construirLogActualizarFormularios
    }

        return servicios;

        ///////////////////////

        function obtenerControlesEditados(anterior, actual) {
            var listaDeEditados = [];
            anterior = obtenerControlesDeDisenador(anterior);
            actual = obtenerControlesDeDisenador(actual);
            angular.forEach(anterior, function (controlAnterior) {
                    angular.forEach(actual, function (controlActual) {
                            if (controlActual.id === controlAnterior.id) {
                                if (controlesDiferentes(controlActual, controlAnterior) && listaDeEditados.indexOf(controlActual.id) < 0) {
                                    listaDeEditados.push(controlActual.id);
                                }
                            }
                        });

                });
            return listaDeEditados.toString() === "" ? $filter('language')('NoHuboElementosModificados') : listaDeEditados.toString();
        }

        function controlesDiferentes(controlUno, controlDos) {
            var propiedad;
// ReSharper disable once MissingHasOwnPropertyInForeach
            for (propiedad in controlUno) {
                if (controlUno[propiedad] !== controlDos[propiedad] && propiedad !== "$$hashKey") {
                    return true;
                }
            }
            return false;
        }


        function obtenerControlesDeDisenador(disenador) {
            var arrayDeControles = [];
            angular.forEach(disenador, function (panel) {
                    angular.forEach(panel.columns, function (columna) {
                            if (columna.length > 0) {
                                arrayDeControles.push(columna[0]);
                            }
                        });
                });
            return arrayDeControles;
        }

        function obtenerControlesActualesParaAuditoria(disenador) {
            var controlesEnDisenador = [];
            angular.forEach(disenador, function (panel) {
                    angular.forEach(panel.columns, function (columna) {
                            if (columna.length > 0) {
                                controlesEnDisenador.push(columna[0].id);
                            }
                        });
                });
            return controlesEnDisenador.toString() === "" ? $filter('language')('NoFueronDefinidosControlesFormulario') : controlesEnDisenador.toString();
        }


        function obtenerCamposEntidad(entidad, tipoDisenador, tipoControl) {
            tipoControl = tipoControl === undefined ? apiPIIPProductoNegocio : tipoControl;

            if (tipoDisenador === undefined || tipoDisenador === null)
                tipoDisenador = constantesFormularios.tipoDisenador.formulario;


            var retorno = [];
            if (entidad && entidad.data) {
                if (tipoDisenador === constantesFormularios.tipoDisenador.encabezado) {
                    angular.forEach(entidad.data.properties,
                        function (value, key) {

                            if (value.hasOwnProperty('type')) {

                                if (value.type === "string" && !value.hasOwnProperty('format')) {
                                    retorno.push(coleccionControles("controlLabel", key));
                                }
                                if (value.type === "string" &&
                                    value.hasOwnProperty('format') &&
                                    value.format === "date-time") {
                                    retorno.push(coleccionControles("controlLabel", key));
                                }
                                if (value.type === "number" || value.type === "integer") {
                                    retorno.push(coleccionControles("controlLabel", key));
                                }
                                if (value.type === "boolean") {
                                    retorno.push(coleccionControles("controlLabel", key));
                                }
                                if (value.type === "controlPowerBI") {
                                            retorno.push(coleccionControles("controlPowerBI", key));
                                }

                            }

                        });
                } else {
                    angular.forEach(entidad.data.properties,
                        function (value, key) {
                            switch (tipoControl) {
                                case apiPIIPProductoCargaArchivo:
                                    if (value.hasOwnProperty('type') && ["string", "number", "integer"].indexOf(value.type) > -1) {
                                        retorno.push(coleccionControles("controlLabel", key));
                                    }

                                    break;
                                default:
                                    if (value.hasOwnProperty('type')) {
                                        if (value.type === "string" && !value.hasOwnProperty('format')) {
                                            retorno.push(coleccionControles("controlTexto", key));
                                        }
                                        if (value.type === "string" &&
                                            value.hasOwnProperty('format') &&
                                            value.format === "date-time") {
                                            retorno.push(coleccionControles("controlFecha", key));
                                        }
                                        if (value.type === "number" || value.type === "integer") {
                                            retorno.push(coleccionControles("controlNumerico", key));
                                        }
                                        if (value.type === "boolean") {
                                            retorno.push(coleccionControles("controlSeleccionUnica", key));
                                        }
                                        if (value.type === "array") { //TODO GRILLA
                                            retorno.push(coleccionControles("controlTabla", key));
                                        }
                                        if (value.type === "controlWbs") {
                                            retorno.push(coleccionControles("controlWbs", key));
                                        }
                                    } else {
                                        retorno.push(coleccionControles("controlSelect", key));
                                    }
                            }

                        });
                }
            }
            return retorno;
        }

        function coleccionControles(tipo, key) {
            switch (tipo) {
            case 'controlTabla':
                return {
                    data: [
                    ],
                    enableSorting: true,
                    columnDefs: [
                        ////{ field: 'nombre' },
                        ////{ field: 'genero' },
                        ////{ field: 'company' }, { field: 'edad', cellClass: 'ageCell', headerClass: 'ageHeader', 
                        ////    cellTemplate: '<div><form name="inputForm"><input type="number" ng-class="\'colt\' + col.uid" ui-grid-editor ng-model="MODEL_COL_FIELD"></form></div>' } 
                    ],
                    lang: 'es',
                    enableFiltering: false,
                    enableGridMenu: false,
                    exporterMenuCsv: false,
                    exporterCsvFilename: key + '.csv',
                    exporterPdfFilename: key + '.pdf',
                    exporterMenuPdf: false,
                    paginationPageSizes: [10, 50, 75],
                    paginationPageSize: 10,
                    enablePaginationControls: false,
                    habilitarEdicion: false,
                    tipoEdicion: undefined,
                    habilitarTotalizacion: false,
                    habilitarFiltrado: false,
                    habilitarPaginacion: false,
                    habilitarEliminacion: false,

                    "id": key,
                    "key": key,
                    "title": key,
                    "tooltip": key,
                    "type": "controlTabla",
                    "tipoControl": "Tabla",
                    "urlServicioGrilla": "",
                    "configuracionCabecera": [],
                    "modeloServicioPreguntasGrilla": "",
                    "tipo": "grid",
                    "icon": "icon icon-grid"
                };
            case 'controlTexto':
                return {
                    "id": key,
                    "key": key,
                    "title": key,
                    "placeholder": "Digite " + key,
                    "tooltip": key,
                    "type": "controlTexto",
                    "tipoControl": "Texto",
                    "tipo": "string",
                    "required": false,
                    "validationText": "",
                    "format": "string",
                    "regex": "",
                    "htmlClass": "form-control",
                    "readonly": false,
                    "tabOrder": 0,
                    "defaultValue": "",
                    "visible": true,
                    "scripts": "",
                    "maxLength": 200,
                    "icon": "icon icon-text",
                    "tipoDato": "texto"
                    };
            case 'controlLabel':
                return {
                    'id': key,
                    'key': key,
                    'title': key,
                    'label': key,
                    'type': 'controlLabel',
                    'tipoControl': 'controlLabel',
                    'tamano': 'small',
                    'tipo': 'controlLabel',
                    'valor': '|' + key + '|',
                    'validationText': '',
                    'format': 'controlLabel',
                    'textAling': 'center',
                    'htmlClass': 'controlLabel',
                    'readonly': false,
                    'tabOrder': 0,
                    'esInformativo': true,
                    'defaultValue': '',
                    'visible': true,
                    'scripts': '',
                    'icon': 'icon icon-font'
                };
            case 'controlNumerico':
                return {
                    "id": key,
                    "key": key,
                    "title": key,
                    "placeholder": "" + key,
                    "tipoControl": "Numerico",
                    "tooltip": "Digite " + key,
                    "type": "controlNumerico",
                    "tipo": constantesFormularios.controlNumerico,
                    "required": false,
                    "validationText": "",
                    "format": "number",
                    "htmlClass": "form-control",
                    "readonly": false,
                    "activo": "",
                    "tabOrder": 0,
                    "regex": "",
                    "formato": " ",
                    "defaultValue": "",
                    "visible": true,
                    "scripts": "",
                    "maxValue": 100,
                    "minValue": 0,
                    "decimalNum": 2,
                    "icon": "icon icon-numerico",
                    "tipoDato": "numero"
                }
            case 'controlSeleccionUnica':
                return {
                    "id": key,
                    "key": key,
                    "title": key,
                    "tooltip": key,
                    "tipoControl": "Selección Única",
                    "type": "controlSeleccionUnica",
                    "validationText": "",
                    "activo": "",
                    "required": false,
                    "format": "boolean",
                    "tipo": "checkbox",
                    "htmlClass": "checkbox",
                    "tabOrder": 0,
                    "defaultValue": false,
                    "visible": true,
                    "scripts": "",
                    "icon": "icon icon-checkbox",
                    "tipoDato": "booleano"
                }
            case 'controlSelect':
                return {
                    "id": key,
                    "key": key,
                    "title": key,
                    "type": "controlSelect",
                    "tipoControl": "Lista desplegable",
                    "placeholder": "Lista " + key,
                    "tipo": "dropdownlist",
                    "format": "dropdownlist",
                    "tooltip": key,
                    "validationText": "",
                    "htmlClass": "form-control",
                    "visible": true,
                    "required": false,
                    "activo": "",
                    "scripts": "",
                    "selectedValue": '',
                    "tabOrder": 0,
                    "urlGet": "",
                    "nameSelected": "",
                    "valueSelected": "",
                    "titleMap": [],
                    "icon": "icon icon-select",
                    "listaDependiente": "",
                    "habilitarBusqueda": false,
                    "tipoDato": "lista"
                }
            case 'controlFecha':
                return {
                    "id": key,
                    "key": key,
                    "title": key,
                    "type": "controlFecha",
                    "tipoControl": "Campo de Fecha",
                    "tipo": "fecha",
                    "format": "YYYY/MM/DD",
                    "tooltip": key,
                    "validationText": "",
                    "htmlClass": "form-control",
                    "visible": true,
                    "required": false,
                    "activo": "",
                    "scripts": "",
                    "tabOrder": 0,
                    "defaultValue": undefined,
                    "today": false,
                    "dateBeg": undefined,
                    "dateEnd": undefined,
                    "icon": "icon icon-calendar",
                    "tipoDato": "fecha"
                }
            default:
            }
// ReSharper disable once NotAllPathsReturnValue
        }


        function obtenerFormularios() {
            return $http.get(apiFormularioServicioBaseUri + "api/Formulario/ObtenerFormularios")
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerFormulariosActivos() {
            return $http.get(apiFormularioServicioBaseUri + "api/Formulario/ObtenerFormulariosActivos")
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerFormulariosPlantilla(formulario) {
            return $http.get(apiFormularioServicioBaseUri + "api/Formulario/ObtenerFormulariosPlantilla?entidad=" + formulario)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function crearFormulario(formulario) {
            return $http.post(apiFormularioServicioBaseUri + "api/Formulario/CrearFormulario", formulario)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerFormulariosPorFiltros(filtros) {
            return $http.post(apiFormularioServicioBaseUri + "api/Formulario/ObtenerFormulariosPorFiltros", filtros)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerFormulariosporId(idFormulario) {
            return $http.get(apiFormularioServicioBaseUri + "api/Formulario/ObtenerFormulariosporId?id=" + idFormulario)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }


        function obtenerPropiedadesApiExterna(urlApi) {

            return $http.get(urlApi)
                .then(function (response) {
                        return response;
                    }, function (response) {
                        return response;
                    });

        }

        function obtenerListadoSwaggers() {

            return $http.get(apiFormularioServicioBaseUri + "api/Formulario/ObtenerListadoSwaggers")
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerExpresionesRegulares() {

            //return $http.get("https://apiformulariospruebas.azure-api.net/Formularios//api/Formulario/ObtenerExpresionesRegulares")
            //    .then(utilidades.httpRequestComplete, utilidades.httpRequestError);

            return $http.get(apiFormularioServicioBaseUri + "api/Formulario/ObtenerExpresionesRegulares")
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function crearExpresionRegular(regex) {
            return $http.post(apiFormularioServicioBaseUri + "api/Formulario/CrearExpresionRegular", regex)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function eliminarExpresionRegular(id) {
            return $http.delete(apiFormularioServicioBaseUri + "api/Formulario/EliminarExpresionRegular?id=" + id)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function eliminarFormularioPorId(id) {
            return $http.delete(apiFormularioServicioBaseUri + "api/Formulario/EliminarFormulario?id=" + id)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }


        function actualizarExpresionRegular(regex) {
            return $http.put(apiFormularioServicioBaseUri + "api/Formulario/ActualizarExpresionRegular", regex)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function actualizarFormulario(formulario) {
            return $http.put(apiFormularioServicioBaseUri + "api/Formulario/ActualizarFormulario", formulario)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerPosiblesTiposDeControl(tipoControl) {
            return $http.get(apiFormularioServicioBaseUri + "api/Formulario/ObtenerTiposCampos?campo=" + tipoControl)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerTipoCampoPropiedad(tipoControl) {
            return $http.get(apiFormularioServicioBaseUri + "api/Formulario/ObtenerTipoCampoPropiedad?campo=" + tipoControl)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerOperadoresCondicionalesDeReglas() {
            var condionales = [];

            var esIgualA = crearItemLista(constantesFormularios.operadorEsIgualA);
            var noEsIgualA = crearItemLista(constantesFormularios.operadorNoEsIgualA);
            var contiene = crearItemLista(constantesFormularios.operadorContiene);
            var noContiene = crearItemLista(constantesFormularios.operadorNoContiene);
            var comienzaCon = crearItemLista(constantesFormularios.operadorComienzaCon);
            var noComienzaCon = crearItemLista(constantesFormularios.operadorNoComienzaCon);
            var finalizaCon = crearItemLista(constantesFormularios.operadorFinalizaCon);
            var noFinalizaCon = crearItemLista(constantesFormularios.operadorNoFinalizaCon);
            var mayorQue = crearItemLista(constantesFormularios.operadorMayorQue);
            var menorQue = crearItemLista(constantesFormularios.operadorMenorQue);
            var tieneValor = crearItemLista(constantesFormularios.operadorTieneValor);
            var estaVacio = crearItemLista(constantesFormularios.operadorEstaVacio);
            var antes = crearItemLista(constantesFormularios.operadorAntes);
            var despues = crearItemLista(constantesFormularios.operadorDespues);

            var tipoNumerico = { "tipo": "numero", "condiciones": [esIgualA, noEsIgualA, estaVacio, tieneValor, menorQue, mayorQue] };
            var tipoFecha = { "tipo": "fecha", "condiciones": [esIgualA, noEsIgualA, estaVacio, tieneValor, antes, despues] };
            var tipoTexto = { "tipo": "texto", "condiciones": [esIgualA, noEsIgualA, estaVacio, tieneValor, contiene, noContiene, comienzaCon, noComienzaCon, finalizaCon, noFinalizaCon] };
            var tipoLista = { "tipo": "lista", "condiciones": [esIgualA, noEsIgualA, estaVacio, tieneValor] };
            var tipoSeleccionUnica = { "tipo": "booleano", "condiciones": [esIgualA, noEsIgualA, estaVacio, tieneValor] };

            condionales.push(tipoNumerico);
            condionales.push(tipoFecha);
            condionales.push(tipoTexto);
            condionales.push(tipoLista);
            condionales.push(tipoSeleccionUnica);

            return condionales;
        }

        function obtenerTipoOperadorLogicoDeReglas() {
            var operador = [];

            operador.push(crearItemLista(constantesFormularios.operadorLogicoAnd));
            operador.push(crearItemLista(constantesFormularios.operadorLogicoOr));

            return operador;
        }

        function obtenerAccionesDeReglas() {
            var acciones = [];

            acciones.push(crearItemLista(constantesFormularios.accionTipoEsconder));
            acciones.push(crearItemLista(constantesFormularios.accionTipoMostrar));
            acciones.push(crearItemLista(constantesFormularios.accionTipoEstablecerValorEnCampos));
            acciones.push(crearItemLista(constantesFormularios.accionNoSePuedeModificar));
            acciones.push(crearItemLista(constantesFormularios.accionEstablecerValorFijo));
            acciones.push(crearItemLista(constantesFormularios.accionEstablecerValorMinimo));
            acciones.push(crearItemLista(constantesFormularios.accionEstablecerValorMaximo));

            return acciones;
        }

// ReSharper disable once UnusedParameter
        function crearItemLista(id, tipo) {
            return {
                "id": id,
                "despliegue": $filter("language")(id)
            };
        }

        function cambiarEstado(estado, listaDeFormularios) {
            var elementosAPublicar = new Object();
            elementosAPublicar.listaDeIdentificaciones = listaDeFormularios.map(function (formulario) {                
                return formulario.Id;
            });

            return $http.put(apiFormularioServicioBaseUri + 'api/Formulario/Estado/' + estado, elementosAPublicar)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function esPublicable(formulario) {
            return formulario.EstadoFormulario !== constantesFormularios.estado.publicado
                && !formulario.EsPlantilla;
        }

        function esInactivable(formulario) {
            return formulario.EstadoFormulario !== constantesFormularios.estado.inactivo
                && !formulario.EsPlantilla;
        }

        function estaElLogDeErroresVacio(respuesta) {
            return !Object.keys(respuesta).some(function(propiedad) {
                return propiedad !== '$id';
            });
        }

        function construirLogActualizarFormularios(listaDeErrores) {
            var titulo = '<div class="container"><p><h4>Se presentaron los siguientes errores.</h4></p>';
            var inicioTabla = "<div ><table class='table table-bordered table-error'>";
            var cabeceraDeTabla = '<thead><tr><th class="text-center">Formulario</th><th class="text-center">Error</th></tr></thead>';
            var cuerpoDeLaTabla = '<tbody>';
            var finDeLaTabla = '</tbody></table></div></div>';

            Object.keys(listaDeErrores).forEach(function(nombreFormulario) {
                if (nombreFormulario === '$id') { return; }

                var formularioTotalErrores = listaDeErrores[nombreFormulario].length;

                cuerpoDeLaTabla += '<tr><th rowspan="' + formularioTotalErrores + '">' +
                    nombreFormulario +
                    '</th>';

                listaDeErrores[nombreFormulario].forEach(function (error, indice) {
                    if (indice === 0) {
                        cuerpoDeLaTabla += '<th>' + error + '</th></tr>';

                        return;
                    }

                    cuerpoDeLaTabla += '<tr><th>' + error + '</th><tr/>';                    
                });
            }); 

            return titulo + inicioTabla + cabeceraDeTabla + cuerpoDeLaTabla + finDeLaTabla;
        }
    }
})();