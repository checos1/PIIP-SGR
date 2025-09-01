angular.module('backbone.core.directives')
.directive('numbers', function () {
    return {
        require: 'ngModel',
        restrict: 'A',

        link: function (scope, element, attr, ngModelCtrl) {

            const ENTEROS_PD = 16;
            const PRECISION_PD = 0;
            const NEGATIVOS_PD = false;

            function parseKeyValueString(str) {

                const pairs = str.split(';');

                var entries = [];
                var result = {};

                pairs.forEach(pair => {
                    const [key, value] = pair.split(':');
                    if (key && value) {
                        var entry = [].concat(key.trim()).concat(value.trim());
                        entries.push(entry);
                    }
                });

                result = Object.fromEntries(entries);

                return result;
            }

            // ActualizarFila
            element.bind('blur', function (event) {

                var parametros = parseKeyValueString(attr.numbers);
                var enteros = typeof (parametros.enteros) !== 'undefined' ? parseInt(parametros.enteros) : ENTEROS_PD;
                var precision = typeof (parametros.precision) !== 'undefined' ? parseInt(parametros.precision) : PRECISION_PD;
                var negativos = typeof (parametros.negativos) !== 'undefined' ? String(parametros.negativos).toLowerCase() === 'true' : NEGATIVOS_PD;

                $(event.target).val(function (index, value) {

                    if (Number.isNaN(value)) {
                        return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                    }

                    if (value == null) {
                        return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                    }

                    if (value == "") {
                        return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                    }

                    var valueStr = value.replaceAll(".", "").replace(",", ".");

                    if (negativos && (valueStr.match(/-/g) || []).length > 0) {
                        enteros = enteros + 1;
                    }

                    var splitArray = valueStr.split('.');
                    var parteEntera = valueStr.split('.')[0];
                    parteEntera = parteEntera.length > enteros ? parteEntera.slice(0, enteros) : parteEntera;
                    valueStr = splitArray.length === 2 ? [parteEntera, splitArray[1]].join(".") : parteEntera;
                    var val = parseFloat(valueStr);
                    val = Number.isNaN(val) ? 0 : val;

                    const decimalCnt = splitArray[1] ? splitArray[1].length : 0;
                    var total = decimalCnt <= precision ? val : val.toFixed(precision);
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: precision, }).format(total);
                });
            });

            // FormatFila
            element.bind('focus', function(event) {
                $(event.target).val(function (index, value) {

                    var valueFmt = value.replaceAll(".", "");
                    var valueStr = valueFmt.replace(",", ".");
                    var valueNbr = parseFloat(valueStr);

                    if (valueNbr == 0) {
                        return "";
                    } else {
                        return valueFmt;
                    }

                });
            });

            // ValidateFormat
            element.bind('keypress', function(event) {

                var parametros = parseKeyValueString(attr.numbers);
                var enteros = typeof (parametros.enteros) !== 'undefined' ? parseInt(parametros.enteros) : ENTEROS_PD;
                var precision = typeof (parametros.precision) !== 'undefined' ? parseInt(parametros.precision) : PRECISION_PD;
                var negativos = typeof (parametros.negativos) !== 'undefined' ? String(parametros.negativos).toLowerCase() === 'true' : NEGATIVOS_PD;

                var pos = event.target.selectionStart;
                var end = event.target.selectionEnd;

                // número o coma o negativo
                var key = event.keyCode;
                if ((key < 48 || key > 57) && ((key !== 44 && key !== 45) || (precision === PRECISION_PD && key === 44) || (!negativos && key === 45))) {
                    event.preventDefault();
                }

                var oldValue = event.target.value;

                if (key === 44 && (oldValue.match(/,/g) || []).length > 0) {
                    event.preventDefault();
                }
                
                if (key === 45 && (((oldValue.match(/-/g) || []).length > 0) || pos > 0)) {
                    event.preventDefault();
                }

                if (negativos && (key === 45 || (oldValue.match(/-/g) || []).length > 0)) {
                    enteros = enteros + 1;
                }

                var spiltArray = String(oldValue).split("");
                var tamanio = event.target.value.length;
                var incluyeDecimal = false;
                incluyeDecimal = event.target.value.toString().includes(",");
                if (incluyeDecimal) {

                    var n = String(oldValue).split(",");

                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] === '-')) return;
                    if (spiltArray.length === 2 && oldValue === '-,') return;

                    var digitaEntero = ((key >= 48 && key <= 57) && pos <= oldValue.indexOf(","));
                    var digitaDecimal = ((key >= 48 && key <= 57) && pos > oldValue.indexOf(","));

                    if (digitaEntero) {
                        if (n[0].length === enteros && pos === end) {
                            event.preventDefault();
                        } else {
                            return;
                        }
                    }
                    else if (digitaDecimal) {
                        if (n[1].length === precision && pos === end) {
                            event.preventDefault();
                        } else {
                            return;
                        }
                    } else {
                        return;
                    }
                } else {
                    if (tamanio === enteros && key >= 48 && key <= 57) {
                        event.preventDefault();
                    } else {
                        return;
                    }
                }
            });
        }
    };
});