/// <reference path="base/ModelBase.js" />
/// <reference path="AlertaMonitoreoConfigModel.js" />

(function() {
    'use strict'

    angular.module('backbone.model').factory('AlertaMonitoreoGeneradaModel', ['ModelBase', 'AlertaMonitoreoConfigModel', function(ModelBase, AlertaMonitoreoConfigModel) {
        return class AlertaMonitoreoGeneradaModel extends ModelBase {
            constructor(data = {}) {
                super(data);

                this.AlertasConfigId = data.AlertasConfigId;
                this.AlertasConfig = new AlertaMonitoreoConfigModel((data.AlertasConfig || {}));
                this.ProyectoId = data.ProyectoId;
                this.Mensaje = data.Mensaje;
                this.Classificacion = data.Classificacion;
            }
        };
    }])
})();