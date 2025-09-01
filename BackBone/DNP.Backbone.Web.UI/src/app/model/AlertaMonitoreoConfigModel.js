/// <reference path="base/ModelBase.js" />
/// <reference path="AlertaMonitoreoReglaModel.js" />

(function() {
    'use strict'

    angular.module('backbone.model').factory('AlertaMonitoreoConfigModel', ['AlertaMonitoreoReglaModel', 'ModelBase', function(AlertaMonitoreoReglaModel, ModelBase) {
        return class AlertaMonitoreoConfigModel extends ModelBase {
            constructor(data = {}) {
                super(data);
                
                this.NombreAlerta = data.NombreAlerta;
                this.TipoAlerta = data.TipoAlerta;
                this.MensajeAlerta = data.MensajeAlerta;
                this.Classificacion = data.Classificacion;
                this.Estado = data.Estado;
                this.AlertasReglasDtos = (data && data.AlertasReglasDtos || []).map(x => new AlertaMonitoreoReglaModel(x));
            }
        };
    }])
})();