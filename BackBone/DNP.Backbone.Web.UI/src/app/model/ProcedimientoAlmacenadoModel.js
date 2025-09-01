///<reference path="base/ModelBase.js" />

(function () {
    'use strict'

    angular.module('backbone.model').factory('ProcedimientoAlmacenadoModel', ['ModelBase', 
        function (ModelBase) {
            return class ProcedimientoAlmacenadoModel extends ModelBase {
                constructor(data = {}) {
                    super(data);
                    this.NombreProcedimiento = data.NombreProcedimiento;
                }
            };
    }])
})();