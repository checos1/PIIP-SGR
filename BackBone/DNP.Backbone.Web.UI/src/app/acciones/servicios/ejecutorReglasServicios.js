(function () {
    "use strict";

    angular.module("backbone").factory("ejecutorReglasServicios", ejecutorReglasServicios);

    ejecutorReglasServicios.$inject = ["constantesFormularios", "$filter"];

    function ejecutorReglasServicios(constantesFormularios, $filter) {
        return {
            controlarError: controlarError,
            ocultarCampos: ocultarCampos,
            mostrarCampos: mostrarCampos,
            establecerValorEnCampos: establecerValorEnCampos,
            obtenerValor: obtenerValor,
            evaluar: evaluar,
            sonIguales: sonIguales,
            contiene: contiene,
            menorQue: menorQue,
            menorOIgualQue: menorOIgualQue,
            mayorQue: mayorQue,
            mayorOIgualQue: mayorOIgualQue,
            tieneValor: tieneValor,
            comienzaCon: comienzaCon,
            finalizaCon: finalizaCon,
            obtenerCampoDelFormulario: obtenerCampoDelFormulario, 
            validarCamposRequeridos: validarCamposRequeridos,
            converterValorWBS: converterValorWBS
        };

        function controlarError(error) {
            console.error($filter("language")("ErrorEjecutarRegla"), error);
        }

        function converterValorWBS(valor, formato) {
            var resultado = null;

            if (valor != undefined || valor != null) {
                switch (formato) {
                    case "integer":
                    case "number":
                        resultado = Number(valor);
                        break;
                    case "string":
                        resultado = valor;
                        break;
                    default:
                        throw (new Error("Formato no soportado"));
                }
            }

            return resultado;
        }

        function ocultarCampos(formulario, campos) {
            return cambiarPropiedadVisibleCampos(formulario, campos, false);
        }

        function mostrarCampos(formulario, campos) {
            return cambiarPropiedadVisibleCampos(formulario, campos, true);
        }

        function validarCamposRequeridos(model, camposRequeridos) {
            var campo;
            if (camposRequeridos.length > 0) {
                for (var i = 0; i < camposRequeridos.length; i++) {

                    campo = camposRequeridos[i];
                    var valor = model[campo];
                    if ((valor === undefined) || (valor === null))
                        return false;
                }
            }
            return true;
        }

        function obtenerCampoDelFormulario(items, idCampo) {
            var resultado = null;
            var item;

            if (items) {
                for (var i = 0; i < items.length; i++) {
                    item = items[i];

                    if (item.hasOwnProperty(constantesFormularios.key)) {
                        if (item[constantesFormularios.key][0] === idCampo) {
                            resultado = item;
                            break;
                        }
                    }

                    if (item.hasOwnProperty(constantesFormularios.items)) {
                        resultado = obtenerCampoDelFormulario(item[constantesFormularios.items], idCampo);
                        if (resultado !== null) {
                            break;
                        }
                    }
                }
            }

            return resultado;
        }

        function cambiarPropiedadVisibleCampos(formulario, campos, visible) {
            var campo;

            if (campos && formulario) {
                for (var i = 0; i < campos.length; i++) {
                    campo = obtenerCampoDelFormulario(formulario, campos[i]);

                    if (campo) {
                        campo.visible = visible;
                    }
                }
            }


            return formulario;
        }

        function establecerValorEnCampos(modelo, campos, valor) {
            if (campos) {
                for (var i = 0; i < campos.length; i++) {
                    modelo[campos[i]] = valor;
                }
            }

            return modelo;
        }

        function obtenerValor(modelo, valor, formato) {
            var resultado = null;

            if (valor) {
                switch (formato) {
                    case constantesFormularios.expresionTipoCampo:
                        resultado = obtenerValorCampo(modelo, valor);
                        break;
                    case constantesFormularios.formatoValorTexto:
                        resultado = valor;
                        break;
                    case constantesFormularios.formatoValorNumero:
                        resultado = Number(valor);
                        break;
                    case constantesFormularios.formatoValorFecha:
                        var fecha = new Date();
                        fecha.setTime(valor);
                        resultado = fecha;
                        break;
                    case constantesFormularios.formatoValorBooleano:
                        resultado = (valor === "true");
                        break;
                    case constantesFormularios.formatoValorLista:
                        resultado = convertirTextoALista(valor);
                        break;
                    default:
                        throw (new Error("Formato no soportado"));
                }
            }

            return resultado;
        }

        function obtenerValorCampo(modelo, campo) {
            var resultado = modelo[campo];

            if (resultado && typeof resultado === "object" && resultado.hasOwnProperty(constantesFormularios.value)) {
                resultado = resultado.value;
            }

            if (resultado instanceof Array && resultado.length === 0) {
                resultado = null;
            }

            return resultado;
        }

        function convertirTextoALista(valor) {
            var lista = [];

            if (valor) {
                if (valor.indexOf(",") >= 0) {
                    lista = (new Function("return [" + valor + "];")());
                } else {
                    lista.push(valor);
                }
            }

            return lista;
        }

        function evaluar(expresion1, operador, expresion2) {
            var resultado;

            switch (operador) {
                case constantesFormularios.operadorEsIgualA:
                    resultado = sonIguales(expresion1, expresion2);
                    break;
                case constantesFormularios.operadorNoEsIgualA:
                    resultado = !sonIguales(expresion1, expresion2);
                    break;
                case constantesFormularios.operadorContiene:
                    resultado = contiene(expresion1, expresion2);
                    break;
                case constantesFormularios.operadorNoContiene:
                    resultado = !contiene(expresion1, expresion2);
                    break;
                case constantesFormularios.operadorComienzaCon:
                    resultado = comienzaCon(expresion1, expresion2);
                    break;
                case constantesFormularios.operadorNoComienzaCon:
                    resultado = !comienzaCon(expresion1, expresion2);
                    break;
                case constantesFormularios.operadorFinalizaCon:
                    resultado = finalizaCon(expresion1, expresion2);
                    break;
                case constantesFormularios.operadorNoFinalizaCon:
                    resultado = !finalizaCon(expresion1, expresion2);
                    break;
                case constantesFormularios.operadorMenorQue:
                    resultado = menorQue(expresion1, expresion2);
                    break;
                case constantesFormularios.operadorMenorIgualQue:
                    resultado = menorOIgualQue(expresion1, expresion2);
                    break;
                case constantesFormularios.operadorMayorQue:
                    resultado = mayorQue(expresion1, expresion2);
                    break;
                case constantesFormularios.operadorMayorIgualQue:
                    resultado = mayorOIgualQue(expresion1, expresion2);
                    break;
                case constantesFormularios.operadorTieneValor:
                    resultado = tieneValor(expresion1);
                    break;
                case constantesFormularios.operadorEstaVacio:
                    resultado = !tieneValor(expresion1);
                    break;
                default:
                    throw (new Error("Operador no soportado"));
            }

            return resultado;
        }

        function sonIguales(expresion1, expresion2) {
            var resultado = false;

            if (typeof (expresion1) === "boolean") {
                expresion1 = expresion1.toString();
            }
            if (typeof (expresion2) === "boolean") {
                expresion2 = expresion2.toString();
            }

            if (expresion1 instanceof Date) {
                expresion1 = expresion1.getTime();
            }
            if (expresion2 instanceof Date) {
                expresion2 = expresion2.getTime();
            }

            if (expresion1 === "") {
                expresion1 = null;
            }
            if (expresion2 === "") {
                expresion2 = null;
            }

            if (typeof (expresion1) === "number") {
                expresion1 = expresion1.toString();
            }
            if (typeof (expresion2) === "number") {
                expresion2 = expresion2.toString();
            }

            if ((expresion1 && expresion2)) {
                if (expresion1 instanceof Array && expresion2 instanceof Array) {
                    resultado = sonListasIguales(expresion1, expresion2);
                } else {
                    resultado = (expresion1 === expresion2);
                }
            } else if (expresion1 === null && expresion2 === null) {
                resultado = true;
            }

            return resultado;
        }

        function sonListasIguales(lista1, lista2) {
            var resultado = false;
            var contadorCoincidencias = 0;

            if (lista1.length === lista2.length) {
                for (var i = 0; i < lista1.length; i++) {
                    for (var j = 0; j < lista2.length; j++) {
                        if (sonIguales(lista1[i], lista2[j])) {
                            contadorCoincidencias += 1;
                            break;
                        }
                    }
                }

                resultado = lista1.length === contadorCoincidencias;
            }

            return resultado;
        }

        function contiene(expresion1, expresion2) {
            var resultado = false;

            if (expresion1 && expresion2) {
                resultado = expresion1.indexOf(expresion2) >= 0;
            }

            return resultado;
        }

        function comienzaCon(expresion1, expresion2) {
            var resultado = false;

            function strStartsWith(str, prefix) {
                return str.indexOf(prefix) === 0;
            }

            if (expresion1 && expresion2) {
                resultado = strStartsWith(expresion1, expresion2);
            }

            return resultado;
        }

        function finalizaCon(expresion1, expresion2) {
            var resultado = false;

            function strEndsWith(str, suffix) {
                return str.match(suffix + "$") === suffix;
            }


            if (expresion1 && expresion2) {
                resultado = strEndsWith(expresion1, expresion2);
            }

            return resultado;
        }

        function menorQue(expresion1, expresion2) {
            var resultado = false;

            if (expresion1 && expresion2) {
                resultado = (expresion1 < expresion2);
            }

            return resultado;
        }

        function menorOIgualQue(expresion1, expresion2) {
            var resultado = false;

            if (expresion1 && expresion2) {
                resultado = (expresion1 <= expresion2);
            }

            return resultado;
        }

        function mayorQue(expresion1, expresion2) {
            var resultado = false;

            if (expresion1 && expresion2) {
                resultado = (expresion1 > expresion2);
            }

            return resultado;
        }

        function mayorOIgualQue(expresion1, expresion2) {
            var resultado = false;

            if (expresion1 && expresion2) {
                resultado = (expresion1 >= expresion2);
            }

            return resultado;
        }

        function tieneValor(expresion) {
            var resultado = false;

            switch (typeof (expresion)) {
                case "string":
                    resultado = expresion.length > 0;
                    break;
                case "object":
                    if (expresion) {
                        if (expresion instanceof Array) {
                            resultado = expresion.length > 0;
                        } else if (expresion instanceof Date) {
                            resultado = true;
                        } else if (expresion.hasOwnProperty(constantesFormularios.value)) {
                            resultado = true;
                        }
                    }
                    break;
                case "number":
                    resultado = !isNaN(expresion);
                    break;
                case "undefined":
                    resultado = false;
                    break;
                default:
                    resultado = true;
                    break;
            }


            return resultado;
        }
    }

})();