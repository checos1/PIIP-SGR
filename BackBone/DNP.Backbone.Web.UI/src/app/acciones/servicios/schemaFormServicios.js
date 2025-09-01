(function () {
    "use strict";

    angular.module("backbone").factory("schemaFormServicios", schemaFormServicios);

    schemaFormServicios.$inject = ["constantesFormularios"];

    function schemaFormServicios(constantesFormularios) {
        var cf = constantesFormularios;

        return {
            convertirASchemaForm: convertirASchemaForm,
            crearSchemaForm: crearSchemaForm,
            crearFilaSchemaForm: crearFilaSchemaForm,
            crearSectionSchemaForm: crearSectionSchemaForm,
            crearItemSchemaForm: crearItemSchemaForm,
            crearPropiedadesSchemaForm: crearPropiedadesSchemaForm,
            crearBotonAceptar: crearBotonAceptar,
            obtenerRequeridos: obtenerRequeridos
        };

        function crearSchemaForm(reglas) {
            var schema = {};
            schema[cf.type] = cf.object;
            schema[cf.title] = cf.comment;
            schema[cf.properties] = {};
            schema[cf.required] = [];
            schema[cf.reglas] = reglas;

            return schema;
        }

        function crearFilaSchemaForm() {
            var fila = {};
            fila[cf.type] = cf.section;
            fila[cf.htmlClass] = cf.row;
            fila[cf.items] = [];

            return fila;
        }

        function crearSectionSchemaForm(cantidadColumnas) {
            var seccion = {}
            seccion[cf.type] = cf.section;
            seccion[cf.htmlClass] = cf.classCol + (12 / cantidadColumnas);
            seccion[cf.items] = [];

            return seccion;
        }

        function crearItemSchemaForm(control) {
            var item = {};
            item[cf.key] = control.key;
            item[cf.title] = control.title;
            if (control.required === true) {
                item[cf.title] += " *";
            }
            item[cf.type] = control.tipo;
            item[cf.defaultValue] = control.defaultValue;
            item[cf.format] = control.format;
            item[cf.required] = control.required;
            item[cf.placeholder] = control.placeholder;
            item[cf.tooltip] = control.tooltip;
            item[cf.tabindex] = control.tabOrder;
            item[cf.readonly] = control.readonly;
            item[cf.urlGet] = control.urlGet;
            item[cf.urlEndPoint] = control.urlEndPoint;
            item[cf.valueSelect] = control.valueSelected;
            item[cf.textSelect] = control.nameSelected;
            item[cf.titleMap] = control.titleMap;
            item[cf.sections] = control.sections;
            item[cf.data] = control.data;
            item[cf.titulo] = control.titulo;
            item[cf.subtitulo] = control.subtitulo;
            item[cf.textAling] = control.textAling;
            item[cf.tamano] = control.tamano;
            item[cf.target] = control.target;
            item[cf.dateBeg] = control.dateBeg;
            item[cf.dateEnd] = control.dateEnd;
            item[cf.etiqueta] = control.etiqueta;
            item[cf.visible] = control.visible === false ? false : true; // Cuando la propiedad venga en null o undefined se muestra el campo.
            item[cf.valorMinimo] = control.valorMinimo;
            item[cf.valorMaximo] = control.valorMaximo;
            item[cf.porcentaje] = control.valorMaximo;
            item[cf.esInformativo] = control.esInformativo;
            item[cf.listaDependiente] = control.listaDependiente;
            item[cf.tipoListaMultiple] = control.tipoListaMultiple;
            item[cf.habilitarBusqueda] = control.habilitarBusqueda;
            item[cf.panels] = control.panels;
            item[cf.condition] = cf.funcionValidadVisibilidadCampo + "(\"" + control.key + "\")";
            item[cf.enableSorting] = control.enableSorting;
            item[cf.columnDefs] = control.columnDefs;
            item[cf.lang] = control.lang;
            item[cf.enableFiltering] = control.enableFiltering;
            item[cf.enableGridMenu] = control.enableGridMenu;
            item[cf.exporterMenuCsv] = control.exporterMenuCsv;
            item[cf.exporterCsvFilename] = control.exporterCsvFilename;
            item[cf.exporterMenuPdf] = control.exporterMenuPdf;
            item[cf.description] = control.tooltip;
            item[cf.fechaActualPorDefecto] = control.today ? control.today === "true" : undefined;
            item[cf.habilitarEdicion] = control.habilitarEdicion;
            item[cf.tipoEdicion] = control.tipoEdicion;
            item[cf.habilitarTotalizacion] = control.habilitarTotalizacion;
            item[cf.habilitarFiltrado] = control.habilitarFiltrado;
            item[cf.habilitarPaginacion] = control.habilitarPaginacion;
            item[cf.habilitarEliminacion] = control.habilitarEliminacion;
            item[cf.label] = control.label;
            item[cf.valor] = control.valor;
            item[cf.buttonTypeSave] = control.buttonTypeSave;
            item[cf.buttonTypeReturn] = control.buttonTypeReturn;
            item[cf.buttonTypeSearch] = control.buttonTypeSearch;
            item[cf.buttonTypeFicha] = control.buttonTypeFicha;
            item[cf.buttonTypeCustom] = control.buttonTypeCustom;
            item[cf.buttonTypeConditions] = control.buttonTypeConditions;
            item[cf.condicion] = control.condicion;
            item[cf.modeloServicioPreguntas] = control.modeloServicioPreguntas;
            item[cf.eventoCustom] = control.eventoCustom;
            item[cf.modeloServicioPreguntasGrilla] = control.modeloServicioPreguntasGrilla;
            return item;
        }

        function crearPropiedadesSchemaForm(control) {
            var propiedades = {};

            propiedades[cf.title] = control.title;
            propiedades[cf.type] = control.tipo;
            propiedades[cf.format] = control.format;
            propiedades[cf.maxLenght] = control.maxLength;
            propiedades[cf.minLenght] = control.minLength;
            propiedades[cf.maximum] = control.maxValue;
            propiedades[cf.minimum] = control.minValue;
            propiedades[cf.decimalNum] = control.decimalNum;
            propiedades[cf.pattern] = control.regex;
            propiedades[cf.description] = control.tooltip;

            switch (control.type) {
                case cf.controlTabla:
                    propiedades[cf.habilitarEdicion] = control.habilitarEdicion;
                    propiedades[cf.habilitarTotalizacion] = control.habilitarTotalizacion;
                    propiedades[cf.habilitarFiltrado] = control.habilitarFiltrado;
                    propiedades[cf.habilitarPaginacion] = control.habilitarPaginacion;
                    propiedades[cf.tipoEdicion] = control.tipoEdicion;
                    propiedades[cf.habilitarEliminacion] = control.habilitarEliminacion;
                    break;
                case cf.controlListaSeleccionMultiple:
                    propiedades[cf.type] = cf.array;
                    propiedades[cf.items] = {};
                    propiedades[cf.items][cf.type] = cf.string;
                    break;
                case cf.controlLista:
                    propiedades[cf.type] = [
                        cf.object,
                        null
                    ];
                    break;
                case cf.controlSeleccionMultiple:
                    propiedades[cf.type] = [
                        cf.array,
                        null
                    ];
                    propiedades[cf.items] = {};
                    propiedades[cf.items][cf.type] = cf.string;
                    break;
            }

            return propiedades;
        }

        function crearBotonAceptar() {
            var boton = {};
            boton[cf.type] = cf.submit;
            boton[cf.style] = cf.estiloBotonAceptar;
            boton[cf.title] = cf.okMayuscula;

            return boton;
        }

        function convertirASchemaForm(formularios, controlesAdicionales, reglas) {
            var formulario;

            var schema = crearSchemaForm(reglas);
            var form = [];
            var model = {};

            schema.required = obtenerRequeridos(formularios);

            var funcionAsignarModeloYEsquema = function (control, item, propiedad) {
                if (!item.esInformativo) {
                    model[control.id] = control.defaultValue ? control.defaultValue : null;
                } else {
                    delete model[control.id];
                }

                schema.properties[control.id] = propiedad;
            }

            for (var i = 0; i < formularios.length; i++) {
                formulario = formularios[i];

                if (formulario.type === constantesFormularios.contenedor ||
                    formulario.type === constantesFormularios.contenedorDosColumnas ||
                    formulario.type === constantesFormularios.contenedorTresColumnas) {

                    form.push(adicionarControles(formulario, funcionAsignarModeloYEsquema));
                }
            }

            if (controlesAdicionales && controlesAdicionales.length > 0) {

                form.push(adicionarControlesAdicionales(controlesAdicionales, funcionAsignarModeloYEsquema));
            }

            form.push(crearBotonAceptar());

            return {
                schema: schema,
                form: form,
                model: model
            };
        }

        function adicionarControles(formulario, funcionAsignacion) {
            var propiedad, seccion, control, item;

            var fila = crearFilaSchemaForm();

            for (var i = 0; i < formulario.columns.length; i++) {
                if (formulario.columns[i][0]) {
                    seccion = crearSectionSchemaForm(formulario.columns.length);
                    control = formulario.columns[i][0];
                    item = crearItemSchemaForm(control);

                    seccion.items.push(item);
                    fila.items.push(seccion);

                    propiedad = crearPropiedadesSchemaForm(control);

                    funcionAsignacion(control, item, propiedad);
                }
            }

            return fila;
        }

        function adicionarControlesAdicionales(controlesAdicionales, funcionAsignacion) {
            var propiedad, seccion, control, item;

            var fila = crearFilaSchemaForm();

            for (var i = 0; i < controlesAdicionales.length; i++) {
                seccion = crearSectionSchemaForm(1);
                control = controlesAdicionales[i];
                item = crearItemSchemaForm(control);

                seccion.items.push(item);
                fila.items.push(seccion);

                propiedad = crearPropiedadesSchemaForm(control);

                funcionAsignacion(control, item, propiedad);
            }

            return fila;
        }

        function obtenerRequeridos(formularios) {
            var requeridos = [];
            var j, control;

            for (var i = 0; i < formularios.length; i++) {
                for (j = 0; j < formularios[i].columns.length; j++) {
                    control = formularios[i].columns[j][0];
                    if (control && control.required) {
                        requeridos.push(control.id);
                    }
                }
            }
            return requeridos;
        }
    };
})();