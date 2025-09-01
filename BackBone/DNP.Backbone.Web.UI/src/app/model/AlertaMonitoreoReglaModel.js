/// <reference path="base/ModelBase.js" />
/// <reference path="MapColumnaModel.js" />
/// <reference path="Constantes/TipoValorColumnaConstante.js" />

(function() {
    'use strict'

    angular.module('backbone.model').factory('AlertaMonitoreoReglaModel', ['ModelBase', 'MapColumnaModel', 'TipoValorColumnaConstante', 
        function(ModelBase, MapColumnaModel, TipoValorColumnaConstante) {
            return class AlertaMonitoreoReglaModel extends ModelBase {
                constructor(data = {}) {
                    super(data);

                    this.Operador = data.Operador;
                    this.Valor = data.Valor;
                    this.Condicional = data.Condicional;
                    this.AlertasMonitoreoConfigId = data.AlertasMonitoreoConfigId;
                    this.MapColumnasUno = new MapColumnaModel((data.MapColumnasUno || {}));
                    this.MapColumnasUnoId = data.MapColumnasUnoId;
                    this.MapColumnasDos = new MapColumnaModel((data.MapColumnasDos || {}));
                    this.MapColumnasDosId = data.MapColumnasDosId;
                    this.AlertasConfigId = data.AlertasConfigId;
                    this.TipoColumna = data.TipoColumna;
                    
                    if (!data.TipoValor) {
                        data.TipoValor = data.Valor ? TipoValorColumnaConstante.Valor : TipoValorColumnaConstante.Columna;
                    }

                    this._tipoValor = data.TipoValor;
                    this._activarValor = data.TipoValor == TipoValorColumnaConstante.Valor;
                    this._activarColunaDos = data.TipoValor == TipoValorColumnaConstante.Columna;
                }

                get ActivarValor() {
                    return this._activarValor;
                }

                set ActivarValor(boolean) {
                    this._activarValor = boolean;
                    this._activarColunaDos = !boolean;
                    this.MapColumnasDosId = null;
                }

                get ActivarColunaDos() {
                    return this._activarColunaDos;
                }

                set ActivarColunaDos(boolean) {
                    this._activarColunaDos = boolean;
                    this._activarValor = !boolean;
                    this.Valor = null;
                }

                get TipoValor() {
                    return this._tipoValor;
                }

                set TipoValor(value) {
                    this._tipoValor = value;
                    this.ActivarValor = value == TipoValorColumnaConstante.Valor;
                    this.ActivarColunaDos = value == TipoValorColumnaConstante.Columna;
                }
            };
        }
    ])
})();